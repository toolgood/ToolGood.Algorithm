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
            var str = @"T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, SUB=27, NUM=28, STRING=29, NULL=30, IF=31, IFERROR=32, 
		ISNUMBER=33, ISTEXT=34, ISERROR=35, ISNONTEXT=36, ISLOGICAL=37, ISEVEN=38, 
		ISODD=39, ISNULL=40, ISNULLORERROR=41, AND=42, OR=43, NOT=44, TRUE=45, 
		FALSE=46, E=47, PI=48, DEC2BIN=49, DEC2HEX=50, DEC2OCT=51, HEX2BIN=52, 
		HEX2DEC=53, HEX2OCT=54, OCT2BIN=55, OCT2DEC=56, OCT2HEX=57, BIN2OCT=58, 
		BIN2DEC=59, BIN2HEX=60, ABS=61, QUOTIENT=62, MOD=63, SIGN=64, SQRT=65, 
		TRUNC=66, INT=67, GCD=68, LCM=69, COMBIN=70, PERMUT=71, DEGREES=72, RADIANS=73, 
		COS=74, COSH=75, SIN=76, SINH=77, TAN=78, TANH=79, ACOS=80, ACOSH=81, 
		ASIN=82, ASINH=83, ATAN=84, ATANH=85, ATAN2=86, ROUND=87, ROUNDDOWN=88, 
		ROUNDUP=89, CEILING=90, FLOOR=91, EVEN=92, ODD=93, MROUND=94, RAND=95, 
		RANDBETWEEN=96, FACT=97, FACTDOUBLE=98, POWER=99, EXP=100, LN=101, LOG=102, 
		LOG10=103, MULTINOMIAL=104, PRODUCT=105, SQRTPI=106, SUMSQ=107, ASC=108, 
		JIS=109, CHAR=110, CLEAN=111, CODE=112, CONCATENATE=113, EXACT=114, FIND=115, 
		FIXED=116, LEFT=117, LEN=118, LOWER=119, MID=120, PROPER=121, REPLACE=122, 
		REPT=123, RIGHT=124, RMB=125, SEARCH=126, SUBSTITUTE=127, T=128, TEXT=129, 
		TRIM=130, UPPER=131, VALUE=132, DATEVALUE=133, TIMEVALUE=134, DATE=135, 
		TIME=136, NOW=137, TODAY=138, YEAR=139, MONTH=140, DAY=141, HOUR=142, 
		MINUTE=143, SECOND=144, WEEKDAY=145, DATEDIF=146, DAYS360=147, EDATE=148, 
		EOMONTH=149, NETWORKDAYS=150, WORKDAY=151, WEEKNUM=152, MAX=153, MEDIAN=154, 
		MIN=155, QUARTILE=156, MODE=157, LARGE=158, SMALL=159, PERCENTILE=160, 
		PERCENTRANK=161, AVERAGE=162, AVERAGEIF=163, GEOMEAN=164, HARMEAN=165, 
		COUNT=166, COUNTIF=167, SUM=168, SUMIF=169, AVEDEV=170, STDEV=171, STDEVP=172, 
		DEVSQ=173, VAR=174, VARP=175, NORMDIST=176, NORMINV=177, NORMSDIST=178, 
		NORMSINV=179, BETADIST=180, BETAINV=181, BINOMDIST=182, EXPONDIST=183, 
		FDIST=184, FINV=185, FISHER=186, FISHERINV=187, GAMMADIST=188, GAMMAINV=189, 
		GAMMALN=190, HYPGEOMDIST=191, LOGINV=192, LOGNORMDIST=193, NEGBINOMDIST=194, 
		POISSON=195, TDIST=196, TINV=197, WEIBULL=198, URLENCODE=199, URLDECODE=200, 
		HTMLENCODE=201, HTMLDECODE=202, BASE64TOTEXT=203, BASE64URLTOTEXT=204, 
		TEXTTOBASE64=205, TEXTTOBASE64URL=206, REGEX=207, REGEXREPALCE=208, ISREGEX=209, 
		GUID=210, MD5=211, SHA1=212, SHA256=213, SHA512=214, CRC32=215, HMACMD5=216, 
		HMACSHA1=217, HMACSHA256=218, HMACSHA512=219, TRIMSTART=220, TRIMEND=221, 
		INDEXOF=222, LASTINDEXOF=223, SPLIT=224, JOIN=225, SUBSTRING=226, STARTSWITH=227, 
		ENDSWITH=228, ISNULLOREMPTY=229, ISNULLORWHITESPACE=230, REMOVESTART=231, 
		REMOVEEND=232, JSON=233, VLOOKUP=234, LOOKUP=235, ARRAY=236, ADDYEARS=237, 
		ADDMONTHS=238, ADDDAYS=239, ADDHOURS=240, ADDMINUTES=241, ADDSECONDS=242, 
		TIMESTAMP=243, PARAMETER=244, PARAMETER2=245, WS=246, COMMENT=247, LINE_COMMENT=248;

RULE_prog=0, RULE_expr=1, RULE_parameter2=2;
";
            var array = str.Split(" ,\r\n\t;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in array) {
                var sp = item.Split('=');
                dict[sp[0].Trim()] = sp[1].Trim();
            }

            var filePath = Path.GetFullPath(@"..\..\..\..\..\g4\bin\mathParser.cs");
            //var filePath = Path.GetFullPath(@"..\..\..\..\..\..\csharp\ToolGood.Algorithm2\math\mathParser.cs");
            var csText = File.ReadAllText(filePath);



            csText = csText.Replace("[System.CLSCompliant(false)]", "");
            csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.9.3\")]", "");
            csText = csText.Replace("[System.CodeDom.Compiler.GeneratedCode(\"ANTLR\", \"4.11.1\")]", "");
            csText = csText.Replace("[System.Diagnostics.DebuggerNonUserCode]", "");
            csText = csText.Replace("[RuleVersion(0)]", "");
            csText = csText.Replace("[NotNull]", "");
            csText = csText.Replace(" public ITerminalNode", "//public ITerminalNode");
            csText = csText.Replace("else return visitor.VisitChildren(this);", "");
            csText = csText.Replace("if (typedVisitor != null) ", "");

            csText = Regex.Replace(csText, "if \\(!\\(Precpred.*\\r\\n\\t*", "");
            csText = Regex.Replace(csText, "State = \\d+;\\r\\n[ \\t]+State =", "State =");
            csText = Regex.Replace(csText, "return GetRuleContext<ExprContext>\\(i\\);\\r\\n", "return GetRuleContext<ExprContext>(i);");
            csText = Regex.Replace(csText, "public ExprContext expr\\(int i\\) \\{\\r\\n", "//public ExprContext expr(int i) {");

            csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*Match\(", "Match(");
            csText = Regex.Replace(csText, @"\bState = (\d+);\r\n[ \t]*ErrorHandler", "ErrorHandler");
            csText = Regex.Replace(csText, @"public partial class mathParser", "partial class mathParser");



            
            var list = dict.Keys.ToList().OrderByDescending(q => q.Length).ToList();

            foreach (var item in list) {
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
            File.WriteAllText("mathParser.cs", csText);





        }
    }

}
