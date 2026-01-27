using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_WEEKNUM : Function_2
    {
        public Function_WEEKNUM(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekNum", 1); if (args1.IsError) { return args1; } }
            var startMyDate = (DateTime)args1.DateValue;

            var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekNum", 2); if (args2.IsError) { return args2; } }
                if (args2.IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "WeekNum");
        }
    }

}
