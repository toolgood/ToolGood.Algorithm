using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_UPPER : Function_1
	{
		public Function_UPPER(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Upper";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "Upper", 1);
			if (args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.ToUpper());
		}

	}

}
