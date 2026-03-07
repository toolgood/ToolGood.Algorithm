using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
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

			var x = args1.NumberValue;
			var n = (int)Math.Truncate(args2.NumberValue);

			if(x <= 0) {
				return FunctionError();
			}

			return Operand.Create(BesselK(n, x));
		}

		private static decimal BesselK(int n, decimal x)
		{
			if(n < 0) n = -n;

			if(n == 0) return BesselK0(x);
			if(n == 1) return BesselK1(x);

			decimal K0 = BesselK0(x);
			decimal K1 = BesselK1(x);
			decimal Kn = 0;

			for(int k = 1; k < n; k++) {
				Kn = K1 + 2.0m * k / x * K0;
				K0 = K1;
				K1 = Kn;
			}

			return K1;
		}

		private static decimal BesselK0(decimal x)
		{
			if(x <= 2.0m) {
				decimal y1 = x * x / 4.0m;
				decimal ans = -MathEx.Log(x / 2.0m) * BesselI0(x) + (-0.57721566m
					+ y1 * (0.42278420m + y1 * (0.23069756m + y1 * (0.03488590m
					+ y1 * (0.00262698m + y1 * (0.00010750m + y1 * 0.00000740m))))));
				return ans;
			}
			decimal y2 = 2.0m / x;
			decimal ans2 = MathEx.Exp(-x) / MathEx.Sqrt(x) * (1.25331414m
				+ y2 * (-0.07832358m + y2 * (0.02189568m + y2 * (-0.01062446m
				+ y2 * (0.00587872m + y2 * (-0.00251540m + y2 * 0.00053208m))))));
			return ans2;
		}

		private static decimal BesselK1(decimal x)
		{
			if(x <= 2.0m) {
				decimal y1 = x * x / 4.0m;
				decimal ans = MathEx.Log(x / 2.0m) * BesselI1(x) + (1.0m / x) * (1.0m
					+ y1 * (0.15443144m + y1 * (-0.67278579m + y1 * (-0.18156897m
					+ y1 * (-0.01919402m + y1 * (0.00110404m + y1 * 0.00004686m))))));
				return ans;
			}
			decimal y2 = 2.0m / x;
			decimal ans2 = MathEx.Exp(-x) / MathEx.Sqrt(x) * (1.25331414m
				+ y2 * (0.23498619m + y2 * (-0.03655620m + y2 * (0.01504268m
				+ y2 * (-0.00780353m + y2 * (0.00325614m + y2 * (-0.00068245m)))))));
			return ans2;
		}

		private static decimal BesselI0(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 3.75m) {
				decimal y1 = x / 3.75m;
				y1 *= y1;
				return 1.0m + y1 * (3.5156229m + y1 * (3.0899424m + y1 * (1.2067492m
					+ y1 * (0.2659732m + y1 * (0.0360768m + y1 * 0.0045813m)))));
			}
			decimal y2 = 3.75m / ax;
			return (MathEx.Exp(ax) / MathEx.Sqrt(2.0m * MathEx.PI * ax)) * (0.39894228m + y2 * (0.01328592m
				+ y2 * (0.00225319m + y2 * (-0.00157565m + y2 * (0.00916281m
				+ y2 * (-0.02057706m + y2 * (0.02635537m + y2 * (-0.01647633m + y2 * 0.00392377m))))))));
		}

		private static decimal BesselI1(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 3.75m) {
				decimal y1 = x / 3.75m;
				y1 *= y1;
				return x * (0.5m + y1 * (0.87890594m + y1 * (0.51498869m + y1 * (0.15084934m
					+ y1 * (0.02658733m + y1 * (0.00301532m + y1 * 0.00032411m))))));
			}
			decimal y2 = 3.75m / ax;
			decimal ans = (MathEx.Exp(ax) / MathEx.Sqrt(2.0m * MathEx.PI * ax)) * (0.39894228m + y2 * (-0.03988024m
				+ y2 * (-0.00362018m + y2 * (0.00163801m + y2 * (-0.01031555m
				+ y2 * (0.02282967m + y2 * (-0.02895312m + y2 * (0.01787654m - y2 * 0.00420059m))))))));
			return (x < 0) ? -ans : ans;
		}
	}
}
