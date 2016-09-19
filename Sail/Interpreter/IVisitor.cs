using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Parse;
using Sail.Parse.Expressions;

namespace Sail.Interpreter
{
    interface IVisitor
    {
        void Visit(AssignmentExpression assignment);
        void Visit(VarDeclarationNoAssignExpression vardec);
        void Visit(BlockExpression block);
        void Visit(FunctionCallExpression funcCall);
        void Visit(PrintExpression print);
        void Visit(PutsExpression puts);
        void Visit(ReturnExpression returnExpr);
        void Visit(FunctionExpression func);

        void Visit(IExpression expr);
    }
}
