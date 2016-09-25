using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class StringLiteralExpression
        : IExpression, ILiteralExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public string Value { get; set; }

        public StringLiteralExpression(int line, int column, string value)
        {
            Value = value;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            //Console.WriteLine("String literal: " + Value);
            return Value;
        }

        public void Accept(IVisitor visitor)
        {

        }

        public object GetValue()
        {
            return Value;
        }
    }
}
