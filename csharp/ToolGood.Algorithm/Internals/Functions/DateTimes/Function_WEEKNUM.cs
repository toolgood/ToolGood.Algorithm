using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_WEEKNUM : Function_2
    {
		public Function_WEEKNUM(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Weeknum";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var startMyDate = (DateTime)args1.DateValue;

			var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
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
