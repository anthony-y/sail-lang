using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Lexical;

namespace Sail.Parse.Parsers
{
    internal class IfParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var condition = parser.ParseExpression(1);

            var ifBlock = parser.ParseExpression(1);
            if (ifBlock == null)
                throw new Exception("If statement is missing an if block");

            var elseIfBlocks = new List<ElseIfExpression>();

            if (parser.TokenStream.Peek().Type == TokenType.ELSEIF)
            {
                while (parser.TokenStream.Peek().Type == TokenType.ELSEIF)
                {
                    var elseIfExpr = parser.ParseExpression(1);

                    elseIfBlocks.Add((ElseIfExpression)elseIfExpr);
                }
            }

            ElseExpression elseExpr = null;

            if (parser.TokenStream.Peek().Type == TokenType.ELSE)
            {
                elseExpr = parser.ParseExpression(1) as ElseExpression;

                if (elseExpr == null)
                    throw new Exception("Else expression is not an else expression which is actually impossible");
            }

            return new IfExpression((BlockExpression)ifBlock, condition, elseIfBlocks, elseExpr);
        }
    }
}
