using System;
using System.Collections.Generic;

namespace Sail.Error
{
    internal static class ErrorManager
    {
        private static List<SailError> _errors = new List<SailError>();

        public static void CreateError(string message, ErrorType type = ErrorType.Error, int line = 0, int column = 0) 
            => _errors.Add(new SailError(line, column, type, message));

        public static bool ShouldPrintErrors() => _errors.Count > 0;

        public static void PrintErrors()
        {
            foreach (var err in _errors)
                Console.WriteLine($"Sail {err.Type} ({err.Line}, {err.Column}): {err.Message}");
        }
    }
}
