using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class MathsExpression
        : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public TokenType OperatorType { get; private set; }

        public MathsExpression(IExpression left, IExpression right, TokenType operatorType)
        {
            Left = left;
            Right = right;

            OperatorType = operatorType;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            // Fix me
            return "";
        }
    }
}
