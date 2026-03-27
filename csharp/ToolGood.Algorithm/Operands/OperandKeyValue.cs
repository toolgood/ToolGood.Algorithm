using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandKeyValue : Operand
	{
		private readonly KeyValue _value;
		public OperandKeyValue(KeyValue obj)
		{
			_value = obj;
		}
		public override bool IsArrayJson => true;
		public override OperandType Type => OperandType.ARRAYJSON;
		public KeyValue Value => _value;
	}
}