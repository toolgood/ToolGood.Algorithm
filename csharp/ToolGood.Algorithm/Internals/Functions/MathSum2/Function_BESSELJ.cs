using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
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
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var x = args1.NumberValue;
			var n = (int)Math.Truncate(args2.NumberValue);

			return Operand.Create(BesselJ(n, x));
		}

		private static decimal BesselJ(int n, decimal x)
		{
			if(x == 0) {
				return (n == 0) ? 1.0m : 0.0m;
			}

			if(n < 0) n = -n;

			decimal ax = Math.Abs(x);
			if(ax < 1e-10m) {
				return (n == 0) ? 1.0m : 0.0m;
			}

			if(n == 0) return BesselJ0(x);
			if(n == 1) return BesselJ1(x);

			if(ax > n) {
				decimal J0 = BesselJ0(x);
				decimal J1 = BesselJ1(x);
				decimal Jn = 0;

				for(int k = 1; k < n; k++) {
					Jn = (2.0m * k / x) * J1 - J0;
					J0 = J1;
					J1 = Jn;
				}
				return J1;
			}

			int m = (int)(1.5 * n + 10);
			decimal[] J = new decimal[m + 2];
			J[m + 1] = 0.0m;
			J[m] = 1.0m;

			for(int k = m; k >= 1; k--) {
				J[k - 1] = (2.0m * k / x) * J[k] - J[k + 1];
			}

			decimal sum = 0.0m;
			for(int k = 0; k <= m; k += 2) {
				sum += 2.0m * J[k];
			}
			sum -= J[0];

			return J[n] / sum;
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
			decimal xx = ax - 0.78539816339744830962m;
			decimal ans3 = 1.0m + y2 * (-0.1098628627e-2m + y2 * (0.2734510407e-4m
				+ y2 * (-0.2073370639e-5m + y2 * 0.2093887211e-6m)));
			decimal ans4 = -0.1562499995e-1m + y2 * (0.1430488765e-3m
				+ y2 * (-0.6911147651e-5m + y2 * (0.7621095161e-6m
				- y2 * 0.934935152e-7m)));
			return MathEx.Sqrt(0.63661977236758134308m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
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
			decimal xx = ax - 2.35619449019234492885m;
			decimal ans3 = 1.0m + y2 * (0.183105e-2m + y2 * (-0.3516396496e-4m
				+ y2 * (0.2457520174e-5m + y2 * (-0.240337019e-6m))));
			decimal ans4 = 0.04687499995m + y2 * (-0.2002690873e-3m
				+ y2 * (0.8449199096e-5m + y2 * (-0.88228987e-6m
				+ y2 * 0.105787412e-6m)));
			decimal ans = MathEx.Sqrt(0.63661977236758134308m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
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
