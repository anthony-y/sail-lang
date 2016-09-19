using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse.Expressions;
using Sail.Parse;

namespace Sail.Interpreter
{
    internal class SailObject
    {
        public object Value { get; set; }
        public SailType Type { get; set; }
        public string Name { get; set; }

        public SailObject(string name, object value, SailType type)
        {
            Value = value;
            Type = type;
            Name = name;
        }
    }
}
