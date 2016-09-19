using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class FloatLiteralExpression
        : IExpression, ILiteralExpression
    {
        public float Value { get; set; }

        public FloatLiteralExpression(string strValue)
        {
            float value;
            float.TryParse(strValue, out value);

            Value = value;
        }

        public string Print()
        {
            //Console.WriteLine("Float literal: " + Value.ToString());
            return Value.ToString();
        }

        public void Accept(IVisitor visit)
        {

        }

        public object GetValue()
        {
            return Value;
        }
    }
}
