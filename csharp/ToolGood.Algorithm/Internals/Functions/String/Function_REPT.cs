using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_REPT : Function_2
	{
		public Function_REPT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_REPT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Rept";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }

			var newtext = args1.TextValue;
			var length = args2.IntValue;
			if (length < 0) {
				return ParameterError(2);
			}
			if (length == 0) {
				return Operand.Create("");
			}
			var sb = new StringBuilder(newtext.Length * length);
			for (int i = 0; i < length; i++) {
				sb.Append(newtext);
			}
			return Operand.Create(sb.ToString());
		}

	}
}