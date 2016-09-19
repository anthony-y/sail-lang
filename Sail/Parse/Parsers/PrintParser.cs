using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class PrintParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            if (parser.TokenStream.Peek().Type == TokenType.OPAREN)
                parser.TokenStream.Read();

            var exprToPrint = parser.ParseExpression(0);

            if (parser.TokenStream.Peek().Type == TokenType.CPAREN)
                parser.TokenStream.Read();

            return new PrintExpression(exprToPrint);
        }
    }
}
