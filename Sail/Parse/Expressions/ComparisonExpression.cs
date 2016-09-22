using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class ComparisonExpression
        : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public TokenType TokenType { get; private set; }

        public ComparisonExpression(IExpression left, IExpression right, TokenType tokenType)
        {
            TokenType = tokenType;

            Left = left;
            Right = right;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Print()
        {
            // Replace
            return "";
        }
    }
}
