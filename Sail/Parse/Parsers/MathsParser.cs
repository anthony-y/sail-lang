using System;

using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

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
            {
                ErrorManager.CreateError("Left of binary operator must be int, float, variable name or another binary operator!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var right = parser.ParseExpression(GetPrecedence());

            if (!(right is IntLiteralExpression || right is FloatLiteralExpression || right is IdentifierExpression || left is MathsExpression))
            {
                ErrorManager.CreateError("Right of binary operator must be int, float, variable name or another binary operator!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new MathsExpression(token.Line, token.Column, left, right, token.Type);
        }
    }
}
