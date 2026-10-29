using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
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

		internal override bool TryGetNumber(out Operand value) { value = _value ? One : Zero; return true; }

		internal override bool TryGetBoolean(out Operand value) { value = this; return true; }

		internal override bool TryGetText(out Operand value) { value = Create(_value ? "TRUE" : "FALSE"); return true; }
		internal override bool TryAppendText(System.Text.StringBuilder sb) { sb.Append(_value ? "TRUE" : "FALSE"); return true; }

		public override string ToString() { return _value ? "true" : "false"; }
	}
}