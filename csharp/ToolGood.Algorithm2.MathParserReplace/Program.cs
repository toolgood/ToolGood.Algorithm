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
		T__24=25, T__25=26, SUB=27, LB=28, RB=29, COMMA=30, NUM=31, STRING=32, 
		NULL=33, IF=34, IFERROR=35, ISNUMBER=36, ISTEXT=37, ISERROR=38, ISNONTEXT=39, 
		ISLOGICAL=40, ISEVEN=41, ISODD=42, ISNULL=43, ISNULLORERROR=44, AND=45, 
		OR=46, NOT=47, TRUE=48, FALSE=49, E=50, PI=51, DEC2BIN=52, DEC2HEX=53, 
		DEC2OCT=54, HEX2BIN=55, HEX2DEC=56, HEX2OCT=57, OCT2BIN=58, OCT2DEC=59, 
		OCT2HEX=60, BIN2OCT=61, BIN2DEC=62, BIN2HEX=63, ABS=64, QUOTIENT=65, MOD=66, 
		SIGN=67, SQRT=68, TRUNC=69, INT=70, GCD=71, LCM=72, COMBIN=73, PERMUT=74, 
		DEGREES=75, RADIANS=76, COS=77, COSH=78, SIN=79, SINH=80, TAN=81, TANH=82, 
		ACOS=83, ACOSH=84, ASIN=85, ASINH=86, ATAN=87, ATANH=88, ATAN2=89, ROUND=90, 
		ROUNDDOWN=91, ROUNDUP=92, CEILING=93, FLOOR=94, EVEN=95, ODD=96, MROUND=97, 
		RAND=98, RANDBETWEEN=99, FACT=100, FACTDOUBLE=101, POWER=102, EXP=103, 
		LN=104, LOG=105, LOG10=106, MULTINOMIAL=107, PRODUCT=108, SQRTPI=109, 
		SUMSQ=110, ASC=111, JIS=112, CHAR=113, CLEAN=114, CODE=115, CONCATENATE=116, 
		EXACT=117, FIND=118, FIXED=119, LEFT=120, LEN=121, LOWER=122, MID=123, 
		PROPER=124, REPLACE=125, REPT=126, RIGHT=127, RMB=128, SEARCH=129, SUBSTITUTE=130, 
		T=131, TEXT=132, TRIM=133, UPPER=134, VALUE=135, DATEVALUE=136, TIMEVALUE=137, 
		DATE=138, TIME=139, NOW=140, TODAY=141, YEAR=142, MONTH=143, DAY=144, 
		HOUR=145, MINUTE=146, SECOND=147, WEEKDAY=148, DATEDIF=149, DAYS360=150, 
		EDATE=151, EOMONTH=152, NETWORKDAYS=153, WORKDAY=154, WEEKNUM=155, MAX=156, 
		MEDIAN=157, MIN=158, QUARTILE=159, MODE=160, LARGE=161, SMALL=162, PERCENTILE=163, 
		PERCENTRANK=164, AVERAGE=165, AVERAGEIF=166, GEOMEAN=167, HARMEAN=168, 
		COUNT=169, COUNTIF=170, SUM=171, SUMIF=172, AVEDEV=173, STDEV=174, STDEVP=175, 
		DEVSQ=176, VAR=177, VARP=178, NORMDIST=179, NORMINV=180, NORMSDIST=181, 
		NORMSINV=182, BETADIST=183, BETAINV=184, BINOMDIST=185, EXPONDIST=186, 
		FDIST=187, FINV=188, FISHER=189, FISHERINV=190, GAMMADIST=191, GAMMAINV=192, 
		GAMMALN=193, HYPGEOMDIST=194, LOGINV=195, LOGNORMDIST=196, NEGBINOMDIST=197, 
		POISSON=198, TDIST=199, TINV=200, WEIBULL=201, URLENCODE=202, URLDECODE=203, 
		HTMLENCODE=204, HTMLDECODE=205, BASE64TOTEXT=206, BASE64URLTOTEXT=207, 
		TEXTTOBASE64=208, TEXTTOBASE64URL=209, REGEX=210, REGEXREPALCE=211, ISREGEX=212, 
		GUID=213, MD5=214, SHA1=215, SHA256=216, SHA512=217, CRC32=218, HMACMD5=219, 
		HMACSHA1=220, HMACSHA256=221, HMACSHA512=222, TRIMSTART=223, TRIMEND=224, 
		INDEXOF=225, LASTINDEXOF=226, SPLIT=227, JOIN=228, SUBSTRING=229, STARTSWITH=230, 
		ENDSWITH=231, ISNULLOREMPTY=232, ISNULLORWHITESPACE=233, REMOVESTART=234, 
		REMOVEEND=235, JSON=236, VLOOKUP=237, LOOKUP=238, ARRAY=239, PARAMETER=240, 
		PARAMETER2=241, WS=242;

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
