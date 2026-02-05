using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_PROPER : Function_1
	{
		public Function_PROPER(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Proper";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "Proper", 1);
			if (args1.IsError) { return args1; }

			var text = args1.TextValue;
			if (string.IsNullOrEmpty(text)) {
				return Operand.Create(text);
			}
			bool needModify = false;
			bool isFirst = true;
			for (int i = 0; i < text.Length; i++) {
				var t = text[i];
				if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
					isFirst = true;
				} else if (isFirst) {
					if (char.IsLower(t)) {
						needModify = true;
						break;
					}
					isFirst = false;
				}
			}
			if (!needModify) {
				return args1; // no change
			}
			char[] chars = text.ToCharArray();
			Span<char> span = chars;
			isFirst = true;
			for (int i = 0; i < span.Length; i++) {
				var t = span[i];
				if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
					isFirst = true;
				} else if (isFirst) {
					span[i] = char.ToUpper(t);
					isFirst = false;
				}
			}
			return Operand.Create(new string(chars));
		}

	}

}
