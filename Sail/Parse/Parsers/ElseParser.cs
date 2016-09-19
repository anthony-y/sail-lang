using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class ElseParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var block = parser.ParseExpression(1);

            if (!(block is BlockExpression))
                throw new Exception("Else expression needs a block!");

            return new ElseExpression((BlockExpression)block);
        }
    }
}
