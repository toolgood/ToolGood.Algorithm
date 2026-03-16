using PetaTest;
using System;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    public class IssuesTest
    {
        [Test]
        public void issues_12()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
			// 防止有误解，禁止数字直接转时间
			//var dt = engine.TryEvaluate("Year(44406)=2021", false);
			//Assert.AreEqual(dt, true);
			//dt = engine.TryEvaluate("MONTH(44406)=7", false);
			//Assert.AreEqual(dt, true);
			//dt = engine.TryEvaluate("DAY(44406)=29", false);
			//Assert.AreEqual(dt, true);

			// 防止有误解，禁止时间直接转数字
			//int num = engine.TryEvaluate("date(2011,2,2)", 0);
            // Assert.AreEqual(num, 40576);
        }

        [Test]
        public void issues_13()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("days360(date(2020,5,31),date(2023,12,15))", 0);
            Assert.AreEqual(dt, 1275);
        }

        // 防止有误解，禁止时间相加减
        //[Test]
        //public void issues_27()
        //{
        //    AlgorithmEngine engine = new AlgorithmEngine();
        //    var dt = engine.TryEvaluate("DATE(2024, 8, 1) + TIME(8, 0, 0)", DateTime.Now);
        //    Assert.AreEqual(dt, DateTime.Parse("2024-08-01 8:0"));
        //}



		[Test]
		public void issues_0()
		{
			AlgorithmEngineEx engine = new AlgorithmEngineEx();
			engine.AddParameter("瓦楞", "BC");
			var dt = engine.TryEvaluate(@"{""A"": 0.6,""B"": 0.4,""C"": 0.6,""E"": 0.33,""F"": 0.29,""Z"": 0.15
,""EB"": 0.7,""EE"": 0.65,""EA"": 0.85,""AB"": 1.0,""BC"": 1.0,""AA"":1.0
,""EBC"": 1.15,""BAB"": 1.25,""BCB"": 1.25,""BBC"": 1.25,""CBB"": 1.25,""EBA"": 1.2,""AAA"": 1.4}[瓦楞]", 0);
			Assert.AreEqual(dt, 1.0);
		}
	}
}