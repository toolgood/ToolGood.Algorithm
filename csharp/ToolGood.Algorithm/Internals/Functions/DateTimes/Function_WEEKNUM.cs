using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_WEEKNUM : Function_2
    {
        public Function_WEEKNUM(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Weeknum";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToDate(args1, "WeekNum", 1);
			if (args1.IsError) { return args1; }

			var startMyDate = (DateTime)args1.DateValue;

			var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
			if (func2 != null) {
				var args2 = func2.Evaluate(work, tempParameter);
				args2 = FunctionUtil.ConvertToNumber(args2, "WeekNum", 2);
				if (args2.IsError) { return args2; }
				if (args2.IntValue == 2) {
					days--;
				}
			}

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }

    }

}
