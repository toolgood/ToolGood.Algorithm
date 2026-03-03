using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_ACOT : Function_1
	{
		public Function_ACOT(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Acot";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(Math.PI / 2 - Math.Atan(args1.DoubleValue));
		}
	}
}
