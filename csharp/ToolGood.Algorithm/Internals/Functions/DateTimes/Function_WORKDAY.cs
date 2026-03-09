using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_WORKDAY : Function_N
    {
        public Function_WORKDAY(FunctionBase[] funcs) : base(funcs)
        {
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
            while (days > 0) {
                startMyDate = startMyDate.AddDays(1);
                if (startMyDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startMyDate)) continue;
                days--;
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