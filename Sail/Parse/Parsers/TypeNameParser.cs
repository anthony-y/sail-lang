using Sail.Lexical;
using Sail.Parse.Expressions;
using Sail.Error;

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
                    {
                        ErrorManager.CreateError("Invalid type name!", ErrorType.Error, token.Line, token.Column);
                        return null;
                    }
            }

            return new TypeNameExpression(token.Line, token.Column, typeName);
        }

        public IExpression Parse(Parser parser, Token token, IExpression left)
        {
            return Parse(parser, token);
        }
    }
}
