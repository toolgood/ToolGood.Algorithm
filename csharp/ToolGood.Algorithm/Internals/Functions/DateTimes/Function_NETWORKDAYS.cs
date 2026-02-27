using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_NETWORKDAYS : Function_N
    {
        public Function_NETWORKDAYS(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "NetworkDays";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate(engine, tempParameter, 0);
			if (args1.IsError) { return args1; }

			var args2 = GetDate(engine, tempParameter, 1);
			if (args2.IsError) { return args2; }

			var startMyDate = (DateTime)args1.DateValue;
			var endMyDate = (DateTime)args2.DateValue;

			var list = new HashSet<DateTime>();
			for (int i = 2; i < funcs.Length; i++) {
				var ar = GetDate(engine, tempParameter, i);
				if (ar.IsError) { return ar; }
				list.Add(ar.DateValue);
			}
            var days = 0;
            while (startMyDate <= endMyDate) {
                if (startMyDate.DayOfWeek != DayOfWeek.Sunday && startMyDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startMyDate) == false) {
                        days++;
                    }
                }
                startMyDate = startMyDate.AddDays(1);
            }
            return Operand.Create(days);
        }

    }

}