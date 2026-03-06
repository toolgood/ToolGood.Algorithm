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

			var x = args1.NumberValue;
			var n = (int)Math.Truncate(args2.NumberValue);

			return Operand.Create(BesselI(n, x));
		}

		private static decimal BesselI(int n, decimal x)
		{
			if(x < 0) {
				return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
			}
			if(x == 0) {
				return (n == 0) ? 1.0m : 0.0m;
			}

			decimal ax = Math.Abs(x);
			if(ax < 1e-10m) {
				return (n == 0) ? 1.0m : 0.0m;
			}

			if(n < 0) n = -n;

			if(ax > 700) {
				decimal factor = MathEx.Exp(ax) / MathEx.Sqrt(2 * MathEx.PI * ax);
				return factor * (1.0m - (4.0m * n * n - 1.0m) / (8.0m * ax));
			}

			if(n == 0) return BesselI0(x);
			if(n == 1) return BesselI1(x);

			decimal I0 = BesselI0(x);
			decimal I1 = BesselI1(x);
			decimal In = 0;

			for(int k = 1; k < n; k++) {
				In = I1 + 2.0m * k / x * I0;
				I0 = I1;
				I1 = In;
			}

			return I1;
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
			return (MathEx.Exp(ax / MathEx.E) / MathEx.Sqrt(ax)) * (0.39894228m + y2 * (0.01328592m
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
			decimal ans = (MathEx.Exp(ax / MathEx.E) / MathEx.Sqrt(ax)) * (0.39894228m + y2 * (-0.03988024m
				+ y2 * (-0.00362018m + y2 * (0.00163801m + y2 * (-0.01031555m
				+ y2 * (0.02282967m + y2 * (-0.02895312m + y2 * (0.01787654m - y2 * 0.00420059m))))))));
			return (x < 0) ? -ans : ans;
		}
	}
}
