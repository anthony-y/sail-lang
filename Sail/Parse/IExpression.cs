using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Interpreter;

namespace Sail.Parse
{
    internal interface IExpression
    {
        void Accept(IVisitor visitor);
        string Print();
    }
}
