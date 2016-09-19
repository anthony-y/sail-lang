using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class TypeNameParser
        : IInfixParser, IPrefixParser
    {
        public int GetPrecedence()
        {
            return Precedence.PREFIX;
        }

        public IExpression Parse(Parser parser, Token token)
        {
            string typeName;

            switch (token.Type)
            {
                case TokenType.BOOL:
                case TokenType.STR:
                case TokenType.INT:
                case TokenType.FLOAT:
                case TokenType.VOID:
                    typeName = token.Value;
                    break;
                default:
                    throw new Exception("Invalid type name!");
            }

            return new TypeNameExpression(typeName);
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return Parse(parser, token);
        }
    }
}
