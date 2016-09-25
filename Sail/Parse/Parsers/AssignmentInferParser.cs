using System;

using Sail.Lexical;
using Sail.Error;
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
            {
                ErrorManager.CreateError("Explicit variable declaration syntax: name : type = value", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var value = parser.ParseExpression(GetPrecedence() - 1);

            SailType type = SailType.UNKNOWN;

            if (value is BoolLiteralExpression || value is StringLiteralExpression || value is FloatLiteralExpression || value is IntLiteralExpression)
                type = TypeResolver.ToSailType(value as ILiteralExpression);

            else if (value is FunctionCallExpression)
                type = SailType.UNKNOWN;

            else if (value is ComparisonExpression)
                type = SailType.BOOL;

            else throw new Exception("Incorrect explicit variable declaration syntax!");

            return new AssignmentExpression(token.Line, token.Column, left, value, type);
        }
    }
}
