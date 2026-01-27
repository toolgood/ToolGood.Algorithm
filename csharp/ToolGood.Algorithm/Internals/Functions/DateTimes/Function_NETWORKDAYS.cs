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

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = funcs[0].Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Evaluate(work, tempParameter); if (args2.IsNotDate) { args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 2); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            var list = new HashSet<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Evaluate(work, tempParameter); if (ar.IsNotDate) { ar = ar.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", i + 1); if (ar.IsError) { return ar; } }
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
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NetWorkdays");
        }
    }

}
