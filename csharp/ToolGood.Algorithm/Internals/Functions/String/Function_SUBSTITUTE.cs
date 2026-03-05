using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_SUBSTITUTE : Function_4
	{
		public Function_SUBSTITUTE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Substitute";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
			var args3 = GetText_3(engine, tempParameter);
			if (args3.IsError) { return args3; }
			if (func4 == null) {
				return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
			}
			var args4 = GetNumber_4(engine, tempParameter);
			if (args4.IsError) { return args4; }
			string text = args1.TextValue;
			string oldtext = args2.TextValue;
			string newtext = args3.TextValue;
			int replaceIndex = args4.IntValue;

			if (oldtext.Length == 0) {
				return Operand.Create(text);
			}

			int estimatedCapacity = Math.Max(text.Length, text.Length + (newtext.Length - oldtext.Length));
			var sb = new StringBuilder(estimatedCapacity);
			int currentIndex = 0;
			int foundCount = 0;
			int searchPos = 0;

			while (searchPos <= text.Length - oldtext.Length) {
				int foundPos = text.IndexOf(oldtext, searchPos, StringComparison.Ordinal);
				if (foundPos < 0) break;

				foundCount++;
				if (foundCount == replaceIndex) {
					sb.Append(text.AsSpan(currentIndex, foundPos - currentIndex));
					sb.Append(newtext);
					currentIndex = foundPos + oldtext.Length;
					break;
				}
				searchPos = foundPos + oldtext.Length;
			}

			if (currentIndex == 0) {
				return Operand.Create(text);
			}
			sb.Append(text.AsSpan(currentIndex));
			return Operand.Create(sb.ToString());
		}

	}

}
