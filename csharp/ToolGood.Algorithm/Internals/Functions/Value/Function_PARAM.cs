using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_PARAM : Function_2
	{
		public Function_PARAM(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_PARAM(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Param";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
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

	}

}
