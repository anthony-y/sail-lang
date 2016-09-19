using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Parse.Expressions
{
    interface ILiteralExpression
    {
        object GetValue();
    }
}
