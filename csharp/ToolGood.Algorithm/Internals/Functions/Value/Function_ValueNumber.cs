using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ValueNumber : Function_0
	{
		private readonly Operand _value;
		private readonly string _showName;

		public Function_ValueNumber(Operand value, string showName)
		{
			_value = value;
			_showName = showName;
		}

		public override string Name => _showName;

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _value;
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append(_showName);
		}
	}
}
