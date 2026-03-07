using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_DELTA : Function_N
	{
		public Function_DELTA(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Delta";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if(funcs.Length < 1) {
				return ParameterError(1);
			}

			var args1 = GetNumber(engine, tempParameter, 0);
			if(args1.IsError) { return args1; }
			var num1 = args1.NumberValue;

			decimal num2 = 0;
			if(funcs.Length >= 2) {
				var args2 = GetNumber(engine, tempParameter, 1);
				if(args2.IsError) { return args2; }
				num2 = args2.NumberValue;
			}

			return Operand.Create(num1 == num2 ? 1 : 0);
		}
	}
}
