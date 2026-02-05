using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	#region second minute hour month year day

	#endregion
	internal class Function_WEEKDAY : Function_2
    {
        public Function_WEEKDAY(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Weekday";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var type = 1;
			if (func2 != null) {
				var args2 = GetNumber_2(work, tempParameter);
				if (args2.IsError) { return args2; }
				type = args2.IntValue;
			}

            var t = ((DateTime)args1.DateValue).DayOfWeek;
            if (type == 1) {
                return Operand.Create((int)(t + 1));
            } else if (type == 2) {
                if (t == 0) return Operand.Create(7);
                return Operand.Create((int)t);
            }
            if (t == 0) {
                return Operand.Create(6);
            }
            return Operand.Create((int)(t - 1));
        }

    }

}
