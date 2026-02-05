using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_MID : Function_3
	{
		public Function_MID(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "Mid";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "Mid", 1);
			if (args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToNumber(args2, "Mid", 2);
			if (args2.IsError) { return args2; }
			var args3 = func3.Evaluate(work, tempParameter);
			args3 = ConvertToNumber(args3, "Mid", 3);
			if (args3.IsError) { return args3; }
			return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
		}

	}

}
