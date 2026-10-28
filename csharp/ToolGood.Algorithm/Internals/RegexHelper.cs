using System;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals
{
	internal sealed partial class RegexHelper
	{
		public static bool IsHex(string value)
		{
			if(string.IsNullOrEmpty(value)) return false;
			foreach(var c in value) {
				if(!IsHexChar(c)) return false;
			}
			return true;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		private static bool IsHexChar(char c)
		{
			return (uint)(c - '0') <= 9 || (uint)(c - 'A') <= 5 || (uint)(c - 'a') <= 5;
		}

		public static bool IsOct(string value)
		{
			if(string.IsNullOrEmpty(value)) return false;
			foreach(var c in value) {
				if(!IsOctChar(c)) return false;
			}
			return true;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		private static bool IsOctChar(char c)
		{
			return (uint)(c - '0') <= 7;
		}

		public static bool IsBin(string value)
		{
			if(string.IsNullOrEmpty(value)) return false;
			foreach(var c in value) {
				if(!IsBinChar(c)) return false;
			}
			return true;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		private static bool IsBinChar(char c)
		{
			return c == '0' || c == '1';
		}

		// 日期/时间解析已改为 MyDate.Parse 的 span 手写扫描，原先 10 条 dateTime/date/dayTime/time 正则已删除。
#if NETSTANDARD2_1
        public static readonly Regex romanRegex = new Regex("^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", RegexOptions.Compiled);
#else
		public static readonly Regex romanRegex = romanRegex_();

		[GeneratedRegex("^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", RegexOptions.Compiled)]
		private static partial Regex romanRegex_();

#endif

	}
}
