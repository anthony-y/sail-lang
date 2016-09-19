using Sail.Lexical;

namespace Sail.Parse
{
    internal interface IInfixParser
    {
        IExpression Parse(Parser parser, Token token, IExpression left);
        int GetPrecedence();
    }
}
