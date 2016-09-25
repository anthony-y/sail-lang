using Sail.Lexical;
using Sail.Parse.Expressions;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class ElseIfParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var elseIfCondition = parser.ParseExpression(1);
            var block = parser.ParseExpression(1) as BlockExpression;
            if (block == null)
            {
                ErrorManager.CreateError("Else if expression is missing a block!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new ElseIfExpression(token.Line, token.Column, block, elseIfCondition);
        }
    }
}
