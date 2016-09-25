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
        public int Line { get; set; }
        public int Column { get; set; }

        public BlockExpression IfBlock { get; set; }
        public ElseExpression ElseBlock { get; set; }

        public List<ElseIfExpression> ElseIfs { get; set; }

        public IExpression IfCondition { get; private set; }

        public IfExpression(int line, int column, BlockExpression ifBlock, IExpression ifCondition,
            List<ElseIfExpression> elseIfs, ElseExpression elseBlock = null)
        {
            IfBlock = ifBlock;
            ElseBlock = elseBlock;
            ElseIfs = elseIfs;
            IfCondition = ifCondition;

            Line = line;
            Column = column;
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
