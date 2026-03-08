using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_FIND : Function_3
	{
		public Function_FIND(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Find";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
			if (func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + engine.ExcelIndex;
				return Operand.Create(p);
			}
			var count = GetNumber_3(engine, tempParameter);
			if (count.IsError) { return count; }
			var startIndex = count.IntValue;
			if (startIndex < 0 || startIndex > args2.TextValue.Length) {
				return ParameterError(3);
			}
			var p2 = args2.TextValue.AsSpan(startIndex).IndexOf(args1.TextValue);
			if (p2 < 0) {
				return Operand.Create(0);
			}
			return Operand.Create(p2 + startIndex + engine.ExcelIndex);
		}
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}

}
