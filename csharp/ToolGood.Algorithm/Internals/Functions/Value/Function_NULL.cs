using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_NULL : FunctionBase
	{
		public Function_NULL()
		{
		}

		public override string Name => "NULL";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return Operand.Null;
		}

		public override OperandType GetResultType()
		{
			return OperandType.NULL;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append("NULL");
		}
	}
}
