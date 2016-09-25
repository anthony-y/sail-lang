using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class AssignmentExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public IExpression Name { get; set; }
        public IExpression Value { get; set; }

        public SailType Type { get; set; }
        public SailType ExpectedType { get; set; }

        public AssignmentExpression(int line, int column, IExpression name, IExpression value, SailType type, SailType expectedType = SailType.UNKNOWN)
        {
            Name = name;
            Value = value;
            Type = type;
            ExpectedType = expectedType;

            Line = line;
            Column = column;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            //Console.WriteLine("ASSIGNMENT EXPRESSION");
            //Console.WriteLine("---------------------");
            //Value.Print();
            //Console.WriteLine("Type: " + Type);

            return Value.Print();
        }
    }
}
