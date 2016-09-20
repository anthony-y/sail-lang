using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Parse.Expressions;

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
                throw new Exception("Else if expression is missing a block");

            return new ElseIfExpression(block, elseIfCondition);
        }
    }
}
