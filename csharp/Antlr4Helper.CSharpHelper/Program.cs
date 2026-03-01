using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Antlr4Helper.CSharpHelper
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["Eof"] = "-1";

			var filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathParser.cs");
			var csText = File.ReadAllText(filePath);

			Regex.Matches(csText, @"public const int([\s\S]*?);").Cast<Match>().ToList().ForEach(q => {
				var str1 = q.Groups[1].Value;
				var array1 = str1.Split(",\r\n\t;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach(var item in array1) {
					if(string.IsNullOrWhiteSpace(item)) { continue; }
					var sp = item.Split('=');
					dict[sp[0].Trim()] = sp[1].Trim();
				}
			});


			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("[RuleVersion(0)]", "");
			csText = csText.Replace("[NotNull]", "");
			csText = csText.Replace("[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode", "//[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode");
			csText = csText.Replace("else return visitor.VisitChildren(this);", "");
			csText = csText.Replace("if (typedVisitor != null) ", "");

			csText = Regex.Replace(csText, "if \\(!\\(Precpred.*\\r\\n\\t*", "");
			csText = Regex.Replace(csText, "State = \\d+;\\r\\n[ \\t]+State =", "State =");
			csText = Regex.Replace(csText, "return GetRuleContext<ExprContext>\\(i\\);\\r\\n", "return GetRuleContext<ExprContext>(i);");
			csText = Regex.Replace(csText, @"\[System.Diagnostics.DebuggerNonUserCode\] public ExprContext expr\(int i\) \{\r\n", "// [System.Diagnostics.DebuggerNonUserCode] public ExprContext expr(int i) {");

			csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*Match\(", "Match(");
			csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*_la", "_la");
			csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*ErrorHandler", "ErrorHandler");
			csText = Regex.Replace(csText, @"public partial class mathParser", "partial class mathParser");
			csText = Regex.Replace(csText, @"public partial class", "internal partial class");
			csText = Regex.Replace(csText, @"private bool expr_sempred[\s\S]*?return true;[\s\S]*?\}", "");
			csText = Regex.Replace(csText, @"switch \(ruleIndex\) \{[\s\S]*?\}", "");
			csText = Regex.Replace(csText, @"public const int[\s\S]*?;", "");

			csText = csText.Replace("ExprContext _prevctx = _localctx;", "");
			csText = csText.Replace("_prevctx = _localctx;", "");
			csText = csText.Replace("internal partial class", "internal sealed class");
			csText = csText.Replace("internal sealed class ExprContext", "internal class ExprContext");
			csText = csText.Replace("public ExprContext expr() {\r\n\t\treturn expr(0);\r\n\t}", "");

			//_prevctx = _localctx;

			var list = dict.Keys.ToList().OrderByDescending(q => q.Length).ToList();

			foreach(var item in list) {
				var value = dict[item];
				csText = Regex.Replace(csText, $"case {item}:", $"case {value}:");
				csText = Regex.Replace(csText, @$"Match\({item}\);", $"Match({value});");
				csText = Regex.Replace(csText, @$"\(1L << {item}\)", $"(1L << {value})");
				csText = Regex.Replace(csText, @$"\(1L << \({item} ", $"(1L << ({value} "); //(1L << (MULTINOMIAL
				csText = csText.Replace(@$"_la=={item}", $"_la=={value}"); //(1L << (MULTINOMIAL
				csText = csText.Replace(@$"_la == {item}", $"_la == {value}"); //(1L << (MULTINOMIAL
				csText = csText.Replace(@$"mathParser.{item}", $"{value}"); //(1L << (MULTINOMIAL

				csText = csText.Replace($"PushNewRecursionContext(_localctx, _startState, {item})", $"PushNewRecursionContext(_localctx, _startState, {value})"); //(1L << (MULTINOMIAL
				csText = csText.Replace($"public override int RuleIndex {{ get {{ return {item}; }} }}", $"public override int RuleIndex {{ get {{ return {value}; }} }}"); //(1L << (MULTINOMIAL
				csText = Regex.Replace(csText, @$"EnterRule\(_localctx, (\d+), {item}\);", $"EnterRule(_localctx, $1, {value});"); //(1L << (MULTINOMIAL
				csText = Regex.Replace(csText, @$"EnterRecursionRule\(_localctx, (\d+), {item}, _p\);", $"EnterRecursionRule(_localctx, $1, {value}, _p);"); //(1L << (MULTINOMIAL
		 
			}

			csText = csText.Replace("//[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER()", "[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER()");

			csText = csText.Replace("public override string GrammarFileName { get { return \"math.g4\"; } }", "public override string GrammarFileName { get { return \"\"; } }");
			csText = Regex.Replace(csText, @"public const int[\s\S]*?;", "");
			csText = Regex.Replace(csText, @"public static string\[\] channelNames = \{[\s\S]*?\}", "public static string[] channelNames = {}");
			csText = Regex.Replace(csText, @"public static string\[\] modeNames = \{[\s\S]*?\}", "public static string[] modeNames = {}");
			csText = Regex.Replace(csText, @"public static readonly string\[\] ruleNames = \{[\s\S]*?\}", "public static readonly string[] ruleNames = {}");
			csText = Regex.Replace(csText, @"private static readonly string\[\] _LiteralNames = \{[\s\S]*?\};", "private static readonly string[] _LiteralNames = {};");
			csText = Regex.Replace(csText, @"private static readonly string\[\] _SymbolicNames = \{[\s\S]*?\};", "private static readonly string[] _SymbolicNames = {};");



			csText = Regex.Replace(csText, @"//.*", "");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathParser.cs", csText);


			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathLexer.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public partial class mathLexer", "partial class mathLexer");

			csText = csText.Replace("public override string GrammarFileName { get { return \"math.g4\"; } }", "public override string GrammarFileName { get { return \"\"; } }");
			csText = Regex.Replace(csText, @"public const int[\s\S]*?;", "");
			csText = Regex.Replace(csText, @"public static string\[\] channelNames = \{[\s\S]*?\}", "public static string[] channelNames = {}");
			csText = Regex.Replace(csText, @"public static string\[\] modeNames = \{[\s\S]*?\}", "public static string[] modeNames = {}");
			csText = Regex.Replace(csText, @"public static readonly string\[\] ruleNames = \{[\s\S]*?\}", "public static readonly string[] ruleNames = {}");
			csText = Regex.Replace(csText, @"private static readonly string\[\] _LiteralNames = \{[\s\S]*?\};", "private static readonly string[] _LiteralNames = {};");
			csText = Regex.Replace(csText, @"private static readonly string\[\] _SymbolicNames = \{[\s\S]*?\};", "private static readonly string[] _SymbolicNames = {};");


			csText = Regex.Replace(csText, @"//.*", "");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathLexer.cs", csText);



			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathVisitor.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public interface ImathVisitor<Result>", "interface ImathVisitor<Result>");
			csText = csText.Replace("[NotNull] ", "");



			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathVisitor.cs", csText);

		}
	}

}
