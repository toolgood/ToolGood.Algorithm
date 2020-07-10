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
            var text = File.ReadAllText("mathParser.js");
            var lines = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            ParserJsHelper.RemoveState(lines);
            ParserJsHelper.MiniAccept(lines);
            ParserJsHelper.MiniExpr(lines);
            ParserJsHelper.MiniToken(lines);
            ParserJsHelper.RemovePrecpred(lines);
            ParserJsHelper.Remove_fun2Context(lines);


            text = string.Join("\r\n", lines);
            text = MiniSerializedATN.MiniAtn(text);
            File.WriteAllText("mathParser.bak.js", text);




            text = File.ReadAllText("mathLexer.js");
            lines = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            LexerJsHelper.MiniToken(lines);


            text = string.Join("\r\n", lines);
            text= LexerJsHelper.RemoveOthers(text);
            text = MiniSerializedATN.MiniAtn(text);
            File.WriteAllText("mathLexer.bak.js", text);

            Console.WriteLine("Hello World!");
        }
    }
}
