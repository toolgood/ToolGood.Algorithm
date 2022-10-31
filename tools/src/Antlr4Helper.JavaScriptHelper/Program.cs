using System;
using System.IO;
using System.Linq;
using Antlr4Helper.JavaScriptHelper.Helpers;

namespace Antlr4Helper.JavaScriptHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.GetFullPath(@"..\..\..\..\..\..\g4\bin\mathParser.js");
            var text = File.ReadAllText(filePath);
            text = ParserJsHelper.ReplaceNumber(text);
            text = ParserJsHelper.ReplaceKey(text);

            var lines = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            ParserJsHelper.RemoveState(lines);
            //ParserJsHelper.RemoveState2(lines);
            ParserJsHelper.MiniAccept(lines);
            ParserJsHelper.MiniExpr(lines);
            ParserJsHelper.MiniToken(lines);
            ParserJsHelper.RemovePrecpred(lines);
            ParserJsHelper.Remove_fun2Context(lines);
            ParserJsHelper.Remove_sempred(lines);
            ParserJsHelper.Remove_expr_sempred(lines);

            text = string.Join("\r\n", lines);
            text = MiniSerializedATN.MiniAtn(text);
            text = ParserJsHelper.RemoveOthers(text);

            text = text.Replace("export default", "");
            text = text.Replace("import antlr4 from 'antlr4';", "antlr4=require('./index')");
            text += "\r\nexports.mathParser = mathParser;";
            File.WriteAllText("mathParser.js", text);


            filePath = Path.GetFullPath(@"..\..\..\..\..\..\g4\bin\mathLexer.js");
            text = File.ReadAllText(filePath);
            lines = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            LexerJsHelper.MiniToken(lines);

            text = string.Join("\r\n", lines);
            text = LexerJsHelper.RemoveOthers(text);
            text = MiniSerializedATN.MiniAtn(text);

            text = text.Replace("export default", "");
            text = text.Replace("import antlr4 from 'antlr4';", "antlr4=require('./index')");
            text += "\r\nexports.mathLexer = mathLexer;";

            File.WriteAllText("mathLexer.js", text);

        }
    }
}
