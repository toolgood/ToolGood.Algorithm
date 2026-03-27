using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_NOW : Function_0
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
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}
	 
	}

}
