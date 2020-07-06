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
		STRING=26, NULL=27, IF=28, IFERROR=29, ISNUMBER=30, ISTEXT=31, ISERROR=32, 
		ISNONTEXT=33, ISLOGICAL=34, ISEVEN=35, ISODD=36, ISNULL=37, ISNULLORERROR=38, 
		AND=39, OR=40, NOT=41, TRUE=42, FALSE=43, E=44, PI=45, DEC2BIN=46, DEC2HEX=47, 
		DEC2OCT=48, HEX2BIN=49, HEX2DEC=50, HEX2OCT=51, OCT2BIN=52, OCT2DEC=53, 
		OCT2HEX=54, BIN2OCT=55, BIN2DEC=56, BIN2HEX=57, ABS=58, QUOTIENT=59, MOD=60, 
		SIGN=61, SQRT=62, TRUNC=63, INT=64, GCD=65, LCM=66, COMBIN=67, PERMUT=68, 
		DEGREES=69, RADIANS=70, COS=71, COSH=72, SIN=73, SINH=74, TAN=75, TANH=76, 
		ACOS=77, ACOSH=78, ASIN=79, ASINH=80, ATAN=81, ATANH=82, ATAN2=83, ROUND=84, 
		ROUNDDOWN=85, ROUNDUP=86, CEILING=87, FLOOR=88, EVEN=89, ODD=90, MROUND=91, 
		RAND=92, RANDBETWEEN=93, FACT=94, FACTDOUBLE=95, POWER=96, EXP=97, LN=98, 
		LOG=99, LOG10=100, MULTINOMIAL=101, PRODUCT=102, SQRTPI=103, SUMSQ=104, 
		ASC=105, JIS=106, CHAR=107, CLEAN=108, CODE=109, CONCATENATE=110, EXACT=111, 
		FIND=112, FIXED=113, LEFT=114, LEN=115, LOWER=116, MID=117, PROPER=118, 
		REPLACE=119, REPT=120, RIGHT=121, RMB=122, SEARCH=123, SUBSTITUTE=124, 
		T=125, TEXT=126, TRIM=127, UPPER=128, VALUE=129, DATEVALUE=130, TIMEVALUE=131, 
		DATE=132, TIME=133, NOW=134, TODAY=135, YEAR=136, MONTH=137, DAY=138, 
		HOUR=139, MINUTE=140, SECOND=141, WEEKDAY=142, DATEDIF=143, DAYS360=144, 
		EDATE=145, EOMONTH=146, NETWORKDAYS=147, WORKDAY=148, WEEKNUM=149, MAX=150, 
		MEDIAN=151, MIN=152, QUARTILE=153, MODE=154, LARGE=155, SMALL=156, PERCENTILE=157, 
		PERCENTRANK=158, AVERAGE=159, AVERAGEIF=160, GEOMEAN=161, HARMEAN=162, 
		COUNT=163, COUNTIF=164, SUM=165, SUMIF=166, AVEDEV=167, STDEV=168, STDEVP=169, 
		DEVSQ=170, VAR=171, VARP=172, NORMDIST=173, NORMINV=174, NORMSDIST=175, 
		NORMSINV=176, BETADIST=177, BETAINV=178, BINOMDIST=179, EXPONDIST=180, 
		FDIST=181, FINV=182, FISHER=183, FISHERINV=184, GAMMADIST=185, GAMMAINV=186, 
		GAMMALN=187, HYPGEOMDIST=188, LOGINV=189, LOGNORMDIST=190, NEGBINOMDIST=191, 
		POISSON=192, TDIST=193, TINV=194, WEIBULL=195, URLENCODE=196, URLDECODE=197, 
		HTMLENCODE=198, HTMLDECODE=199, BASE64TOTEXT=200, BASE64URLTOTEXT=201, 
		TEXTTOBASE64=202, TEXTTOBASE64URL=203, REGEX=204, REGEXREPALCE=205, ISREGEX=206, 
		GUID=207, MD5=208, SHA1=209, SHA256=210, SHA512=211, CRC8=212, CRC16=213, 
		CRC32=214, HMACMD5=215, HMACSHA1=216, HMACSHA256=217, HMACSHA512=218, 
		TRIMSTART=219, TRIMEND=220, INDEXOF=221, LASTINDEXOF=222, SPLIT=223, JOIN=224, 
		SUBSTRING=225, STARTSWITH=226, ENDSWITH=227, ISNULLOREMPTY=228, ISNULLORWHITESPACE=229, 
		REMOVESTART=230, REMOVEEND=231, JSON=232, VLOOKUP=233, LOOKUP=234, PARAMETER=235, 
		WS=236;

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
