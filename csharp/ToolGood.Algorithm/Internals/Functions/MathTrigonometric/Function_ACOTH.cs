using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_ACOTH : Function_1
	{
		public Function_ACOTH(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Acoth";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var d = args1.NumberValue;
			if(Math.Abs(d) <= 1) {
				return FunctionError();
			}
			return Operand.Create(0.5m * MathEx.Log((d + 1) / (d - 1)));
		}
	}
}
