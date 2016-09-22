using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class FetchParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var fileNameExpr = parser.ParseExpression(1);
            if (!(fileNameExpr is StringLiteralExpression))
                throw new Exception("Right hand side of fetch expression must be str literal of file name!");

            string fileName = ((StringLiteralExpression)fileNameExpr).Value;

            if (parser.TokenStream.Peek().Type == TokenType.SEMICOLON)
                parser.TokenStream.Read();

            return new FetchExpression(fileName);
        }
    }
}
