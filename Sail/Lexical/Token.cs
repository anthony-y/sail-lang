using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Lexical
{
    internal class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }

        internal Token(TokenType type, string value, int line, int column)
        {
            Type = type;

            Value = value;

            Line = line;
            Column = column;
        }
    }
}
