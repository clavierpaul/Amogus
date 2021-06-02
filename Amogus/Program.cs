using System;
using System.IO;

namespace Amogus
{
    public static class Program
    {
        private static void Run(string program)
        {
            var interpreter = new Interpreter(program);
            try
            {
                interpreter.Execute();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
            }
        }
        
        public static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("ERROR: No file supplied");
                    Console.WriteLine("amogus <file>");
                    break;
                case 1:
                    Run(File.ReadAllText(args[0]).TrimEnd());
                    break;
                default:
                    Console.WriteLine("ERROR: Invalid usage");
                    Console.WriteLine("amogus <file>");
                    break;
            }
        }
    }
}