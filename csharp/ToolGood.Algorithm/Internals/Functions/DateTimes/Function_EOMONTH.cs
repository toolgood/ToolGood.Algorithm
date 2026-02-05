using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_EOMONTH : Function_2
    {
		public Function_EOMONTH(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "EOMonth";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }

    }

}
