using Sail.Lexical;
using Sail.Parse.Expressions;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class TypeOfParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var obj = parser.ParseExpression(1);

            string variableName = null;
            object value = null;

            if (obj is IdentifierExpression)
                variableName = ((IdentifierExpression)obj).Value;

            else if (obj is ILiteralExpression)
                value = ((ILiteralExpression)obj).GetValue();

            else
            {
                ErrorManager.CreateError("Can only find types of variables and literals!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new TypeOfExpression(token.Line, token.Column, variableName, value);
        }
    }
}
