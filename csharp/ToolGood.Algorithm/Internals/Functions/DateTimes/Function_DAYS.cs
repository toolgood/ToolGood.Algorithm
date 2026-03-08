using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DAYS : Function_2
	{
		public Function_DAYS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_DAYS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Days";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetDate_2(engine, tempParameter);
			if (args2.IsError) { return args2; }

			var endDate = args1.DateValue.ToDateTime();
			var startDate = args2.DateValue.ToDateTime();

			return Operand.Create((endDate.Date - startDate.Date).Days);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
