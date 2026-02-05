using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_REPLACE : Function_4
	{
		public Function_REPLACE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
		{
		}

		public override string Name => "Replace";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var oldtext = args1.TextValue;
			if (func4 == null) {
				var args22 = GetText_2(work, tempParameter);
				if (args22.IsError) { return args22; }
				var args32 = GetText_3(work, tempParameter);
				if (args32.IsError) { return args32; }

				var old = args22.TextValue;
				var newstr = args32.TextValue;
				return Operand.Create(oldtext.Replace(old, newstr));
			}

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			var args3 = GetNumber_3(work, tempParameter);
			if (args3.IsError) { return args3; }
			var args4 = GetText_4(work, tempParameter);
			if (args4.IsError) { return args4; }

			var start = args2.IntValue - work.ExcelIndex;
			var length = args3.IntValue;
			var newtext = args4.TextValue;

			var sb = new StringBuilder(oldtext.Length - length + newtext.Length);
			for (int i = 0; i < oldtext.Length; i++) {
				if (i < start) {
					sb.Append(oldtext[i]);
				} else if (i == start) {
					sb.Append(newtext);
				} else if (i >= start + length) {
					sb.Append(oldtext[i]);
				}
			}
			return Operand.Create(sb.ToString());
		}

	}

}
