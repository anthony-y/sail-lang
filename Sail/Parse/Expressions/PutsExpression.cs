using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class PutsExpression
        : IExpression
    {
        public IExpression Value { get; set; }

        public PutsExpression(IExpression value)
        {
            Value = value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            return Value.Print();
        }
    }
}
