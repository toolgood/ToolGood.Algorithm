using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_EXACT : Function_2
	{
		public Function_EXACT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Exact";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			return Operand.Create(args1.TextValue == args2.TextValue);
		}

	}

}
