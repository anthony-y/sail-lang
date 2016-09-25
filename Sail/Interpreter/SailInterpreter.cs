using System;
using System.Collections.Generic;
using System.Linq;

using Sail.Lexical;
using Sail.Parse;
using Sail.Parse.Expressions;
using Sail.Error;

namespace Sail.Interpreter
{
    internal class SailInterpreter
        : IVisitor
    {
        private List<SailObject> _variables;
        private List<SailFunction> _functions;

        private Stack<SailObject> _functionVariables;
        private Stack<SailObject> _returnValues;
        private Stack<SailObject> _comparisonResults;
        private Stack<SailObject> _mathsResults;

        private Stack<SailType> _typeOfTypes;

        private List<string> _usedFiles;

        public string RelativePathPrefix { get; private set; }

        public SailInterpreter(string relativePathPrefix)
        {
            _variables = new List<SailObject>();
            _functions = new List<SailFunction>();

            _functionVariables = new Stack<SailObject>();
            _returnValues = new Stack<SailObject>();
            _typeOfTypes = new Stack<SailType>();
            _usedFiles = new List<string>();
            _comparisonResults = new Stack<SailObject>();
            _mathsResults = new Stack<SailObject>();

            RelativePathPrefix = relativePathPrefix;
        }

        public void Visit(BlockExpression block)
        {
            if (block.Owner != null)
            {
                if (_functionVariables.Count != block.Owner.Params.Count)
                {
                    ErrorManager.CreateError("Wrong amount of arguments passed to function!", ErrorType.Error, block.Line, block.Column);
                    return;
                }

                for (int i = block.Owner.Params.Count - 1; i >= 0; i--)
                {
                    var param = _functionVariables.Pop();
                    var current = block.Owner.Params[i];

                    if (current.Type != param.Type)
                    {
                        ErrorManager.CreateError("Type mismatch in function! Expected " + block.Owner.Params[i].Type + " but got " + param.Type, ErrorType.Error, block.Line, block.Column);
                        return;
                    }

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
            _functionVariables.Clear();

            var funcToCall = _functions.FirstOrDefault(f => f.Name == funcCall.FunctionName);

            foreach (var v in funcCall.Parameters)
            {
                object value = null;
                SailType type = SailType.UNKNOWN;

                if (v is IdentifierExpression)
                {
                    var variable = _variables.FirstOrDefault(va => va.Name == (v as IdentifierExpression).Value);

                    if (variable == null)
                    {
                        ErrorManager.CreateError("Argument passed to function doesn't exist!", ErrorType.Error, funcCall.Line, funcCall.Column);
                        return;
                    }

                    value = variable.Value;
                    type = variable.Type;
                }

                else if (v is ILiteralExpression)
                {
                    var literal = v as ILiteralExpression;

                    type = TypeResolver.ToSailType(literal);
                    value = literal.GetValue();
                }

                else if (v is ComparisonExpression)
                {
                    Visit(v);

                    type = SailType.BOOL;
                    value = (bool)_comparisonResults.Pop().Value;
                }

                else
                {
                    ErrorManager.CreateError("Argument passed to function must be a variable name or value!", ErrorType.Error, funcCall.Line, funcCall.Column);
                    return;
                }

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

            else if (puts.Value is MathsExpression)
            {
                Visit(puts.Value);

                var value = _mathsResults.Pop().Value;
                Console.Write(value);
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
                var specificValue = assignment.Value as ILiteralExpression;

                if (assignment.Value is FunctionCallExpression)
                {
                    string functionName = (assignment.Value as FunctionCallExpression).FunctionName;
                    var function = _functions.FirstOrDefault(f => f.Name == functionName);

                    if (function == null)
                    {
                        ErrorManager.CreateError("Tried to assign to function that doesn't exist!", ErrorType.Error, assignment.Line, assignment.Column);
                        return;
                    }

                    // Call the function
                    Visit(assignment.Value);

                    if (!_returnValues.Any())
                    {
                        ErrorManager.CreateError("Attempted to assign to function that doesn't return anything!", ErrorType.Error, assignment.Line, assignment.Column);
                        return;
                    }

                    var returnVal = _returnValues.Pop();
                    varValue = returnVal.Value;

                    var funcReturnType = TypeResolver.ToSailType(function.ReturnTypes[0]);

                    // DIRTY HACK
                    if (assignment.ExpectedType == SailType.UNKNOWN)
                        assignment.ExpectedType = funcReturnType;

                    if (funcReturnType != assignment.ExpectedType)
                    {
                        ErrorManager.CreateError("Type mismatch in function return assignment! Expected " + assignment.ExpectedType + " but got " + funcReturnType, ErrorType.Error, assignment.Line, assignment.Column);
                        return;
                    }

                    assignment.Type = TypeResolver.ToSailType(function.ReturnTypes[0].ToString());

                } else if (assignment.Value is ComparisonExpression)
                {
                    Visit(assignment.Value);

                    varValue = (bool)_comparisonResults.Pop().Value;
                }

                else varValue = specificValue.GetValue();

                _variables.Add(new SailObject(varName, varValue, assignment.Type));
            } else
            {
                if (variable.Type != TypeResolver.ToSailType(literal))
                {
                    ErrorManager.CreateError("Attempted to reassign to variable of different type than given value!", ErrorType.Error, assignment.Line, assignment.Column);
                    return;
                }

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
                {
                    ErrorManager.CreateError("Can't return non-existant variable!", ErrorType.Error, returnExpr.Line, returnExpr.Column);
                    return;
                }

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
                object val = ((ILiteralExpression)ifExpr.IfCondition).GetValue();
                boolVal = Convert.ToBoolean(val);
            }

            else if (ifExpr.IfCondition is IdentifierExpression)
            {
                string name = ((IdentifierExpression)ifExpr.IfCondition).Value;

                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                {
                    ErrorManager.CreateError("Variable not found in if condition!", ErrorType.Error, ifExpr.Line, ifExpr.Column);
                    return;
                }

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Cannot compare null object!", ErrorType.Error, ifExpr.Line, ifExpr.Column);
                    return;
                }

                boolVal = Convert.ToBoolean(variable.Value);
            }

            else if (ifExpr.IfCondition is ComparisonExpression)
            {
                Visit(ifExpr.IfCondition);

                boolVal = (bool)_comparisonResults.Pop().Value;
            }

            BlockExpression elseIfBlock = null;
            bool elseIfValAsBool = false;

            foreach (var elseIf in ifExpr.ElseIfs)
            {
                if (elseIf.Condition is IdentifierExpression)
                {
                    string name = ((IdentifierExpression)elseIf.Condition).Value;

                    var variable = _variables.FirstOrDefault(v => v.Name == name);

                    if (variable == null)
                    {
                        ErrorManager.CreateError("Variable not found in else if condition!", ErrorType.Error, elseIf.Line, elseIf.Column);
                        return;
                    }

                    if (variable.Value == null)
                    {
                        ErrorManager.CreateError("Cannot compare null object!", ErrorType.Error, elseIf.Line, elseIf.Column);
                        return;
                    }

                    elseIfValAsBool = Convert.ToBoolean(variable.Value);
                }

                else if (elseIf.Condition is ILiteralExpression)
                {
                    object val = ((ILiteralExpression)elseIf.Condition).GetValue();
                    elseIfValAsBool = Convert.ToBoolean(val);
                }

                else if (elseIf.Condition is ComparisonExpression)
                {
                    Visit(ifExpr.IfCondition);

                    boolVal = (bool)_comparisonResults.Pop().Value;
                }

                if (elseIfValAsBool) { elseIfBlock = elseIf.Block; break; }
            }

            if (boolVal) Visit(ifExpr.IfBlock);
            else if (elseIfBlock != null && !boolVal) Visit(elseIfBlock);
            else if (ifExpr.ElseBlock != null && !boolVal) Visit(ifExpr.ElseBlock.Block);
        }

        public void Visit(ForExpression forExpr)
        {
            int lowerBound = 0;
            int upperBound = 0;

            object iteratee = null;

            if (forExpr.Iterator != null)
            {
                if (forExpr.Iterator.VariableLower != null)
                {
                    var lowerVariable = _variables.FirstOrDefault(v => v.Name == forExpr.Iterator.VariableLower);
                    if (lowerVariable == null)
                    {
                        ErrorManager.CreateError("Cannot iterate over non-existant list!", ErrorType.Error, forExpr.Iterator.Line, forExpr.Iterator.Column);
                        return;
                    }

                    // TODO (anthony) : add checking to make sure the variable is actually a list, when lists are actually implemented.

                    if (!(lowerVariable.Value is int))
                    {
                        ErrorManager.CreateError("Lower bound of for expression is not a number!", ErrorType.Error, forExpr.Iterator.Line, forExpr.Iterator.Column);
                        return;
                    }

                    lowerBound = (int)lowerVariable.Value;
                }

                if (forExpr.Iterator.VariableUpper != null)
                {
                    var upperVariable = _variables.FirstOrDefault(v => v.Name == forExpr.Iterator.VariableUpper);
                    if (upperVariable == null)
                    {
                        ErrorManager.CreateError("Cannot iterate over non-existant list!", ErrorType.Error, forExpr.Iterator.Line, forExpr.Iterator.Column);
                        return;
                    }

                    // TODO (anthony) : add checking to make sure the variable is actually a list, when lists are actually implemented.

                    if (!(upperVariable.Value is int))
                    {
                        ErrorManager.CreateError("Upper bound of for expression is not a number!", ErrorType.Error, forExpr.Iterator.Line, forExpr.Iterator.Column);
                        return;
                    }

                    upperBound = (int)upperVariable.Value;
                }

                if (forExpr.Iterator.LowerBound != null)
                    lowerBound = (int)forExpr.Iterator.LowerBound;

                if (forExpr.Iterator.UpperBound != null)
                    upperBound = (int)forExpr.Iterator.UpperBound;
            }

            else if (forExpr.Iterator == null && forExpr.ListName != null)
            {
                var variable = _variables.FirstOrDefault(v => v.Name == forExpr.ListName);

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Cannot iterate over non-null value!", ErrorType.Error, forExpr.Iterator.Line, forExpr.Iterator.Column);
                    return;
                }

                if (variable.Type == SailType.STR)
                    iteratee = (string)variable.Value;
            }

            if (iteratee == null)
            {
                _variables.Add(new SailObject("it", lowerBound, SailType.INT));
                var it = _variables.FirstOrDefault(v => v.Name == "it");

                for (float iter = lowerBound; iter <= upperBound; iter++)
                {
                    it.Value = iter;

                    Visit(forExpr.Block);
                }
            }

            else if (iteratee is string)
            {
                _variables.Add(new SailObject("it", iteratee, SailType.STR));
                var it = _variables.FirstOrDefault(v => v.Name == "it");

                foreach (char c in (string)iteratee)
                {
                    it.Value = c.ToString();

                    Visit(forExpr.Block);
                }
            }
        }

        public void Visit(FetchExpression fetch)
        {
            string fileName = RelativePathPrefix + fetch.FileName + ".sail";

            if (_usedFiles.Contains(fileName))
                return;

            var ast = new Parser(new Lexer(fileName)).Parse();

            _usedFiles.Add(fileName);

            foreach (var expr in ast)
                Visit(expr);
        }

        public void Visit(ComparisonExpression comparison)
        {
            object leftVal = null;
            object rightVal = null;

            // Left
            if (comparison.Left is IntLiteralExpression)
                leftVal = ((IntLiteralExpression)comparison.Left).Value;

            else if (comparison.Left is FloatLiteralExpression)
                leftVal = ((FloatLiteralExpression)comparison.Left).Value;

            else if (comparison.Left is IdentifierExpression)
            {
                string name = ((IdentifierExpression)comparison.Left).Value;
                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                {
                    ErrorManager.CreateError("Can't compare non-existant variable!", ErrorType.Error, comparison.Line, comparison.Column);
                    return;
                }

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Can't compare null valued variable!", ErrorType.Error, comparison.Line, comparison.Column);
                    return;
                }

                leftVal = variable.Value;
            }

            else if (comparison.Left is BoolLiteralExpression)
                leftVal = ((BoolLiteralExpression)comparison.Left).Value;

            // Right
            if (comparison.Right is IntLiteralExpression)
                rightVal = ((IntLiteralExpression)comparison.Right).Value;

            else if (comparison.Right is FloatLiteralExpression)
                rightVal = ((FloatLiteralExpression)comparison.Right).Value;

            else if (comparison.Right is IdentifierExpression)
            {
                string name = ((IdentifierExpression)comparison.Right).Value;
                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                {
                    ErrorManager.CreateError("Can't compare non-existant variable!", ErrorType.Error, comparison.Line, comparison.Column);
                    return;
                }

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Can't compare null valued variable!", ErrorType.Error, comparison.Line, comparison.Column);
                    return;
                }

                rightVal = variable.Value;
            }

            else if (comparison.Right is BoolLiteralExpression)
                rightVal = ((BoolLiteralExpression)comparison.Right).Value;

            if ((comparison.TokenType == TokenType.GREATERTHAN
            || comparison.TokenType == TokenType.LESSTHAN
            || comparison.TokenType == TokenType.GTHANEQUAL
            || comparison.TokenType == TokenType.LTHANEQUAL) && leftVal is string
            || rightVal is string)
            {
                ErrorManager.CreateError("You can only use maths comparisons on number or variables!", ErrorType.Error, comparison.Line, comparison.Column);
                return;
            }

            bool value = false;

            switch (comparison.TokenType)
            {
                case TokenType.GREATERTHAN:
                    value = Convert.ToSingle(leftVal) > Convert.ToSingle(rightVal);
                    break;

                case TokenType.LESSTHAN:
                    value = Convert.ToSingle(leftVal) < Convert.ToSingle(rightVal);
                    break;

                case TokenType.EQUALTO:
                    value = leftVal == rightVal;
                    break;

                case TokenType.NOTEQUALTO:
                    value = leftVal != rightVal;
                    break;

                case TokenType.GTHANEQUAL:
                    value = Convert.ToSingle(leftVal) >= Convert.ToSingle(rightVal);
                    break;

                case TokenType.LTHANEQUAL:
                    value = Convert.ToSingle(leftVal) <= Convert.ToSingle(rightVal);
                    break;
            }

            _comparisonResults.Push(new SailObject(null, value, SailType.BOOL));
        }

        public void Visit(MathsExpression maths)
        {
            object leftVal = null;
            object rightVal = null;

            SailType resultType = SailType.UNKNOWN;

            // Left
            if (maths.Left is IntLiteralExpression)
            {
                leftVal = ((IntLiteralExpression)maths.Left).Value;
                resultType = SailType.INT;
            }

            else if (maths.Left is FloatLiteralExpression)
            {
                leftVal = ((FloatLiteralExpression)maths.Left).Value;
                resultType = SailType.FLOAT;
            }

            else if (maths.Left is IdentifierExpression)
            {
                string name = ((IdentifierExpression)maths.Left).Value;
                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                {
                    ErrorManager.CreateError("Can't compare non-existant variable!", ErrorType.Error, maths.Left.Line, maths.Left.Column);
                    return;
                }

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Can't compare null valued variable!", ErrorType.Error, maths.Left.Line, maths.Left.Column);
                    return;
                }

                leftVal = variable.Value;

                resultType = variable.Type;
            }

            else if (maths.Left is MathsExpression)
            {
                Visit(maths.Left);

                var mathsValue = _mathsResults.Pop();

                rightVal = mathsValue.Value;
                resultType = mathsValue.Type;
            }

            // Right
            if (maths.Right is IntLiteralExpression)
            {
                rightVal = ((IntLiteralExpression)maths.Right).Value;
                resultType = SailType.INT;
            }

            else if (maths.Right is FloatLiteralExpression)
            {
                rightVal = ((FloatLiteralExpression)maths.Right).Value;
                resultType = SailType.FLOAT;
            }

            else if (maths.Right is IdentifierExpression)
            {
                string name = ((IdentifierExpression)maths.Right).Value;
                var variable = _variables.FirstOrDefault(v => v.Name == name);

                if (variable == null)
                {
                    ErrorManager.CreateError("Can't compare non-existant variable!", ErrorType.Error, maths.Right.Line, maths.Right.Column);
                    return;
                }

                if (variable.Value == null)
                {
                    ErrorManager.CreateError("Can't compare null valued variable!", ErrorType.Error, maths.Right.Line, maths.Right.Column);
                    return;
                }

                rightVal = variable.Value;

                resultType = variable.Type;
            }

            else if (maths.Right is MathsExpression)
            {
                Visit(maths.Right);

                var mathsValue = _mathsResults.Pop();

                rightVal = mathsValue.Value;
                resultType = mathsValue.Type;
            }

            object value = null;

            switch (maths.OperatorType)
            {
                case TokenType.PLUS:
                    value = Convert.ToSingle(leftVal) + Convert.ToSingle(rightVal);
                    break;

                case TokenType.MINUS:
                    value = Convert.ToSingle(leftVal) - Convert.ToSingle(rightVal);
                    break;

                case TokenType.ASTERISK:
                    value = Convert.ToSingle(leftVal) * Convert.ToSingle(rightVal);
                    break;

                case TokenType.FSLASH:
                    value = Convert.ToSingle(leftVal) / Convert.ToSingle(rightVal);
                    break;

                case TokenType.MODULO:
                    value = Convert.ToSingle(leftVal) % Convert.ToSingle(rightVal);
                    break;
            }

            _mathsResults.Push(new SailObject(null, value, resultType));
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
            if (expr is FetchExpression)                  Visit((FetchExpression)expr);
            if (expr is ComparisonExpression)             Visit((ComparisonExpression)expr);
            if (expr is MathsExpression)                  Visit((MathsExpression)expr);
        }

        public void InvokeFunction(string name)
        {
            _functions.FirstOrDefault(f => f.Name == name).Invoke(this);
        }
    }
}