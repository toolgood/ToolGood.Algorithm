using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ShowNumber : FunctionBase
	{
		private readonly Operand _value;
		private readonly string _showName;

		public Function_ShowNumber(Operand value, string showName)
		{
			_value = value;
			_showName = showName;
		}

		public override string Name => throw new NotImplementedException();

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _value;
		}

		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(_showName);
		}
	}
}
