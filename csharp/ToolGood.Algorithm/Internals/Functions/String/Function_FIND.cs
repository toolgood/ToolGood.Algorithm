using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_FIND : Function_3
	{
		public Function_FIND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "Find";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			if (func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + work.ExcelIndex;
				return Operand.Create(p);
			}
			var count = GetNumber_3(work, tempParameter);
			if (count.IsError) { return count; }
			var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + work.ExcelIndex;
			return Operand.Create(p2);
		}

	}

}
