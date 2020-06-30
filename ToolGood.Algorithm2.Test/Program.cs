using System;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            //string input = @"1 + (2 - 3) * 4";
            string input = @"'abcd' & '123'";

            var b = engine.Parse(input);

            var r = engine.Evaluate();

            Console.WriteLine("Hello World!");
        }
    }
}
