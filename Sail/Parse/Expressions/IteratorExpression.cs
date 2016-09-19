using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class IteratorExpression
        : IExpression
    {
        public int? LowerBound { get; private set; }
        public int? UpperBound { get; private set; }

        public string VariableLower { get; private set; }
        public string VariableUpper { get; private set; }

        public IteratorExpression(int? lowerBound, int? upperBound, string variableLower, string variableUpper)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;

            VariableLower = variableLower;
            VariableUpper = variableUpper;
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
