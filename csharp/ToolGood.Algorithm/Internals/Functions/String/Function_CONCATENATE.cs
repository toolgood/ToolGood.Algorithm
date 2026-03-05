using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_CONCATENATE : Function_N
	{
		public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Concatenate";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length == 0) {
				return Operand.Create("");
			}
			if (funcs.Length == 1) {
				var a = GetText(engine, tempParameter, 0);
				if (a.IsError) { return a; }
				return a; // 只有一�?
			}
			var sb = new StringBuilder();
			for (int i = 0; i < funcs.Length; i++) {
				var a = GetText(engine, tempParameter, i);
				if (a.IsError) { return a; }
				sb.Append(a.TextValue);
			}
			return Operand.Create(sb.ToString());
		}

	}
}