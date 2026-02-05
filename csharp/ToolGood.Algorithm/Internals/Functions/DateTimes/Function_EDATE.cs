using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_EDATE : Function_2
    {
        public Function_EDATE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "EDate";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToDate(args1, "EDate", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToNumber(args2, "EDate", 2);
			if (args2.IsError) { return args2; }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }

    }

}
