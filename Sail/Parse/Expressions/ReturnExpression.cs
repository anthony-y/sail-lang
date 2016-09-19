using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class ReturnExpression
        : IExpression
    {
        public IExpression Value { get; private set; }

        public ReturnExpression(IExpression value)
        {
            Value = value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            return "";
        }
    }
}