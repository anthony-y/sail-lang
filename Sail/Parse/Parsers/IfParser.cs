using System;
using System.Collections.Generic;

using Sail.Parse.Expressions;
using Sail.Lexical;
using Sail.Error;

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
            {
                ErrorManager.CreateError("If statement is missing a body! ({ ... })", ErrorType.Error, token.Line, token.Column);
                return null;
            }

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
                elseExpr = parser.ParseExpression(1) as ElseExpression;

            return new IfExpression(token.Line, token.Column, (BlockExpression)ifBlock, condition, elseIfBlocks, elseExpr);
        }
    }
}
