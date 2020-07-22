using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.PerformanceTest.Test
{
    public class AlgorithmEngineTest_dateTime
    {
        [Benchmark]
        public void DATEVALUE_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEVALUE('2016-01-01')", DateTime.MinValue);
        }
        [Benchmark]
        public void TIMEVALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("TIMEVALUE('12:12:12')", TimeSpan.MinValue);
        }
        [Benchmark]
        public void DATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATE(2016,1,1)", DateTime.MinValue);
        }
        [Benchmark]
        public void time_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("time(11,12,13)", TimeSpan.MinValue);
        }
        [Benchmark]
        public void Now_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("now()", DateTime.MinValue);
        }
        [Benchmark]
        public void Today_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Today()", DateTime.MinValue);
        }
        [Benchmark]
        public void Year_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("year(now())", 0);
        }
        [Benchmark]
        public void Month_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Month(now())", 0);
        }
        [Benchmark]
        public void Day_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Day(now())", 0);
        }
        [Benchmark]
        public void Hour_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Hour(now())", 0);
        }
        [Benchmark]
        public void Minute_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Minute(now())", 0);
        }
        [Benchmark]
        public void Second_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Second(now())", 0);
        }
        [Benchmark]
        public void WEEKDAY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
        }

        [Benchmark]
        public void DATEDIF_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
        }
        [Benchmark]
        public void DATEDIF_Test_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
        }
        [Benchmark]
        public void DATEDIF_Test_3()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
        }
        [Benchmark]
        public void DATEDIF_Test_4()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0);
 

        }
        [Benchmark]
        public void DATEDIF_Test_5()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0);
        }
        [Benchmark]
        public void DATEDIF_Test_6()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0);
        }

        [Benchmark]
        public void DAYS360_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
        }
        [Benchmark]
        public void EDATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("EDATE(\"2012-1-31\",32)", DateTime.MinValue);
        }
        [Benchmark]
        public void EOMONTH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", DateTime.MinValue);
        }

        [Benchmark]
        public void NETWORKDAYS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
        }
        [Benchmark]
        public void WORKDAY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", DateTime.MinValue);
        }

        [Benchmark]
        public void WEEKNUM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
        }
        [Benchmark]
        public void WEEKNUM_test_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("WEEKNUM(\"2016-1-2\",2)", 0);
        }
    }
}
