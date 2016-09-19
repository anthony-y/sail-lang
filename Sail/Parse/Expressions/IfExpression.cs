using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class IfExpression
        : IExpression
    {
        public BlockExpression IfBlock { get; set; }
        public ElseExpression ElseBlock { get; set; }
        public Dictionary<IExpression, BlockExpression> ElseIfBlocks { get; set; }

        public IExpression IfCondition { get; private set; }

        public IfExpression(BlockExpression ifBlock, IExpression ifCondition, 
            Dictionary<IExpression, BlockExpression> elseIfBlocks, ElseExpression elseBlock = null)
        {
            IfBlock = ifBlock;
            ElseBlock = elseBlock;
            ElseIfBlocks = elseIfBlocks;
            IfCondition = ifCondition;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            return "";
        }
    }
}
