using System.Globalization;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandInt : Operand
	{
		private readonly int _value;
		public OperandInt(int obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override bool IsNotNumber => false;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => _value;
		public override decimal NumberValue => _value;
		public override long LongValue => _value;
		public override double DoubleValue => _value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return IntValue != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return IntValue != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }

		public override Operand ToMyDate(string errorMessage) { return Create((MyDate)NumberValue); }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return Create((MyDate)NumberValue); }

		public override string ToString() { return IntValue.ToString(CultureInfo.InvariantCulture); }
	}
}