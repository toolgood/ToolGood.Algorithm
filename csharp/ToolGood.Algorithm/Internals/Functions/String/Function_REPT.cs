using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_REPT : Function_2
	{
		public Function_REPT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Rept";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
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