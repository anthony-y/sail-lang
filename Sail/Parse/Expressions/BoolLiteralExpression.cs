using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class BoolLiteralExpression
        : IExpression, ILiteralExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public bool Value { get; set; }

        public BoolLiteralExpression(int line, int column, string strValue)
        {
            Value = strValue == "true" ? true : false;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            //Console.WriteLine("Boolean literal: " + Value.ToString());
            return Value.ToString().ToLower();
        }

        public void Accept(IVisitor visit)
        {

        }

        public object GetValue()
        {
            return Value.ToString().ToLower();
        }
    }
}
