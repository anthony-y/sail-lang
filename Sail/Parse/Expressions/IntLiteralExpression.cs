using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class IntLiteralExpression
        : IExpression, ILiteralExpression
    {
        public int Value { get; set; }

        public IntLiteralExpression(string strValue)
        {
            int value;
            int.TryParse(strValue, out value);

            Value = value;
        }

        public string Print()
        {
            //Console.WriteLine("Int literal: " + Value.ToString());
            return Value.ToString();
        }

        public void Accept(IVisitor visitor)
        {
            
        }

        public object GetValue()
        {
            return Value;
        }

        public static implicit operator IntLiteralExpression(FloatLiteralExpression v)
        {
            throw new NotImplementedException();
        }
    }
}
