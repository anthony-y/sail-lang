using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class FunctionCallExpression
        : IExpression
    {
        public string FunctionName { get; private set; }
        public List<IExpression> Parameters { get; private set; }

        public FunctionCallExpression(string name, List<IExpression> parameters)
        {
            FunctionName = name;
            Parameters = parameters;
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
