using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

namespace Sail.Parse.Parsers
{
    internal class FetchParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var fileNameExpr = parser.ParseExpression(1);
            if (!(fileNameExpr is StringLiteralExpression))
            {
                ErrorManager.CreateError("Right hand side of fetch expression must be a str literal containing the desired file name!", ErrorType.Error, token.Line, token.Column);
                return null;
            }

            string fileName = ((StringLiteralExpression)fileNameExpr).Value;

            if (parser.TokenStream.Peek().Type == TokenType.SEMICOLON)
                parser.TokenStream.Read();

            return new FetchExpression(token.Line, token.Column, fileName);
        }
    }
}
