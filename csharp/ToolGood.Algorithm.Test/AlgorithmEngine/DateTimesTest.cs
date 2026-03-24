using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.DateTimes
{
    [TestFixture]
    internal class DateTimesTest
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

            dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.Now);
            Assert.AreEqual(dt.ToLocalTime(), new DateTime(2023, 8, 5, 19, 28, 19));

            dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.Now);
            Assert.AreEqual(dt.ToLocalTime(), new DateTime(2023, 8, 5, 19, 28, 19));

            engine.UseLocalTime = true;
            dt = engine.TryEvaluate("DATEVALUE('1691234899000',0)", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            dt = engine.TryEvaluate("DATEVALUE('1691234899',0)", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            dt = engine.TryEvaluate("DATEVALUE('1691234899000')", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));

            dt = engine.TryEvaluate("DATEVALUE('1691234899')", DateTime.Now);
            Assert.AreEqual(dt, new DateTime(2023, 8, 5, 19, 28, 19));
        }

        [Test]
        public void TIMESTAMP_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseLocalTime = true;

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
        public void WEEKDAY_ExcelCompatible_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            // 2017年1月1日是周日，1月7日是周六，1月2日是周一
            // type 1: 周日=1, 周六=7
            // type 2: 周一=1, 周日=7
            // type 3: 周一=0, 周日=6

            // 测试所有 type (1-3, 11-17)
            // 2017-1-2 (周一)
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),1)", 0), 2);  // type 1: 周一=2
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),2)", 0), 1);  // type 2: 周一=1
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),3)", 0), 0);  // type 3: 周一=0
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),11)", 0), 1); // type 11: 周一=1
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),12)", 0), 7); // type 12: 周二=1, 周一=7
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),13)", 0), 6); // type 13: 周三=1, 周一=6
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),14)", 0), 5); // type 14: 周四=1, 周一=5
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),15)", 0), 4); // type 15: 周五=1, 周一=4
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),16)", 0), 3); // type 16: 周六=1, 周一=3
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,2),17)", 0), 2); // type 17: 周日=1, 周一=2

            // 2017-1-7 (周六)
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),1)", 0), 7);  // type 1: 周六=7
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),2)", 0), 6);  // type 2: 周六=6
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),3)", 0), 5);  // type 3: 周六=5
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),11)", 0), 6); // type 11: 周六=6
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),12)", 0), 5); // type 12: 周六=5
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),13)", 0), 4); // type 13: 周六=4
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),14)", 0), 3); // type 14: 周六=3
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),15)", 0), 2); // type 15: 周六=2
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),16)", 0), 1); // type 16: 周六=1
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,7),17)", 0), 7); // type 17: 周六=7

            // 2017-1-8 (周日)
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),1)", 0), 1);  // type 1: 周日=1
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),2)", 0), 7);  // type 2: 周日=7
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),3)", 0), 6);  // type 3: 周日=6
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),11)", 0), 7); // type 11: 周日=7
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),12)", 0), 6); // type 12: 周日=6
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),13)", 0), 5); // type 13: 周日=5
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),14)", 0), 4); // type 14: 周日=4
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),15)", 0), 3); // type 15: 周日=3
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),16)", 0), 2); // type 16: 周日=2
            Assert.AreEqual(engine.TryEvaluate("WEEKDAY(date(2017,1,8),17)", 0), 1); // type 17: 周日=1
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
        public void DAYS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DAYS('2017-1-7','1975-1-30')", 0);
            Assert.AreEqual(dt, 15318);

            dt = engine.TryEvaluate("DAYS('2016-3-1','2016-2-28')", 0);
            Assert.AreEqual(dt, 2);

            dt = engine.TryEvaluate("DAYS('2016-2-28','2016-3-1')", 0);
            Assert.AreEqual(dt, -2);
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
        public void WORKDAY_ExcelCompatible_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("WORKDAY(\"2024-01-15\", 5)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 22));

            dt = engine.TryEvaluate("WORKDAY(\"2024-01-15\", -5)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 8));

            dt = engine.TryEvaluate("WORKDAY(\"2024-01-15\", 0)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 15));

            dt = engine.TryEvaluate("WORKDAY(\"2024-01-12\", 1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 15));

            dt = engine.TryEvaluate("WORKDAY(\"2024-01-15\", -1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 12));
        }

        [Test]
        public void DATE_ExcelCompatible_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2024, 1, 15)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 1, 15));

            dt = engine.TryEvaluate("DATE(2024, 13, 1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2025, 1, 1));

            dt = engine.TryEvaluate("DATE(2024, 1, 32)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 2, 1));

            dt = engine.TryEvaluate("DATE(2024, 0, 1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2023, 12, 1));

            dt = engine.TryEvaluate("DATE(2024, 2, 30)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 3, 1));

            dt = engine.TryEvaluate("DATE(2024, -1, 1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2023, 11, 1));

            dt = engine.TryEvaluate("DATE(2024, 12, 0)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 11, 30));

            dt = engine.TryEvaluate("DATE(2024, 1, -1)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2023, 12, 30));
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
        public void WEEKNUM_ExcelCompatible_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            // 2016年1月1日是周五
            // type 1: 周日作为第一天
            // type 2: 周一作为第一天
            // type 11: 周一作为第一天 (同type 2)
            // type 12-16: 不同起始日
            // type 17: 周日作为第一天 (同type 1)
            // type 21: ISO 8601，周一作为第一天

            // 2016-1-1 (周五)
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),1)", 0), 1);   // type 1: 周五在第1周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),2)", 0), 1);  // type 2: 周一为起始日，周五在第53周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),11)", 0), 1);// type 11: 同type 2
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),12)", 0), 1); // type 12: 周二为起始日
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),13)", 0), 1); // type 13: 周三为起始日
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),14)", 0), 1); // type 14: 周四为起始日
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),15)", 0), 1); // type 15: 周五为起始日
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),16)", 0), 1);  // type 16: 周六为起始日，周五在第1周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,1),17)", 0), 1);  // type 17: 同type 1

            // 2016-1-3 (周日)
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),1)", 0), 2);   // type 1: 周日为新一周开始
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),2)", 0), 1);   // type 2: 周一为新一周开始
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),11)", 0), 1);  // type 11: 同type 2
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),16)", 0), 2);  // type 16: 周六为起始日，周日是下一周第一天
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),17)", 0), 2);  // type 17: 同type 1

            // 2016-1-4 (周一)
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,4),1)", 0), 2);  // type 1: 周日在第1周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,4),2)", 0), 2);  // type 2: 周一在第2周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,4),11)", 0), 2); // type 11: 同type 2
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,4),21)", 0), 0); // type 21: ISO 8601，周一是第1周

            // 测试 ISO 8601 (type 21)
            // 2016年1月4日是ISO 8601标准的第1周第一天
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,4),21)", 0), 0);
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2016,1,3),21)", 0), 0); // 2016-1-3是2015年的第53周

            // 2021年1月1日是周五，ISO 8601第53周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2021,1,1),21)", 0), 0);
            // 2021年1月4日是周一，ISO 8601第1周
            Assert.AreEqual(engine.TryEvaluate("WEEKNUM(date(2021,1,4),21)", 0), 0);
        }

        [Test]
        public void YEARFRAC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1')", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.5, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2013-1-1')", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(1.0, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 0)", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.5, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 1)", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.49726776, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 2)", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.505555556, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 3)", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.498630137, 3));

            t = engine.TryEvaluate("YEARFRAC('2012-1-1', '2012-7-1', 4)", 0.0);
            Assert.AreEqual(Math.Round(t, 3), Math.Round(0.5, 3));
        }

        [Test]
        public void AddYears_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01'.addYears(1).year()", 0);
            Assert.AreEqual(dt, 2001);

            dt = engine.TryEvaluate("'2000/02/01'.addYears(1).year()", 0);
            Assert.AreEqual(dt, 2001);
        }

        [Test]
        public void AddMonths_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01'.AddMonths(1).Month()", 0);
            Assert.AreEqual(dt, 3);
        }

        [Test]
        public void AddDays_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01'.AddDays(1).Day()", 0);
            Assert.AreEqual(dt, 2);
        }

        [Test]
        public void AddHours_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0);
            Assert.AreEqual(dt, 13);
        }

        [Test]
        public void AddMinutes_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0);
            Assert.AreEqual(dt, 6);
        }

        [Test]
        public void AddSeconds_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0);
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

            dt = engine.TryEvaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0);
            Assert.AreEqual(dt, 0);
        }

        #region 方法式调用测试

        [Test]
        public void MethodStyle_DATEVALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2016-01-01'.DATEVALUE()", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1));
        }

        [Test]
        public void MethodStyle_TIMEVALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'12:12:12'.TIMEVALUE()", TimeSpan.MinValue);
            Assert.AreEqual(dt, new DateTime(2016, 1, 1, 12, 12, 12).TimeOfDay);
        }

        [Test]
        public void MethodStyle_YEAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2016-05-15'.YEAR()", 0);
            Assert.AreEqual(dt, 2016);
        }

        [Test]
        public void MethodStyle_MONTH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2016-05-15'.MONTH()", 0);
            Assert.AreEqual(dt, 5);
        }

        [Test]
        public void MethodStyle_DAY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2016-05-15'.DAY()", 0);
            Assert.AreEqual(dt, 15);
        }

        [Test]
        public void MethodStyle_HOUR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'12:30:45'.HOUR()", 0);
            Assert.AreEqual(dt, 12);
        }

        [Test]
        public void MethodStyle_MINUTE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'12:30:45'.MINUTE()", 0);
            Assert.AreEqual(dt, 30);
        }

        [Test]
        public void MethodStyle_SECOND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'12:30:45'.SECOND()", 0);
            Assert.AreEqual(dt, 45);
        }

        [Test]
        public void MethodStyle_ADDYEARS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-01'.ADDYEARS(1).YEAR()", 0);
            Assert.AreEqual(dt, 2001);
        }

        [Test]
        public void MethodStyle_ADDMONTHS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-15'.ADDMONTHS(2).MONTH()", 0);
            Assert.AreEqual(dt, 3);
        }

        [Test]
        public void MethodStyle_ADDDAYS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-01'.ADDDAYS(10).DAY()", 0);
            Assert.AreEqual(dt, 11);
        }

        [Test]
        public void MethodStyle_ADDHOURS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-01 10:00:00'.ADDHOURS(5).HOUR()", 0);
            Assert.AreEqual(dt, 15);
        }

        [Test]
        public void MethodStyle_ADDMINUTES_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-01 10:30:00'.ADDMINUTES(45).MINUTE()", 0);
            Assert.AreEqual(dt, 15);
        }

        [Test]
        public void MethodStyle_ADDSECONDS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2000-01-01 10:00:30'.ADDSECONDS(45).SECOND()", 0);
            Assert.AreEqual(dt, 15);
        }

        [Test]
        public void MethodStyle_TIMESTAMP_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseLocalTime = true;
            var dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP()", 0L);
            Assert.AreEqual(dt, 1451577600000L);

            dt = engine.TryEvaluate("'2016-01-01'.TIMESTAMP(1)", 0L);
            Assert.AreEqual(dt, 1451577600L);
        }

        #endregion 方法式调用测试

        #region 边界值测试

        [Test]
        public void LeapYear_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2020,2,29)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2020, 2, 29));

            dt = engine.TryEvaluate("DATE(2024,2,29)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2024, 2, 29));

            dt = engine.TryEvaluate("DATE(2019,2,28)", DateTime.MinValue);
            Assert.AreEqual(dt, new DateTime(2019, 2, 28));

            var year = engine.TryEvaluate("'2020-02-29'.YEAR()", 0);
            Assert.AreEqual(year, 2020);
            var month = engine.TryEvaluate("'2020-02-29'.MONTH()", 0);
            Assert.AreEqual(month, 2);
            var day = engine.TryEvaluate("'2020-02-29'.DAY()", 0);
            Assert.AreEqual(day, 29);
        }

        [Test]
        public void LeapYear_AddDays_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'2020-02-28'.ADDDAYS(1).DAY()", 0);
            Assert.AreEqual(dt, 29);

            dt = engine.TryEvaluate("'2020-03-01'.ADDDAYS(-1).DAY()", 0);
            Assert.AreEqual(dt, 29);
        }

        [Test]
        public void MonthBoundary_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2020,1,31).ADDMONTHS(1).MONTH()", 0);
            Assert.AreEqual(dt, 2);

            dt = engine.TryEvaluate("DATE(2020,3,31).ADDMONTHS(-1).MONTH()", 0);
            Assert.AreEqual(dt, 2);
        }

        [Test]
        public void YearBoundary_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).YEAR()", 0);
            Assert.AreEqual(dt, 2021);
            dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).MONTH()", 0);
            Assert.AreEqual(dt, 1);
            dt = engine.TryEvaluate("DATE(2020,12,31).ADDDAYS(1).DAY()", 0);
            Assert.AreEqual(dt, 1);

            dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).YEAR()", 0);
            Assert.AreEqual(dt, 2020);
            dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).MONTH()", 0);
            Assert.AreEqual(dt, 12);
            dt = engine.TryEvaluate("DATE(2021,1,1).ADDDAYS(-1).DAY()", 0);
            Assert.AreEqual(dt, 31);
        }

        #endregion 边界值测试
    }
}
