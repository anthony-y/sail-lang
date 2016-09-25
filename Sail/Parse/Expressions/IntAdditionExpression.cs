using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Parse.Expressions
{
    internal class IntAdditionExpression
    {
        public int Left { get; set; }
        public int Right { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }
    }
}
