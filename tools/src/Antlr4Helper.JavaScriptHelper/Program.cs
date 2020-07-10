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

            

            File.WriteAllText("mathParser.bak.js", string.Join("\r\n", lines));

            Console.WriteLine("Hello World!");
        }
    }
}
