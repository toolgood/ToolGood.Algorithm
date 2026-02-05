using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_SEARCH : Function_3
	{
		public Function_SEARCH(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_SEARCH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "Search";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(work, tempParameter);
			if (args2.IsError) { return args2; }

			if (func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + work.ExcelIndex;
				return Operand.Create(p);
			}
			var args3 = GetNumber_3(work, tempParameter);
			if (args3.IsError) { return args3; }
			var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + work.ExcelIndex;
			return Operand.Create(p2);
		}

	}

}
