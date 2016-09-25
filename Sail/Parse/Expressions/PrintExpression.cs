using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class PrintExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public IExpression Value { get; set; }

        public PrintExpression(int line, int column, IExpression value)
        {
            Value = value;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            return "";
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
