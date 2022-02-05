using System;
using System.Collections.Generic;
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

        public static string StandardString(string s)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++) {
                var c = s[i];
                sb.Append(StandardChar(c));
            }
            return sb.ToString();
        }

        private static bool EqualsOnce(string left, string right)
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
            if (arg1 != null && EqualsOnce(left, arg1))
                return true;
            if (arg2 != null && EqualsOnce(left, arg2))
                return true;
            if (arg3 != null && EqualsOnce(left, arg3))
                return true;
            return false;
        }

        public static List<String> SplitFormula(String formula, List<char> splitChars)
        {
            List<String> result = new List<String>();
            bool inSquareBrackets = false;
            bool inBraceBrackets = false;
            int inBracketsCount = 0;
            bool inText = false;
            char textChar = (char)0;

            StringBuilder str = new StringBuilder();
            int i = 0;
            while (i < formula.Length) {
                char c = formula[i];
                if (inSquareBrackets) {
                    str.Append(c);
                    if (c == ']') inSquareBrackets = false;
                } else if (inBraceBrackets) {
                    str.Append(c);
                    if (c == '}') inBraceBrackets = false;
                } else if (inText) {
                    str.Append(c);
                    if (c == '\\') {
                        i++;
                        if (i < formula.Length) {
                            str.Append(formula[i]);
                        }
                    } else if (c == textChar) {
                        inText = false;
                    }
                } else if (splitChars.Contains(c) && inBracketsCount == 0) {
                    result.Add(str.ToString());
                    result.Add(c.ToString());
                    str = new StringBuilder();
                } else {
                    str.Append(c);
                    if (c == '\'' || c == '"' || c == '`') {
                        textChar = c;
                        inText = true;
                    } else if (c == '[') {
                        inSquareBrackets = true;
                    } else if (c == '{') {
                        inBraceBrackets = true;
                    } else if (c == '(') {
                        inBracketsCount++;
                    } else if (c == ')') {
                        inBracketsCount--;
                    }
                }
                i++;
            }
            if (str.Length > 0)
                result.Add(str.ToString());
            return result;

        }


        public static List<string> SplitFormulaForAndOr(string formula)
        {
            List<String> result = new List<String>();
            bool inSquareBrackets = false;
            bool inBraceBrackets = false;
            int inBracketsCount = 0;
            bool inText = false;
            char textChar = (char)0;

            StringBuilder str = new StringBuilder();
            int i = 0;
            while (i < formula.Length) {
                char c = formula[i];
                if (inSquareBrackets) {
                    str.Append(c);
                    if (c == ']') inSquareBrackets = false;
                } else if (inBraceBrackets) {
                    str.Append(c);
                    if (c == '}') inBraceBrackets = false;
                } else if (inText) {
                    str.Append(c);
                    if (c == '\\') {
                        i++;
                        if (i < formula.Length) {
                            str.Append(formula[i]);
                        }
                    } else if (c == textChar) {
                        inText = false;
                    }
                } else if (c == '&' && inBracketsCount == 0) {
                    if (i + 1 < formula.Length && formula[i + 1] == '&') {
                        i++;
                        result.Add(str.ToString());
                        str = new StringBuilder();
                    } else {
                        result.Add(str.ToString());
                    }
                } else if (c == '|' && inBracketsCount == 0) {
                    if (i + 1 < formula.Length && formula[i + 1] == '|') {
                        i++;
                        result.Add(str.ToString());
                        str = new StringBuilder();
                        str.Append(string.Join("&&", result));
                        str.Append("||");
                        result.Clear();
                    } else {
                        result.Add(str.ToString());
                    }
                } else {
                    str.Append(c);
                    if (c == '\'' || c == '"' || c == '`') {
                        textChar = c;
                        inText = true;
                    } else if (c == '[') {
                        inSquareBrackets = true;
                    } else if (c == '{') {
                        inBraceBrackets = true;
                    } else if (c == '(') {
                        inBracketsCount++;
                    } else if (c == ')') {
                        inBracketsCount--;
                    }
                }
                i++;
            }
            if (str.Length > 0)
                result.Add(str.ToString());
            return result;



        }

    }
}
