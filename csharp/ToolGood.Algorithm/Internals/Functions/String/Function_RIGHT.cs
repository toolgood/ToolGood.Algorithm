using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_RIGHT : Function_2
	{
		public Function_RIGHT(FunctionBase func1) : base(func1, null)
		{
		}

		public Function_RIGHT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Right", 1); if (args1.IsError) { return args1; } }

			if (args1.TextValue.Length == 0) {
				return Operand.Create("");
			}
			if (func2 == null) {
				return Operand.Create(args1.TextValue.AsSpan(args1.TextValue.Length - 1, 1).ToString());
			}
			var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Right", 2); if (args2.IsError) { return args2; } }
			int length = Math.Min(args2.IntValue, args1.TextValue.Length);
			int start = args1.TextValue.Length - length;
			return Operand.Create(args1.TextValue.AsSpan(start, length).ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Right");
		}
	}

}
