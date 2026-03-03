using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_BESSELK : Function_2
	{
		public Function_BESSELK(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_BESSELK(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "BesselK";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var x = args1.DoubleValue;
			var n = (int)Math.Truncate(args2.DoubleValue);

			if(x <= 0) {
				return Operand.Error("#NUM!");
			}

			return Operand.Create(BesselK(n, x));
		}

		private static double BesselK(int n, double x)
		{
			if(n < 0) n = -n;

			if(n == 0) return BesselK0(x);
			if(n == 1) return BesselK1(x);

			double K0 = BesselK0(x);
			double K1 = BesselK1(x);
			double Kn = 0;

			for(int k = 1; k < n; k++) {
				Kn = K1 + 2.0 * k / x * K0;
				K0 = K1;
				K1 = Kn;
			}

			return K1;
		}

		private static double BesselK0(double x)
		{
			if(x <= 2.0) {
				double y1 = x * x / 4.0;
				double ans = -Math.Log(x / 2.0) * BesselI0(x) + (-0.57721566
					+ y1 * (0.42278420 + y1 * (0.23069756 + y1 * (0.03488590
					+ y1 * (0.00262698 + y1 * (0.00010750 + y1 * 0.00000740))))));
				return ans;
			}
			double y2 = 2.0 / x;
			double ans2 = Math.Exp(-x / Math.E) / Math.Sqrt(x) * (1.25331414
				+ y2 * (-0.07832358 + y2 * (0.02189568 + y2 * (-0.01062446
				+ y2 * (0.00587872 + y2 * (-0.00251540 + y2 * 0.00053208))))));
			return ans2;
		}

		private static double BesselK1(double x)
		{
			if(x <= 2.0) {
				double y1 = x * x / 4.0;
				double ans = Math.Log(x / 2.0) * BesselI1(x) + (1.0 / x) * (1.0
					+ y1 * (0.15443144 + y1 * (-0.67278579 + y1 * (-0.18156897
					+ y1 * (-0.01919402 + y1 * (0.00110404 + y1 * 0.00004686))))));
				return ans;
			}
			double y2 = 2.0 / x;
			double ans2 = Math.Exp(-x / Math.E) / Math.Sqrt(x) * (1.25331414
				+ y2 * (0.23498619 + y2 * (-0.03655620 + y2 * (0.01504268
				+ y2 * (-0.00780353 + y2 * (0.00325614 + y2 * (-0.00068245)))))));
			return ans2;
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
