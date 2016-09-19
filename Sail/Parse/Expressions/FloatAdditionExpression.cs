using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Parse.Expressions
{
    internal class FloatAdditionExpression
    {
        public float Left { get; set; }
        public float Right { get; set; }

        public FloatAdditionExpression(float left, float right)
        {
            Left = left;
            Right = right;
        }
    }
}
