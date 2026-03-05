using PetaTest;
using System;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Test.Helpers
{
    [TestFixture]
    public class MyDateTest
    {
        #region 构造函数测试

        [Test]
        public void Constructor_DateTime_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            var myDate = new MyDate(dt);

            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        [Test]
        public void Constructor_TimeSpan_Test()
        {
            var ts = new TimeSpan(1, 10, 30, 45);
            var myDate = new MyDate(ts);

            Assert.IsNull(myDate.Year);
            Assert.IsNull(myDate.Month);
            Assert.AreEqual(1, myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        [Test]
        public void Constructor_Params_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);

            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        #endregion

        #region Parse 测试

        [Test]
        public void Parse_DateTime_Test()
        {
            var myDate = MyDate.Parse("2024-06-15 10:30:45");
            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        [Test]
        public void Parse_DateTime_Slash_Test()
        {
            var myDate = MyDate.Parse("2024/06/15 10:30:45");
            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        [Test]
        public void Parse_Date_Test()
        {
            var myDate = MyDate.Parse("2024-06-15");
            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
            Assert.AreEqual(0, myDate.Hour);
            Assert.AreEqual(0, myDate.Minute);
            Assert.AreEqual(0, myDate.Second);
        }

        [Test]
        public void Parse_Time_Test()
        {
            var myDate = MyDate.Parse("10:30:45");
            Assert.IsNull(myDate.Year);
            Assert.IsNull(myDate.Month);
            Assert.IsNull(myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(45, myDate.Second);
        }

        [Test]
        public void Parse_Time_NoSeconds_Test()
        {
            var myDate = MyDate.Parse("10:30");
            Assert.IsNull(myDate.Year);
            Assert.IsNull(myDate.Month);
            Assert.IsNull(myDate.Day);
            Assert.AreEqual(10, myDate.Hour);
            Assert.AreEqual(30, myDate.Minute);
            Assert.AreEqual(0, myDate.Second);
        }

        [Test]
        public void Parse_Invalid_Test()
        {
            var myDate = MyDate.Parse("invalid date");
            Assert.IsNull(myDate);
        }

        #endregion

        #region ToDateTime 测试

        [Test]
        public void ToDateTime_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var dt = myDate.ToDateTime();

            Assert.AreEqual(2024, dt.Year);
            Assert.AreEqual(6, dt.Month);
            Assert.AreEqual(15, dt.Day);
            Assert.AreEqual(10, dt.Hour);
            Assert.AreEqual(30, dt.Minute);
            Assert.AreEqual(45, dt.Second);
            Assert.AreEqual(DateTimeKind.Utc, dt.Kind);
        }

        [Test]
        public void ToDateTime_Kind_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var dt = myDate.ToDateTime(DateTimeKind.Local);

            Assert.AreEqual(DateTimeKind.Local, dt.Kind);
        }

        #endregion

        #region ToTimeSpan 测试

        [Test]
        public void ToTimeSpan_Test()
        {
            var myDate = new MyDate(null, null, 1, 10, 30, 45);
            var ts = myDate.ToTimeSpan();

            Assert.AreEqual(1, ts.Days);
            Assert.AreEqual(10, ts.Hours);
            Assert.AreEqual(30, ts.Minutes);
            Assert.AreEqual(45, ts.Seconds);
        }

        #endregion

        #region AddYears 测试

        [Test]
        public void AddYears_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddYears(1);

            Assert.AreEqual(2025, result.Year);
            Assert.AreEqual(6, result.Month);
            Assert.AreEqual(15, result.Day);
        }

        [Test]
        public void AddYears_Negative_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddYears(-1);

            Assert.AreEqual(2023, result.Year);
        }

        #endregion

        #region AddMonths 测试

        [Test]
        public void AddMonths_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddMonths(1);

            Assert.AreEqual(7, result.Month);
        }

        [Test]
        public void AddMonths_Overflow_Test()
        {
            var myDate = new MyDate(2024, 11, 15, 10, 30, 45);
            var result = myDate.AddMonths(2);

            Assert.AreEqual(2025, result.Year);
            Assert.AreEqual(1, result.Month);
        }

        #endregion

        #region AddDays 测试

        [Test]
        public void AddDays_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddDays(5);

            Assert.AreEqual(20, result.Day);
        }

        [Test]
        public void AddDays_Negative_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddDays(-5);

            Assert.AreEqual(10, result.Day);
        }

        #endregion

        #region AddHours 测试

        [Test]
        public void AddHours_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddHours(5);

            Assert.AreEqual(15, result.Hour);
        }

        [Test]
        public void AddHours_NoOverflow_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddHours(2);

            Assert.AreEqual(12, result.Hour);
        }

        #endregion

        #region AddMinutes 测试

        [Test]
        public void AddMinutes_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddMinutes(15);

            Assert.AreEqual(45, result.Minute);
        }

        [Test]
        public void AddMinutes_NoOverflow_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddMinutes(10);

            Assert.AreEqual(40, result.Minute);
        }

        #endregion

        #region AddSeconds 测试

        [Test]
        public void AddSeconds_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddSeconds(10);

            Assert.AreEqual(55, result.Second);
        }

        [Test]
        public void AddSeconds_NoOverflow_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var result = myDate.AddSeconds(5);

            Assert.AreEqual(50, result.Second);
        }

        #endregion

        #region ToString 测试

        [Test]
        public void ToString_DateTime_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            var str = myDate.ToString();

            Assert.IsTrue(str.Contains("2024"));
            Assert.IsTrue(str.Contains("06"));
            Assert.IsTrue(str.Contains("15"));
        }

        [Test]
        public void ToString_TimeOnly_Test()
        {
            var myDate = new MyDate(null, null, null, 10, 30, 45);
            var str = myDate.ToString();

            Assert.IsTrue(str.Contains("10:30:45"));
        }

        #endregion

        #region 隐式转换测试

        [Test]
        public void Implicit_DateTime_To_MyDate_Test()
        {
            var dt = new DateTime(2024, 6, 15, 10, 30, 45);
            MyDate myDate = dt;

            Assert.AreEqual(2024, myDate.Year);
            Assert.AreEqual(6, myDate.Month);
            Assert.AreEqual(15, myDate.Day);
        }

        [Test]
        public void Implicit_MyDate_To_DateTime_Test()
        {
            var myDate = new MyDate(2024, 6, 15, 10, 30, 45);
            DateTime dt = myDate;

            Assert.AreEqual(2024, dt.Year);
            Assert.AreEqual(6, dt.Month);
            Assert.AreEqual(15, dt.Day);
        }

        [Test]
        public void Implicit_MyDate_To_TimeSpan_Test()
        {
            var myDate = new MyDate(null, null, 1, 10, 30, 45);
            TimeSpan ts = myDate;

            Assert.AreEqual(1, ts.Days);
            Assert.AreEqual(10, ts.Hours);
            Assert.AreEqual(30, ts.Minutes);
            Assert.AreEqual(45, ts.Seconds);
        }

        #endregion
    }
}
