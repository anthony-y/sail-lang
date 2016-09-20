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
        public List<ElseIfExpression> ElseIfs { get; set; }

        public IExpression IfCondition { get; private set; }

        public IfExpression(BlockExpression ifBlock, IExpression ifCondition,
            List<ElseIfExpression> elseIfs, ElseExpression elseBlock = null)
        {
            IfBlock = ifBlock;
            ElseBlock = elseBlock;
            ElseIfs = elseIfs;
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
