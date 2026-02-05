using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TODAY : FunctionBase
    {
        public override string Name => "Today";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            DateTime now;
            if (work.UseLocalTime) {
                now = DateTime.Now;
            } else {
                now = DateTime.UtcNow;
            }
            return Operand.Create(new MyDate(now.Year, now.Month, now.Day, 0, 0, 0));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Today()");
        }
    }

}
