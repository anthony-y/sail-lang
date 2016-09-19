using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class IdentifierParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            return new IdentifierExpression(token.Value);
        }
    }
}
