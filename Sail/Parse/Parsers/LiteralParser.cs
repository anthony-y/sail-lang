using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class LiteralParser
        : IPrefixParser, IInfixParser
    {
        public int GetPrecedence()
        {
            return 0;
        }

        public IExpression Parse(Parser parser, Token token)
        {
            switch (token.Type)
            {
                case TokenType.STRLITERAL:
                    return new StringLiteralExpression(token.Value);
                case TokenType.INTLITERAL:
                    return new IntLiteralExpression(token.Value);
                case TokenType.FLOATLITERAL:
                    return new FloatLiteralExpression(token.Value);
                case TokenType.BOOLLITERAL:
                    return new BoolLiteralExpression(token.Value);
            }

            throw new Exception("Unexpected literal value!");
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return Parse(parser, token);
        }
    }
}
