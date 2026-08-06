using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DAYS360 : Function_3
    {
		public Function_DAYS360(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2 || funcs.Length > 3) {
				throw new ArgumentException($"Function '{Name}' requires 2 to 3 parameters.");
			}
		}

        public override string Name => "Days360";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetDate_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }

			var startMyDate = args1.DateValue.ToDateTime();
			var endMyDate = args2.DateValue.ToDateTime();

			var method = false;
			if (func3 != null) {
				var args3 = GetBoolean_3(engine, tempParameter);
				if (args3.IsErrorOrNone) { return args3; }
				method = args3.BooleanValue;
			}
            var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                        - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
            if (method) {
                if (endMyDate.Day == 31) days += 30;
                if (startMyDate.Day == 31) days -= 30;
            } else {
                // US (NASD) 方法: start 若为月末(含 2 月最后一天)则调整为 30 日,
                // end 若为月末, 依据调整后的 startDay 决定按 31 日(下月1日)或 30 日计算
                var startDay = startMyDate.Day;
                var endDay = endMyDate.Day;
                if (startMyDate.Month == 12) {
                    if (startDay == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        startDay = 30;
                    }
                } else {
                    if (startDay == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
                        startDay = 30;
                    }
                }
                if (endMyDate.Month == 12) {
                    if (endDay == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        endDay = startDay < 30 ? 31 : 30;
                    }
                } else {
                    if (endDay == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
                        endDay = startDay < 30 ? 31 : 30;
                    }
                }
                days += endDay - startDay;
            }
            return Operand.Create(days);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			}
		}
	}

}
