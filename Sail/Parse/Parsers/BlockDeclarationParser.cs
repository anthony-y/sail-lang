using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class BlockDeclarationParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.ASSIGNMENT;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return null;
        }
    }
}
