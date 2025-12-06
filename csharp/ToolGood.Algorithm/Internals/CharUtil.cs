using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    internal static class CharUtil
    {
        public static char StandardChar(char o)
        {
            if (o <= 'Z') return o;
            if (o > 127) {
                if (o == '‘') return '\'';
                if (o == '’') return '\'';
                if (o == '“') return '"';
                if (o == '”') return '"';
                if (o == '〔') return '(';
                if (o == '〕') return ')';
                if (o == '＝') return '=';
                if (o == '＋') return '+';
                if (o == '－') return '-';
                if (o == '×') return '*';
                if (o == '÷') return '/';
                if (o == '／') return '/';
                //if (o == '【') return '[';
                //if (o == '】') return ']';
                //if (o == '（') return '(';
                //if (o == '）') return ')';
                //if (o == '，') return ',';
                //if (o == '！') return '!';
                if (o == 12288) return (char)32;
                if (o <= 65280) {
                    return o;
                } else if (o < 65375) {
                    o = (char)(o - 65248);
                }
            }
            return char.ToUpperInvariant(o);
        }

        public static string StandardString(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++) {
                sb.Append(StandardChar(s[i]));
            }
            return sb.ToString();
        }

        public static bool Equals(string left, char right)
        {
            if (left.Length != 1) return false;
            if (left[0] != right) {
                var a = StandardChar(left[0]);
                if (a != right) return false;
            }
            return true;
        }

        public static bool Equals(string left, string right)
        {
            if (left.Length != right.Length) return false;
            for (int i = 0; i < left.Length; i++) {
                if (left[i] != right[i]) {
                    var a = StandardChar(left[i]);
                    if (a != right[i]) return false;
                }
            }
            return true;
        }

        public static bool Equals(string left, string arg1, string arg2)
        {
            if (Equals(left, arg1)) return true;
            if (Equals(left, arg2)) return true;
            return false;
        }

        public static bool Equals(string left, string arg1, string arg2, string arg3)
        {
            if (Equals(left, arg1)) return true;
            if (Equals(left, arg2)) return true;
            if (Equals(left, arg3)) return true;
            return false;
        }
         
    }
}