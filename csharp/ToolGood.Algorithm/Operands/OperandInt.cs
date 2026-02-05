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
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => _value;
		public override decimal NumberValue => _value;
		public override long LongValue => _value;
		public override double DoubleValue => _value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage)
		{
			if(_value == 0) {
				return False;
			} else if(_value == 1) {
				return True;
			}
			return base.ToBoolean(errorMessage);
		}
		public override Operand ToBoolean(string errorMessage, params object[] args)
		{
			if(_value == 0) {
				return False;
			} else if(_value == 1) {
				return True;
			}
			return base.ToBoolean(errorMessage);
		}

		public override Operand ToText(string errorMessage) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }

		public override string ToString() { return IntValue.ToString(CultureInfo.InvariantCulture); }
	}
}