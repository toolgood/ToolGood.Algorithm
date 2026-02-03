using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandMyDate : Operand
	{
		private readonly MyDate _value;
		public OperandMyDate(MyDate obj)
		{
			_value = obj;
		}
		public override bool IsDate => true;
		public override bool IsNotDate => false;
		public override OperandType Type => OperandType.DATE;
		public override MyDate DateValue => _value;

		public override Operand ToNumber(string errorMessage) { return Create((decimal)DateValue); }
		public override Operand ToNumber(string errorMessage, params object[] args) { return Create((decimal)DateValue); }

		public override Operand ToBoolean(string errorMessage) { return ((decimal)DateValue) != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return ((decimal)DateValue) != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(DateValue.ToString()); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(DateValue.ToString()); }

		public override Operand ToMyDate(string errorMessage) { return this; }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return this; }

		public override string ToString() { return $"\"{DateValue}\""; }
	}
}