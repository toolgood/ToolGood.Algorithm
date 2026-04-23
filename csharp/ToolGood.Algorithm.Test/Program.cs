using Antlr4.Runtime.Misc;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm.Test
{
	internal class Program
	{
		public static readonly double DoublePrecision = Math.Pow(2, -53);

		private static void Main(string[] args)
		{
			var t222 = Convert.ToString(122223, 16);
			AlgorithmEngine engine = new AlgorithmEngine();

			//var a1 = Math.Sqrt(2) / 3 / 7 * 21 * Math.Sqrt(2);              // 返回	2.0000000000000004
			//var a2 = engine.TryEvaluate("sqrt(2)/3/7*21*sqrt(2)", 0.0m);    // 返回 1.9999999999999999999999999991M    sqrt 会将精度降低 ，不可避免有误差 
			//var a3 = engine.TryEvaluate("sqrt(2)/3/7*21*sqrt(2)", 0.0);     // 返回 2     内部使用 decimal，还回double ,精度应转化而提高了。。。（应该是四舍五入）
			//var a4 = engine.TryEvaluate("2/3/7/4*5*21", 0.0m);              // 返回 2.4999999999999999999999999990M
			//var a5 = engine.TryEvaluate("2/3/7/4*5*21", 0.0);               // 返回 2.5
			//var a6 = 2.0 / 3 / 7 / 4 * 5 * 21;                              // 返回	2.5

			//var a11 = Math.Sqrt(2);                                         // 返回	1.4142135623730951
			//var a12 = engine.TryEvaluate("sqrt(2)", 0.0m);                  // 返回 1.4142135623730950488016887242M 

			var b = engine.TryEvaluate("1=1 && 1<2 || 7-8>1", 0);// Support(支持) && || and or
			var c = engine.TryEvaluate("2+3", 0);
			var d = engine.TryEvaluate("count(array(1,2,3,4))", 0);//{} represents array, return: 4 {}代表数组,返回:4
			var s = engine.TryEvaluate("'aa'&'bb'", ""); //String connection, return: AABB 字符串连接,返回:aabb
			var r = engine.TryEvaluate("(1=1)*9+2", 0); //Return: 11 返回:11
			var e = engine.TryEvaluate("'2016-1-1'+1", DateTime.MinValue); //Return date: 2016-1-2 返回日期:2016-1-2
			var t = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.MinValue);//Return datetime:2016-1-1 9:0  返回日期:2016-1-1 9:0
			var j = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);//Return 51 返回51
			var k = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", null);//Return to "William Shakespeare"  返回"William Shakespeare" (不带空格)
			var l = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null); ;//Return "ill"  返回"ill"
			var n = engine.TryEvaluate("{Name:\"William Shakespeare\", Age:51, Birthday:\"04/26/1564 00:00:00\"}.Age", null);//Return 51 返回51
			var m = engine.TryEvaluate("[1,2,3,4,5,6].has(13)", true);//Return false 返回false

			Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			//ToolGood.Algorithm.math.LexerDiagnostic.Run();
			PetaTest.Runner.RunMain(args);
		}

		public static double Increment(double value, int count = 1)
		{
			if(double.IsInfinity(value) || double.IsNaN(value) || count == 0) {
				return value;
			}

			// Translate the bit pattern of the double to an integer.
			// Note that this leads to:
			// double > 0 --> long > 0, growing as the double value grows
			// double < 0 --> long < 0, increasing in absolute magnitude as the double
			//                          gets closer to zero!
			//                          i.e. 0 - double.epsilon will give the largest long value!
			long intValue = BitConverter.DoubleToInt64Bits(value);
			if(intValue < 0) {
				intValue -= count;
			} else {
				intValue += count;
			}

			// Note that long.MinValue has the same bit pattern as -0.0.
			if(intValue == long.MinValue) {
				return 0;
			}

			// Note that not all long values can be translated into double values. There's a whole bunch of them
			// which return weird values like infinity and NaN
			return BitConverter.Int64BitsToDouble(intValue);
		}
	}
}