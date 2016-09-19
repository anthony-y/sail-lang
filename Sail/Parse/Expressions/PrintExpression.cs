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
        public IExpression Value { get; set; }

        public PrintExpression(IExpression value)
        {
            Value = value;
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
