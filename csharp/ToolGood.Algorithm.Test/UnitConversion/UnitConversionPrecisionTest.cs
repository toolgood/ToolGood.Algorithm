using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test.UnitConversion
{
	/// <summary>
	/// 精确分数系数防回归测试。
	/// 这些系数由 6 位近似值重构为精确分数（如 short ton = 50000/45359237），
	/// 反向换算误差应远小于 1e-9。若有人改回近似值，本测试将失败。
	/// </summary>
	[TestFixture]
	public class UnitConversionPrecisionTest
	{
		private static void AssertClose(string src, string tar, decimal value, double expected)
		{
			var result = AlgorithmEngineHelper.UnitConversion(value, src, tar, "精度测试");
			var d = (double)result;
			var rel = System.Math.Abs(d - expected) / System.Math.Max(System.Math.Abs(expected), 1e-30);
			Assert.IsTrue(rel < 1e-9);
		}

		#region 质量

		[Test]
		public void Mass_ShortTon_Exact()
		{
			AssertClose("short ton", "kg", 1m, 907.18474);
			AssertClose("kg", "short ton", 1m, 1.0 / 907.18474);
			AssertClose("short ton", "lb", 1m, 2000);
		}

		[Test]
		public void Mass_LongTon_Exact()
		{
			AssertClose("long ton", "kg", 1m, 1016.0469088);
			AssertClose("kg", "long ton", 1m, 1.0 / 1016.0469088);
			AssertClose("long ton", "lb", 1m, 2240);
		}

		#endregion

		#region 面积

		[Test]
		public void Area_Mu_Exact()
		{
			AssertClose("亩", "m²", 1m, 666.6666666666667);
			AssertClose("m²", "亩", 1m, 0.0015);
		}

		#endregion

		#region 体积

		[Test]
		public void Volume_CubicFoot_Exact()
		{
			AssertClose("ft³", "l", 1m, 28.316846592);
			AssertClose("l", "ft³", 1m, 0.03531466672148859);
			AssertClose("ft³", "in³", 1m, 1728);
		}

		[Test]
		public void Volume_CubicInch_Exact()
		{
			AssertClose("in³", "l", 1m, 0.016387064);
			AssertClose("l", "in³", 1m, 61.0237440947323);
		}

		[Test]
		public void Volume_ImperialPint_Exact()
		{
			AssertClose("imperial pint", "l", 1m, 0.56826125);
			AssertClose("l", "imperial pint", 1m, 1.7597539863927023);
		}

		[Test]
		public void Volume_ImperialGallon_Exact()
		{
			AssertClose("imperial gallon", "l", 1m, 4.54609);
			AssertClose("l", "imperial gallon", 1m, 0.2199692482990878);
			AssertClose("imperial gallon", "imperial pint", 1m, 8);
			AssertClose("imperial gallon", "imperial quart", 1m, 4);
		}

		[Test]
		public void Volume_ImperialQuart_Exact()
		{
			AssertClose("imperial quart", "l", 1m, 1.1365225);
			AssertClose("l", "imperial quart", 1m, 0.87987699319635115);
		}

		[Test]
		public void Volume_USPint_Exact()
		{
			AssertClose("US pint", "l", 1m, 0.473176473);
			AssertClose("l", "US pint", 1m, 2.1133764188651873);
		}

		[Test]
		public void Volume_USGallon_Exact()
		{
			AssertClose("US gallon", "l", 1m, 3.785411784);
			AssertClose("l", "US gallon", 1m, 0.2641720523581484);
			AssertClose("US gallon", "US quart", 1m, 4);
		}

		#endregion
	}
}
