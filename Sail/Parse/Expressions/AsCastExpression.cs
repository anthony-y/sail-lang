using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class AsCastExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public AsCastExpression(IExpression left, SailType type)
        {

        }

        public string Print()
        {
            throw new NotImplementedException();
        }

        public void Accept(IVisitor visit)
        {

        }
    }
}
