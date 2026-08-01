using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_NETWORKDAYS : Function_N
    {
        public Function_NETWORKDAYS(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 2 || funcs.Length > 3) {
                throw new ArgumentException($"Function '{Name}' requires 2 to 3 parameters.");
            }
        }

        public override string Name => "NetworkDays";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate(engine, tempParameter, 0);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetDate(engine, tempParameter, 1);
			if (args2.IsErrorOrNone) { return args2; }

			var startMyDate = args1.DateValue.ToDateTime();
			var endMyDate = args2.DateValue.ToDateTime();

			var list = new HashSet<DateTime>();
			for (int i = 2; i < funcs.Length; i++) {
				var ar = GetDate(engine, tempParameter, i);
				if (ar.IsErrorOrNone) { return ar; }
				list.Add(ar.DateValue.ToDateTime());
			}
            var days = 0;
            var negative = false;
            if (startMyDate > endMyDate) {
                var tmp = startMyDate;
                startMyDate = endMyDate;
                endMyDate = tmp;
                negative = true;
            }
            // 数学统计工作日:完整周数 × 5 + 余数逐天,再减去落在工作日的节假日
            var startDay = startMyDate.Date;
            var endDay = endMyDate.Date;
            var totalDays = (endDay - startDay).Days + 1;
            var fullWeeks = totalDays / 7;
            var remainder = totalDays % 7;
            days = fullWeeks * 5;
            for (int i = 0; i < remainder; i++) {
                var d = startDay.AddDays(i);
                if (d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday) {
                    days++;
                }
            }
            foreach (var h in list) {
                var hd = h.Date;
                if (hd >= startDay && hd <= endDay
                    && hd.DayOfWeek != DayOfWeek.Sunday && hd.DayOfWeek != DayOfWeek.Saturday) {
                    days--;
                }
            }
            return Operand.Create(negative ? -days : days);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			foreach(var item in funcs) {
				item.GetParameterTypes(noneEngine, result, OperandType.DATE);
			}
		}
	}

}