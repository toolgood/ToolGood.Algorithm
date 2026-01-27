using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_SUBSTITUTE : Function_4
	{
		public Function_SUBSTITUTE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Substitute", 1); if (args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Substitute", 2); if (args2.IsError) { return args2; } }
			var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "Substitute", 3); if (args3.IsError) { return args3; } }
			if (func4 == null) {
				return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
			}
			var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotNumber) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Substitute", 4); if (args4.IsError) { return args4; } }
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
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Substitute");
		}
	}

}
