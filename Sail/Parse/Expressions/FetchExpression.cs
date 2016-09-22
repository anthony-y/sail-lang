using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class FetchExpression
        : IExpression
    {
        public string FileName { get; private set; }

        public FetchExpression(string fileName)
        {
            FileName = fileName;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            return FileName;
        }
    }
}
