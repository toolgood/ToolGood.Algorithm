using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_SUBSTRING : Function_3
	{
		public Function_SUBSTRING(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "Substring";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex).ToString());
			}

			var args3 = GetNumber_3(work, tempParameter);
			if(args3.IsError) { return args3; }

			return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
		}

	}


}
