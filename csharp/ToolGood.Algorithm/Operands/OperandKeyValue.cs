using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandKeyValue : Operand
	{
		private readonly KeyValue _value;
		public OperandKeyValue(KeyValue obj)
		{
			_value = obj;
		}
		public override bool IsArrayJson => true;
		public override OperandType Type => OperandType.ARRARYJSON;
		public KeyValue Value => _value;
	}
}