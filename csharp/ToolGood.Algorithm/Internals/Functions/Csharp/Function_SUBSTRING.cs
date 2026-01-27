using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_SUBSTRING : Function_3
	{
		public Function_SUBSTRING(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Substring", 1); if(args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Substring", 2); if(args2.IsError) { return args2; } }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex).ToString());
			}
			var args3 = func3.Evaluate(work, tempParameter); if(args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Substring", 3); if(args3.IsError) { return args3; } }
			return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Substring");
		}
	}


}
