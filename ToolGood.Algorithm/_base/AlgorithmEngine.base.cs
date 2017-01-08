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


    }
}
