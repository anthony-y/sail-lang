using System;
using Sail.Lexical;
using Sail.Parse.Expressions;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class AssignmentExplicitParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.ASSIGNMENT;
        }

        // identifier : type = value [as type];
        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            var type = parser.ParseExpression(GetPrecedence() - 1);

            if (!(type is TypeNameExpression))
            {
                ErrorManager.CreateError("Variable name must be a valid identifier!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var resolved = TypeResolver.ToSailType(type as TypeNameExpression);

            var equals = parser.TokenStream.PeekAny(TokenType.ASSIGNMENT);
            if (!equals)
                return new VarDeclarationNoAssignExpression(token.Line, token.Column, (left as IdentifierExpression).Value, resolved);

            // Skip equals
            parser.TokenStream.Read();

            var value = parser.TokenStream.Peek();
            if (value == null)
            {
                ErrorManager.CreateError("Expected value on variable declaration!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var valueExpr = parser.ParseExpression(GetPrecedence());
            var typeOfValue = TypeResolver.ToSailType(valueExpr as ILiteralExpression);

            // DIRTY HACK
            if (!(valueExpr is FunctionCallExpression || valueExpr is ComparisonExpression))
                if (typeOfValue != resolved)
                {
                    ErrorManager.CreateError("Type mismatch! Expecting " + resolved + " but got " + typeOfValue, ErrorType.Error, token.Line, token.Column);
                    return null;
                }

            // Unfinished casting support :3
            bool cast = parser.TokenStream.PeekAll(TokenType.AS);
            if (cast)
            {
                parser.TokenStream.Read();

                var castType = parser.ParseExpression(1);
                if (castType != null)
                {
                    var castTypeExpr = TypeResolver.ToSailType(castType as TypeNameExpression);
                    resolved = castTypeExpr;
                }
            }

            return new AssignmentExpression(token.Line, token.Column, left, valueExpr, resolved, resolved);
        }
    }
}
