using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class PutsParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            if (parser.TokenStream.Peek().Type == TokenType.OPAREN)
                parser.TokenStream.Read();

            var exprToPrint = parser.ParseExpression(0);

            if (parser.TokenStream.Peek().Type == TokenType.CPAREN)
                parser.TokenStream.Read();

            return new PutsExpression(token.Line, token.Column, exprToPrint);
        }
    }
}