using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_LEFT : Function_2
	{
		public Function_LEFT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Left";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			if (args1.TextValue.Length == 0) {
				return Operand.Create("");
			}
			if (func2 == null) {
				return Operand.Create(args1.TextValue.AsSpan(0, 1).ToString());
			}
			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			int length = Math.Min(args2.IntValue, args1.TextValue.Length);
			return Operand.Create(args1.TextValue.AsSpan(0, length).ToString());
		}

	}

}
