using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

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
                throw new Exception("Left hand side of comparison operator must be identifier or literal");

            var right = parser.ParseExpression(GetPrecedence() - 1);

            if (!(right is ILiteralExpression || right is IdentifierExpression))
                throw new Exception("Right hand side of comparison operator must be identifier or literal");

            return new ComparisonExpression(left, right, token.Type);
        }
    }
}
