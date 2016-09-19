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
        public BlockExpression Block { get; set; }

        public ElseExpression(BlockExpression block)
        {
            Block = block;
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
