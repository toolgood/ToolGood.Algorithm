using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandInt : Operand
	{
		private readonly int _value;
		/// <summary>
		/// 构造时算好的 decimal 形式。NumberValue 是聚合类函数（SUM/AVERAGE/MEDIAN…）的逐元素热点，
		/// 直接返字段可省掉每次一次 int→decimal 构造；对象因此由 24 字节变 32 字节
		/// </summary>
		private readonly decimal _asDecimal;

		public OperandInt(int obj)
		{
			_value = obj;
			_asDecimal = obj;
		}
		public override bool IsNumber => true;
		public override OperandType Type => OperandType.NUMBER;
		/// <summary>整数原值，供四则运算/比较的整数快路径直接取用，避免一次 int→decimal 转换</summary>
		internal int RawInt => _value;
		public override int IntValue => _value;
		public override decimal NumberValue => _asDecimal;
		public override long LongValue => _value;
		public override double DoubleValue => _value;

		internal override bool TryGetNumber(out Operand value) { value = this; return true; }

		internal override bool TryGetBoolean(out Operand value)
		{
			// 只有 0/1 能转布尔，其余值原本就会得到 ToBoolean 的错误结果，此处返回 false 让调用方回落到原路径
			if(_value == 0) { value = False; return true; }
			if(_value == 1) { value = True; return true; }
			value = null;
			return false;
		}

		internal override bool TryGetText(out Operand value) { value = Create(IntValue.ToString(CultureInfo.InvariantCulture)); return true; }
		internal override bool TryAppendText(System.Text.StringBuilder sb) { sb.Append(_value); return true; }

		public override string ToString() { return IntValue.ToString(CultureInfo.InvariantCulture); }
	}
}