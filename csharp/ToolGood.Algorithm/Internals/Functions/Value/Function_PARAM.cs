using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_PARAM : Function_2
	{
		public Function_PARAM(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText(); if (args1.IsError) return args1; }
			if (tempParameter != null) {
				var r = tempParameter(work, args1.TextValue);
				if (r != null) return r;
			}
			var result = work.GetParameter(args1.TextValue);
			if (result.IsError) {
				if (func2 != null) {
					return func2.Evaluate(work, tempParameter);
				}
			}
			return result;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Param");
		}
	}

}
