using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandDecimal : Operand
	{
		private readonly decimal _value;
		public OperandDecimal(decimal obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => _value;
		public override long LongValue => (long)_value;
		public override double DoubleValue => (double)_value;

		internal override bool TryGetNumber(out Operand value) { value = this; return true; }

		internal override bool TryGetBoolean(out Operand value)
		{
			// 只有 0/1 能转布尔，其余值原本就会得到 ToBoolean 的错误结果，此处返回 false 让调用方回落到原路径
			if(_value == 0) { value = False; return true; }
			if(_value == 1) { value = True; return true; }
			value = null;
			return false;
		}

		internal override bool TryGetText(out Operand value) { value = Create(Format(_value)); return true; }
		internal override bool TryAppendText(System.Text.StringBuilder sb) { sb.Append(Normalize(_value)); return true; }

		public override string ToString() { return Format(_value); }

		/// <summary>
		/// 尾零归一：运算结果不再做整数归一（如 1.5*2 得到 3.0m），输出时统一成 "3"，保持原有可见行为
		/// </summary>
		private static string Format(decimal v)
		{
			return Normalize(v).ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// 去掉 decimal 的尾零（3.0m → 3m），保证运算结果与整数归一后的输出一致
		/// </summary>
		internal static decimal Normalize(decimal v)
		{
			var truncate = decimal.Truncate(v);
			return truncate == v ? truncate : v;
		}
	}
}