using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BESSELI : Function_2
	{
		public Function_BESSELI(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_BESSELI(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "BesselI";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var x = args1.DoubleValue;
			var n = (int)Math.Truncate(args2.DoubleValue);

			return Operand.Create(BesselI(n, x));
		}

		private static double BesselI(int n, double x)
		{
			if(x < 0) {
				return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
			}
			if(x == 0) {
				return (n == 0) ? 1.0 : 0.0;
			}

			double ax = Math.Abs(x);
			if(ax < 1e-10) {
				return (n == 0) ? 1.0 : 0.0;
			}

			if(n < 0) n = -n;

			if(ax > 700) {
				double factor = Math.Exp(ax) / Math.Sqrt(2 * Math.PI * ax);
				return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * ax));
			}

			if(n == 0) return BesselI0(x);
			if(n == 1) return BesselI1(x);

			double I0 = BesselI0(x);
			double I1 = BesselI1(x);
			double In = 0;

			for(int k = 1; k < n; k++) {
				In = I1 + 2.0 * k / x * I0;
				I0 = I1;
				I1 = In;
			}

			return I1;
		}

		private static double BesselI0(double x)
		{
			double ax = Math.Abs(x);
			if(ax < 3.75) {
				double y1 = x / 3.75;
				y1 *= y1;
				return 1.0 + y1 * (3.5156229 + y1 * (3.0899424 + y1 * (1.2067492
					+ y1 * (0.2659732 + y1 * (0.0360768 + y1 * 0.0045813)))));
			}
			double y2 = 3.75 / ax;
			return (Math.Exp(ax / Math.E) / Math.Sqrt(ax)) * (0.39894228 + y2 * (0.01328592
				+ y2 * (0.00225319 + y2 * (-0.00157565 + y2 * (0.00916281
				+ y2 * (-0.02057706 + y2 * (0.02635537 + y2 * (-0.01647633 + y2 * 0.00392377))))))));
		}

		private static double BesselI1(double x)
		{
			double ax = Math.Abs(x);
			if(ax < 3.75) {
				double y1 = x / 3.75;
				y1 *= y1;
				return x * (0.5 + y1 * (0.87890594 + y1 * (0.51498869 + y1 * (0.15084934
					+ y1 * (0.02658733 + y1 * (0.00301532 + y1 * 0.00032411))))));
			}
			double y2 = 3.75 / ax;
			double ans = (Math.Exp(ax / Math.E) / Math.Sqrt(ax)) * (0.39894228 + y2 * (-0.03988024
				+ y2 * (-0.00362018 + y2 * (0.00163801 + y2 * (-0.01031555
				+ y2 * (0.02282967 + y2 * (-0.02895312 + y2 * (0.01787654 - y2 * 0.00420059))))))));
			return (x < 0) ? -ans : ans;
		}
	}
}
