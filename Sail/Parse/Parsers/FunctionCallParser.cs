using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class FunctionCallParser
        : IInfixParser
    {
        public int GetPrecedence()
        {
            return Precedence.CALL;
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            string functionName = (left as IdentifierExpression).Value;

            var parameters = new List<IExpression>();

            if (parser.TokenStream.Peek().Type != TokenType.CPAREN)
            {
                do
                {
                    var variable = parser.ParseExpression(0);
                    parameters.Add(variable);
                } while (parser.TokenStream.MatchAll(TokenType.COMMA));

                parser.Expect(TokenType.CPAREN);
            }
            else parser.TokenStream.Read();

            return new FunctionCallExpression(token.Line, token.Column, functionName, parameters);
        }
    }
}