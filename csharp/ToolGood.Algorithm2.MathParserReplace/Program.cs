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
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, SUB=24, IF=25, 
		IFERROR=26, ISNUMBER=27, ISTEXT=28, ISERROR=29, ISNONTEXT=30, ISLOGICAL=31, 
		ISEVEN=32, ISODD=33, AND=34, OR=35, NOT=36, TRUE=37, FALSE=38, E=39, PI=40, 
		DEC2BIN=41, DEC2HEX=42, DEC2OCT=43, HEX2BIN=44, HEX2DEC=45, HEX2OCT=46, 
		OCT2BIN=47, OCT2DEC=48, OCT2HEX=49, BIN2OCT=50, BIN2DEC=51, BIN2HEX=52, 
		ABS=53, QUOTIENT=54, MOD=55, SIGN=56, SQRT=57, TRUNC=58, INT=59, GCD=60, 
		LCM=61, COMBIN=62, PERMUT=63, DEGREES=64, RADIANS=65, COS=66, COSH=67, 
		SIN=68, SINH=69, TAN=70, TANH=71, ACOS=72, ACOSH=73, ASIN=74, ASINH=75, 
		ATAN=76, ATANH=77, ATAN2=78, ROUND=79, ROUNDDOWN=80, ROUNDUP=81, CEILING=82, 
		FLOOR=83, EVEN=84, ODD=85, MROUND=86, RAND=87, RANDBETWEEN=88, FACT=89, 
		FACTDOUBLE=90, POWER=91, EXP=92, LN=93, LOG=94, LOG10=95, MULTINOMIAL=96, 
		PRODUCT=97, SQRTPI=98, SUMSQ=99, ASC=100, JIS=101, CHAR=102, CLEAN=103, 
		CODE=104, CONCATENATE=105, EXACT=106, FIND=107, FIXED=108, LEFT=109, LEN=110, 
		LOWER=111, MID=112, PROPER=113, REPLACE=114, REPT=115, RIGHT=116, RMB=117, 
		SEARCH=118, SUBSTITUTE=119, T=120, TEXT=121, TRIM=122, UPPER=123, VALUE=124, 
		DATEVALUE=125, TIMEVALUE=126, DATE=127, TIME=128, NOW=129, TODAY=130, 
		YEAR=131, MONTH=132, DAY=133, HOUR=134, MINUTE=135, SECOND=136, WEEKDAY=137, 
		DATEDIF=138, DAYS360=139, EDATE=140, EOMONTH=141, NETWORKDAYS=142, WORKDAY=143, 
		WEEKNUM=144, MAX=145, MEDIAN=146, MIN=147, QUARTILE=148, MODE=149, LARGE=150, 
		SMALL=151, PERCENTILE=152, PERCENTRANK=153, AVERAGE=154, AVERAGEIF=155, 
		GEOMEAN=156, HARMEAN=157, COUNT=158, COUNTIF=159, SUM=160, SUMIF=161, 
		AVEDEV=162, STDEV=163, STDEVP=164, DEVSQ=165, VAR=166, VARP=167, NORMDIST=168, 
		NORMINV=169, NORMSDIST=170, NORMSINV=171, BETADIST=172, BETAINV=173, BINOMDIST=174, 
		EXPONDIST=175, FDIST=176, FINV=177, FISHER=178, FISHERINV=179, GAMMADIST=180, 
		GAMMAINV=181, GAMMALN=182, HYPGEOMDIST=183, LOGINV=184, LOGNORMDIST=185, 
		NEGBINOMDIST=186, POISSON=187, TDIST=188, TINV=189, WEIBULL=190, URLENCODE=191, 
		URLDECODE=192, HTMLENCODE=193, HTMLDECODE=194, BASE64TOTEXT=195, BASE64URLTOTEXT=196, 
		TEXTTOBASE64=197, TEXTTOBASE64URL=198, REGEX=199, REGEXREPALCE=200, ISREGEX=201, 
		GUID=202, MD5=203, SHA1=204, SHA256=205, SHA512=206, CRC8=207, CRC16=208, 
		CRC32=209, HMACMD5=210, HMACSHA1=211, HMACSHA256=212, HMACSHA512=213, 
		TRIMSTART=214, TRIMEND=215, INDEXOF=216, LASTINDEXOF=217, SPLIT=218, JOIN=219, 
		SUBSTRING=220, STARTSWITH=221, ENDSWITH=222, ISNULLOREMPTY=223, ISNULLORWHITESPACE=224, 
		REMOVESTART=225, REMOVEEND=226, JSON=227, NUM=228, STRING=229, PARAMETER=230, 
		WS=231

RULE_prog=0, RULE_expr=1, RULE_parameter=2, RULE_parameter2=3, RULE_parameter2=4
";
            var array = str.Split(" ,\r\n\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
