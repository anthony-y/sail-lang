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
        public BlockExpression Block { get; set; }
        public IExpression Condition { get; set; }

        public ElseIfExpression(BlockExpression block, IExpression condition)
        {
            Block = block;
            Condition = condition;
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
