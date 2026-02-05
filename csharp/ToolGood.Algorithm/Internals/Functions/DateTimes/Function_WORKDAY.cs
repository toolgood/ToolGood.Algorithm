using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_WORKDAY : Function_N
    {
        public Function_WORKDAY(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Workday";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate(work, tempParameter, 0);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber(work, tempParameter, 1);
			if (args2.IsError) { return args2; }

			var startMyDate = (DateTime)args1.DateValue;
			var days = args2.IntValue;
			var list = new HashSet<DateTime>();
			for (int i = 2; i < funcs.Length; i++) {
				var ar = GetDate(work, tempParameter, i);
				if (ar.IsError) { return ar; }
				list.Add(ar.DateValue);
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

    }

}