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
    }
}
