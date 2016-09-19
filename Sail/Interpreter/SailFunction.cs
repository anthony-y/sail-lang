using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse;
using Sail.Parse.Expressions;

namespace Sail.Interpreter
{
    internal class SailFunction
    {
        public List<SailReturnType> ReturnTypes { get; set; }
        public List<VarDeclarationNoAssignExpression> Parameters { get; set; }
        public string Name { get; set; }

        private BlockExpression _block;

        public SailFunction(FunctionExpression func)
        {
            Name = func.Name;
            ReturnTypes = func.ReturnTypes;
            Parameters = func.Params;

            _block = func.Block;
        }

        public void Invoke(IVisitor visitor)
        {
            _block.Accept(visitor);
        }
    }
}
