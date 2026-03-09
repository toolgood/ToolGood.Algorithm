using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
		}
	}
}
