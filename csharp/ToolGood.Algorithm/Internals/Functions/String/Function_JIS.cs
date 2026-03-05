using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_JIS : Function_1
	{
		public Function_JIS(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Jis";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(F_base_ToSBC(args1.TextValue));
		}

		private static string F_base_ToSBC(string input)
		{
			bool needModify = false;
			for (int i = 0; i < input.Length; i++) {
				var c = input[i];
				if (c == ' ' || c < 127) {
					needModify = true;
					break;
				}
			}
			if (!needModify) {
				return input;
			}
			char[] chars = input.ToCharArray();
			Span<char> span = chars;
			for (int i = 0; i < span.Length; i++) {
				var c = span[i];
				if (c == ' ') {
					span[i] = (char)12288;
				} else if (c < 127) {
					span[i] = (char)(c + 65248);
				}
			}
			return new string(chars);
		}

	}

}
