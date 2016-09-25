using System;

using Sail.Lexical;
using Sail.Error;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class IteratorParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.SUM;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            var right = parser.ParseExpression(1);
            
            int? lowerBound = null;
            int? upperBound = null;

            string lowerBoundVar = null;
            string upperBoundVar = null;

            if (left is IntLiteralExpression)
                lowerBound = ((IntLiteralExpression)left).Value;

            else if (left is IdentifierExpression)
                lowerBoundVar = ((IdentifierExpression)left).Value;

            else
            {
                ErrorManager.CreateError("Left of iterator expression must be an integer or variable name!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            if (right is IntLiteralExpression)
                upperBound = ((IntLiteralExpression)right).Value;

            else if (right is IdentifierExpression)
                upperBoundVar = ((IdentifierExpression)right).Value;

            else
            {
                ErrorManager.CreateError("Right of iterator expression must be an integer or variable name!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new IteratorExpression(token.Line, token.Column, lowerBound, upperBound, lowerBoundVar, upperBoundVar);
        }
    }
}