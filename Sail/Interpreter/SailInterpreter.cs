using System;
using System.Collections.Generic;
using System.Linq;

using Sail.Parse;
using Sail.Parse.Expressions;

namespace Sail.Interpreter
{
    internal class SailInterpreter
        : IVisitor
    {
        private List<SailObject> _variables;
        private List<SailFunction> _functions;

        private Stack<SailObject> _functionVariables;
        private Stack<SailObject> _returnValues;

        private Stack<SailType> _typeOfTypes;

        public SailInterpreter()
        {
            _variables = new List<SailObject>();
            _functions = new List<SailFunction>();

            _functionVariables = new Stack<SailObject>();
            _returnValues = new Stack<SailObject>();
            _typeOfTypes = new Stack<SailType>();
        }

        public void Visit(BlockExpression block)
        {
            if (block.Owner != null)
            {
                if (_functionVariables.Count != block.Owner.Params.Count)
                    throw new Exception("Wrong amount of arguments passed to function!");

                for (int i = block.Owner.Params.Count - 1; i >= 0; i--)
                {
                    var param = _functionVariables.Pop();
                    var current = block.Owner.Params[i];

                    if (current.Type != param.Type)
                        throw new Exception("Type mismatch in function! Expected " + block.Owner.Params[i].Type + " but got " + param.Type);

                    _variables.Add(new SailObject(current.Name, param.Value, param.Type));
                }
            }

            foreach (var ex in block.Expressions)
                Visit(ex);
        }

        public void Visit(FunctionExpression func)
        {
            _functions.Add(new SailFunction(func));

            if (func.Name == "main" && !func.Params.Any() && func.ReturnTypes.Count == 1 && func.ReturnTypes[0] == SailReturnType.VOID)
                Visit(func.Block);
        }

        public void Visit(FunctionCallExpression funcCall)
        {
            var funcToCall = _functions.FirstOrDefault(f => f.Name == funcCall.FunctionName);

            foreach (var v in funcCall.Parameters)
            {
                object value = null;
                SailType type = SailType.UNKNOWN;

                if (v is IdentifierExpression)
                {
                    var variable = _variables.FirstOrDefault(va => va.Name == (v as IdentifierExpression).Value);

                    if (variable == null)
                        throw new Exception("Argument passed to function doesn't exist!");

                    value = variable.Value;
                    type = variable.Type;
                }

                else if (v is ILiteralExpression)
                {
                    var literal = v as ILiteralExpression;

                    type = TypeResolver.ToSailType(literal);
                    value = literal.GetValue();
                }

                else throw new Exception("Argument passed to function must be variable name or value!");

                _functionVariables.Push(new SailObject(null, value, type));
            }

            if (funcCall != null) funcToCall.Invoke(this);
        }

        public void Visit(PrintExpression print)
        {
            if (print.Value is ILiteralExpression)
            {
                var value = ((ILiteralExpression)print.Value).GetValue();

                Console.WriteLine(value);
            }

            else if (print.Value is IdentifierExpression)
            {
                string varName = ((IdentifierExpression)print.Value).Value;

                var variable = _variables.FirstOrDefault(var => var.Name == varName);
                if (variable != null)

                    if (variable.Value == null)
                        Console.WriteLine("null");

                    else Console.WriteLine(variable.Value);
            }

            else if (print.Value is FunctionCallExpression)
            {
                Visit(print.Value);

                var value = _returnValues.Pop();
                Console.WriteLine(value.Value);
            }

            else Console.WriteLine(print.Value.Print());
        }

        public void Visit(PutsExpression puts)
        {
            if (puts.Value is ILiteralExpression)
            {
                var value = ((ILiteralExpression)puts.Value).GetValue();

                Console.Write(value);
            }

            else if (puts.Value is IdentifierExpression)
            {
                string varName = ((IdentifierExpression)puts.Value).Value;

                var variable = _variables.FirstOrDefault(var => var.Name == varName);
                if (variable != null)

                    if (variable.Value == null)
                        Console.Write("null");

                    else Console.Write(variable.Value);
            }

            else if (puts.Value is FunctionCallExpression)
            {
                Visit(puts.Value);

                var value = _returnValues.Pop();
                Console.Write(value.Value);
            }

            else Console.Write(puts.Value.Print());
        }

        public void Visit(VarDeclarationNoAssignExpression vardec)
        {
            _variables.Add(new SailObject(vardec.Name, null, vardec.Type));
        }

        public void Visit(AssignmentExpression assignment)
        {
            var literal = assignment.Value as ILiteralExpression;
            string varName = (assignment.Name as IdentifierExpression).Value;

            var variable = _variables.FirstOrDefault(var => var.Name == varName);

            object varValue = null;

            if (variable == null)
            {
                var specificValue = TypeResolver.ToSpecificLiteral(assignment.Value as ILiteralExpression);

                if (assignment.Value is FunctionCallExpression)
                {
                    string functionName = (assignment.Value as FunctionCallExpression).FunctionName;
                    var function = _functions.FirstOrDefault(f => f.Name == functionName);

                    if (function == null)
                        throw new Exception("Tried to assign to function that doesn't exist!");

                    // Call the function
                    Visit(assignment.Value);

                    if (!_returnValues.Any())
                        throw new Exception("Attempted to assign to function that doesn't return anything!");

                    var returnVal = _returnValues.Pop();
                    varValue = returnVal.Value;

                    var funcReturnType = TypeResolver.ToSailType(function.ReturnTypes[0]);

                    // DIRTY HACK
                    if (assignment.ExpectedType == SailType.UNKNOWN)
                        assignment.ExpectedType = funcReturnType;

                    if (funcReturnType != assignment.ExpectedType)
                        throw new Exception("Type mismatch in function return assignment! Expected " + assignment.ExpectedType + " but got " + funcReturnType);

                    assignment.Type = TypeResolver.ToSailType(function.ReturnTypes[0].ToString());

                } else varValue = specificValue.GetValue();

                _variables.Add(new SailObject(varName, varValue, assignment.Type));
            } else
            {
                if (variable.Type != TypeResolver.ToSailType(literal))
                    throw new Exception("Attempted to reassign to variable of different type than given value!");

                variable.Value = literal?.GetValue();
            }
        }

        public void Visit(ReturnExpression returnExpr)
        {
            if (returnExpr.Value is ILiteralExpression)
            {
                var literal = returnExpr.Value as ILiteralExpression;
                var type = TypeResolver.ToSailType(literal);

                _returnValues.Push(new SailObject(null, literal.GetValue(), type));

            } else if (returnExpr.Value is IdentifierExpression)
            {
                string varName = (returnExpr.Value as IdentifierExpression).Value;
                var variable = _variables.FirstOrDefault(va => va.Name == varName);

                if (variable == null)
                    throw new Exception("Can't return non-existant variable!");

                _returnValues.Push(new SailObject(null, variable.Value, variable.Type));
            }
        }

        public void Visit(TypeOfExpression typeOfExpr)
        {
            //_typeOfTypes.Push()
        }

        public void Visit(IfExpression ifExpr)
        {
            bool boolVal = false;

            if (ifExpr.IfCondition is ILiteralExpression)
            {
                var val = ((ILiteralExpression)ifExpr.IfCondition).GetValue();
                boolVal = Convert.ToBoolean(val);

                if (boolVal)
                    Visit(ifExpr.IfBlock);
            }

            else if (ifExpr.IfCondition is IdentifierExpression)
            {
                string name = ((IdentifierExpression)ifExpr.IfCondition).Value;

                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                    throw new Exception("Variable not found in if condition!");

                if (variable.Value == null)
                    throw new Exception("Cannot compare null object");

                boolVal = Convert.ToBoolean(variable.Value);

                if (boolVal)
                    Visit(ifExpr.IfBlock);

                if (ifExpr.ElseBlock != null && !boolVal) Visit(ifExpr.ElseBlock.Block);
            }

            BlockExpression elseIfBlock = null;
            foreach (var elseIf in ifExpr.ElseIfs)
            {
                object varValue = null;

                if (elseIf.Condition is IdentifierExpression)
                {
                    string name = ((IdentifierExpression)elseIf.Condition).Value;

                    var variable = _variables.FirstOrDefault(v => v.Name == name);

                    if (variable == null)
                        throw new Exception("Variable not found in if condition!");

                    if (variable.Value == null)
                        throw new Exception("Cannot compare null object");

                    varValue = variable.Value;
                }

                var elseIfValAsBool = Convert.ToBoolean(varValue);
                if (elseIfValAsBool && !boolVal)
                {
                    elseIfBlock = elseIf.Block;
                    break;
                }
            }

            if (elseIfBlock != null && !boolVal) { Visit(elseIfBlock); return; }
            if (elseIfBlock == null && !boolVal) Visit(ifExpr.ElseBlock);
        }

        public void Visit(ForExpression forExpr)
        {
            int lowerBound = 0;
            int upperBound = 0;

            if (forExpr.Iterator != null)
            {
                if (forExpr.Iterator.VariableLower != null)
                {
                    var lowerVariable = _variables.FirstOrDefault(v => v.Name == forExpr.Iterator.VariableLower);
                    if (lowerVariable == null)
                        throw new Exception("Cannot iterate over non-existant list");

                    // TODO (anthony) : add checking to make sure the variable is actually a list, when lists are actually implemented.

                    if (!(lowerVariable.Value is int))
                        throw new Exception("Lower bound variable is not a number");

                    lowerBound = (int)lowerVariable.Value;
                }

                if (forExpr.Iterator.VariableUpper != null)
                {
                    var upperVariable = _variables.FirstOrDefault(v => v.Name == forExpr.Iterator.VariableUpper);
                    if (upperVariable == null)
                        throw new Exception("Cannot iterate over non-existant list");

                    // TODO (anthony) : add checking to make sure the variable is actually a list, when lists are actually implemented.

                    if (!(upperVariable.Value is int))
                        throw new Exception("Upper bound variable is not a number");

                    upperBound = (int)upperVariable.Value;
                }

                if (forExpr.Iterator.LowerBound != null)
                    lowerBound = (int)forExpr.Iterator.LowerBound;

                if (forExpr.Iterator.UpperBound != null)
                    upperBound = (int)forExpr.Iterator.UpperBound;
            }

            _variables.Add(new SailObject("it", lowerBound, SailType.INT));
            var it = _variables.FirstOrDefault(v => v.Name == "it");

            for (float iter = lowerBound; iter <= upperBound; iter++)
            {
                it.Value = iter;

                Visit(forExpr.Block);
            }
        }

        public void Visit(IExpression expr)
        {
            if (expr is AssignmentExpression)             Visit((AssignmentExpression)expr);
            if (expr is VarDeclarationNoAssignExpression) Visit((VarDeclarationNoAssignExpression)expr);
            if (expr is BlockExpression)                  Visit((BlockExpression)expr);
            if (expr is PrintExpression)                  Visit((PrintExpression)expr);
            if (expr is FunctionExpression)               Visit((FunctionExpression)expr);
            if (expr is FunctionCallExpression)           Visit((FunctionCallExpression)expr);
            if (expr is PutsExpression)                   Visit((PutsExpression)expr);
            if (expr is ReturnExpression)                 Visit((ReturnExpression)expr);
            if (expr is TypeOfExpression)                 Visit((TypeOfExpression)expr);
            if (expr is IfExpression)                     Visit((IfExpression)expr);
            if (expr is ForExpression)                    Visit((ForExpression)expr);
        }

        public void InvokeFunction(string name)
        {
            _functions.FirstOrDefault(f => f.Name == name).Invoke(this);
        }
    }
}