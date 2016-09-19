using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sail.Lexical;
using Sail.Parse.Expressions;

namespace Sail.Parse
{
    internal class TypeResolver
    {
        public static SailType ToSailType(string expr)
        {
            switch (expr)
            {
                case "str":
                    return SailType.STR;
                case "int":
                    return SailType.INT;
                case "float":
                    return SailType.FLOAT;
                case "bool":
                    return SailType.BOOL;
            }

            return SailType.UNKNOWN;
        }

        public static SailReturnType ToReturnType(TypeNameExpression expr)
        {
            switch (expr.TypeName)
            {
                case "str":
                    return SailReturnType.STR;
                case "bool":
                    return SailReturnType.BOOL;
                case "int":
                    return SailReturnType.INT;
                case "float":
                    return SailReturnType.FLOAT;
                case "void":
                    return SailReturnType.VOID;
            }

            return SailReturnType.UNKNOWN;
        }

        public static SailType ToSailType(Token token)
        {
            return ToSailType(token.Value);
        }

        public static SailType ToSailType(TypeNameExpression expr)
        {
            return ToSailType(expr.TypeName);
        }

        public static SailType ToSailType(SailReturnType type)
        {
            return ToSailType(type.ToString().ToLower());
        }

        public static SailType ToSailType(ILiteralExpression expr)
        {
            if (expr is StringLiteralExpression)
                return SailType.STR;
            else if (expr is IntLiteralExpression)
                return SailType.INT;
            else if (expr is FloatLiteralExpression)
                return SailType.FLOAT;
            else if (expr is BoolLiteralExpression)
                return SailType.BOOL;

            return SailType.UNKNOWN;
        }

        public static T ToSpecificLiteral<T>(T expr)
            where T : ILiteralExpression
        {
            return expr;
        }

        public static bool IsValidType(Token token)
        {
            return (ToSailType(token) != SailType.UNKNOWN);
        }
    }
}
