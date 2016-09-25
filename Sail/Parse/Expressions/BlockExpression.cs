using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class BlockExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public List<IExpression> Expressions { get; private set; }
        public FunctionExpression Owner { get; set; }

        public BlockExpression(int line, int column, List<IExpression> expressions)
        {
            Expressions = expressions;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            //if (!Expressions.Any())
            //    return "Block";

            //Console.WriteLine("BLOCK");
            //Console.WriteLine("-----");

            //foreach (var ex in Expressions)
            //    ex.Print();

            return "Block";
        }

        public void Accept(IVisitor visit)
        {
            visit.Visit(this);
        }
    }
}