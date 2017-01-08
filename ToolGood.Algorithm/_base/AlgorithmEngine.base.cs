using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        /// <summary>
        /// 转成全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private String ToSBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == ' ') {
                    sb[i] = (char)12288;
                } else if (c < 127) {
                    sb[i] = (char)(c + 65248);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 转成半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private String ToDBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == 12288) {
                    sb[i] = (char)32;
                    continue;
                } else if (c > 65280 && c < 65375) {
                    sb[i] = (char)(c - 65248);
                }
            }
            return sb.ToString();
        }


        private string ToChineseRMB(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }


        private int gcd(List<int> list)
        {
            list = list.OrderBy(q => q).ToList();
            var g = gcd(list[1], list[0]);
            for (int i = 2; i < list.Count; i++) {
                g = gcd(list[i], g);
            }
            return g;
        }
        private int gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return gcd(b, a % b);
        }

        private int lgm(List<int> list)
        {
            list = list.OrderBy(q => q).ToList();
            list.RemoveAll(q => q <= 1);

            var a = list[0];
            for (int i = 1; i < list.Count; i++) {
                var b = list[i];
                int g = b > a ? gcd(b, a) : gcd(a, b);
                a = a / g * b;
            }
            return a;
        }



        private double sumif(List<double> dbs, string s, List<double> sumdbs)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value);
            var ss = m.Groups[1].Value;
            double sum = 0;

            for (int i = 0; i < dbs.Count; i++) {
                if (compare(dbs[i], d, s)) {
                    sum += sumdbs[i];
                }
            }
            return sum;
        }

        private int countif(List<double> dbs, double d)
        {
            int count = 0;
            d = Math.Round(d, 12);
            foreach (var item in dbs) {
                if (Math.Round(item, 12) == d) {
                    count++;
                }
            }
            return count;
        }

        private bool compare(double a, double b, string ss)
        {
            if (ss == "<") {
                return a < b;
            } else if (ss == "<=") {
                return a <= b;
            } else if (ss == ">") {
                return a > b;
            } else if (ss == ">=") {
                return a >= b;
            } else if (ss == "=" || ss == "==") {
                return a == b;
            }
            return a != b;
        }

        private int countif(List<double> dbs, string s)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value);
            var ss = m.Groups[1].Value;
            int count = 0;

            foreach (var item in dbs) {
                if (compare(item, d, s)) {
                    count++;
                }
            }
            return count;
        }
        private List<double> GetList(List<Operand> arg)
        {
            List<double> list = new List<double>();
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.NumberValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    if (ls == null) continue;
                    foreach (var d in ls) {
                        list.Add(d);
                    }
                }
            }
            return list;
        }

    }
}
