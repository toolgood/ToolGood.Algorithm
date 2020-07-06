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
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, SUB=24, NUM=25, 
		STRING=26, IF=27, IFERROR=28, ISNUMBER=29, ISTEXT=30, ISERROR=31, ISNONTEXT=32, 
		ISLOGICAL=33, ISEVEN=34, ISODD=35, AND=36, OR=37, NOT=38, TRUE=39, FALSE=40, 
		E=41, PI=42, DEC2BIN=43, DEC2HEX=44, DEC2OCT=45, HEX2BIN=46, HEX2DEC=47, 
		HEX2OCT=48, OCT2BIN=49, OCT2DEC=50, OCT2HEX=51, BIN2OCT=52, BIN2DEC=53, 
		BIN2HEX=54, ABS=55, QUOTIENT=56, MOD=57, SIGN=58, SQRT=59, TRUNC=60, INT=61, 
		GCD=62, LCM=63, COMBIN=64, PERMUT=65, DEGREES=66, RADIANS=67, COS=68, 
		COSH=69, SIN=70, SINH=71, TAN=72, TANH=73, ACOS=74, ACOSH=75, ASIN=76, 
		ASINH=77, ATAN=78, ATANH=79, ATAN2=80, ROUND=81, ROUNDDOWN=82, ROUNDUP=83, 
		CEILING=84, FLOOR=85, EVEN=86, ODD=87, MROUND=88, RAND=89, RANDBETWEEN=90, 
		FACT=91, FACTDOUBLE=92, POWER=93, EXP=94, LN=95, LOG=96, LOG10=97, MULTINOMIAL=98, 
		PRODUCT=99, SQRTPI=100, SUMSQ=101, ASC=102, JIS=103, CHAR=104, CLEAN=105, 
		CODE=106, CONCATENATE=107, EXACT=108, FIND=109, FIXED=110, LEFT=111, LEN=112, 
		LOWER=113, MID=114, PROPER=115, REPLACE=116, REPT=117, RIGHT=118, RMB=119, 
		SEARCH=120, SUBSTITUTE=121, T=122, TEXT=123, TRIM=124, UPPER=125, VALUE=126, 
		DATEVALUE=127, TIMEVALUE=128, DATE=129, TIME=130, NOW=131, TODAY=132, 
		YEAR=133, MONTH=134, DAY=135, HOUR=136, MINUTE=137, SECOND=138, WEEKDAY=139, 
		DATEDIF=140, DAYS360=141, EDATE=142, EOMONTH=143, NETWORKDAYS=144, WORKDAY=145, 
		WEEKNUM=146, MAX=147, MEDIAN=148, MIN=149, QUARTILE=150, MODE=151, LARGE=152, 
		SMALL=153, PERCENTILE=154, PERCENTRANK=155, AVERAGE=156, AVERAGEIF=157, 
		GEOMEAN=158, HARMEAN=159, COUNT=160, COUNTIF=161, SUM=162, SUMIF=163, 
		AVEDEV=164, STDEV=165, STDEVP=166, DEVSQ=167, VAR=168, VARP=169, NORMDIST=170, 
		NORMINV=171, NORMSDIST=172, NORMSINV=173, BETADIST=174, BETAINV=175, BINOMDIST=176, 
		EXPONDIST=177, FDIST=178, FINV=179, FISHER=180, FISHERINV=181, GAMMADIST=182, 
		GAMMAINV=183, GAMMALN=184, HYPGEOMDIST=185, LOGINV=186, LOGNORMDIST=187, 
		NEGBINOMDIST=188, POISSON=189, TDIST=190, TINV=191, WEIBULL=192, URLENCODE=193, 
		URLDECODE=194, HTMLENCODE=195, HTMLDECODE=196, BASE64TOTEXT=197, BASE64URLTOTEXT=198, 
		TEXTTOBASE64=199, TEXTTOBASE64URL=200, REGEX=201, REGEXREPALCE=202, ISREGEX=203, 
		GUID=204, MD5=205, SHA1=206, SHA256=207, SHA512=208, CRC8=209, CRC16=210, 
		CRC32=211, HMACMD5=212, HMACSHA1=213, HMACSHA256=214, HMACSHA512=215, 
		TRIMSTART=216, TRIMEND=217, INDEXOF=218, LASTINDEXOF=219, SPLIT=220, JOIN=221, 
		SUBSTRING=222, STARTSWITH=223, ENDSWITH=224, ISNULLOREMPTY=225, ISNULLORWHITESPACE=226, 
		REMOVESTART=227, REMOVEEND=228, JSON=229, VLOOKUP=230, PARAMETER=231, 
		WS=232;

RULE_prog=0, RULE_expr=1, RULE_expr2=2, RULE_parameter=3, RULE_parameter2=4
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
