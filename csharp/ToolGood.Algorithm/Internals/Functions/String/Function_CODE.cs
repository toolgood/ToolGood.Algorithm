using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_CODE : Function_1
	{
		public Function_CODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Code";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			if (string.IsNullOrEmpty(args1.TextValue)) {
				return FunctionError();
			}
			char c = args1.TextValue[0];
			return Operand.Create((int)c);
		}

	}

}
