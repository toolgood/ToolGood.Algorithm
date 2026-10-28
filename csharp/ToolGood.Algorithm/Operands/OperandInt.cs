using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
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
		/// <summary>整数原值，供四则运算/比较的整数快路径直接取用，避免一次 int→decimal 转换</summary>
		internal int RawInt => _value;
		public override int IntValue => _value;
		public override decimal NumberValue => _value;
		public override long LongValue => _value;
		public override double DoubleValue => _value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		internal override bool TryGetNumber(out Operand value) { value = this; return true; }

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
			return base.ToBoolean(errorMessage, args);
		}

		internal override bool TryGetBoolean(out Operand value)
		{
			// 只有 0/1 能转布尔，其余值原本就会得到 ToBoolean 的错误结果，此处返回 false 让调用方回落到原路径
			if(_value == 0) { value = False; return true; }
			if(_value == 1) { value = True; return true; }
			value = null;
			return false;
		}

		public override Operand ToText(string errorMessage) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }
		internal override bool TryGetText(out Operand value) { value = Create(IntValue.ToString(CultureInfo.InvariantCulture)); return true; }

		public override string ToString() { return IntValue.ToString(CultureInfo.InvariantCulture); }
	}
}