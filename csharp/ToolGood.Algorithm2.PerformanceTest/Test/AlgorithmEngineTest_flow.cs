using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.PerformanceTest.Test
{
    class AlgorithmEngineTest_flow
    {
        [Benchmark]
        public void If_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(1=1,1,2)", 0);
        }

        [Benchmark]
        public void iferror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("iferror(1/0,1,2)", 0);
        }

        [Benchmark]
        public void iferror_test_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("iferror(1-'rrr',1,2)", 0);
        }

        [Benchmark]
        public void iserror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("iserror(1/0,1)", 0);
        }
        [Benchmark]
        public void ifnull_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("isnull(null,1)", 0);
        }

        [Benchmark]
        public void isnullorerror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("isnullorerror(null,1)", 0);
        }



        [Benchmark]
        public void ISNUMBER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0);
        }

        [Benchmark]
        public void ISTEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISTEXT(1),1,2)", 0);
        }


        [Benchmark]
        public void ISNONTEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0);
        }


        [Benchmark]
        public void ISLOGICAL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0);
        }

        [Benchmark]
        public void ISEVEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISEVEN(1),1,2)", 0);
        }

        [Benchmark]
        public void ISODD_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("if(ISODD(1),1,2)", 0);
        }

        [Benchmark]
        public void And_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("and(true(),1=1)", false);
        }

        [Benchmark]
        public void Or_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("or(true(),1=1)", false);
        }

        [Benchmark]
        public void Not_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("not(true())", false);
        }

    }
}
