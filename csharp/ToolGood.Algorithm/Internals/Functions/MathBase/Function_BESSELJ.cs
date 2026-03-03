using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_BESSELJ : Function_2
	{
		public Function_BESSELJ(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_BESSELJ(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "BesselJ";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var x = args1.DoubleValue;
			var n = (int)Math.Truncate(args2.DoubleValue);

			return Operand.Create(BesselJ(n, x));
		}

		private static double BesselJ(int n, double x)
		{
			if(x == 0) {
				return (n == 0) ? 1.0 : 0.0;
			}

			if(n < 0) n = -n;

			double ax = Math.Abs(x);
			if(ax < 1e-10) {
				return (n == 0) ? 1.0 : 0.0;
			}

			if(n == 0) return BesselJ0(x);
			if(n == 1) return BesselJ1(x);

			if(ax > (double)n) {
				double J0 = BesselJ0(x);
				double J1 = BesselJ1(x);
				double Jn = 0;

				for(int k = 1; k < n; k++) {
					Jn = (2.0 * k / x) * J1 - J0;
					J0 = J1;
					J1 = Jn;
				}
				return J1;
			}

			int m = (int)(1.5 * n + 10);
			double[] J = new double[m + 2];
			J[m + 1] = 0.0;
			J[m] = 1.0;

			for(int k = m; k >= 1; k--) {
				J[k - 1] = (2.0 * k / x) * J[k] - J[k + 1];
			}

			double sum = 0.0;
			for(int k = 0; k <= m; k += 2) {
				sum += 2.0 * J[k];
			}
			sum -= J[0];

			return J[n] / sum;
		}

		private static double BesselJ0(double x)
		{
			double ax = Math.Abs(x);
			if(ax < 8.0) {
				double y1 = x * x;
				double ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7
					+ y1 * (-11214424.18 + y1 * (77392.33017 + y1 * (-184.9052456)))));
				double ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718
					+ y1 * (59272.64853 + y1 * (267.8532712 + y1 * 1.0))));
				return ans1 / ans2;
			}
			double z = 8.0 / ax;
			double y2 = z * z;
			double xx = ax - 0.785398164;
			double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
				+ y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
			double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
				+ y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
				- y2 * 0.934935152e-7)));
			return Math.Sqrt(0.636619772 / ax) * (Math.Cos(xx) * ans3 - z * Math.Sin(xx) * ans4);
		}

		private static double BesselJ1(double x)
		{
			double ax = Math.Abs(x);
			if(ax < 8.0) {
				double y1 = x * x;
				double ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1
					+ y1 * (-2972611.439 + y1 * (15704.48260 + y1 * (-30.16036606))))));
				double ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74
					+ y1 * (99447.43394 + y1 * (376.9991397 + y1 * 1.0))));
				return ans1 / ans2;
			}
			double z = 8.0 / ax;
			double y2 = z * z;
			double xx = ax - 2.356194491;
			double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
				+ y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
			double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
				+ y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
				+ y2 * 0.105787412e-6)));
			double ans = Math.Sqrt(0.636619772 / ax) * (Math.Cos(xx) * ans3 - z * Math.Sin(xx) * ans4);
			return (x < 0) ? -ans : ans;
		}
	}
}
