using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

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
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

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

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
