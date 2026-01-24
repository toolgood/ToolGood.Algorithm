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
			var str = @"T__0 = 1, T__1 = 2, T__2 = 3, T__3 = 4, T__4 = 5, T__5 = 6, T__6 = 7, T__7 = 8, T__8 = 9,
			T__9 = 10, T__10 = 11, T__11 = 12, T__12 = 13, T__13 = 14, T__14 = 15, T__15 = 16, T__16 = 17,
			T__17 = 18, T__18 = 19, T__19 = 20, T__20 = 21, T__21 = 22, T__22 = 23, T__23 = 24,
			T__24 = 25, T__25 = 26, T__26 = 27, T__27 = 28, SUB = 29, NUM = 30, STRING = 31, NULL = 32,
			ERROR = 33, UNIT = 34, IF = 35, IFERROR = 36, ISNUMBER = 37, ISTEXT = 38, ISERROR = 39,
			ISNONTEXT = 40, ISLOGICAL = 41, ISEVEN = 42, ISODD = 43, ISNULL = 44, ISNULLORERROR = 45,
			AND = 46, OR = 47, NOT = 48, TRUE = 49, FALSE = 50, E = 51, PI = 52, DEC2BIN = 53, DEC2HEX = 54,
			DEC2OCT = 55, HEX2BIN = 56, HEX2DEC = 57, HEX2OCT = 58, OCT2BIN = 59, OCT2DEC = 60,
			OCT2HEX = 61, BIN2OCT = 62, BIN2DEC = 63, BIN2HEX = 64, ABS = 65, QUOTIENT = 66, MOD = 67,
			SIGN = 68, SQRT = 69, TRUNC = 70, INT = 71, GCD = 72, LCM = 73, COMBIN = 74, PERMUT = 75,
			DEGREES = 76, RADIANS = 77, COS = 78, COSH = 79, SIN = 80, SINH = 81, TAN = 82, TANH = 83,
			ACOS = 84, ACOSH = 85, ASIN = 86, ASINH = 87, ATAN = 88, ATANH = 89, ATAN2 = 90, ROUND = 91,
			ROUNDDOWN = 92, ROUNDUP = 93, CEILING = 94, FLOOR = 95, EVEN = 96, ODD = 97, MROUND = 98,
			RAND = 99, RANDBETWEEN = 100, FACT = 101, FACTDOUBLE = 102, POWER = 103, EXP = 104,
			LN = 105, LOG = 106, LOG10 = 107, MULTINOMIAL = 108, PRODUCT = 109, SQRTPI = 110,
			SUMSQ = 111, ASC = 112, JIS = 113, CHAR = 114, CLEAN = 115, CODE = 116, CONCATENATE = 117,
			EXACT = 118, FIND = 119, FIXED = 120, LEFT = 121, LEN = 122, LOWER = 123, MID = 124,
			PROPER = 125, REPLACE = 126, REPT = 127, RIGHT = 128, RMB = 129, SEARCH = 130, SUBSTITUTE = 131,
			T = 132, TEXT = 133, TRIM = 134, UPPER = 135, VALUE = 136, DATEVALUE = 137, TIMEVALUE = 138,
			DATE = 139, TIME = 140, NOW = 141, TODAY = 142, YEAR = 143, MONTH = 144, DAY = 145,
			HOUR = 146, MINUTE = 147, SECOND = 148, WEEKDAY = 149, DATEDIF = 150, DAYS360 = 151,
			EDATE = 152, EOMONTH = 153, NETWORKDAYS = 154, WORKDAY = 155, WEEKNUM = 156, MAX = 157,
			MEDIAN = 158, MIN = 159, QUARTILE = 160, MODE = 161, LARGE = 162, SMALL = 163, PERCENTILE = 164,
			PERCENTRANK = 165, AVERAGE = 166, AVERAGEIF = 167, GEOMEAN = 168, HARMEAN = 169,
			COUNT = 170, COUNTIF = 171, SUM = 172, SUMIF = 173, AVEDEV = 174, STDEV = 175, STDEVP = 176,
			COVAR = 177, COVARIANCES = 178, DEVSQ = 179, VAR = 180, VARP = 181, NORMDIST = 182,
			NORMINV = 183, NORMSDIST = 184, NORMSINV = 185, BETADIST = 186, BETAINV = 187, BINOMDIST = 188,
			EXPONDIST = 189, FDIST = 190, FINV = 191, FISHER = 192, FISHERINV = 193, GAMMADIST = 194,
			GAMMAINV = 195, GAMMALN = 196, HYPGEOMDIST = 197, LOGINV = 198, LOGNORMDIST = 199,
			NEGBINOMDIST = 200, POISSON = 201, TDIST = 202, TINV = 203, WEIBULL = 204, URLENCODE = 205,
			URLDECODE = 206, HTMLENCODE = 207, HTMLDECODE = 208, BASE64TOTEXT = 209, BASE64URLTOTEXT = 210,
			TEXTTOBASE64 = 211, TEXTTOBASE64URL = 212, REGEX = 213, REGEXREPALCE = 214, ISREGEX = 215,
			GUID = 216, MD5 = 217, SHA1 = 218, SHA256 = 219, SHA512 = 220, CRC32 = 221, HMACMD5 = 222,
			HMACSHA1 = 223, HMACSHA256 = 224, HMACSHA512 = 225, TRIMSTART = 226, TRIMEND = 227,
			INDEXOF = 228, LASTINDEXOF = 229, SPLIT = 230, JOIN = 231, SUBSTRING = 232, STARTSWITH = 233,
			ENDSWITH = 234, ISNULLOREMPTY = 235, ISNULLORWHITESPACE = 236, REMOVESTART = 237,
			REMOVEEND = 238, JSON = 239, VLOOKUP = 240, ARRAY = 241, ALGORITHMVERSION = 242,
			ADDYEARS = 243, ADDMONTHS = 244, ADDDAYS = 245, ADDHOURS = 246, ADDMINUTES = 247,
			ADDSECONDS = 248, TIMESTAMP = 249, HAS = 250, HASVALUE = 251, PARAM = 252, PARAMETER = 253,
			WS = 254, COMMENT = 255, LINE_COMMENT = 256;

RULE_prog = 0, RULE_expr = 1, RULE_num = 2, RULE_unit = 3, RULE_arrayJson = 4,
			RULE_parameter2 = 5;
";
			var array = str.Split(",\r\n\t;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["Eof"] = "-1";
			foreach(var item in array) {
				var sp = item.Split('=');
				dict[sp[0].Trim()] = sp[1].Trim();
			}

			var filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathParser.cs");
			var csText = File.ReadAllText(filePath);



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
			csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*ErrorHandler", "ErrorHandler");
			csText = Regex.Replace(csText, @"public partial class mathParser", "partial class mathParser");
			csText = Regex.Replace(csText, @"public partial class", "internal partial class");



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


				//EnterRule(_localctx, 4, RULE_parameter);
				//public override int RuleIndex { get { return RULE_expr; } }
				//EnterRecursionRule(_localctx, 2, RULE_expr, _p);
				//PushNewRecursionContext(_localctx, _startState, RULE_expr);


				//mathParser.STRING
				//_la==T__16
			}

			csText = csText.Replace("//[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER()", "[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PARAMETER()");

			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathParser.cs", csText);


			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathLexer.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public partial class mathLexer", "partial class mathLexer");

			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathLexer.cs", csText);



			filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathVisitor.cs");
			csText = File.ReadAllText(filePath);

			csText = csText.Replace("[System.CLSCompliant(false)]", "");
			csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.13.2\")]", "");
			csText = csText.Replace("public interface ImathVisitor<Result>", "interface ImathVisitor<Result>");
			csText = csText.Replace("[NotNull] ", "");

			csText = "namespace ToolGood.Algorithm.math\r\n{" + csText + "\r\n}";
			File.WriteAllText("mathVisitor.cs", csText);

		}
	}

}
