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
        public int Line { get; set; }
        public int Column { get; set; }

        public string TypeName { get; private set; }

        public TypeNameExpression(int line, int column, string typeName)
        {
            TypeName = typeName;

            Line = line;
            Column = column;
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
