using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.PerformanceTest.Test
{
    public class AlgorithmEngineTest
    {
        [Benchmark]
        public void Test_Add()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var c = engine.TryEvaluate("2+3", 0);
        }
        [Benchmark]
        public void Test_Sub()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var c = engine.TryEvaluate("2-3", 0);
        }

        [Benchmark]
        public void Test_Mul()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var c = engine.TryEvaluate("2*3", 0);
        }
        [Benchmark]
        public void Test_Div()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var c = engine.TryEvaluate("2/3", 0);
        }


    }
}
