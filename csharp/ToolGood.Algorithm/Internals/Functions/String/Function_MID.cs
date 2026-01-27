using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_MID : Function_3
	{
		public Function_MID(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Mid", 1); if (args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Mid", 2); if (args2.IsError) { return args2; } }
			var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Mid", 3); if (args3.IsError) { return args3; } }
			return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Mid");
		}
	}

}
