using Sail.Lexical;

namespace Sail.Parse
{
    internal interface IPrefixParser
    {
        IExpression Parse(Parser parser, Token token);
    }
}
