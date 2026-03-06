using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BESSELY : Function_2
	{
		public Function_BESSELY(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_BESSELY(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "BesselY";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var x = args1.NumberValue;
			var n = (int)Math.Truncate(args2.NumberValue);

			if(x <= 0) {
				return FunctionError();
			}

			return Operand.Create(BesselY(n, x));
		}

		private static decimal BesselY(int n, decimal x)
		{
			if(n < 0) n = -n;

			if(n == 0) return BesselY0(x);
			if(n == 1) return BesselY1(x);

			decimal Y0 = BesselY0(x);
			decimal Y1 = BesselY1(x);
			decimal Yn = 0;

			for(int k = 1; k < n; k++) {
				Yn = (2.0m * k / x) * Y1 - Y0;
				Y0 = Y1;
				Y1 = Yn;
			}

			return Y1;
		}

		private static decimal BesselY0(decimal x)
		{
			if(x < 8.0m) {
				decimal y1 = x * x;
				decimal ans1 = -2957821389.0m + y1 * (7062834065.0m + y1 * (-512359803.6m
					+ y1 * (10879881.29m + y1 * (-86327.92757m + y1 * 228.4622733m))));
				decimal ans2 = 40076544269.0m + y1 * (745249964.8m + y1 * (7189466.438m
					+ y1 * (47447.26470m + y1 * (226.1030244m + y1 * 1.0m))));
				return (ans1 / ans2) + 0.636619772m * BesselJ0(x) * MathEx.Log(x);
			}
			decimal z = 8.0m / x;
			decimal y2 = z * z;
			decimal xx = x - 0.785398164m;
			decimal ans3 = 1.0m + y2 * (-0.1098628627e-2m + y2 * (0.2734510407e-4m
				+ y2 * (-0.2073370639e-5m + y2 * 0.2093887211e-6m)));
			decimal ans4 = -0.1562499995e-1m + y2 * (0.1430488765e-3m
				+ y2 * (-0.6911147651e-5m + y2 * (0.7621095161e-6m
				+ y2 * (-0.934935152e-7m))));
			return MathEx.Sqrt(0.636619772m / x) * (MathEx.Sin(xx) * ans3 + z * MathEx.Cos(xx) * ans4);
		}

		private static decimal BesselY1(decimal x)
		{
			if(x < 8.0m) {
				decimal y1 = x * x;
				decimal ans1 = x * (-0.4900604943e13m + y1 * (0.1275274390e13m
					+ y1 * (-0.5153438139e11m + y1 * (0.7349264551e9m
					+ y1 * (-0.4237922726e7m + y1 * 0.8511937935e4m)))));
				decimal ans2 = 0.2499580570e14m + y1 * (0.4244419664e12m
					+ y1 * (0.3733650367e10m + y1 * (0.2245904002e8m
					+ y1 * (0.1020426050e6m + y1 * (0.3549632885e3m + y1)))));
				return (ans1 / ans2) + 0.636619772m * (BesselJ1(x) * MathEx.Log(x) - 1.0m / x);
			}
			decimal z = 8.0m / x;
			decimal y2 = z * z;
			decimal xx = x - 2.356194491m;
			decimal ans3 = 1.0m + y2 * (0.183105e-2m + y2 * (-0.3516396496e-4m
				+ y2 * (0.2457520174e-5m + y2 * (-0.240337019e-6m))));
			decimal ans4 = 0.04687499995m + y2 * (-0.2002690873e-3m
				+ y2 * (0.8449199096e-5m + y2 * (-0.88228987e-6m
				+ y2 * 0.105787412e-6m)));
			return MathEx.Sqrt(0.636619772m / x) * (MathEx.Sin(xx) * ans3 + z * MathEx.Cos(xx) * ans4);
		}

		private static decimal BesselJ0(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 8.0m) {
				decimal y1 = x * x;
				decimal ans1 = 57568490574.0m + y1 * (-13362590354.0m + y1 * (651619640.7m
					+ y1 * (-11214424.18m + y1 * (77392.33017m + y1 * (-184.9052456m)))));
				decimal ans2 = 57568490411.0m + y1 * (1029532985.0m + y1 * (9494680.718m
					+ y1 * (59272.64853m + y1 * (267.8532712m + y1 * 1.0m))));
				return ans1 / ans2;
			}
			decimal z = 8.0m / ax;
			decimal y2 = z * z;
			decimal xx = ax - 0.785398164m;
			decimal ans3 = 1.0m + y2 * (-0.1098628627e-2m + y2 * (0.2734510407e-4m
				+ y2 * (-0.2073370639e-5m + y2 * 0.2093887211e-6m)));
			decimal ans4 = -0.1562499995e-1m + y2 * (0.1430488765e-3m
				+ y2 * (-0.6911147651e-5m + y2 * (0.7621095161e-6m
				- y2 * 0.934935152e-7m)));
			return MathEx.Sqrt(0.636619772m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
		}

		private static decimal BesselJ1(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 8.0m) {
				decimal y1 = x * x;
				decimal ans1 = x * (72362614232.0m + y1 * (-7895059235.0m + y1 * (242396853.1m
					+ y1 * (-2972611.439m + y1 * (15704.48260m + y1 * (-30.16036606m))))));
				decimal ans2 = 144725228442.0m + y1 * (2300535178.0m + y1 * (18583304.74m
					+ y1 * (99447.43394m + y1 * (376.9991397m + y1 * 1.0m))));
				return ans1 / ans2;
			}
			decimal z = 8.0m / ax;
			decimal y2 = z * z;
			decimal xx = ax - 2.356194491m;
			decimal ans3 = 1.0m + y2 * (0.183105e-2m + y2 * (-0.3516396496e-4m
				+ y2 * (0.2457520174e-5m + y2 * (-0.240337019e-6m))));
			decimal ans4 = 0.04687499995m + y2 * (-0.2002690873e-3m
				+ y2 * (0.8449199096e-5m + y2 * (-0.88228987e-6m
				+ y2 * 0.105787412e-6m)));
			decimal ans = MathEx.Sqrt(0.636619772m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
			return (x < 0) ? -ans : ans;
		}
	}
}
