using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;

using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class AssignmentInferParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.ASSIGNMENT;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            if (!(left is IdentifierExpression))
                throw new Exception("Left of assignment must be identifier!");

            var value = parser.ParseExpression(GetPrecedence() - 1);

            SailType type = SailType.UNKNOWN;

            if (value is BoolLiteralExpression || value is StringLiteralExpression || value is FloatLiteralExpression || value is IntLiteralExpression)
                type = TypeResolver.ToSailType(value as ILiteralExpression);

            else if (value is FunctionCallExpression)
                type = SailType.UNKNOWN;

            else throw new Exception("Incorrect explicit variable declaration syntax!");

            return new AssignmentExpression(left, value, type);
        }
    }
}
