using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class ReturnParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var value = parser.ParseExpression(1);

            //if (!(value is ILiteralExpression) || !(value is IdentifierExpression))
            //    throw new Exception("Return must be followed by variable name or literal!");

            return new ReturnExpression(token.Line, token.Column, value);
        }
    }
}
