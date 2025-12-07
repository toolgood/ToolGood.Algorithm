using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class AlgorithmEngineTest_dateTime
    {
        [Test]
        public void DATEVALUE_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATEVALUE('2016-01-01')", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));
            dt = engine.TryEvaluate("DATEVALUE（'2016-01-01'）", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));

            dt = engine.TryEvaluate("DATEVALUE('2016/01/01')", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));

            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.Now);
            Assert.AreEqual(dt.ToLocalTime(), new DateTime(2023, 8, 5, 19, 28, 19));

            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.Now);
            Assert.AreEqual(dt.ToLocalTime(), new DateTime(2023, 8, 5, 19, 28, 19));

            engine.UseLocalTime = true;
            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899000')", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            // chinese time
            dt = engine.TryEvaluate("DATEVALUE('1691234899')", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));
        }

        [Test]
        public void TIMESTAMP_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseLocalTime = true;

            // chinese time
            var dt = engine.TryEvaluate("TIMESTAMP('2016-01-01')", 0L);
            Assert.AreEqual(dt, 1451577600000L);

            dt = engine.TryEvaluate("TIMESTAMP('2016/01/01')", 0L);
            Assert.AreEqual(dt, 1451577600000L);

            dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',0)", 0L);
            Assert.AreEqual(dt, 1451577600000L);

            dt = engine.TryEvaluate("TIMESTAMP('2016-01-01',1)", 0L);
            Assert.AreEqual(dt, 1451577600L);
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

        [Test]
        public void Add_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
            Assert.AreEqual(dt, 2001);

            dt = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
            Assert.AreEqual(dt, 2001);

            dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
            Assert.AreEqual(dt, 3);

            dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
            Assert.AreEqual(dt, 2);

            dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
            Assert.AreEqual(dt, 13);

            dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
            Assert.AreEqual(dt, 6);

            dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'2000-02-01 10:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'2000-02-01 20:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'2000-02-01 9:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'7:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'10:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            dt = engine.TryEvaluate("'20:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 7);

            // 错误
            dt = engine.TryEvaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 0);

            //
            //dt = engine.TryEvaluate("'24:05:06'.AddSeconds(1).Second()", 0);
            //Assert.AreEqual(dt, 0);
        }
    }
}