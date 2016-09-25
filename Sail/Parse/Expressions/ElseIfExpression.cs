using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class ElseIfExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public BlockExpression Block { get; set; }
        public IExpression Condition { get; set; }

        public ElseIfExpression(int line, int column, BlockExpression block, IExpression condition)
        {
            Block = block;
            Condition = condition;

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
