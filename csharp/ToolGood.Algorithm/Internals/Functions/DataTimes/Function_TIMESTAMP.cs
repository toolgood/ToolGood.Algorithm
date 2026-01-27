using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DataTimes
{
	internal class Function_TIMESTAMP : Function_2
    {
        public Function_TIMESTAMP(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args0 = func1.Evaluate(work, tempParameter); if (args0.IsError) { return args0; }

            int type = 0; // 毫秒
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TimeStamp", 2); if (args2.IsError) { return args2; } }
                type = args2.IntValue;
            }
            DateTime args1;
            if (work.UseLocalTime) {
                args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1).DateValue.ToDateTime(DateTimeKind.Local).ToUniversalTime();
            } else {
                args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1).DateValue.ToDateTime(DateTimeKind.Utc);
            }
            if (type == 0) {
				var ms = (args1 - FunctionUtil.StartDateUtc).TotalMilliseconds;
                return Operand.Create(ms);
            } else if (type == 1) {
                var s = (args1 - FunctionUtil.StartDateUtc).TotalSeconds;
                return Operand.Create(s);
            }
            return Operand.Error("Function '{0}' parameter is error!", "TimeStamp");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TimeStamp");
        }
    }

}
