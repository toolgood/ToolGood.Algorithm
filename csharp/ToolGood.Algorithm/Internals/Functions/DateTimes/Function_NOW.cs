using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_NOW : FunctionBase
    {
        public override string Name => "Now";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            if (engine.UseLocalTime) {
                return Operand.Create(DateTime.Now);
            } else {
                return Operand.Create(DateTime.UtcNow);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Now()");
        }
    }

}
