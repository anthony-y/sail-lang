using Sail.Lexical;
using Sail.Parse.Expressions;
using Sail.Error;

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
                    return new StringLiteralExpression(token.Line, token.Column, token.Value);
                case TokenType.INTLITERAL:
                    return new IntLiteralExpression(token.Line, token.Column, token.Value);
                case TokenType.FLOATLITERAL:
                    return new FloatLiteralExpression(token.Line, token.Column, token.Value);
                case TokenType.BOOLLITERAL:
                    return new BoolLiteralExpression(token.Line, token.Column, token.Value);
            }

            ErrorManager.CreateError("Right of iterator expression must be an integer or variable name!", ErrorType.Error, token.Line, token.Column);
            return null;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return Parse(parser, token);
        }
    }
}
