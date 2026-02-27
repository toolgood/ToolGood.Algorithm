using System;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LOG10 : Function_1
	{
		public Function_LOG10(FunctionBase func1) : base(func1)
		{
		}

		public Function_LOG10(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Log10";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			return Operand.Create(Math.Log10(args1.DoubleValue));
		}
	}
}
