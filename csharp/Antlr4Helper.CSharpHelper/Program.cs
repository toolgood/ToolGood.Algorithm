using Antlr4.Runtime;
using Antlr4Helper.CSharpHelper.ANTLRv4;
using Antlr4Helper.CSharpHelper.RegexEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RegexParseException = Antlr4Helper.CSharpHelper.RegexEngine.RegexParseException;

namespace Antlr4Helper.CSharpHelper
{
	class Program
	{
		static void Main(string[] args)
		{
			var filePath = Path.GetFullPath(@"..\..\..\..\..\g4\math.g4");
			var lrs = LoadLexerRegexs(filePath);
			StringBuilder sb=new StringBuilder();
			foreach(var item in lrs) {
				sb.Append(item.Name);
				sb.Append('=');
				sb.Append(item.Regex);
				sb.Append('\n');
			}
			File.WriteAllText(@"math_regex_temp.txt", sb.ToString());


			var merger = new RegexMerger();
			var patterns = new List<string> { "hell", "[a-z]+", @"\d+", "\"[^\"]*\"" };
			var dfa = merger.MergePatterns(patterns);

			var result = dfa.Match("hello");  // 匹配模式 0
			var result2 = dfa.Match("123");   // 匹配模式 2

			Console.WriteLine("=== 正则表达式解析系统 ===");
			Console.WriteLine();

			RunDemo();

			Console.WriteLine();
			Console.WriteLine("按任意键退出...");
			Console.ReadKey();
		}

		static void RunDemo()
		{
			var patterns = new List<string>
			{
				@"[a-zA-Z_][a-zA-Z0-9_]*",
				@"\d+",
				@"""[^""]*""",
				@"'[^']*'",
				@"//.*",
				@"/\*[\s\S]*?\*/"
			};

			Console.WriteLine("输入的正则表达式模式:");
			for(int i = 0; i < patterns.Count; i++) {
				Console.WriteLine($"  [{i}] {patterns[i]}");
			}
			Console.WriteLine();

			try {
				var merger = new RegexMerger();
				Console.WriteLine("正在解析和合并正则表达式...");

				var dfa = merger.MergePatterns(patterns);

				Console.WriteLine($"DFA状态数: {dfa.States.Count}");
				Console.WriteLine($"字母表大小: {dfa.Alphabet.Count}");
				Console.WriteLine();

				var testInputs = new List<string>
				{
					"identifier",
					"12345",
					"\"string literal\"",
					"'c'",
					"// comment",
					"/* multi\nline\ncomment */",
					"invalid@symbol"
				};

				Console.WriteLine("测试匹配:");
				foreach(var input in testInputs) {
					var result = dfa.Match(input);
					if(result.Success) {
						Console.WriteLine($"  \"{input}\" -> 匹配模式 {result.PatternId}, 长度 {result.EndIndex}");
					} else {
						Console.WriteLine($"  \"{input}\" -> 无匹配");
					}
				}
				Console.WriteLine();

				var generator = new DFACodeGenerator();
				var code = generator.GenerateCode(dfa, "GeneratedLexer", "Generated");

				var outputPath = Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					"..", "..", "..",
					"GeneratedLexer.cs");

				Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
				File.WriteAllText(outputPath, code);

				Console.WriteLine($"DFA代码已生成: {Path.GetFullPath(outputPath)}");
				Console.WriteLine();

				GenerateDetailedReport(patterns, dfa);
			} catch(RegexParseException ex) {
				Console.WriteLine($"解析错误: {ex.Message}");
			} catch(Exception ex) {
				Console.WriteLine($"错误: {ex.Message}");
			}
		}

		static void GenerateDetailedReport(List<string> patterns, DFA dfa)
		{
			Console.WriteLine("=== DFA详细信息 ===");
			Console.WriteLine();

			Console.WriteLine("状态列表:");
			foreach(var state in dfa.States) {
				var acceptStr = state.IsAccept ? $" [Accept: Pattern {state.AcceptId}]" : "";
				Console.WriteLine($"  State {state.Id}{acceptStr}");

				if(state.Transitions.Count > 0) {
					var transitionCount = Math.Min(5, state.Transitions.Count);
					var i = 0;
					foreach(var kvp in state.Transitions) {
						if(i >= transitionCount) {
							Console.WriteLine($"    ... 还有 {state.Transitions.Count - transitionCount} 个转换");
							break;
						}
						var charStr = GetCharDisplay(kvp.Key);
						Console.WriteLine($"    '{charStr}' -> State {kvp.Value.Id}");
						i++;
					}
				}
			}
			Console.WriteLine();

			Console.WriteLine("模式说明:");
			Console.WriteLine("  [0] 标识符: 字母或下划线开头，后跟字母、数字或下划线");
			Console.WriteLine("  [1] 整数: 一个或多个数字");
			Console.WriteLine("  [2] 字符串字面量: 双引号包围的任意字符序列");
			Console.WriteLine("  [3] 字符字面量: 单引号包围的单个字符");
			Console.WriteLine("  [4] 单行注释: // 开头到行尾");
			Console.WriteLine("  [5] 多行注释: /* */ 包围的内容");
		}

		static string GetCharDisplay(char c)
		{
			if(c >= 32 && c <= 126 && c != '\\' && c != '\'')
				return c.ToString();
			if(c == '\n') return "\\n";
			if(c == '\r') return "\\r";
			if(c == '\t') return "\\t";
			return $"\\x{((int)c):X2}";
		}

		static void RunInteractiveMode()
		{
			Console.WriteLine("=== 交互模式 ===");
			Console.WriteLine("输入正则表达式（每行一个，空行结束）:");

			var patterns = new List<string>();
			string line;
			while(!string.IsNullOrEmpty(line = Console.ReadLine())) {
				patterns.Add(line);
			}

			if(patterns.Count == 0) {
				Console.WriteLine("未输入任何模式。");
				return;
			}

			try {
				var merger = new RegexMerger();
				var dfa = merger.MergePatterns(patterns);

				Console.WriteLine($"\n合并后的DFA有 {dfa.States.Count} 个状态。");
				Console.WriteLine("\n输入测试字符串（空行退出）:");

				while(!string.IsNullOrEmpty(line = Console.ReadLine())) {
					var result = dfa.Match(line);
					if(result.Success) {
						Console.WriteLine($"匹配成功: 模式 {result.PatternId}, 位置 [{result.StartIndex}, {result.EndIndex})");
					} else {
						Console.WriteLine("无匹配");
					}
				}
			} catch(RegexParseException ex) {
				Console.WriteLine($"解析错误: {ex.Message}");
			}
		}

		static void TestSinglePattern()
		{
			Console.WriteLine("=== 单模式测试 ===");
			Console.Write("输入正则表达式: ");
			var pattern = Console.ReadLine();

			if(string.IsNullOrEmpty(pattern)) {
				Console.WriteLine("模式不能为空。");
				return;
			}

			try {
				var parser = new RegexParser();
				var ast = parser.Parse(pattern);

				Console.WriteLine("\nAST结构:");
				PrintAst(ast, 0);

				var builder = new NFABuilder();
				var nfa = builder.Build(ast, 0);

				Console.WriteLine($"\nNFA状态数: {nfa.States.Count}");

				var converter = new NFAToDFAConverter();
				var dfa = converter.Convert(nfa);

				Console.WriteLine($"转换后DFA状态数: {dfa.States.Count}");

				var minimizer = new DFAMinimizer();
				var minDfa = minimizer.Minimize(dfa);

				Console.WriteLine($"最小化后DFA状态数: {minDfa.States.Count}");

				Console.WriteLine("\n输入测试字符串（空行退出）:");
				string line;
				while(!string.IsNullOrEmpty(line = Console.ReadLine())) {
					var result = minDfa.Match(line);
					if(result.Success) {
						Console.WriteLine($"匹配成功: 位置 [{result.StartIndex}, {result.EndIndex})");
					} else {
						Console.WriteLine("无匹配");
					}
				}
			} catch(RegexParseException ex) {
				Console.WriteLine($"解析错误: {ex.Message}");
			}
		}

		static void PrintAst(RegexNode node, int indent)
		{
			var prefix = new string(' ', indent * 2);

			switch(node) {
				case CharNode cn:
					Console.WriteLine($"{prefix}Char: '{cn.Value}'");
					break;
				case CharClassNode ccn:
					var ranges = string.Join(", ", ccn.Ranges.Select(r => r.Min == r.Max ? $"'{r.Min}'" : $"'{r.Min}'-'{r.Max}'"));
					Console.WriteLine($"{prefix}CharClass: {(ccn.Negated ? "^" : "")}[{ranges}]");
					break;
				case AnyCharNode:
					Console.WriteLine($"{prefix}AnyChar: .");
					break;
				case ConcatNode concat:
					Console.WriteLine($"{prefix}Concat:");
					foreach(var child in concat.Children)
						PrintAst(child, indent + 1);
					break;
				case AlternationNode alt:
					Console.WriteLine($"{prefix}Alternation:");
					foreach(var child in alt.Alternatives)
						PrintAst(child, indent + 1);
					break;
				case QuantifierNode qn:
					var max = qn.MaxCount?.ToString() ?? "∞";
					Console.WriteLine($"{prefix}Quantifier: {{{qn.MinCount},{max}}}{(qn.Greedy ? "" : "?")}");
					PrintAst(qn.Child, indent + 1);
					break;
				case GroupNode gn:
					Console.WriteLine($"{prefix}Group({gn.GroupIndex}):");
					PrintAst(gn.Child, indent + 1);
					break;
				case AnchorNode an:
					Console.WriteLine($"{prefix}Anchor: {an.Type}");
					break;
				case EmptyNode:
					Console.WriteLine($"{prefix}Empty");
					break;
			}
		}

		static List<LexerRegex> LoadLexerRegexs(string file)
		{
			string exp = File.ReadAllText(file);
			var stream = new AntlrInputStream(exp);
			var lexer = new ANTLRv4Lexer(stream, TextWriter.Null, TextWriter.Null);
			var tokens = new CommonTokenStream(lexer);
			var parser = new ANTLRv4Parser(tokens, TextWriter.Null, TextWriter.Null);
			var ts = parser.grammarSpec();
			var lexerVisitor = new LexerVisitor2();
			lexerVisitor.Visit(ts);
			var lexerRegexes = lexerVisitor.LexerRegexes;

			for(int i = lexerRegexes.Count - 1; i >= 0; i--) {
				var lr = lexerRegexes[i];
				if(lr.IsFragment == false) { continue; }
				var lrtxt = "(" + lr.Regex + ")";
				for(int j = i - 1; j >= 0; j--) {
					var lrr = lexerRegexes[j];
					if(lrr.IsFragment) { continue; }
					lrr.Regex = lrr.Regex.Replace(lr.Name, lrtxt);
				}
				lexerRegexes.RemoveAt(i);
			}
			for(int i = 0; i < lexerRegexes.Count; i++) {
				lexerRegexes[i].Id = i + 1;
			}
			return lexerRegexes;
		}

		static void Main2(string[] args)
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

			csText = Regex.Replace(csText, @"State = \d+;([\r\n\t]*\(.*_localctx\).f)", "$1");
			csText = Regex.Replace(csText, @"State = \d+;([\r\n\t]*\(.*_localctx\).op)", "$1");
			csText = Regex.Replace(csText, @"State = \d+;([\r\n\t]*_localctx.key)", "$1");
			csText = Regex.Replace(csText, @"State = \d+;([\r\n\t]*parameter2\(\))", "$1");
			csText = Regex.Replace(csText, @"State = \d+;([\r\n\t]*arrayJson\(\))", "$1");

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

			csText = csText.Replace("ImathVisitor<TResult> typedVisitor = visitor as ImathVisitor<TResult>;", "");
			csText = csText.Replace("public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {", "public override TResult Accept2<TResult>(ImathVisitor<TResult> typedVisitor) {");


			csText = Regex.Replace(csText, @"//.*", "");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");

			csText = csText.Replace("public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {\r\n\t\treturn true;\r\n\t}", "");

			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText(@"..\..\..\..\..\csharp\ToolGood.Algorithm\math\mathParser.cs", csText);


			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathLexer.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public partial class mathLexer", "partial class mathLexer");

			csText = csText.Replace("public override string GrammarFileName { get { return \"math.g4\"; } }", "public override string GrammarFileName { get { return \"\"; } }");
			//csText = Regex.Replace(csText, @"public const int[\s\S]*?;", "");
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
			File.WriteAllText(@"..\..\..\..\..\csharp\ToolGood.Algorithm\math\mathLexer.cs", csText);



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
			File.WriteAllText(@"..\..\..\..\..\csharp\ToolGood.Algorithm\math\mathVisitor.cs", csText);



			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathBaseVisitor.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public partial class mathBaseVisitor<Result>", "partial class mathBaseVisitor<Result>");
			csText = csText.Replace("[NotNull] ", "");



			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"[\r\n]+[ \t]*[\r\n]+", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = Regex.Replace(csText, @"\r\n[ \t]*\r\n", "\r\n");
			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText(@"..\..\..\..\..\csharp\ToolGood.Algorithm\math\mathBaseVisitor.cs", csText);
		}
	}
}

