using System;
using System.Diagnostics;

using Sail.Interpreter;
using Sail.Lexical;
using Sail.Parse;

namespace Sail
{
    public class SailBackend
    {
        private void Reload(string src)
        {
            var lexer = new Lexer(src);
            var parser = new Parser(lexer);
            var ast = parser.Parse();

            var interpreter = new SailInterpreter();

            var stopwatch = Stopwatch.StartNew();

            foreach (var node in ast)
                interpreter.Visit(node);

            stopwatch.Stop();

            Console.WriteLine("\nDone! (" + stopwatch.Elapsed.TotalMilliseconds + "ms)");
        }

        public SailBackend(string src)
        {
            Reload(src);

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.R)
                {
                    Console.Clear();
                    Reload(src);
                }

                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    break;
            }

            Console.ReadKey();
        }
    }
}
