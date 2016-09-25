using Sail.Interpreter;

namespace Sail.Parse
{
    internal interface IExpression
    {
        int Line { get; set; }
        int Column { get; set; }

        void Accept(IVisitor visitor);

        string Print();
    }
}
