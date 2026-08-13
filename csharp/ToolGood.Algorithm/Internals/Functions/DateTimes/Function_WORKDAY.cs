using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_WORKDAY : Function_N
    {
        public Function_WORKDAY(FunctionBase[] funcs) : base(funcs)
        {
            if (funcs.Length < 2) {
                throw new ArgumentException($"Function '{Name}' requires at least 2 parameters.");
            }
        }

        public override string Name => "Workday";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate(engine, tempParameter, 0);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber(engine, tempParameter, 1);
			if (args2.IsErrorOrNone) { return args2; }

			var startMyDate = args1.DateValue.ToDateTime();
			var days = args2.IntValue;
			var list = new HashSet<DateTime>();
			for (int i = 2; i < funcs.Length; i++) {
				var ar = GetDate(engine, tempParameter, i);
				if (ar.IsErrorOrNone) { return ar; }
				list.Add(ar.DateValue.ToDateTime());
			}
            if (days > 0) {
                // 先逐天对齐到周一（最多 6 天），期间消耗工作日
                while (startMyDate.DayOfWeek != DayOfWeek.Monday && days > 0) {
                    startMyDate = startMyDate.AddDays(1);
                    if (startMyDate.DayOfWeek == DayOfWeek.Saturday || startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                    if (list.Contains(startMyDate)) continue;
                    days--;
                }
                if (days == 0) return Operand.Create(startMyDate);

                // 整周粗跳：起点已是周一，每 5 个工作日 = 7 天
                var afterJump = startMyDate.AddDays((days / 5) * 7);
                var extra = 0;
                foreach (var h in list) {
                    if (h.Date > startMyDate.Date && h.Date <= afterJump.Date
                        && h.DayOfWeek != DayOfWeek.Saturday && h.DayOfWeek != DayOfWeek.Sunday) {
                        extra++;
                    }
                }
                startMyDate = afterJump;
                days = days % 5 + extra;
                while (days > 0) {
                    startMyDate = startMyDate.AddDays(1);
                    if (startMyDate.DayOfWeek == DayOfWeek.Saturday || startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                    if (list.Contains(startMyDate)) continue;
                    days--;
                }
            } else if (days < 0) {
                // 先逐天对齐到周五（最多 6 天），期间消耗工作日
                while (startMyDate.DayOfWeek != DayOfWeek.Friday && days < 0) {
                    startMyDate = startMyDate.AddDays(-1);
                    if (startMyDate.DayOfWeek == DayOfWeek.Saturday || startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                    if (list.Contains(startMyDate)) continue;
                    days++;
                }
                if (days == 0) return Operand.Create(startMyDate);

                // 整周粗跳：起点已是周五，每 5 个工作日 = 7 天
                var afterJump = startMyDate.AddDays((-days / 5) * -7);
                var extra = 0;
                foreach (var h in list) {
                    if (h.Date >= afterJump.Date && h.Date < startMyDate.Date
                        && h.DayOfWeek != DayOfWeek.Saturday && h.DayOfWeek != DayOfWeek.Sunday) {
                        extra++;
                    }
                }
                startMyDate = afterJump;
                days = -((-days) % 5) - extra;
                while (days < 0) {
                    startMyDate = startMyDate.AddDays(-1);
                    if (startMyDate.DayOfWeek == DayOfWeek.Saturday || startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                    if (list.Contains(startMyDate)) continue;
                    days++;
                }
            }
            return Operand.Create(startMyDate);
        }
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			funcs[0].GetParameterTypes(noneEngine, result, OperandType.DATE);
			funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			for(int i = 2; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.DATE);
			}
		}
	}

}