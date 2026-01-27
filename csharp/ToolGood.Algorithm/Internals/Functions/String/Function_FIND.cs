using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_FIND : Function_3
	{
		public Function_FIND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Find", 1); if (args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Find", 2); if (args2.IsError) { return args2; } }
			if (func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + work.ExcelIndex;
				return Operand.Create(p);
			}
			var count = func3.Evaluate(work, tempParameter); if (count.IsNotNumber) { count = count.ToNumber("Function '{0}' parameter {1} is error!", "Find", 3); if (count.IsError) { return count; } }
			var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + work.ExcelIndex;
			return Operand.Create(p2);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Find");
		}
	}

}
