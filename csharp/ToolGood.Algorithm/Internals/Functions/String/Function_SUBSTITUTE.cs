using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_SUBSTITUTE : Function_4
	{
		public Function_SUBSTITUTE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Substitute";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			var args3 = GetText_3(work, tempParameter);
			if (args3.IsError) { return args3; }
			if (func4 == null) {
				return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
			}
			var args4 = GetNumber_4(work, tempParameter);
			if (args4.IsError) { return args4; }
			string text = args1.TextValue;
			string oldtext = args2.TextValue;
			string newtext = args3.TextValue;
			int index = args4.IntValue;

			int index2 = 0;
			int estimatedCapacity = Math.Max(text.Length, text.Length + (newtext.Length - oldtext.Length));
			var sb = new StringBuilder(estimatedCapacity);
			for (int i = 0; i < text.Length; i++) {
				bool b = true;
				for (int j = 0; j < oldtext.Length; j++) {
					if (i + j >= text.Length) {
						b = false;
						break;
					}
					var t = text[i + j];
					var t2 = oldtext[j];
					if (t != t2) {
						b = false;
						break;
					}
				}
				if (b) {
					index2++;
				}
				if (b && index2 == index) {
					sb.Append(newtext);
					i += oldtext.Length - 1;
				} else {
					sb.Append(text[i]);
				}
			}
			return Operand.Create(sb.ToString());
		}

	}

}
