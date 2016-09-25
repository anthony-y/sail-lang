using System;

using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class ComparisonParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.SUM;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            if (!(left is ILiteralExpression || left is IdentifierExpression))
            {
                ErrorManager.CreateError("Left hand side of comparison operator must be variable name or literal!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var right = parser.ParseExpression(GetPrecedence() - 1);

            if (!(right is ILiteralExpression || right is IdentifierExpression))
            {
                ErrorManager.CreateError("Right hand side of comparison operator must be variable name or literal!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new ComparisonExpression(token.Line, token.Column, left, right, token.Type);
        }
    }
}
