using System;
using System.Runtime.CompilerServices;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal static class CharUtil
	{
		private static readonly char[] _StandardChars;

		static CharUtil()
		{
			_StandardChars = new char[65536];
			for(int i = 0; i < _StandardChars.Length; i++) {
				_StandardChars[i] = StandardChar2((char)i);
			}
		}

		private static char StandardChar2(char c)
		{
			if(c == '\r') return '\n';
			if(c == '\t') return ' ';
			if(c == '\f') return ' ';
			if(c < 'a') return c;
			if(c <= 'z') return (char)(c - 32);
			if(c > 65280 && c < 65375) {
				c = (char)(c - 65248);
				c = char.ToUpperInvariant(c);
			}
			if((c >= '\u00c0' && c <= '\u00d6') ||
			   (c >= '\u00d8' && c <= '\u00f6') ||
			   (c >= '\u00f8' && c <= '\u00ff') ||
			   (c >= '\u0100' && c <= '\u1fff') ||
			   (c >= '\u2c00' && c <= '\u2fff') ||
			   (c >= '\u3040' && c <= '\u318f') ||
			   (c >= '\u3300' && c <= '\u337f') ||
			   (c >= '\u3400' && c <= '\u3fff') ||
			   (c >= '\u4e00' && c <= '\u9fff') ||
			   (c >= '\ua000' && c <= '\ud7ff') ||
			   (c >= '\uf900' && c <= '\ufaff') ||
			   (c >= '\uff00' && c <= '\ufff0')) return '_';
			if(c < 127) return c;
			switch(c) {
				case (char)215: return '*';
				case (char)247: return '/';
				case (char)8216:
				case (char)8217: return '\'';
				case (char)8220:
				case (char)8221: return '"';
				case (char)12288: return ' ';
				case (char)12304: return '[';
				case (char)12305: return ']';
				case (char)12308: return '(';
				case (char)12309: return ')';
			}

			return char.ToUpperInvariant(c);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static char StandardChar(char c)
		{
			return _StandardChars[c];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(string left, char right)
		{
			if(left.Length != 1) return false;
			return left[0] == right || StandardChar(left[0]) == right;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(string left, string right)
		{
			return Equals(left.AsSpan(), right.AsSpan());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(ReadOnlySpan<char> left, ReadOnlySpan<char> right)
		{
			if(left.Length != right.Length) return false;
			for(int i = 0; i < left.Length; i++) {
				var l = left[i];
				var r = right[i];
				if(l == r) continue;
				l = StandardChar(l);
				if(l != r) return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(string left, string option1, string option2, string option3)
		{
			return Equals(left, option1) || Equals(left, option2) || Equals(left, option3);
		}

	}
}
