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
				return ParameterError(1);
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
				decimal ans = -MathEx.Log(x / 2.0m) * BesselI0(x) + (-0.57721566490153286061m
					+ y1 * (0.42278433509846713939m + y1 * (0.230697567077446m + y1 * (0.034885890266341m
					+ y1 * (0.002626979711643m + y1 * (0.000107502176243m + y1 * 0.000007400456812m))))));
				return ans;
			}
			decimal y2 = 2.0m / x;
			decimal ans2 = MathEx.Exp(-x) / MathEx.Sqrt(x) * (1.253314137315500m
				+ y2 * (-0.078323582855262m + y2 * (0.021895687854228m + y2 * (-0.010624628097740m
				+ y2 * (0.005878072214632m + y2 * (-0.002515401617640m + y2 * 0.000532080305632m))))));
			return ans2;
		}

		private static decimal BesselK1(decimal x)
		{
			if(x <= 2.0m) {
				decimal y1 = x * x / 4.0m;
				decimal ans = MathEx.Log(x / 2.0m) * BesselI1(x) + (1.0m / x) * (1.0m
					+ y1 * (0.154431442036717m + y1 * (-0.672785797513523m + y1 * (-0.181568943578864m
					+ y1 * (-0.019194020400716m + y1 * (0.001104044918568m + y1 * 0.000046862429868m))))));
				return ans;
			}
			decimal y2 = 2.0m / x;
			decimal ans2 = MathEx.Exp(-x) / MathEx.Sqrt(x) * (1.253314137315500m
				+ y2 * (0.234986192707248m + y2 * (-0.036556202034020m + y2 * (0.015042680553908m
				+ y2 * (-0.007803534366237m + y2 * (0.003256142832609m + y2 * (-0.000682450383692m)))))));
			return ans2;
		}

		private static decimal BesselI0(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 3.75m) {
				decimal y1 = x / 3.75m;
				y1 *= y1;
				return 1.0m + y1 * (3.515622965380465m + y1 * (3.089942465562116m + y1 * (1.206749160761352m
					+ y1 * (0.265973256598487m + y1 * (0.036076845538912m + y1 * 0.004581327358717m)))));
			}
			decimal y2 = 3.75m / ax;
			return (MathEx.Exp(ax) / MathEx.Sqrt(2.0m * MathEx.PI * ax)) * (0.398942280401433m + y2 * (0.013285921344730m
				+ y2 * (0.002253193626842m + y2 * (-0.001575649875251m + y2 * (0.009162816703917m
				+ y2 * (-0.020577062932649m + y2 * (0.026355373177924m + y2 * (-0.016476329612910m + y2 * 0.003923769605236m))))))));
		}

		private static decimal BesselI1(decimal x)
		{
			decimal ax = Math.Abs(x);
			if(ax < 3.75m) {
				decimal y1 = x / 3.75m;
				y1 *= y1;
				return x * (0.5m + y1 * (0.878905941521392m + y1 * (0.514988692842374m + y1 * (0.150849342225664m
					+ y1 * (0.026587328231117m + y1 * (0.003015319414231m + y1 * 0.000324111013968m))))));
			}
			decimal y2 = 3.75m / ax;
			decimal ans = (MathEx.Exp(ax) / MathEx.Sqrt(2.0m * MathEx.PI * ax)) * (0.398942280401433m + y2 * (-0.039880242337502m
				+ y2 * (-0.003620182649157m + y2 * (0.001638105403528m + y2 * (-0.010315550635288m
				+ y2 * (0.022829679456897m + y2 * (-0.028953129286367m + y2 * (0.017876545768998m - y2 * 0.004200596567986m))))))));
			return (x < 0) ? -ans : ans;
		}
	}
}
