using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_CLEAN : Function_1
	{
		public Function_CLEAN(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "Clean"); if (args1.IsError) { return args1; } }
			var t = args1.TextValue;
			bool needClean = false;
			for (int i = 0; i < t.Length; i++) {
				var c = t[i];
				if (c == '\f' || c == '\n' || c == '\r' || c == '\t' || c == '\v') {
					needClean = true;
					break;
				}
			}
			if (!needClean) {
				return args1; // no change
			}
			var sb = new StringBuilder(t.Length);
			for (int i = 0; i < t.Length; i++) {
				var c = t[i];
				if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
					sb.Append(c);
				}
			}
			return Operand.Create(sb.ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Clean");
		}
	}

}
