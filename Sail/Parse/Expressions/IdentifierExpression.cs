using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class IdentifierExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public string Value { get; set; }

        public IdentifierExpression(int line, int column, string name)
        {
            Value = name;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            //Console.WriteLine("Identifier: " + Value);
            return Value;
        }

        public void Accept(IVisitor visit)
        {

        }
    }
}