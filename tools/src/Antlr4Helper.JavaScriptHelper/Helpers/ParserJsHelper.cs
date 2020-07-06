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


        #endregion

        #region mini expr
        //TEXTTOBASE64URL_funContext.prototype.expr = function(i)
        //{
        //    if (i === undefined)
        //    {
        //        i = null;
        //    }
        //    if (i === null)
        //    {
        //        return this.getTypedRuleContexts(ExprContext);
        //    }
        //    else
        //    {
        //        return this.getTypedRuleContext(ExprContext, i);
        //    }
        //};
        #endregion

        #region remove expr_sempred
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
        //TDIST_fun2Context.prototype.TDIST = function()
        //{
        //    return this.getToken(mathParser.TDIST, 0);
        //};


        #endregion

        #region min accept
        //TDIST_fun2Context.prototype.accept = function(visitor)
        //{
        //    if (visitor instanceof mathVisitor ) {
        //        return visitor.visitTDIST_fun2(this);
        //    } else
        //    {
        //        return visitor.visitChildren(this);
        //    }
        //};



        #endregion

        #region remove precpred
        //if (!( this.precpred(this._ctx, 13))) {
        //    throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 13)");
        //}


        #endregion

        #region remove state
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
        public static string ReplaceTokens(string txt,string fileName, Dictionary<string,string> dict)
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
