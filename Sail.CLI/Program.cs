using System;

using System.IO;

namespace Sail.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
                throw new Exception("Expected file name as first argument!");

            var run = new SailBackend(Path.Combine(Environment.CurrentDirectory, args[0]));
        }
    }
}
