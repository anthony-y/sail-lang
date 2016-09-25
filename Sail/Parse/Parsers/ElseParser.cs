using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class ElseParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var block = parser.ParseExpression(1);

            if (!(block is BlockExpression))
            {
                ErrorManager.CreateError("Else expression is missing a block!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new ElseExpression(token.Line, token.Column, (BlockExpression)block);
        }
    }
}
