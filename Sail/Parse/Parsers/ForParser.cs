using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

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

            else throw new Exception("You can only iterate over a list or iterator expression");

            var block = parser.ParseExpression(1);
            if (!(block is BlockExpression))
                throw new Exception("For expression needs a block");

            return new ForExpression(iteratorExpr, listName, (BlockExpression)block);
        }
    }
}
