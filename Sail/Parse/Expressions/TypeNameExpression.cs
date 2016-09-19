using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class TypeNameExpression
        : IExpression
    {
        public string TypeName { get; private set; }

        public TypeNameExpression(string typeName)
        {
            TypeName = typeName;
        }

        public string Print()
        {
            //Console.WriteLine("Typename: " + TypeName);
            return TypeName;
        }

        public void Accept(IVisitor visitor)
        {
        
        }
    }
}
