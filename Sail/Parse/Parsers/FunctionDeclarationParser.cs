using System;
using System.Collections.Generic;
using System.Linq;

using Sail.Lexical;
using Sail.Error;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class FunctionDeclarationParser
        : IInfixParser, IPrefixParser
    {
        public int GetPrecedence()
        {
            return Precedence.ASSIGNMENT;
        }

        public IExpression Parse(Parser parser, Token token)
        {
            return Parse(parser, token);
        }

        // main :: (args...) -> void { // Block }        
        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            if (!(left is IdentifierExpression))
            {
                ErrorManager.CreateError("Expected identifier as name for function declaration!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            string name = (left as IdentifierExpression).Value;

            if (parser.TokenStream.Peek().Type != TokenType.OPAREN)
            {
                ErrorManager.CreateError("Function parameters must be enclosed in parenthesis!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            // Read past open parenthesis
            parser.TokenStream.Read();

            var parameters = new List<VarDeclarationNoAssignExpression>();

            if (parser.TokenStream.Peek().Type != TokenType.CPAREN)
            {
                do
                {
                    var variable = parser.ParseExpression(GetPrecedence() - 1);

                    if (!(variable is VarDeclarationNoAssignExpression))
                    {
                        ErrorManager.CreateError("Parameters must look like: (name: type, ...)!", ErrorType.Error, token.Line, token.Column);
                        return null;
                    }

                    parameters.Add((VarDeclarationNoAssignExpression)variable);

                } while (parser.TokenStream.MatchAll(TokenType.COMMA));

                parser.TokenStream.Read();

            } else parser.TokenStream.Read();

            parser.Expect(TokenType.RIGHTARROW);

            var returnTypeExpressions = new List<TypeNameExpression>();
            TypeNameExpression returnType = null;
            
            if (parser.TokenStream.Peek().Type == TokenType.OPAREN)
            {
                parser.TokenStream.Read();

                do
                {
                    var type = parser.ParseExpression(GetPrecedence());

                    if (!(type is TypeNameExpression))
                    {
                        ErrorManager.CreateError("Function return type(s) must be a typename!", ErrorType.Error, token.Line, token.Column);
                        return null;
                    }

                    returnTypeExpressions.Add((TypeNameExpression)type);
                } while (parser.TokenStream.MatchAll(TokenType.COMMA));

                parser.Expect(TokenType.CPAREN);

            } else
            {
                returnType = (parser.ParseExpression(0) as TypeNameExpression);
                if (returnType == null)
                {
                    ErrorManager.CreateError("Your function must have a return type: -> [(]type [, type, ...)]!", ErrorType.Error, token.Line, token.Column);
                    return null;
                }

                if (!(returnType is TypeNameExpression))
                {
                    ErrorManager.CreateError("Function return type(s) must be a typename!", ErrorType.Error, token.Line, token.Column);
                    return null;
                }
            }

            var block = parser.ParseExpression(GetPrecedence());
            if (block == null || !(block is BlockExpression))
            {
                ErrorManager.CreateError("Function declaration must have a body ({ ... })!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var returnTypes = new List<SailReturnType>();

            if (returnTypeExpressions.Any())
                foreach (var rt in returnTypeExpressions)
                    returnTypes.Add(TypeResolver.ToReturnType(rt));

            else returnTypes.Add(TypeResolver.ToReturnType(returnType));

            return new FunctionExpression(token.Line, token.Column, name, parameters, returnTypes, block as BlockExpression);
        }
    }
}
