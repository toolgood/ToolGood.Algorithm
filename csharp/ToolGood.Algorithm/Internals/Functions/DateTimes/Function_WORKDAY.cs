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

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = funcs[0].Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "Workday", 1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Workday", 2); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var days = args2.IntValue;
            var list = new HashSet<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Evaluate(work, tempParameter); if (ar.IsNotDate) { ar = ar.ToMyDate("Function '{0}' parameter {1} is error!", "Workday", i + 1); if (ar.IsError) { return ar; } }
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
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Workday");
        }
    }

}
