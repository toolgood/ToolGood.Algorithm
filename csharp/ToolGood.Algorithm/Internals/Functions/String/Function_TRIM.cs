using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_TRIM : Function_1
	{
		public Function_TRIM(FunctionBase[] func1) : base(func1)
		{
		}

		public override string Name => "Trim";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.AsSpan().Trim().ToString());
		}

	}

}
