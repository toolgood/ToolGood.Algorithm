using System;
using System.Globalization;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	/// <summary>
	/// 进制转换函数(DEC2BIN、BIN2DEC、HEX2OCT 等)的公共常量与解析/校验逻辑。
	/// 统一 10 位补码语义、places 参数校验与文本合法性判断，
	/// 避免各函数重复实现导致参数校验与错误类型不一致。
	/// </summary>
	internal static class NumberBaseConverter
	{
		/// <summary>places 参数允许的最大位数(与 10 位补码表示一致)</summary>
		public const int MaxPlaces = 10;

		/// <summary>二进制 10 位补码的符号位权值(2^9)</summary>
		public const int BinHalf = 512;
		/// <summary>二进制 10 位补码的模(2^10)</summary>
		public const int BinModulus = 1024;

		/// <summary>八进制 10 位补码的符号位权值(2^29)</summary>
		public const int OctHalf = 536870912;
		/// <summary>八进制 10 位补码的模(2^30)</summary>
		public const int OctModulus = 1073741824;
		/// <summary>八进制 10 位补码的掩码(2^30-1)</summary>
		public const int OctMask = 0x3FFFFFFF;

		/// <summary>十六进制 10 位补码的符号位权值(2^39)</summary>
		public const long HexHalf = 0x8000000000L;
		/// <summary>十六进制 10 位补码的模(2^40)</summary>
		public const long HexModulus = 0x10000000000L;
		/// <summary>十六进制 10 位补码的掩码(2^40-1)</summary>
		public const long HexMask = 0xFFFFFFFFFFL;

		/// <summary>
		/// 读取并校验 places 参数:必须为 0~10 的整数。
		/// 数值超出 int 范围或不在 0~10 之间时返回 false(对应 Excel #NUM!)。
		/// </summary>
		public static bool TryGetPlaces(Operand arg, out int places)
		{
			places = 0;
			try {
				places = arg.IntValue;
			} catch (OverflowException) {
				// decimal 超出 int 范围，按 places 非法处理，避免向调用方抛出异常
				return false;
			}
			return places >= 0 && places <= MaxPlaces;
		}

		/// <summary>
		/// 解析进制文本为有符号整数:先裁剪首尾空白，再校验字符与长度(不超过 10 位)，
		/// 最后按 10 位补码规则还原符号。
		/// </summary>
		/// <param name="value">待解析文本</param>
		/// <param name="fromBase">源进制，取值 2、8、16</param>
		/// <returns>解析结果；文本非法或超过 10 位时返回 null(对应 Excel #NUM!)</returns>
		public static long? ParseComplement(string value, int fromBase)
		{
			if (string.IsNullOrWhiteSpace(value)) return null;
			var text = value.Trim();
			if (text.Length > MaxPlaces) return null;
			if (IsValidText(text, fromBase) == false) return null;

			var num = Convert.ToInt64(text, fromBase);
			var half = GetHalf(fromBase);
			return num >= half ? num - half * 2 : num;
		}

		/// <summary>
		/// 按 places 补零格式化进制文本。
		/// 负数结果固定为 10 位补码，按 Excel 语义忽略 places 的位数限制。
		/// </summary>
		/// <param name="value">已生成的进制文本</param>
		/// <param name="negative">该文本是否表示负数</param>
		/// <param name="places">places 参数，为 null 表示未指定</param>
		/// <param name="result">格式化结果</param>
		/// <returns>所需位数超过 places 时返回 false(对应 Excel #NUM!)</returns>
		public static bool TryFormat(string value, bool negative, int? places, out string result)
		{
			result = value;
			if (places.HasValue == false) return true;
			if (negative == false && value.Length > places.Value) return false;
			result = value.PadLeft(places.Value, '0');
			return true;
		}

		/// <summary>
		/// 按 places 对齐十进制文本(负号占一位)。
		/// </summary>
		/// <param name="value">待格式化的十进制整数</param>
		/// <param name="places">places 参数</param>
		/// <param name="result">格式化结果</param>
		/// <returns>文本长度超过 places 时返回 false(对应 Excel #NUM!)</returns>
		public static bool TryFormatDecimal(long value, int places, out string result)
		{
			var text = value.ToString(CultureInfo.InvariantCulture);
			if (text.Length > places) {
				result = null;
				return false;
			}
			if (value < 0) {
				result = "-" + text.Substring(1).PadLeft(places - 1, '0');
			} else {
				result = text.PadLeft(places, '0');
			}
			return true;
		}

		private static bool IsValidText(string text, int fromBase)
		{
			if (fromBase == 2) return RegexHelper.IsBin(text);
			if (fromBase == 8) return RegexHelper.IsOct(text);
			return RegexHelper.IsHex(text);
		}

		private static long GetHalf(int fromBase)
		{
			if (fromBase == 2) return BinHalf;
			if (fromBase == 8) return OctHalf;
			return HexHalf;
		}
	}
}
