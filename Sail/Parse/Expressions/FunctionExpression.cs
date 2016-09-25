using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sail.Interpreter;

namespace Sail.Parse.Expressions
{
    internal class FunctionExpression
        : IExpression
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public string Name { get; set; }

        public List<VarDeclarationNoAssignExpression> Params { get; set; }
        public List<SailReturnType> ReturnTypes { get; set; }
        public BlockExpression Block { get; set; }

        public FunctionExpression(int line, int column, string name, List<VarDeclarationNoAssignExpression> parames, List<SailReturnType> returnType, BlockExpression block)
        {
            Name = name;
            Params = parames;
            Block = block;
            ReturnTypes = returnType;

            Block.Owner = this;

            Line = line;
            Column = column;
        }

        public string Print()
        {
            Console.WriteLine("Function declaration: " + Name);
            Console.WriteLine("Params: ");
            foreach (var p in Params)
            {
                Console.WriteLine("Param");
                Console.WriteLine("-----");

                p.Print();

                Console.WriteLine("");
            }

            //Console.WriteLine("FUNCTION " + Name);

            return Name + "gay";
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
