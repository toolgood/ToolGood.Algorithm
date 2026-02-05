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
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "Replace", 1);
			if (args1.IsError) { return args1; }
			var oldtext = args1.TextValue;
			if (func4 == null) {
				var args22 = func2.Evaluate(work, tempParameter);
				args22 = FunctionUtil.ConvertToText(args22, "Replace", 2);
				if (args22.IsError) { return args22; }
				var args32 = func3.Evaluate(work, tempParameter);
				args32 = FunctionUtil.ConvertToText(args32, "Replace", 3);
				if (args32.IsError) { return args32; }

				var old = args22.TextValue;
				var newstr = args32.TextValue;
				return Operand.Create(oldtext.Replace(old, newstr));
			}

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Replace", 2);
			if (args2.IsError) { return args2; }
			var args3 = func3.Evaluate(work, tempParameter);
			args3 = FunctionUtil.ConvertToNumber(args3, "Replace", 3);
			if (args3.IsError) { return args3; }
			var args4 = func4.Evaluate(work, tempParameter);
			args4 = FunctionUtil.ConvertToText(args4, "Replace", 4);
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
