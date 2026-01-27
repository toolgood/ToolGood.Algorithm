using System.Text;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal static class CharUtil
	{
		public static char StandardChar(char o)
		{
			if(o <= 'Z') return o;
			if(o <= 127) return char.ToUpperInvariant(o);
			if(o <= 65280) {
				if(o == '×') return '*';//215
				if(o == '÷') return '/';//247
				if(o == '‘') return '\'';//8216
				if(o == '’') return '\'';//8217
				if(o == '“') return '"';//8220
				if(o == '”') return '"';//8221
				if(o == (char)12288) return (char)32;
				if(o == '【') return '[';//12304
				if(o == '】') return ']';//12305
				if(o == '〔') return '(';//12308
				if(o == '〕') return ')';//12309
				return o;
			} else if(o < 65375) {
				//if(o == '＝') return '=';//65309
				//if(o == '＋') return '+';//65291
				//if(o == '－') return '-';//65293
				//if(o == '／') return '/';//65295
				//if (o == '（') return '(';//65288
				//if (o == '）') return ')';//65289
				//if (o == '，') return ',';//65292
				//if (o == '！') return '!';//65281
				o = (char)(o - 65248);
			}
			return char.ToUpperInvariant(o);
		}

		public static string StandardString(string s)
		{
			var sb = new StringBuilder(s.Length);
			for(int i = 0; i < s.Length; i++) {
				sb.Append(StandardChar(s[i]));
			}
			return sb.ToString();
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

		public static bool Equals(string left, string arg1, string arg2)
		{
			if(Equals(left, arg1)) return true;
			if(Equals(left, arg2)) return true;
			return false;
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