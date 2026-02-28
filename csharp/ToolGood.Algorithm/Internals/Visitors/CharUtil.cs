using System;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal static class CharUtil
	{
		public static char StandardChar(char o)
		{
			if(o < 'a') return o;  // '[', ']', '^', '_', '`' 等字符保持不变
			if(o <= 'z') return (char)(o - 32); // a-z 直接转大写，避免方法调用
			if(o < 127) return o; // 其他 ASCII 字符保持不变
			if(o <= 65280) {
				switch(o) {
					case (char)215: return '*';  // ×
					case (char)247: return '/';  // ÷
					case (char)8216: return '\''; // '
					case (char)8217: return '\''; // '
					case (char)8220: return '"';  // "
					case (char)8221: return '"';  // "
					case (char)12288: return (char)32;
					case (char)12304: return '['; // 【
					case (char)12305: return ']'; // 】
					case (char)12308: return '('; // 〔
					case (char)12309: return ')'; // 〕
					default: return o;
				}
			} else if(o < 65375) {
				o = (char)(o - 65248);
			}
			return char.ToUpperInvariant(o);
		}
	 
		public static bool Equals(string left, char right)
		{
			if(left.Length != 1) return false;
			if(left[0] != right) {
				var a = StandardChar(left[0]);
				if(a != right) return false;
			}
			return true;
		}

		public static bool Equals(string left, string right)
		{
			if(left.Length != right.Length) return false;
			for(int i = 0; i < left.Length; i++) {
				if(left[i] != right[i]) {
					var a = StandardChar(left[i]);
					if(a != right[i]) return false;
				}
			}
			return true;
		}

		public static bool Equals(string left, string arg1, string arg2, string arg3)
		{
			if(Equals(left, arg1)) return true;
			if(Equals(left, arg2)) return true;
			if(Equals(left, arg3)) return true;
			return false;
		}

	}
}
