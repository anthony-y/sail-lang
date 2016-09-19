using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse.Parsers
{
    internal class TypeOfParser
        : IPrefixParser
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var obj = parser.ParseExpression(1);

            string variableName = null;
            object value = null;

            if (obj is IdentifierExpression)
                variableName = ((IdentifierExpression)obj).Value;

            else if (obj is ILiteralExpression)
                value = ((ILiteralExpression)obj).GetValue();

            else throw new Exception("Can only find types of variables or literals!");

            return new TypeOfExpression(variableName, value);
        }
    }
}
