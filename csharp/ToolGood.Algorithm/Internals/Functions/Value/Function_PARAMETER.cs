using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_PARAMETER : FunctionBase
	{
		private readonly string name;
		private readonly FunctionBase func1;

		public Function_PARAMETER(string name)
		{
			this.name = name;
		}

		public Function_PARAMETER(FunctionBase func1)
		{
			this.func1 = func1;
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var txt = name;
			if (string.IsNullOrEmpty(name)) {
				var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Parameter", 1); if (args1.IsError) return args1; }
				txt = args1.TextValue;
			} else {
				txt = name;
			}
			if (tempParameter != null) {
				var r = tempParameter(work, txt);
				if (r != null) return r;
			}
			return work.GetParameter(txt);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if (string.IsNullOrEmpty(name)) {
				func1.ToString(stringBuilder, false);
			} else {
				stringBuilder.Append(name);
			}
		}
	}

}
