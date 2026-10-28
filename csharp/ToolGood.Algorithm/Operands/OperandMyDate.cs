using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandMyDate : Operand
	{
		private readonly MyDate _value;
		public OperandMyDate(MyDate obj)
		{
			_value = obj;
		}
		public override bool IsDate => true;
		public override OperandType Type => OperandType.DATE;
		public override MyDate DateValue => _value;

		public override Operand ToText(string errorMessage) { return Create(DateValue.ToString()); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(DateValue.ToString()); }
		internal override bool TryGetText(out Operand value) { value = Create(DateValue.ToString()); return true; }

		public override Operand ToMyDate(string errorMessage) { return this; }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return this; }

		internal override bool TryGetDate(out Operand value) { value = this; return true; }

		public override string ToString() { return $"\"{DateValue}\""; }
	}
}