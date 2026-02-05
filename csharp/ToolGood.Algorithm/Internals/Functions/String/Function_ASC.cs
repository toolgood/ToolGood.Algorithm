using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_ASC : Function_1
	{
		public Function_ASC(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Asc";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(F_base_ToDBC(args1.TextValue));
		}

		private static string F_base_ToDBC(string input)
		{
			bool needModify = false;
			for (int i = 0; i < input.Length; i++) {
				var c = input[i];
				if (c == 12288 || (c > 65280 && c < 65375)) {
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
				if (c == 12288) {
					span[i] = (char)32;
				} else if (c > 65280 && c < 65375) {
					span[i] = (char)(c - 65248);
				}
			}
			return new string(chars);
		}

	}

}
