using System;

using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class ForParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var iterator = parser.ParseExpression(1);

            IteratorExpression iteratorExpr = null;
            string listName = null;

            if (iterator is IteratorExpression)
                iteratorExpr = (IteratorExpression)iterator;

            else if (iterator is IdentifierExpression)
                listName = ((IdentifierExpression)iterator).Value;

            else
            {
                ErrorManager.CreateError("You can only iterate over a list, str literal or iterator expression!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            var block = parser.ParseExpression(1);
            if (!(block is BlockExpression))
            {
                ErrorManager.CreateError("For expression needs a block!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            return new ForExpression(token.Line, token.Column, iteratorExpr, listName, (BlockExpression)block);
        }
    }
}
