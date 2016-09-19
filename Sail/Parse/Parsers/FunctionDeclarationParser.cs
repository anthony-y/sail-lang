using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;

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
                throw new Exception("Expected identifier as name for function declaration!");

            string name = (left as IdentifierExpression).Value;

            if (parser.TokenStream.Peek().Type != TokenType.OPAREN)
                throw new Exception("Function parameters must be enclosed in parenthesis!");

            // Read past open parenthesis
            parser.TokenStream.Read();

            var parameters = new List<VarDeclarationNoAssignExpression>();

            if (parser.TokenStream.Peek().Type != TokenType.CPAREN)
            {
                do
                {
                    var variable = parser.ParseExpression(GetPrecedence() - 1);

                    if (!(variable is VarDeclarationNoAssignExpression))
                        throw new Exception("Invalid parameters in function!");

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
                        throw new Exception("Invalid return types in function declaration!");

                    returnTypeExpressions.Add((TypeNameExpression)type);
                } while (parser.TokenStream.MatchAll(TokenType.COMMA));

                parser.Expect(TokenType.CPAREN);

            } else
            {
                returnType = (parser.ParseExpression(0) as TypeNameExpression);
                if (returnType == null)
                    throw new Exception("You must have a return type for your function!");

                if (!(returnType is TypeNameExpression))
                    throw new Exception("Return type of function must be a type name expression!");
            }

            var block = parser.ParseExpression(GetPrecedence());
            if (block == null || !(block is BlockExpression))
                throw new Exception("Function body missing!");

            var returnTypes = new List<SailReturnType>();

            if (returnTypeExpressions.Any())
                foreach (var rt in returnTypeExpressions)
                    returnTypes.Add(TypeResolver.ToReturnType(rt));

            else returnTypes.Add(TypeResolver.ToReturnType(returnType));

            return new FunctionExpression(name, parameters, returnTypes, block as BlockExpression);
        }
    }
}
