using System;

using Sail.Lexical;
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
            if (!(right is IntLiteralExpression || right is IdentifierExpression))
                throw new Exception("Right of iterator expression needs to be an integer or a variable name");

            int? lowerBound = null; // "NaN"
            int? upperBound = null;

            string lowerBoundVar = null;
            string upperBoundVar = null;

            if (left is IntLiteralExpression)
                lowerBound = ((IntLiteralExpression)left).Value;

            else if (left is IdentifierExpression)
                lowerBoundVar = ((IdentifierExpression)left).Value;

            else throw new Exception("Left of iterator expression needs to be an integer or a variable name");

            if (right is IntLiteralExpression)
                upperBound = ((IntLiteralExpression)right).Value;

            else if (right is IdentifierExpression)
                upperBoundVar = ((IdentifierExpression)right).Value;

            else throw new Exception("Right of iterator expression needs to be a number or a variable name");

            return new IteratorExpression(lowerBound, upperBound, lowerBoundVar, upperBoundVar);
        }
    }
}