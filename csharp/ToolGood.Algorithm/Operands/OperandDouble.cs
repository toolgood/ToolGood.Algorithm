using System.Globalization;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandDouble : Operand
	{
		private readonly double _value;
		public OperandDouble(double obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override bool IsNotNumber => false;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => (decimal)_value;
		public override long LongValue => (long)_value;
		public override double DoubleValue => (double)_value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return DoubleValue != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return DoubleValue != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(DoubleValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(DoubleValue.ToString(CultureInfo.InvariantCulture)); }

		public override Operand ToMyDate(string errorMessage) { return Create((MyDate)NumberValue); }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return Create((MyDate)NumberValue); }

		public override string ToString() { return NumberValue.ToString(CultureInfo.InvariantCulture); }
	}

}