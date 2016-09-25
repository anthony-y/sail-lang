using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class ElseExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public BlockExpression Block { get; set; }

        public ElseExpression(int line, int column, BlockExpression block)
        {
            Block = block;

            Line = line;
            Column = column;
        }

        public void Accept(IVisitor visitor)
        {
            
        }

        public string Print()
        {
            return "";
        }
    }
}
