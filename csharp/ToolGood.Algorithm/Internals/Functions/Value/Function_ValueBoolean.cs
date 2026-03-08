using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ValueBoolean : FunctionBase
	{
		private readonly bool _value;
		public Function_ValueBoolean(bool value)
		{
			_value = value;
		}

		public override string Name => _value ? "True" : "False";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _value ? Operand.True : Operand.False;
		}

		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(Name);
		}
	}
}
