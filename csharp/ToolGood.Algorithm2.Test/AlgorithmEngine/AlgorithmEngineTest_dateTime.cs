using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_dateTime
    {
        [Test]
        public void DATEVALUE_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));
            dt = engine.TryEvaluate("DATEVALUE（'2016-01-01'）", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));


        }
        [Test]
        public void TIMEVALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TIMEVALUE('12:12:12')", TimeSpan.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1, 12, 12, 12).TimeOfDay);
        }
        [Test]
        public void DATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2016,1,1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));
        }
        [Test]
        public void time_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("time(11,12,13)", TimeSpan.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1, 11, 12, 13).TimeOfDay);
        }
        [Test]
        public void Now_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("now()", DateTime.MinValue);
            Assert.AreEqual(dt.ToString(), DateTime.Now.ToString());
        }
        [Test]
        public void Today_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Today()", DateTime.MinValue);
            Assert.AreEqual(dt.ToString(), DateTime.Today.ToString());
        }
        [Test]
        public void Year_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("year(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Year);
        }
        [Test]
        public void Month_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Month(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Month);
        }
        [Test]
        public void Day_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Day(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Day);
        }
        [Test]
        public void Hour_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Hour(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Hour);
        }
        [Test]
        public void Minute_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Minute(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Minute);
        }
        [Test]
        public void Second_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Second(now())", 0);
            Assert.AreEqual(dt, DateTime.Now.Second);
        }
        [Test]
        public void WEEKDAY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7))", 0);
            Assert.AreEqual(dt, 7);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0);
            Assert.AreEqual(dt, 7);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0);
            Assert.AreEqual(dt, 6);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0);
            Assert.AreEqual(dt, 5);

            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0);
            Assert.AreEqual(dt, 1);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0);
            Assert.AreEqual(dt, 7);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0);
            Assert.AreEqual(dt, 6);

            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0);
            Assert.AreEqual(dt, 2);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0);
            Assert.AreEqual(dt, 1);
            dt = engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0);
            Assert.AreEqual(dt, 0);

        }

        [Test]
        public void DATEDIF_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0);
            Assert.AreEqual(dt, 41);

            dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0);
            Assert.AreEqual(dt, 503);

            dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0);
            Assert.AreEqual(dt, 15318);


            dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0);
            Assert.AreEqual(dt, 342);

            dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0);
            Assert.AreEqual(dt, 11);

            dt = engine.TryEvaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0);
            Assert.AreEqual(dt, 8);
        }
        [Test]
        public void DAYS360_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DAYS360('1975-1-30','2017-1-7')", 0);
            Assert.AreEqual(dt, 15097);
        }
        [Test]
        public void EDATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("EDATE(\"2012-1-31\",32)", DateTime.MinValue);
            Assert.AreEqual(dt, DateTime.Parse("2014-09-30"));
        }
        [Test]
        public void EOMONTH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("EOMONTH(\"2012-2-1\",32)", DateTime.MinValue);
            Assert.AreEqual(dt, DateTime.Parse("2014-10-31"));
        }

        [Test]
        public void NETWORKDAYS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")", 0);
            Assert.AreEqual(dt, 262);
        }
        [Test]
        public void WORKDAY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("WORKDAY(\"2012-1-2\",145)", DateTime.MinValue);
            Assert.AreEqual(dt, DateTime.Parse("2012-07-23"));
        }

        [Test]
        public void WEEKNUM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\")", 0);
            Assert.AreEqual(dt, 2);
            dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\")", 0);
            Assert.AreEqual(dt, 1);

            dt = engine.TryEvaluate("WEEKNUM(\"2016-1-4\",2)", 0);
            Assert.AreEqual(dt, 2);
            dt = engine.TryEvaluate("WEEKNUM(\"2016-1-3\",2)", 0);
            Assert.AreEqual(dt, 1);
            dt = engine.TryEvaluate("WEEKNUM(\"2016-1-2\",2)", 0);
            Assert.AreEqual(dt, 1);
        }


    }
}
