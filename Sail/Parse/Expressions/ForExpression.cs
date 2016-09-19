using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class ForExpression
        : IExpression
    {
        public IteratorExpression Iterator { get; private set; }
        public BlockExpression Block { get; private set; }
        public string ListName { get; private set; }

        public ForExpression(IteratorExpression iterator, string listName, BlockExpression block)
        {
            Iterator = iterator;
            ListName = listName;

            Block = block;
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
