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
        public int Line { get; set; }
        public int Column { get; set; }

        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public TokenType TokenType { get; private set; }

        public ComparisonExpression(int line, int column, IExpression left, IExpression right, TokenType tokenType)
        {
            TokenType = tokenType;

            Left = left;
            Right = right;

            Line = line;
            Column = column;
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
