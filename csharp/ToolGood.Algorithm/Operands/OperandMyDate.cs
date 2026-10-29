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

		internal override bool TryGetText(out Operand value) { value = Create(DateValue.ToString()); return true; }
		internal override bool TryAppendText(System.Text.StringBuilder sb) { sb.Append(DateValue.ToString()); return true; }

		internal override bool TryGetDate(out Operand value) { value = this; return true; }

		public override string ToString() { return $"\"{DateValue}\""; }
	}
}