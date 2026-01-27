using PetaTest;
using ToolGood.Algorithm;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm2.Test.AlgorithmEngineHelper2
{
	[TestFixture]
	public class AlgorithmEngineHelperTest
	{
		[Test]
		public void Test()
		{
			DiyNameInfo p = AlgorithmEngineHelper.GetDiyNames("dd");
			Assert.AreEqual("dd", p.Parameters[0].ToString());



			DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("dd");
			Assert.AreEqual("dd", p3.Parameters[0].ToString());


			DiyNameInfo p5 = AlgorithmEngineHelper.GetDiyNames("ddd(d1,22)");
			Assert.AreEqual("ddd", p5.Functions[0].Name);
			Assert.AreEqual("d1", p5.Parameters[0].ToString());

			DiyNameInfo p6 = AlgorithmEngineHelper.GetDiyNames("长");
			Assert.AreEqual("长", p6.Parameters[0].ToString());

			DiyNameInfo p7 = AlgorithmEngineHelper.GetDiyNames("ddd+2");
			Assert.AreEqual("ddd", p7.Parameters[0].ToString());


			DiyNameInfo p8 = AlgorithmEngineHelper.GetDiyNames(@"{""A"": 0.6,""B"": 0.4,""C"": 0.6,""E"": 0.33,""F"": 0.29,""Z"": 0.15
,""EB"": 0.7,""EE"": 0.65,""EA"": 0.85,""AB"": 1.0,""BC"": 1.0,""AA"":1.0
,""EBC"": 1.15,""BAB"": 1.25,""BCB"": 1.25,""BBC"": 1.25,""CBB"": 1.25,""EBA"": 1.2,""AAA"": 1.4}[瓦楞]");
			Assert.AreEqual("瓦楞", p8.Parameters[0].ToString());
		}

		[Test]
		public void Test2()
		{
			var b = AlgorithmEngineHelper.IsKeywords("false");
			Assert.IsTrue(b);
		}

		[Test]
		public void Test3()
		{
			var b = AlgorithmEngineHelper.UnitConversion(1M, "米", "千米", "测试");
			Assert.AreEqual(0.001M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "米", "分米", "测试");
			Assert.AreEqual(10M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "米", "厘米", "测试");
			Assert.AreEqual(100M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "米", "mm", "测试");
			Assert.AreEqual(1000M, b);

			b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "dm2", "测试");
			Assert.AreEqual(100M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "cm2", "测试");
			Assert.AreEqual(10000M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "mm2", "测试");
			Assert.AreEqual(1000000M, b);

			b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "dm3", "测试");
			Assert.AreEqual(1000M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "cm3", "测试");
			Assert.AreEqual(1000000M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "mm3", "测试");
			Assert.AreEqual(1000000000M, b);

			b = AlgorithmEngineHelper.UnitConversion(1M, "t", "kg", "测试");
			Assert.AreEqual(1000M, b);
			b = AlgorithmEngineHelper.UnitConversion(1M, "t", "g", "测试");
			Assert.AreEqual(1000000M, b);
		}

		[Test]
		public void Test5()
		{
			var b = AlgorithmEngineHelper.CheckFormula("1+1");
			Assert.IsTrue(b);

			b = AlgorithmEngineHelper.CheckFormula("1+");
			Assert.AreEqual(false, b);

			b = AlgorithmEngineHelper.CheckFormula("@123+1");
			Assert.AreEqual(false, b);
		}

	}
}