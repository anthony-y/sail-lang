using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class BlockParser
        : IInfixParser, IPrefixParser
    {
        public int GetPrecedence()
        {
            return 0;
        }

        public IExpression Parse(Parser parser, Token token)
        {
            var exprs = new List<IExpression>();

            while (parser.TokenStream.Peek().Type != TokenType.CBRACE)
            {
                var expr = parser.ParseExpression(GetPrecedence());
                exprs.Add(expr);

                if (parser.TokenStream.Peek().Type == TokenType.SEMICOLON)
                    parser.TokenStream.Read();
            }

            parser.Expect(TokenType.CBRACE);

            return new BlockExpression(token.Line, token.Column, exprs);
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return Parse(parser, token);
        }
    }
}
