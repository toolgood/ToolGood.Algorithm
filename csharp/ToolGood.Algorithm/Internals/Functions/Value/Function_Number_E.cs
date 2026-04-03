using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_Number_E : Function_0
	{
		private static Operand _Operand=Operand.Create(MathEx.E);
		public override string Name => "E";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _Operand;
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append("E()");
		}
	}
}
