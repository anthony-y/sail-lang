using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class MathsParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.SUM;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            if (!(left is IntLiteralExpression || left is FloatLiteralExpression || left is IdentifierExpression || left is MathsExpression))
                throw new Exception("Left of binary operator must be int, float, variable name or another binary operator!");

            var right = parser.ParseExpression(GetPrecedence());

            if (!(right is IntLiteralExpression || right is FloatLiteralExpression || right is IdentifierExpression || left is MathsExpression))
                throw new Exception("Right of binary operator must be int, float, variable name or another binary operator!");

            return new MathsExpression(left, right, token.Type);
        }
    }
}
