using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_CONCATENATE : Function_N
	{
		public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Concatenate";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length == 0) {
				return Operand.Create("");
			}
			if (funcs.Length == 1) {
				var a = funcs[0].Evaluate(work, tempParameter);
				a = FunctionUtil.ConvertToText(a, "Concatenate", 1);
				if (a.IsError) { return a; }
				return a; // 只有一个
			}
			var sb = new StringBuilder();
			for (int i = 0; i < funcs.Length; i++) {
				var a = funcs[i].Evaluate(work, tempParameter);
				a = FunctionUtil.ConvertToText(a, "Concatenate", i + 1);
				if (a.IsError) { return a; }
				sb.Append(a.TextValue);
			}
			return Operand.Create(sb.ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Concatenate");
		}
	}

}
