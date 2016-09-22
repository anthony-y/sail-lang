using System;
using Sail.Lexical;
using Sail.Parse.Expressions;

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
                throw new Exception("Variable name must be a valid identifier!");

            var resolved = TypeResolver.ToSailType(type as TypeNameExpression);

            var equals = parser.TokenStream.PeekAny(TokenType.ASSIGNMENT);
            if (!equals)
                return new VarDeclarationNoAssignExpression((left as IdentifierExpression).Value, resolved);

            // Skip equals
            parser.TokenStream.Read();

            var value = parser.TokenStream.Peek();
            if (value == null)
                throw new Exception("Expected value on variable declaration");

            var valueExpr = parser.ParseExpression(GetPrecedence());
            var typeOfValue = TypeResolver.ToSailType(valueExpr as ILiteralExpression);

            if (!(valueExpr is FunctionCallExpression || valueExpr is ComparisonExpression))
                if (typeOfValue != resolved)
                    throw new Exception("Type mismatch! Expecting " + resolved + " but got " + typeOfValue);

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

            return new AssignmentExpression(left, valueExpr, resolved, resolved);
        }
    }
}
