using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
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
			var x = args1.NumberValue;
			return Operand.Create(Erfc(x));
		}

		private static decimal Erfc(decimal x)
		{
			return 1.0m - Erf(x);
		}

		private static decimal Erf(decimal x)
		{
			const decimal a1 = 0.254829592m;
			const decimal a2 = -0.284496736m;
			const decimal a3 = 1.421413741m;
			const decimal a4 = -1.453152027m;
			const decimal a5 = 1.061405429m;
			const decimal p = 0.3275911m;

			int sign = x < 0 ? -1 : 1;
			x = Math.Abs(x);

			decimal t = 1.0m / (1.0m + p * x);
			decimal y = 1.0m - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * MathEx.Exp(-x * x);

			return sign * y;
		}
	}
}
