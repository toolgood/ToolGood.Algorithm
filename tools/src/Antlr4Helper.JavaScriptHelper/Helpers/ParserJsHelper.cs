using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Antlr4Helper.JavaScriptHelper.Helpers
{
    public class ParserJsHelper
    {
        #region remove _fun2Context
        public static void Remove_fun2Context(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                line = line.Replace("_fun2Context", "_funContext");
                lines[i] = line.Replace("Expr2Context", "ExprContext");
            }
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @"[A-Za-z0-9]+?Parser\.[A-Za-z0-9]+?_funContext = [A-Za-z0-9]+?_funContext;"))
                {
                    lines.RemoveAt(i);
                }
            }
            List<string> funNames = new List<string>();
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @"function (.*?)_funContext\(parser, ctx\) \{"))
                {
                    Match m = Regex.Match(line, @"function (.*?)_funContext\(parser, ctx\) \{");
                    if (m.Success)
                    {
                        var name = m.Groups[1].Value;
                        if (funNames.Contains(name))
                        {
                            Remove_func(lines, i, name);


                        }
                        else
                        {
                            funNames.Add(m.Groups[1].Value);
                        }
                    }
                }
            }

        }
        private static void Remove_func(List<string> lines, int start, string name)
        {
            var i = start + 50;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (line == $"function {name}_funContext(parser, ctx) {{")
                {
                    lines.RemoveAt(i);
                    line = lines[i];
                    while (line != "}")
                    {
                        lines.RemoveAt(i);
                        line = lines[i];
                    }
                    lines.RemoveAt(i);
                    continue;
                }
                if (line.StartsWith($"{name}_funContext.prototype = Object.create("))
                {
                    lines.RemoveAt(i);
                    continue;
                }
                if (line.StartsWith($"{name}_funContext.prototype.constructor = {name}_funContext;"))
                {
                    lines.RemoveAt(i);
                    continue;
                }
                if (line.EndsWith($"Parser.{name}_funContext = {name}_funContext;"))
                {
                    lines.RemoveAt(i);
                    continue;
                }
                if (Regex.IsMatch(line, name + @"_funContext\.prototype\.[a-z]+ ="))
                {
                    lines.RemoveAt(i);
                    line = lines[i];
                    while (line != "};")
                    {
                        lines.RemoveAt(i);
                        line = lines[i];
                    }
                    lines.RemoveAt(i);
                    continue;
                }

                //HOUR_funContext.prototype = Object.create(ExprContext.prototype);
                //HOUR_funContext.prototype.constructor = HOUR_funContext;
                //mathParser.HOUR_funContext = HOUR_funContext;

                i++;
            }




        }

        #endregion

        #region mini expr
        public static void MiniExpr(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @".*Context\.prototype\.expr = function\(i\) \{"))
                {
                    lines.RemoveAt(i + 8);
                    lines.RemoveAt(i + 7);
                    lines.RemoveAt(i + 6);
                    lines.RemoveAt(i + 4);
                    lines.RemoveAt(i + 3);
                    lines.RemoveAt(i + 2);
                    lines.RemoveAt(i + 1);
                }
            }
        }
        #endregion

        #region remove expr_sempred
        public static void Remove_sempred(List<string> lines)
        {
            var i = 0;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (line == "mathParser.prototype.sempred = function(localctx, ruleIndex, predIndex) {")
                {
                    lines.RemoveAt(i);
                    line = lines[i];
                    while (line != "};")
                    {
                        lines.RemoveAt(i);
                        line = lines[i];
                    }
                    lines.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        public static void Remove_expr_sempred(List<string> lines)
        {
            var i = 0;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (line == "mathParser.prototype.expr_sempred = function(localctx, predIndex) {")
                {
                    lines.RemoveAt(i);
                    line = lines[i];
                    while (line != "};")
                    {
                        lines.RemoveAt(i);
                        line = lines[i];
                    }
                    lines.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        //mathParser.prototype.sempred = function(localctx, ruleIndex, predIndex)
        //{
        //    switch (ruleIndex)
        //    {
        //        case 1:
        //            return this.expr_sempred(localctx, predIndex);
        //        default:
        //            throw "No predicate with index:" + ruleIndex;
        //    }
        //};
        //mathParser.prototype.expr_sempred = function(localctx, predIndex) {



        #endregion


        #region remove token
        public static void MiniToken(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @".*\.prototype\.[A-Z0-9]+ = function\(\) \{"))
                {
                    if (Regex.IsMatch(line, @".*\.prototype\.(NUM|SUB|STRING|PARAMETER) = function\(\) \{"))
                    {
                        continue;
                    }
                    lines.RemoveAt(i);
                    lines.RemoveAt(i);
                    lines.RemoveAt(i);
                    continue;
                }
                if (Regex.IsMatch(line, @"^.*Parser.[A-Z0-9_]+ = \d+;$"))
                {
                    lines.RemoveAt(i);
                }
                if (Regex.IsMatch(line, @"^.*Parser.RULE_[a-z0-9_]+ = \d+;$"))
                {
                    lines.RemoveAt(i);
                }
            }
        }
        #endregion

        #region min accept
        public static void MiniAccept(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @".*\.prototype\.accept = function\(visitor\)"))
                {
                    lines.RemoveAt(i + 5);
                    lines.RemoveAt(i + 4);
                    lines.RemoveAt(i + 3);
                    lines.RemoveAt(i + 1);
                }
            }
        }
        #endregion

        #region remove precpred
        public static void RemovePrecpred(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @"if \(!\( this\.precpred\(this\._ctx, \d+\)\)\) \{"))
                {
                    lines.RemoveAt(i);
                    lines.RemoveAt(i);
                    lines.RemoveAt(i);
                }
            }
        }

        //if (!( this.precpred(this._ctx, 13))) {
        //    throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 13)");
        //}


        #endregion

        #region remove state
        public static void RemoveState(List<string> lines)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (Regex.IsMatch(line, @"this\.state = \d+;"))
                {
                    var line2 = lines[i + 1];
                    if (Regex.IsMatch(line2, @"this\.(expr|parameter2)\((.*?)\);") == false)
                    {
                        lines.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        public static void RemoveState2(List<string> lines)
        {
            var b = false;
            var i = 0;
            while (i < lines.Count)
            {
                var line = lines[i];
                if (line == "mathParser.prototype.expr2 = function() {")
                {
                    b = true;
                }
                if (b)
                {
                    if (Regex.IsMatch(line, @"this\.state = \d+;"))
                    {
                        lines.RemoveAt(i);
                    }
                }
                i++;
            }

        }

        //this.state = 621;
        // this.match(mathParser.T__18);




        #endregion

        #region ReplaceNumber
        public static string ReplaceNumber(string str)
        {
            return str.Replace("antlr4.atn.ATN.INVALID_ALT_NUMBER", "0");
        }
        #endregion
        #region ReplaceTokens
        public static string ReplaceTokens(string txt, string fileName, Dictionary<string, string> dict)
        {
            var list = dict.Select(q => q.Key).OrderByDescending(q => q.Length).ToList();
            foreach (var item in list)
            {
                txt = txt.Replace(item, dict[item].ToString());
            }
            txt = txt.Replace("antlr4.Token.EOF", "-1");
            txt = txt.Replace(fileName + "Parser.EOF", "-1");

            Regex regex = new Regex(@"[\-\d]+ ?= ?[\-\d]+;");
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

        #region ReplaceRuleTokens
        public static string ReplaceRuleTokens(string txt, string fileName)
        {
            Regex regex = new Regex(@"\d+ ?= ?\d+;");
            Regex regex2 = new Regex("(" + fileName + @"Parser\.RULE_.*?) ?= ?(\d+);");
            var str = txt.Replace("\r\n", "\n").Replace("\r", "\n");
            var texts = str.Split('\n').ToList();

            bool b = false;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < texts.Count; i++)
            {
                var t = texts[i];
                var m = regex2.Match(t);
                if (m.Success)
                {
                    b = true;
                    dict[m.Groups[1].Value] = m.Groups[2].Value;
                }
                else if (b)
                {
                    break;
                }
            }

            var list = dict.Select(q => q.Key).OrderByDescending(q => q.Length).ToList();
            foreach (var item in list)
            {
                txt = txt.Replace(item, dict[item].ToString());
            }

            str = txt.Replace("\r\n", "\n").Replace("\r", "\n");
            texts = str.Split('\n').ToList();
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

        #region RemoveOthers
        public static string RemoveOthers(string txt)
        {
            Regex regex = new Regex(@"var grammarFileName = "".*\.g4\"";");
            txt = regex.Replace(txt, "");
            Regex regex2 = new Regex(@".*Parser\.EOF = antlr4\.Token\.EOF;");
            txt = regex2.Replace(txt, "");

            //Regex regex3 = new Regex(@"var literalNames = [\s\S]*?\];", RegexOptions.Multiline);
            //txt = regex3.Replace(txt, "");

            //Regex regex4 = new Regex(@"var symbolicNames = [\s\S]*?\];", RegexOptions.Multiline);
            //txt = regex4.Replace(txt, "");
            //Regex regex5 = new Regex(@"var ruleNames = [\s\S]*?\];", RegexOptions.Multiline);
            //txt = regex5.Replace(txt, "");

            //txt = txt.Replace("this.ruleNames = ruleNames;", "");
            //txt = txt.Replace("this.literalNames = literalNames;", "");
            //txt = txt.Replace("this.symbolicNames = symbolicNames;", "");

            return txt;
        }


        #endregion
    }
}
