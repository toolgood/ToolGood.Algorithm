using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_WEEKNUM : Function_2
    {
		public Function_WEEKNUM(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Weeknum";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var startMyDate = args1.DateValue.ToDateTime();

			var returnType = 1;
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if (args2.IsErrorOrNone) { return args2; }
				returnType = args2.IntValue;
				if (returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 && returnType != 13
					&& returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17 ) {
					return ParameterError(2);
				}
			}

			if (returnType == 21) {
				var jan4 = new DateTime(startMyDate.Year, 1, 4);
				var daysSinceJan4 = (startMyDate - jan4).Days;
				var dayOfWeekJan4 = (int)jan4.DayOfWeek;
				var mondayOffset = dayOfWeekJan4 == 0 ? 6 : dayOfWeekJan4 - 1;
				var weekStart = jan4.AddDays(-mondayOffset);
				var weekNumber = (daysSinceJan4 + mondayOffset) / 7 + 1;
				return Operand.Create(weekNumber);
			}

			var jan1 = new DateTime(startMyDate.Year, 1, 1);
			var dayOfYear = startMyDate.DayOfYear;
			var dayOfWeekJan1 = (int)jan1.DayOfWeek;

			int weekStartDay;
			if (returnType == 1 || returnType == 17) {
				weekStartDay = 0;
			} else if (returnType == 2 || returnType == 11) {
				weekStartDay = 1;
			} else if (returnType == 12) {
				weekStartDay = 2;
			} else if (returnType == 13) {
				weekStartDay = 3;
			} else if (returnType == 14) {
				weekStartDay = 4;
			} else if (returnType == 15) {
				weekStartDay = 5;
			} else {
				weekStartDay = 6;
			}

			var daysUntilWeekStart = (dayOfWeekJan1 - weekStartDay + 7) % 7;
			var adjustedDayOfYear = dayOfYear + daysUntilWeekStart;
			var week = (int)Math.Ceiling(adjustedDayOfYear / 7.0m);

            return Operand.Create(week);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
