using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_MID : Function_3
	{
		public Function_MID(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Mid";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
			var args3 = GetNumber_3(engine, tempParameter);
			if (args3.IsError) { return args3; }

			var text = args1.TextValue;
			var startIndex = args2.IntValue - engine.ExcelIndex;
			var length = args3.IntValue;

			if (startIndex < 0 || length < 0) {
				return ParameterError(2);
			}
			if (startIndex >= text.Length) {
				return Operand.Create(string.Empty);
			}
			if (startIndex + length > text.Length) {
				length = text.Length - startIndex;
			}
			return Operand.Create(text.AsSpan(startIndex, length).ToString());
		}

	}

}
