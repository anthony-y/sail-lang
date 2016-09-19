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
        public bool Value { get; set; }

        public BoolLiteralExpression(string strValue)
        {
            Value = strValue == "true" ? true : false;
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
