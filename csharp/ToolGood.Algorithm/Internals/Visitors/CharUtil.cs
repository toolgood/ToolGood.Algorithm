using System;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal static class CharUtil
	{
#if NETSTANDARD2_1
		public static char StandardChar(char o)
		{
			if(o < 'a') return o;
			if(o <= 'z') return (char)(o - 32);
			if(o < 127) return o;
			switch(o) {
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
			if(o > 65280 && o < 65375) {
				o = (char)(o - 65248);
			}
			return char.ToUpperInvariant(o);
		}
#else
		public static char StandardChar(char o) => o switch
		{
			>= 'a' and <= 'z' => (char)(o - 32),
			< (char)127 => o,
			(char)215 => '*',
			(char)247 => '/',
			(char)8216 or (char)8217 => '\'',
			(char)8220 or (char)8221 => '"',
			(char)12288 => ' ',
			(char)12304 => '[',
			(char)12305 => ']',
			(char)12308 => '(',
			(char)12309 => ')',
			> (char)65280 and < (char)65375 => (char)(o - 65248),
			_ => char.ToUpperInvariant(o)
		};
#endif
 
		public static bool Equals(string left, char right)
		{
			if(left.Length != 1) return false;
			return left[0] == right || StandardChar(left[0]) == right;
		}

		public static bool Equals(string left, string right)
		{
			return Equals(left.AsSpan(), right.AsSpan());
		}

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

		public static bool Equals(string left, string option1, string option2, string option3)
		{
			return Equals(left, option1) || Equals(left, option2) || Equals(left, option3);
		}

	}
}
