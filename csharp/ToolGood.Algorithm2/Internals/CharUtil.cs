using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    internal class CharUtil
    {
        public static char StandardChar(char c)
        {
            if (c <= 0) return c;
            char o = (char)c;
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

            if (c == 12288) {
                o = (char)32;
            } else if (c > 65280 && c < 65375) {
                o = (char)(c - 65248);
            }
            return char.ToUpperInvariant(o);
        }

        public static bool EqualsOnce(string left, string right)
        {
            if (left.Length != right.Length) return false;
            for (int i = 0; i < left.Length; i++) {
                if (left[i] != right[i]) {
                    var a = StandardChar(left[i]);
                    var b = StandardChar(right[i]);
                    if (a != b) return false;
                }
            }
            return true;
        }

        public static bool Equals(string left, string right)
        {
            if (left == null) return false;
            if (right == null) return false;
            return EqualsOnce(left, right);
        }
        public static bool Equals(string left, string arg1, string arg2)
        {
            if (left == null) return false;
            if (arg1 != null && EqualsOnce(left, arg1))
                return true;
            if (arg2 != null && EqualsOnce(left, arg2))
                return true;
            return false;
        }

        public static bool Equals(string left, string arg1, string arg2, string arg3)
        {
            if (left == null) return false;
            if (left == null) return false;
            if (arg1 != null && EqualsOnce(left, arg1))
                return true;
            if (arg2 != null && EqualsOnce(left, arg2))
                return true;
            if (arg3 != null && EqualsOnce(left, arg3))
                return true;
            return false;
        }

    }
}
