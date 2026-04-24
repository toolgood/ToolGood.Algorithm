using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ToolGood.Algorithm.math
{
	internal sealed partial class mathLexer : ITokenSource
	{
		public const int
			T__0 = 1, T__1 = 2, T__2 = 3, T__3 = 4, T__4 = 5, T__5 = 6, T__6 = 7, T__7 = 8, T__8 = 9,
			T__9 = 10, T__10 = 11, OPAND = 12, OPOR = 13, OPADD = 14, OPSUB = 15, OPMUL = 16, OPDIV = 17,
			OPMOD = 18, OPCAT = 19, OPGE = 20, OPLE = 21, OPGT = 22, OPLT = 23, OPNE = 24, OPEQ = 25,
			NUM = 26, STRING = 27, ALGORITHMVERSION = 28, FALSE = 29, NULL = 30, TRUE = 31, E = 32,
			GUID = 33, NOW = 34, PI = 35, RAND = 36, TODAY = 37, ABS = 38, ACOS = 39, ACOSH = 40,
			ACOT = 41, ACOTH = 42, ADDDAYS = 43, ADDHOURS = 44, ADDMINUTES = 45, ADDMONTHS = 46,
			ADDSECONDS = 47, ADDYEARS = 48, AND = 49, ARABIC = 50, ARRAY = 51, ASC = 52, ASIN = 53,
			ASINH = 54, ATAN = 55, ATAN2 = 56, ATANH = 57, AVEDEV = 58, AVERAGE = 59, AVERAGEIF = 60,
			BASE64TOTEXT = 61, BASE64URLTOTEXT = 62, BESSELI = 63, BESSELJ = 64, BESSELK = 65,
			BESSELY = 66, BETADIST = 67, BETAINV = 68, BIN2DEC = 69, BIN2HEX = 70, BIN2OCT = 71,
			BINOMDIST = 72, CEILING = 73, CHAR = 74, CLEAN = 75, CODE = 76, COMBIN = 77, CONCATENATE = 78,
			CORREL = 79, COS = 80, COSH = 81, COT = 82, COTH = 83, COUNT = 84, COUNTIF = 85, COVAR = 86,
			COVARIANCES = 87, CSC = 88, CSCH = 89, DATE = 90, DATEDIF = 91, DATEVALUE = 92, DAY = 93,
			DAYS = 94, DAYS360 = 95, DB = 96, DDB = 97, DEC2BIN = 98, DEC2HEX = 99, DEC2OCT = 100,
			DEGREES = 101, DELTA = 102, DEVSQ = 103, EDATE = 104, ENDSWITH = 105, EOMONTH = 106,
			ERF = 107, ERFC = 108, ERROR = 109, EVEN = 110, EXACT = 111, EXP = 112, EXPONDIST = 113,
			FACT = 114, FACTDOUBLE = 115, FDIST = 116, FIND = 117, FINV = 118, FISHER = 119, FISHERINV = 120,
			FIXED = 121, FLOOR = 122, FORECAST = 123, FV = 124, GAMMADIST = 125, GAMMAINV = 126,
			GAMMALN = 127, GCD = 128, GEOMEAN = 129, GESTEP = 130, HARMEAN = 131, HAS = 132, HASVALUE = 133,
			HEX2BIN = 134, HEX2DEC = 135, HEX2OCT = 136, HMACMD5 = 137, HMACSHA1 = 138, HMACSHA256 = 139,
			HMACSHA512 = 140, HOUR = 141, HTMLDECODE = 142, HTMLENCODE = 143, HYPGEOMDIST = 144,
			IF = 145, IFERROR = 146, IFS = 147, INDEXOF = 148, INT = 149, INTERCEPT = 150, IPMT = 151,
			IRR = 152, ISERROR = 153, ISEVEN = 154, ISLOGICAL = 155, ISNONTEXT = 156, ISNULL = 157,
			ISNULLOREMPTY = 158, ISNULLORERROR = 159, ISNULLORWHITESPACE = 160, ISNUMBER = 161,
			ISODD = 162, ISREGEX = 163, ISTEXT = 164, JIS = 165, JOIN = 166, JSON = 167, LARGE = 168,
			LASTINDEXOF = 169, LCM = 170, LEFT = 171, LEN = 172, LN = 173, LOG = 174, LOG10 = 175,
			LOGINV = 176, LOGNORMDIST = 177, LOOKCEILING = 178, LOOKFLOOR = 179, LOWER = 180,
			MAX = 181, MD5 = 182, MEDIAN = 183, MID = 184, MIN = 185, MINUTE = 186, MIRR = 187,
			MOD = 188, MODE = 189, MONTH = 190, MROUND = 191, MULTINOMIAL = 192, NEGBINOMDIST = 193,
			NETWORKDAYS = 194, NORMDIST = 195, NORMINV = 196, NORMSDIST = 197, NORMSINV = 198, NOT = 199,
			NPER = 200, NPV = 201, OCT2BIN = 202, OCT2DEC = 203, OCT2HEX = 204, ODD = 205,
			OR = 206, PARAM = 207, PEARSON = 208, PERCENTILE = 209, PERCENTRANK = 210, PERMUT = 211,
			PMT = 212, POISSON = 213, POWER = 214, PPMT = 215, PRODUCT = 216, PROPER = 217, PV = 218,
			QUARTILE = 219, QUOTIENT = 220, RADIANS = 221, RANDBETWEEN = 222, RANK = 223, RATE = 224,
			REGEX = 225, REGEXREPLACE = 226, REMOVEEND = 227, REMOVESTART = 228, REPLACE = 229,
			REPT = 230, RIGHT = 231, RMB = 232, ROMAN = 233, ROUND = 234, ROUNDDOWN = 235, ROUNDUP = 236,
			SEARCH = 237, SEC = 238, SECH = 239, SECOND = 240, SERIESSUM = 241, SHA1 = 242, SHA256 = 243,
			SHA512 = 244, SIGN = 245, SIN = 246, SINH = 247, SLN = 248, SLOPE = 249, SMALL = 250,
			SPLIT = 251, SQRT = 252, SQRTPI = 253, STARTSWITH = 254, STDEV = 255, STDEVP = 256,
			SUBSTITUTE = 257, SUBSTRING = 258, SUM = 259, SUMIF = 260, SUMPRODUCT = 261, SUMSQ = 262,
			SUMX2MY2 = 263, SUMX2PY2 = 264, SUMXMY2 = 265, SWITCH = 266, SYD = 267, T = 268, TAN = 269,
			TANH = 270, TDIST = 271, TEXT = 272, TEXTTOBASE64 = 273, TEXTTOBASE64URL = 274,
			TIME = 275, TIMESTAMP = 276, TIMEVALUE = 277, TINV = 278, TRIM = 279, TRIMEND = 280,
			TRIMSTART = 281, TRUNC = 282, UNICHAR = 283, UNICODE = 284, UPPER = 285, URLDECODE = 286,
			URLENCODE = 287, VALUE = 288, VAR = 289, VARP = 290, WEEKDAY = 291, WEEKNUM = 292,
			WEIBULL = 293, WORKDAY = 294, XIRR = 295, XNPV = 296, XOR = 297, YEAR = 298, YEARFRAC = 299,
			PARAMETER = 300;

		private static readonly Dictionary<string, int> _keywords;
		private readonly ICharStream _input;
		private int _startCharIndex;

		static mathLexer()
		{
			_keywords = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
			{
				{ "ALGORITHMVERSION", ALGORITHMVERSION }, { "ENGINEVERSION", ALGORITHMVERSION },
				{ "FALSE", FALSE }, { "NO", FALSE },
				{ "NULL", NULL },
				{ "TRUE", TRUE }, { "YES", TRUE },
				{ "E", E },
				{ "GUID", GUID },
				{ "NOW", NOW },
				{ "PI", PI },
				{ "RAND", RAND },
				{ "TODAY", TODAY },
				{ "ABS", ABS },
				{ "ACOS", ACOS },
				{ "ACOSH", ACOSH },
				{ "ACOT", ACOT },
				{ "ACOTH", ACOTH },
				{ "ADDDAYS", ADDDAYS },
				{ "ADDHOURS", ADDHOURS },
				{ "ADDMINUTES", ADDMINUTES },
				{ "ADDMONTHS", ADDMONTHS },
				{ "ADDSECONDS", ADDSECONDS },
				{ "ADDYEARS", ADDYEARS },
				{ "AND", AND },
				{ "ARABIC", ARABIC },
				{ "ARRAY", ARRAY },
				{ "ASC", ASC },
				{ "ASIN", ASIN },
				{ "ASINH", ASINH },
				{ "ATAN", ATAN },
				{ "ATAN2", ATAN2 },
				{ "ATANH", ATANH },
				{ "AVEDEV", AVEDEV },
				{ "AVERAGE", AVERAGE },
				{ "AVERAGEIF", AVERAGEIF },
				{ "BASE64TOTEXT", BASE64TOTEXT },
				{ "BASE64URLTOTEXT", BASE64URLTOTEXT },
				{ "BESSELI", BESSELI },
				{ "BESSELJ", BESSELJ },
				{ "BESSELK", BESSELK },
				{ "BESSELY", BESSELY },
				{ "BETA.DIST", BETADIST }, { "BETADIST", BETADIST },
				{ "BETA.INV", BETAINV }, { "BETAINV", BETAINV },
				{ "BIN2DEC", BIN2DEC },
				{ "BIN2HEX", BIN2HEX },
				{ "BIN2OCT", BIN2OCT },
				{ "BINOM.DIST", BINOMDIST }, { "BINOMDIST", BINOMDIST },
				{ "CEILING", CEILING }, { "CEIL", CEILING },
				{ "CHAR", CHAR },
				{ "CLEAN", CLEAN },
				{ "CODE", CODE },
				{ "COMBIN", COMBIN },
				{ "CONCATENATE", CONCATENATE }, { "CONCAT", CONCATENATE },
				{ "CORREL", CORREL },
				{ "COS", COS },
				{ "COSH", COSH },
				{ "COT", COT },
				{ "COTH", COTH },
				{ "COUNT", COUNT },
				{ "COUNTIF", COUNTIF },
				{ "COVAR", COVAR }, { "COVARIANCE.P", COVAR },
				{ "COVARIANCE.S", COVARIANCES },
				{ "CSC", CSC },
				{ "CSCH", CSCH },
				{ "DATE", DATE },
				{ "DATEDIF", DATEDIF },
				{ "DATEVALUE", DATEVALUE },
				{ "DAY", DAY },
				{ "DAYS", DAYS },
				{ "DAYS360", DAYS360 },
				{ "DB", DB },
				{ "DDB", DDB },
				{ "DEC2BIN", DEC2BIN },
				{ "DEC2HEX", DEC2HEX },
				{ "DEC2OCT", DEC2OCT },
				{ "DEGREES", DEGREES },
				{ "DELTA", DELTA },
				{ "DEVSQ", DEVSQ },
				{ "EDATE", EDATE },
				{ "ENDSWITH", ENDSWITH },
				{ "EOMONTH", EOMONTH },
				{ "ERF", ERF },
				{ "ERFC", ERFC },
				{ "ERROR", ERROR },
				{ "EVEN", EVEN },
				{ "EXACT", EXACT },
				{ "EXP", EXP },
				{ "EXPON.DIST", EXPONDIST }, { "EXPONDIST", EXPONDIST },
				{ "FACT", FACT },
				{ "FACTDOUBLE", FACTDOUBLE },
				{ "F.DIST", FDIST }, { "FDIST", FDIST },
				{ "FIND", FIND },
				{ "F.INV", FINV }, { "FINV", FINV },
				{ "FISHER", FISHER },
				{ "FISHERINV", FISHERINV },
				{ "FIXED", FIXED },
				{ "FLOOR", FLOOR },
				{ "FORECAST", FORECAST },
				{ "FV", FV },
				{ "GAMMA.DIST", GAMMADIST }, { "GAMMADIST", GAMMADIST },
				{ "GAMMA.INV", GAMMAINV }, { "GAMMAINV", GAMMAINV },
				{ "GAMMALN", GAMMALN }, { "GAMMALN.PRECISE", GAMMALN },
				{ "GCD", GCD },
				{ "GEOMEAN", GEOMEAN },
				{ "GESTEP", GESTEP },
				{ "HARMEAN", HARMEAN },
				{ "HASKEY", HAS }, { "HAS", HAS }, { "CONTAINSKEY", HAS }, { "CONTAINS", HAS },
				{ "HASVALUE", HASVALUE }, { "CONTAINSVALUE", HASVALUE },
				{ "HEX2BIN", HEX2BIN },
				{ "HEX2DEC", HEX2DEC },
				{ "HEX2OCT", HEX2OCT },
				{ "HMACMD5", HMACMD5 },
				{ "HMACSHA1", HMACSHA1 },
				{ "HMACSHA256", HMACSHA256 },
				{ "HMACSHA512", HMACSHA512 },
				{ "HOUR", HOUR },
				{ "HTMLDECODE", HTMLDECODE },
				{ "HTMLENCODE", HTMLENCODE },
				{ "HYPGEOM.DIST", HYPGEOMDIST }, { "HYPGEOMDIST", HYPGEOMDIST },
				{ "IF", IF },
				{ "IFERROR", IFERROR },
				{ "IFS", IFS },
				{ "INDEXOF", INDEXOF },
				{ "INT", INT },
				{ "INTERCEPT", INTERCEPT },
				{ "IPMT", IPMT },
				{ "IRR", IRR },
				{ "ISERROR", ISERROR },
				{ "ISEVEN", ISEVEN },
				{ "ISLOGICAL", ISLOGICAL },
				{ "ISNONTEXT", ISNONTEXT },
				{ "ISNULL", ISNULL },
				{ "ISNULLOREMPTY", ISNULLOREMPTY },
				{ "ISNULLORERROR", ISNULLORERROR },
				{ "ISNULLORWHITESPACE", ISNULLORWHITESPACE },
				{ "ISNUMBER", ISNUMBER },
				{ "ISODD", ISODD },
				{ "ISREGEX", ISREGEX }, { "ISMATCH", ISREGEX },
				{ "ISTEXT", ISTEXT },
				{ "JIS", JIS }, { "WIDECHAR", JIS },
				{ "JOIN", JOIN },
				{ "JSON", JSON },
				{ "LARGE", LARGE },
				{ "LASTINDEXOF", LASTINDEXOF },
				{ "LCM", LCM },
				{ "LEFT", LEFT },
				{ "LEN", LEN },
				{ "LN", LN },
				{ "LOG", LOG },
				{ "LOG10", LOG10 },
				{ "LOGNORM.INV", LOGINV }, { "LOGINV", LOGINV },
				{ "LOGNORM.DIST", LOGNORMDIST }, { "LOGNORMDIST", LOGNORMDIST },
				{ "LOOKCEILING", LOOKCEILING }, { "LOOKCEIL", LOOKCEILING },
				{ "LOOKFLOOR", LOOKFLOOR },
				{ "LOWER", LOWER }, { "TOLOWER", LOWER },
				{ "MAX", MAX },
				{ "MD5", MD5 },
				{ "MEDIAN", MEDIAN },
				{ "MID", MID },
				{ "MIN", MIN },
				{ "MINUTE", MINUTE },
				{ "MIRR", MIRR },
				{ "MOD", MOD },
				{ "MODE", MODE },
				{ "MONTH", MONTH },
				{ "MROUND", MROUND },
				{ "MULTINOMIAL", MULTINOMIAL },
				{ "NEGBINOM.DIST", NEGBINOMDIST }, { "NEGBINOMDIST", NEGBINOMDIST },
				{ "NETWORKDAYS", NETWORKDAYS },
				{ "NORM.DIST", NORMDIST }, { "NORMDIST", NORMDIST },
				{ "NORM.INV", NORMINV }, { "NORMINV", NORMINV },
				{ "NORMS.DIST", NORMSDIST }, { "NORMSDIST", NORMSDIST },
				{ "NORMS.INV", NORMSINV }, { "NORMSINV", NORMSINV },
				{ "NOT", NOT },
				{ "NPER", NPER },
				{ "NPV", NPV },
				{ "OCT2BIN", OCT2BIN },
				{ "OCT2DEC", OCT2DEC },
				{ "OCT2HEX", OCT2HEX },
				{ "ODD", ODD },
				{ "OR", OR },
				{ "PARAM", PARAM }, { "PARAMETER", PARAM }, { "GETPARAMETER", PARAM },
				{ "PEARSON", PEARSON },
				{ "PERCENTILE", PERCENTILE }, { "PERCENTILE.INC", PERCENTILE },
				{ "PERCENTRANK", PERCENTRANK }, { "PERCENTRANK.INC", PERCENTRANK },
				{ "PERMUT", PERMUT },
				{ "PMT", PMT },
				{ "POISSON", POISSON }, { "POISSON.DIST", POISSON },
				{ "POWER", POWER },
				{ "PPMT", PPMT },
				{ "PRODUCT", PRODUCT },
				{ "PROPER", PROPER },
				{ "PV", PV },
				{ "QUARTILE", QUARTILE },
				{ "QUOTIENT", QUOTIENT },
				{ "RADIANS", RADIANS },
				{ "RANDBETWEEN", RANDBETWEEN },
				{ "RANK", RANK },
				{ "RATE", RATE },
				{ "REGEX", REGEX },
				{ "REGEXREPLACE", REGEXREPLACE },
				{ "REMOVEEND", REMOVEEND },
				{ "REMOVESTART", REMOVESTART },
				{ "REPLACE", REPLACE },
				{ "REPT", REPT },
				{ "RIGHT", RIGHT },
				{ "RMB", RMB },
				{ "ROMAN", ROMAN },
				{ "ROUND", ROUND },
				{ "ROUNDDOWN", ROUNDDOWN },
				{ "ROUNDUP", ROUNDUP },
				{ "SEARCH", SEARCH },
				{ "SEC", SEC },
				{ "SECH", SECH },
				{ "SECOND", SECOND },
				{ "SERIESSUM", SERIESSUM },
				{ "SHA1", SHA1 },
				{ "SHA256", SHA256 },
				{ "SHA512", SHA512 },
				{ "SIGN", SIGN },
				{ "SIN", SIN },
				{ "SINH", SINH },
				{ "SLN", SLN },
				{ "SLOPE", SLOPE },
				{ "SMALL", SMALL },
				{ "SPLIT", SPLIT },
				{ "SQRT", SQRT },
				{ "SQRTPI", SQRTPI },
				{ "STARTSWITH", STARTSWITH },
				{ "STDEV", STDEV }, { "STDEV.S", STDEV },
				{ "STDEVP", STDEVP }, { "STDEV.P", STDEVP },
				{ "SUBSTITUTE", SUBSTITUTE },
				{ "SUBSTRING", SUBSTRING },
				{ "SUM", SUM },
				{ "SUMIF", SUMIF },
				{ "SUMPRODUCT", SUMPRODUCT },
				{ "SUMSQ", SUMSQ },
				{ "SUMX2MY2", SUMX2MY2 },
				{ "SUMX2PY2", SUMX2PY2 },
				{ "SUMXMY2", SUMXMY2 },
				{ "SWITCH", SWITCH },
				{ "SYD", SYD },
				{ "T", T },
				{ "TAN", TAN },
				{ "TANH", TANH },
				{ "T.DIST", TDIST }, { "TDIST", TDIST },
				{ "TEXT", TEXT },
				{ "TEXTTOBASE64", TEXTTOBASE64 },
				{ "TEXTTOBASE64URL", TEXTTOBASE64URL },
				{ "TIME", TIME },
				{ "TIMESTAMP", TIMESTAMP },
				{ "TIMEVALUE", TIMEVALUE },
				{ "T.INV", TINV }, { "TINV", TINV },
				{ "TRIM", TRIM },
				{ "TRIMEND", TRIMEND }, { "RTRIM", TRIMEND },
				{ "TRIMSTART", TRIMSTART }, { "LTRIM", TRIMSTART },
				{ "TRUNC", TRUNC },
				{ "UNICHAR", UNICHAR },
				{ "UNICODE", UNICODE },
				{ "UPPER", UPPER }, { "TOUPPER", UPPER },
				{ "URLDECODE", URLDECODE },
				{ "URLENCODE", URLENCODE },
				{ "VALUE", VALUE },
				{ "VAR", VAR }, { "VAR.S", VAR },
				{ "VARP", VARP }, { "VAR.P", VARP },
				{ "WEEKDAY", WEEKDAY },
				{ "WEEKNUM", WEEKNUM },
				{ "WEIBULL", WEIBULL },
				{ "WORKDAY", WORKDAY },
				{ "XIRR", XIRR },
				{ "XNPV", XNPV },
				{ "XOR", XOR },
				{ "YEAR", YEAR },
				{ "YEARFRAC", YEARFRAC }
			};
		}

		public mathLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
		{
			_input = input;
		}

		public int Line => 0;
		public int Column => 0;
		public ICharStream InputStream => _input;
		public string SourceName => _input.SourceName;
		public ITokenFactory TokenFactory { get; set; } = new CommonTokenFactory();

		public IToken NextToken()
		{
			while(true) {
				int c = _input.LA(1);
				if(c == IntStreamConstants.EOF) {
					return TokenFactory.Create(TokenConstants.EOF, "");
				}
				if(IsDigit(c)) {
					_startCharIndex = _input.Index;
					return ReadNumber();
				} else if(c == '\'' || c == '"' || c == '`') {
					_startCharIndex = _input.Index;
					return ReadString(c);
				} else if(IsIdentifierStart(c)) {
					_startCharIndex = _input.Index;
					return ReadIdentifier();
				} else if(IsWhitespace(c)) {
					ConsumeWhitespace();
					continue;
				} else if(c == '/') {
					var c2 = _input.LA(2);
					if(c2 == '*') {
						ConsumeBlockComment();
						continue;
					} else if(c2 == '/') {
						ConsumeLineComment();
						continue;
					}
				}
				_startCharIndex = _input.Index;
				return ReadOperator(c);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private IToken CreateToken(int type)
		{
			int stopIndex = _input.Index - 1;
			//string text = _input.GetText(Interval.Of(_startCharIndex, stopIndex));
			return TokenFactory.Create(Tuple.Create((ITokenSource)this, (ICharStream)_input), type, null, TokenConstants.DefaultChannel, _startCharIndex, stopIndex, 0, 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsWhitespace(int c)
		{
			return c == ' ' || c == '\n';
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsDigit(int c)
		{
			return c >= '0' && c <= '9';
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsIdentifierStart(int c)
		{
			return (c >= 'A' && c <= 'Z') || c == '_';
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsIdentifierPart(int c)
		{
			return (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ConsumeWhitespace()
		{
			_input.Consume();
			while(IsWhitespace(_input.LA(1))) {
				_input.Consume();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ConsumeBlockComment()
		{
			_input.Consume();
			_input.Consume();
			while(true) {
				int c = _input.LA(1);
				if(c == IntStreamConstants.EOF) break;
				if(c == '*' && _input.LA(2) == '/') {
					_input.Consume();
					_input.Consume();
					break;
				}
				_input.Consume();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ConsumeLineComment()
		{
			_input.Consume();
			_input.Consume();
			while(true) {
				int c = _input.LA(1);
				if(c == IntStreamConstants.EOF || c == '\n') break;
				_input.Consume();
			}
		}

		private IToken ReadString(int quote)
		{
			_input.Consume();
			while(true) {
				int c = _input.LA(1);
				if(c == IntStreamConstants.EOF) {
					break;
				}
				if(c == '\\') {
					_input.Consume();
					if(_input.LA(1) != IntStreamConstants.EOF) {
						_input.Consume();
					}
					continue;
				}
				if(c == quote) {
					_input.Consume();
					break;
				}
				_input.Consume();
			}
			return CreateToken(STRING);
		}

		private IToken ReadNumber()
		{
			_input.Consume();

			int c = _input.LA(1);
			while(IsDigit(c)) {
				_input.Consume();
				c = _input.LA(1);
			}

			if(c == '.' && IsDigit(_input.LA(2))) {
				_input.Consume();
				_input.Consume();
				c = _input.LA(1);
				while(IsDigit(c)) {
					_input.Consume();
					c = _input.LA(1);
				}
			}

			if(c == 'E') {
				var c2 = _input.LA(2);
				if(IsDigit(c2) || c2 == '+' || c2 == '-') {
					_input.Consume();
					_input.Consume();
					while(IsDigit(_input.LA(1))) {
						_input.Consume();
					}
				}
				return CreateToken(NUM);
			}

			if(c == 'K') {
				int c2 = _input.LA(2);
				if(c2 == 'M') {
					_input.Consume();
					_input.Consume();
					int c3 = _input.LA(1);
					if(c3 == '2' || c3 == '3') {
						_input.Consume();
					}
				} else if(c2 == 'G') {
					_input.Consume();
					_input.Consume();
				}
			} else if(c == 'C' || c == 'D') {
				int c2 = _input.LA(2);
				if(c2 == 'M') {
					_input.Consume();
					_input.Consume();
					int c3 = _input.LA(1);
					if(c3 == '2' || c3 == '3') {
						_input.Consume();
					}
				}
			} else if(c == 'M') {
				_input.Consume();
				int c2 = _input.LA(1);
				if(c2 == 'M') {
					_input.Consume();
					int c3 = _input.LA(1);
					if(c3 == '2' || c3 == '3') {
						_input.Consume();
					}
				} else if(c2 == '2' || c2 == '3' || c2 == 'L') {
					_input.Consume();
				}
			} else if(c == 'G') {
				_input.Consume();
			} else if(c == 'T') {
				_input.Consume();
			} else if(c == 'L') {
				_input.Consume();
			}

			return CreateToken(NUM);
		}

		private IToken ReadIdentifier()
		{
			_input.Consume();
			int c = _input.LA(1);
			while(IsIdentifierPart(c)) {
				_input.Consume();
				c = _input.LA(1);
			}

			if(c == '.' && IsIdentifierStart(_input.LA(2))) {
				int savedIndex = _input.Index;

				_input.Consume();
				while(IsIdentifierPart(_input.LA(1))) {
					_input.Consume();
				}

				string fullText = _input.GetText(Interval.Of(_startCharIndex, _input.Index - 1));
				if(_keywords.TryGetValue(fullText, out int fullTokenType)) {
					return CreateToken(fullTokenType);
				}

				_input.Seek(savedIndex);
			}

			string text = _input.GetText(Interval.Of(_startCharIndex, _input.Index - 1));
			if(_keywords.TryGetValue(text, out int tokenType)) {
				return CreateToken(tokenType);
			}

			return CreateToken(PARAMETER);
		}

		private IToken ReadOperator(int c)
		{
			_input.Consume();
			switch(c) {
				case '(':
					return CreateToken(T__0);
				case ')':
					return CreateToken(T__1);
				case '.':
					return CreateToken(T__2);
				case ',':
					return CreateToken(T__3);
				case '[':
					return CreateToken(T__4);
				case ']':
					return CreateToken(T__5);
				case '!':
					if(_input.LA(1) == '=') {
						_input.Consume();
						if(_input.LA(1) == '=') {
							_input.Consume();
						}
						return CreateToken(OPNE);
					}
					return CreateToken(T__6);
				case '?':
					return CreateToken(T__7);
				case ':':
					return CreateToken(T__8);
				case '{':
					return CreateToken(T__9);
				case '}':
					return CreateToken(T__10);
				case '&':
					if(_input.LA(1) == '&') {
						_input.Consume();
						return CreateToken(OPAND);
					}
					return CreateToken(OPCAT);
				case '|':
					if(_input.LA(1) == '|') {
						_input.Consume();
						return CreateToken(OPOR);
					}
					break;
				case '+':
					return CreateToken(OPADD);
				case '-':
					return CreateToken(OPSUB);
				case '*':
					return CreateToken(OPMUL);
				case '/':
					return CreateToken(OPDIV);
				case '%':
					return CreateToken(OPMOD);
				case '>':
					if(_input.LA(1) == '=') {
						_input.Consume();
						return CreateToken(OPGE);
					}
					return CreateToken(OPGT);
				case '<':
					if(_input.LA(1) == '=') {
						_input.Consume();
						return CreateToken(OPLE);
					}
					if(_input.LA(1) == '>') {
						_input.Consume();
						return CreateToken(OPNE);
					}
					return CreateToken(OPLT);
				case '=':
					if(_input.LA(1) == '=') {
						_input.Consume();
						if(_input.LA(1) == '=') {
							_input.Consume();
						}
						return CreateToken(OPEQ);
					}
					return CreateToken(OPEQ);
			}
			return CreateToken(TokenConstants.InvalidType);
		}
	}
}