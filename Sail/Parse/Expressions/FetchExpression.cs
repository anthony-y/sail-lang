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
        public int Line { get; set; }
        public int Column { get; set; }

        public string FileName { get; private set; }

        public FetchExpression(int line, int column, string fileName)
        {
            FileName = fileName;

            Line = line;
            Column = column;
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
