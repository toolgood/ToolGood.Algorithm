using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_SEARCH : Function_3
	{
		public Function_SEARCH(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Search";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if (args2.IsError) { return args2; }

			if (func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + engine.ExcelIndex;
				return Operand.Create(p);
			}
			var args3 = GetNumber_3(engine, tempParameter);
			if (args3.IsError) { return args3; }
			var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + engine.ExcelIndex;
			return Operand.Create(p2);
		}

	}

}
