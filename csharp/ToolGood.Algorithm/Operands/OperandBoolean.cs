using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm
{
	internal sealed class OperandBoolean : Operand
	{
		private readonly bool _value;
		public OperandBoolean(bool obj)
		{
			_value = obj;
		}
		public override bool IsBoolean => true;
		public override OperandType Type => OperandType.BOOLEAN;
		public override bool BooleanValue => _value;

		public override Operand ToNumber(string errorMessage) { return _value ? One : Zero; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return _value ? One : Zero; }

		public override Operand ToBoolean(string errorMessage) { return this; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return this; }

		public override Operand ToText(string errorMessage) { return Create(_value ? "TRUE" : "FALSE"); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(_value ? "TRUE" : "FALSE"); }

		public override string ToString() { return _value ? "true" : "false"; }
	}
}