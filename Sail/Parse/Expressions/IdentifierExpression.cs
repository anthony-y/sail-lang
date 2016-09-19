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
        public string Value { get; set; }

        public IdentifierExpression(string name)
        {
            Value = name;
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