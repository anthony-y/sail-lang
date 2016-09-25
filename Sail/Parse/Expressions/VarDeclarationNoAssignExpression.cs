using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class VarDeclarationNoAssignExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public string Name { get; set; }
        public SailType Type { get; set; }

        public VarDeclarationNoAssignExpression(int line, int column, string name, SailType type)
        {
            Name = name;
            Type = type;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            //Console.WriteLine("Variable declaration (not assigned)");
            //Console.WriteLine("Type " + Type);
            //return Name;
            return "";
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
