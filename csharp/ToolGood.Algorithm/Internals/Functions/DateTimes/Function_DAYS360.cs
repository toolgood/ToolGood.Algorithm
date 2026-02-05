using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_DAYS360 : Function_3
    {
        public Function_DAYS360(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "Days360";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToDate(args1, "Days360", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToDate(args2, "Days360", 2);
			if (args2.IsError) { return args2; }

			var startMyDate = (DateTime)args1.DateValue;
			var endMyDate = (DateTime)args2.DateValue;

			var method = false;
			if (func3 != null) {
				var args3 = func3.Evaluate(work, tempParameter);
				args3 = FunctionUtil.ConvertToBoolean(args3, "Days360", 3);
				if (args3.IsError) { return args3; }
				method = args3.BooleanValue;
			}
            var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                        - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
            if (method) {
                if (endMyDate.Day == 31) days += 30;
                if (startMyDate.Day == 31) days -= 30;
            } else {
                if (startMyDate.Month == 12) {
                    if (startMyDate.Day == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                } else {
                    if (startMyDate.Day == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                }
                if (endMyDate.Month == 12) {
                    if (endMyDate.Day == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                } else {
                    if (endMyDate.Day == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                }
            }
            return Operand.Create(days);
        }

    }

}
