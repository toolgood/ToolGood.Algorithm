using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Antlr4Helper.JavaScriptHelper.Helpers
{
    public class LexerJsHelper
    {
        #region ReplaceTokens
        public static string ReplaceTokens(string txt, Dictionary<string,string> dict)
        {
            var list = dict.Select(q => q.Key).OrderByDescending(q => q.Length).ToList();
            foreach (var item in list)
            {
                txt = txt.Replace(item, dict[item].ToString());
            }
            Regex regex = new Regex(@"\d+ ?= ?\d+;");
            var str = txt.Replace("\r\n", "\n").Replace("\r", "\n");
            var texts = str.Split('\n').ToList();
            for (int i = texts.Count - 1; i >= 0; i--)
            {
                var s = texts[i];
                if (regex.IsMatch(s))
                {
                    texts.RemoveAt(i);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var t in texts)
            {
                sb.Append(t);
                sb.Append("\n");
            }
            return sb.ToString();
        }
        #endregion

        public static void MiniToken(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @"^.*Lexer.[A-Z0-9_]+ = \d+;$"))
                {
                    //mathLexer.IFERROR = 29;
                    lines.RemoveAt(i);
                }
            }
        }

        #region RemoveOthers
        public static string RemoveOthers(string txt)
        {
            Regex regex = new Regex(@".*Lexer\.prototype\.grammarFileName = "".*\.g4\"";");
            txt = regex.Replace(txt, "");
            Regex regex2 = new Regex(@".*Lexer\.EOF = antlr4\.Token\.EOF;");
            txt = regex2.Replace(txt, "");

            Regex regex3 = new Regex(@".*Lexer\.prototype\.channelNames = [\s\S]*?\];", RegexOptions.Multiline);
            txt = regex3.Replace(txt, "");

            Regex regex4 = new Regex(@".*Lexer\.prototype\.modeNames = [\s\S]*?\];", RegexOptions.Multiline);
            txt = regex4.Replace(txt, "");
            Regex regex5 = new Regex(@".*Lexer\.prototype\.literalNames = [\s\S]*?\];", RegexOptions.Multiline);
            txt = regex5.Replace(txt, "");
            Regex regex6 = new Regex(@".*Lexer\.prototype\.symbolicNames = [\s\S]*?\];", RegexOptions.Multiline);
            txt = regex6.Replace(txt, "");
            Regex regex7 = new Regex(@".*Lexer\.prototype\.ruleNames = [\s\S]*?\];", RegexOptions.Multiline);
            txt = regex7.Replace(txt, "");

            return txt;
        }
        #endregion

    }
}
