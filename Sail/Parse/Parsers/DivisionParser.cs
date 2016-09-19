using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class DivisionParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.PRODUCT - 1;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            var right = parser.ParseExpression(GetPrecedence() - 1);

            if (left is IntLiteralExpression && right is IntLiteralExpression)
                return new IntLiteralExpression(((left as IntLiteralExpression).Value / (right as IntLiteralExpression).Value).ToString());

            else if (left is FloatLiteralExpression && right is FloatLiteralExpression)
                return new FloatLiteralExpression(((left as FloatLiteralExpression).Value / (right as FloatLiteralExpression).Value).ToString());

            else if (left is IntLiteralExpression && right is FloatLiteralExpression)
                return new FloatLiteralExpression(((left as IntLiteralExpression).Value / (right as FloatLiteralExpression).Value).ToString());

            else if (left is FloatLiteralExpression && right is IntLiteralExpression)
                return new FloatLiteralExpression(((left as FloatLiteralExpression).Value / (right as IntLiteralExpression).Value).ToString());

            throw new Exception("You can only add numbers, dumbass");
        }
    }
}
