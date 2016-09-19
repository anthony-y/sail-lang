using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class TypeOfExpression
        : IExpression
    {
        public string VariableName { get; private set; }
        public object Value { get; private set; }

        public TypeOfExpression(string variableName, object value)
        {
            VariableName = variableName;
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
