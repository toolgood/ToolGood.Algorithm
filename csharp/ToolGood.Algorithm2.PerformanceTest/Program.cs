using System;
using BenchmarkDotNet.Running;
using ToolGood.Algorithm;
using ToolGood.Algorithm2.PerformanceTest.Test;

namespace ToolGood.Algorithm2.PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                AlgorithmEngine engine = new AlgorithmEngine();
                var c = engine.TryEvaluate("2+3", 0);
            }

            //   BenchmarkRunner.Run<AlgorithmEngineTest>();
            BenchmarkRunner.Run<AlgorithmEngineTest_csharp>();
            BenchmarkRunner.Run<AlgorithmEngineTest_dateTime>();


        }
    }
}
