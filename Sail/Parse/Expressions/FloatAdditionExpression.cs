using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Parse.Expressions
{
    internal class FloatAdditionExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public float Left { get; set; }
        public float Right { get; set; }

        public FloatAdditionExpression(int line, int column, float left, float right)
        {
            Left = left;
            Right = right;

            Line = line;
            Column = column;
        }
    }
}
