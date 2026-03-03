using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ERFC : Function_1
	{
		public Function_ERFC(FunctionBase func) : base(func)
		{
		}

		public Function_ERFC(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Erfc";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var x = args1.DoubleValue;
			return Operand.Create(Erfc(x));
		}

		private static double Erfc(double x)
		{
			return 1.0 - Erf(x);
		}

		private static double Erf(double x)
		{
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;

			int sign = x < 0 ? -1 : 1;
			x = Math.Abs(x);

			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

			return sign * y;
		}
	}
}
