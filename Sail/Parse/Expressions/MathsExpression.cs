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
        public int Line { get; set; }
        public int Column { get; set; }

        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public TokenType OperatorType { get; private set; }

        public MathsExpression(int line, int column, IExpression left, IExpression right, TokenType operatorType)
        {
            Left = left;
            Right = right;

            OperatorType = operatorType;

            Line = line;
            Column = column;
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
