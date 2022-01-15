using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm2.MathParserReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = @"T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, SUB=26, NUM=27, STRING=28, NULL=29, IF=30, IFERROR=31, ISNUMBER=32, 
		ISTEXT=33, ISERROR=34, ISNONTEXT=35, ISLOGICAL=36, ISEVEN=37, ISODD=38, 
		ISNULL=39, ISNULLORERROR=40, AND=41, OR=42, NOT=43, TRUE=44, FALSE=45, 
		E=46, PI=47, DEC2BIN=48, DEC2HEX=49, DEC2OCT=50, HEX2BIN=51, HEX2DEC=52, 
		HEX2OCT=53, OCT2BIN=54, OCT2DEC=55, OCT2HEX=56, BIN2OCT=57, BIN2DEC=58, 
		BIN2HEX=59, ABS=60, QUOTIENT=61, MOD=62, SIGN=63, SQRT=64, TRUNC=65, INT=66, 
		GCD=67, LCM=68, COMBIN=69, PERMUT=70, DEGREES=71, RADIANS=72, COS=73, 
		COSH=74, SIN=75, SINH=76, TAN=77, TANH=78, ACOS=79, ACOSH=80, ASIN=81, 
		ASINH=82, ATAN=83, ATANH=84, ATAN2=85, ROUND=86, ROUNDDOWN=87, ROUNDUP=88, 
		CEILING=89, FLOOR=90, EVEN=91, ODD=92, MROUND=93, RAND=94, RANDBETWEEN=95, 
		FACT=96, FACTDOUBLE=97, POWER=98, EXP=99, LN=100, LOG=101, LOG10=102, 
		MULTINOMIAL=103, PRODUCT=104, SQRTPI=105, SUMSQ=106, ASC=107, JIS=108, 
		CHAR=109, CLEAN=110, CODE=111, CONCATENATE=112, EXACT=113, FIND=114, FIXED=115, 
		LEFT=116, LEN=117, LOWER=118, MID=119, PROPER=120, REPLACE=121, REPT=122, 
		RIGHT=123, RMB=124, SEARCH=125, SUBSTITUTE=126, T=127, TEXT=128, TRIM=129, 
		UPPER=130, VALUE=131, DATEVALUE=132, TIMEVALUE=133, DATE=134, TIME=135, 
		NOW=136, TODAY=137, YEAR=138, MONTH=139, DAY=140, HOUR=141, MINUTE=142, 
		SECOND=143, WEEKDAY=144, DATEDIF=145, DAYS360=146, EDATE=147, EOMONTH=148, 
		NETWORKDAYS=149, WORKDAY=150, WEEKNUM=151, MAX=152, MEDIAN=153, MIN=154, 
		QUARTILE=155, MODE=156, LARGE=157, SMALL=158, PERCENTILE=159, PERCENTRANK=160, 
		AVERAGE=161, AVERAGEIF=162, GEOMEAN=163, HARMEAN=164, COUNT=165, COUNTIF=166, 
		SUM=167, SUMIF=168, AVEDEV=169, STDEV=170, STDEVP=171, DEVSQ=172, VAR=173, 
		VARP=174, NORMDIST=175, NORMINV=176, NORMSDIST=177, NORMSINV=178, BETADIST=179, 
		BETAINV=180, BINOMDIST=181, EXPONDIST=182, FDIST=183, FINV=184, FISHER=185, 
		FISHERINV=186, GAMMADIST=187, GAMMAINV=188, GAMMALN=189, HYPGEOMDIST=190, 
		LOGINV=191, LOGNORMDIST=192, NEGBINOMDIST=193, POISSON=194, TDIST=195, 
		TINV=196, WEIBULL=197, URLENCODE=198, URLDECODE=199, HTMLENCODE=200, HTMLDECODE=201, 
		BASE64TOTEXT=202, BASE64URLTOTEXT=203, TEXTTOBASE64=204, TEXTTOBASE64URL=205, 
		REGEX=206, REGEXREPALCE=207, ISREGEX=208, GUID=209, MD5=210, SHA1=211, 
		SHA256=212, SHA512=213, CRC32=214, HMACMD5=215, HMACSHA1=216, HMACSHA256=217, 
		HMACSHA512=218, TRIMSTART=219, TRIMEND=220, INDEXOF=221, LASTINDEXOF=222, 
		SPLIT=223, JOIN=224, SUBSTRING=225, STARTSWITH=226, ENDSWITH=227, ISNULLOREMPTY=228, 
		ISNULLORWHITESPACE=229, REMOVESTART=230, REMOVEEND=231, JSON=232, VLOOKUP=233, 
		LOOKUP=234, ARRAY=235, PARAMETER=236, PARAMETER2=237, WS=238;

RULE_prog=0, RULE_expr=1, RULE_expr2=2, RULE_parameter2=3;
";
            var array = str.Split(" ,\r\n\t;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in array) {
                var sp = item.Split('=');
                dict[sp[0]] = sp[1];
            }
            var csText = File.ReadAllText(@"..\..\..\..\ToolGood.Algorithm2\math\mathParser.cs");

            var list = dict.Keys.ToList().OrderByDescending(q => q.Length).ToList();

            foreach (var item in list) {
                var value = dict[item];
				csText= Regex.Replace(csText, $"case {item}:", $"case {value}:");
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
