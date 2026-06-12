// Generated from math.g4 by ANTLR 4.13.2
package toolgood.algorithm.math;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class mathParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, OPAND=12, OPOR=13, OPADD=14, OPSUB=15, OPMUL=16, OPDIV=17, 
		OPMOD=18, OPCAT=19, OPGE=20, OPLE=21, OPGT=22, OPLT=23, OPNE=24, OPEQ=25, 
		NUM=26, STRING=27, ALGORITHMVERSION=28, FALSE=29, NULL=30, TRUE=31, E=32, 
		GUID=33, NOW=34, PI=35, RAND=36, TODAY=37, ABS=38, ACOS=39, ACOSH=40, 
		ACOT=41, ACOTH=42, ADDDAYS=43, ADDHOURS=44, ADDMINUTES=45, ADDMONTHS=46, 
		ADDSECONDS=47, ADDYEARS=48, AND=49, ARABIC=50, ARRAY=51, ASC=52, ASIN=53, 
		ASINH=54, ATAN=55, ATAN2=56, ATANH=57, AVEDEV=58, AVERAGE=59, AVERAGEIF=60, 
		BASE64TOTEXT=61, BASE64URLTOTEXT=62, BESSELI=63, BESSELJ=64, BESSELK=65, 
		BESSELY=66, BETADIST=67, BETAINV=68, BIN2DEC=69, BIN2HEX=70, BIN2OCT=71, 
		BINOMDIST=72, CEILING=73, CHAR=74, CLEAN=75, CODE=76, COMBIN=77, CONCATENATE=78, 
		CORREL=79, COS=80, COSH=81, COT=82, COTH=83, COUNT=84, COUNTIF=85, COVAR=86, 
		COVARIANCES=87, CSC=88, CSCH=89, DATE=90, DATEDIF=91, DATEVALUE=92, DAY=93, 
		DAYS=94, DAYS360=95, DB=96, DDB=97, DEC2BIN=98, DEC2HEX=99, DEC2OCT=100, 
		DEGREES=101, DELTA=102, DEVSQ=103, EDATE=104, ENDSWITH=105, EOMONTH=106, 
		ERF=107, ERFC=108, ERROR=109, EVEN=110, EXACT=111, EXP=112, EXPONDIST=113, 
		FACT=114, FACTDOUBLE=115, FDIST=116, FIND=117, FINV=118, FISHER=119, FISHERINV=120, 
		FIXED=121, FLOOR=122, FORECAST=123, FV=124, GAMMADIST=125, GAMMAINV=126, 
		GAMMALN=127, GCD=128, GEOMEAN=129, GESTEP=130, HARMEAN=131, HAS=132, HASVALUE=133, 
		HEX2BIN=134, HEX2DEC=135, HEX2OCT=136, HMACMD5=137, HMACSHA1=138, HMACSHA256=139, 
		HMACSHA512=140, HOUR=141, HTMLDECODE=142, HTMLENCODE=143, HYPGEOMDIST=144, 
		IF=145, IFERROR=146, IFS=147, INDEXOF=148, INT=149, INTERCEPT=150, IPMT=151, 
		IRR=152, ISERROR=153, ISEVEN=154, ISLOGICAL=155, ISNONTEXT=156, ISNULL=157, 
		ISNULLOREMPTY=158, ISNULLORERROR=159, ISNULLORWHITESPACE=160, ISNUMBER=161, 
		ISODD=162, ISREGEX=163, ISTEXT=164, JIS=165, JOIN=166, JSON=167, LARGE=168, 
		LASTINDEXOF=169, LCM=170, LEFT=171, LEN=172, LN=173, LOG=174, LOG10=175, 
		LOGINV=176, LOGNORMDIST=177, LOOKCEILING=178, LOOKFLOOR=179, LOWER=180, 
		MAX=181, MD5=182, MEDIAN=183, MID=184, MIN=185, MINUTE=186, MIRR=187, 
		MOD=188, MODE=189, MONTH=190, MROUND=191, MULTINOMIAL=192, NEGBINOMDIST=193, 
		NETWORKDAYS=194, NORMDIST=195, NORMINV=196, NORMSDIST=197, NORMSINV=198, 
		NOT=199, NPER=200, NPV=201, OCT2BIN=202, OCT2DEC=203, OCT2HEX=204, ODD=205, 
		OR=206, PARAM=207, PEARSON=208, PERCENTILE=209, PERCENTRANK=210, PERMUT=211, 
		PMT=212, POISSON=213, POWER=214, PPMT=215, PRODUCT=216, PROPER=217, PV=218, 
		QUARTILE=219, QUOTIENT=220, RADIANS=221, RANDBETWEEN=222, RANK=223, RATE=224, 
		REGEX=225, REGEXREPLACE=226, REMOVEEND=227, REMOVESTART=228, REPLACE=229, 
		REPT=230, RIGHT=231, RMB=232, ROMAN=233, ROUND=234, ROUNDDOWN=235, ROUNDUP=236, 
		SEARCH=237, SEC=238, SECH=239, SECOND=240, SERIESSUM=241, SHA1=242, SHA256=243, 
		SHA512=244, SIGN=245, SIN=246, SINH=247, SLN=248, SLOPE=249, SMALL=250, 
		SPLIT=251, SQRT=252, SQRTPI=253, STARTSWITH=254, STDEV=255, STDEVP=256, 
		SUBSTITUTE=257, SUBSTRING=258, SUM=259, SUMIF=260, SUMPRODUCT=261, SUMSQ=262, 
		SUMX2MY2=263, SUMX2PY2=264, SUMXMY2=265, SWITCH=266, SYD=267, T=268, TAN=269, 
		TANH=270, TDIST=271, TEXT=272, TEXTTOBASE64=273, TEXTTOBASE64URL=274, 
		TIME=275, TIMESTAMP=276, TIMEVALUE=277, TINV=278, TRIM=279, TRIMEND=280, 
		TRIMSTART=281, TRUNC=282, UNICHAR=283, UNICODE=284, UPPER=285, URLDECODE=286, 
		URLENCODE=287, VALUE=288, VAR=289, VARP=290, WEEKDAY=291, WEEKNUM=292, 
		WEIBULL=293, WORKDAY=294, XIRR=295, XNPV=296, XOR=297, YEAR=298, YEARFRAC=299, 
		PARAMETER=300, WS=301, COMMENT=302, LINE_COMMENT=303;
	public static final int
		RULE_prog = 0, RULE_expr = 1, RULE_arrayJson = 2, RULE_parameter2 = 3;
	private static String[] makeRuleNames() {
		return new String[] {
			"prog", "expr", "arrayJson", "parameter2"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'('", "')'", "'.'", "','", "'['", "']'", "'!'", "'?'", "':'", 
			"'{'", "'}'", "'&&'", "'||'", "'+'", "'-'", "'*'", "'/'", "'%'", "'&'", 
			"'>='", "'<='", "'>'", "'<'", null, null, null, null, null, null, "'NULL'", 
			null, "'E'", "'GUID'", "'NOW'", "'PI'", "'RAND'", "'TODAY'", "'ABS'", 
			"'ACOS'", "'ACOSH'", "'ACOT'", "'ACOTH'", "'ADDDAYS'", "'ADDHOURS'", 
			"'ADDMINUTES'", "'ADDMONTHS'", "'ADDSECONDS'", "'ADDYEARS'", "'AND'", 
			"'ARABIC'", "'ARRAY'", "'ASC'", "'ASIN'", "'ASINH'", "'ATAN'", "'ATAN2'", 
			"'ATANH'", "'AVEDEV'", "'AVERAGE'", "'AVERAGEIF'", "'BASE64TOTEXT'", 
			"'BASE64URLTOTEXT'", "'BESSELI'", "'BESSELJ'", "'BESSELK'", "'BESSELY'", 
			null, null, "'BIN2DEC'", "'BIN2HEX'", "'BIN2OCT'", null, null, "'CHAR'", 
			"'CLEAN'", "'CODE'", "'COMBIN'", null, "'CORREL'", "'COS'", "'COSH'", 
			"'COT'", "'COTH'", "'COUNT'", "'COUNTIF'", null, "'COVARIANCE.S'", "'CSC'", 
			"'CSCH'", "'DATE'", "'DATEDIF'", "'DATEVALUE'", "'DAY'", "'DAYS'", "'DAYS360'", 
			"'DB'", "'DDB'", "'DEC2BIN'", "'DEC2HEX'", "'DEC2OCT'", "'DEGREES'", 
			"'DELTA'", "'DEVSQ'", "'EDATE'", "'ENDSWITH'", "'EOMONTH'", "'ERF'", 
			"'ERFC'", "'ERROR'", "'EVEN'", "'EXACT'", "'EXP'", null, "'FACT'", "'FACTDOUBLE'", 
			null, "'FIND'", null, "'FISHER'", "'FISHERINV'", "'FIXED'", "'FLOOR'", 
			"'FORECAST'", "'FV'", null, null, null, "'GCD'", "'GEOMEAN'", "'GESTEP'", 
			"'HARMEAN'", null, null, "'HEX2BIN'", "'HEX2DEC'", "'HEX2OCT'", "'HMACMD5'", 
			"'HMACSHA1'", "'HMACSHA256'", "'HMACSHA512'", "'HOUR'", "'HTMLDECODE'", 
			"'HTMLENCODE'", null, "'IF'", "'IFERROR'", "'IFS'", "'INDEXOF'", "'INT'", 
			"'INTERCEPT'", "'IPMT'", "'IRR'", "'ISERROR'", "'ISEVEN'", "'ISLOGICAL'", 
			"'ISNONTEXT'", "'ISNULL'", "'ISNULLOREMPTY'", "'ISNULLORERROR'", "'ISNULLORWHITESPACE'", 
			"'ISNUMBER'", "'ISODD'", null, "'ISTEXT'", null, "'JOIN'", "'JSON'", 
			"'LARGE'", "'LASTINDEXOF'", "'LCM'", "'LEFT'", "'LEN'", "'LN'", "'LOG'", 
			"'LOG10'", null, null, null, "'LOOKFLOOR'", null, "'MAX'", "'MD5'", "'MEDIAN'", 
			"'MID'", "'MIN'", "'MINUTE'", "'MIRR'", "'MOD'", "'MODE'", "'MONTH'", 
			"'MROUND'", "'MULTINOMIAL'", null, "'NETWORKDAYS'", null, null, null, 
			null, "'NOT'", "'NPER'", "'NPV'", "'OCT2BIN'", "'OCT2DEC'", "'OCT2HEX'", 
			"'ODD'", "'OR'", null, "'PEARSON'", null, null, "'PERMUT'", "'PMT'", 
			null, "'POWER'", "'PPMT'", "'PRODUCT'", "'PROPER'", "'PV'", "'QUARTILE'", 
			"'QUOTIENT'", "'RADIANS'", "'RANDBETWEEN'", "'RANK'", "'RATE'", "'REGEX'", 
			"'REGEXREPLACE'", "'REMOVEEND'", "'REMOVESTART'", "'REPLACE'", "'REPT'", 
			"'RIGHT'", "'RMB'", "'ROMAN'", "'ROUND'", "'ROUNDDOWN'", "'ROUNDUP'", 
			"'SEARCH'", "'SEC'", "'SECH'", "'SECOND'", "'SERIESSUM'", "'SHA1'", "'SHA256'", 
			"'SHA512'", "'SIGN'", "'SIN'", "'SINH'", "'SLN'", "'SLOPE'", "'SMALL'", 
			"'SPLIT'", "'SQRT'", "'SQRTPI'", "'STARTSWITH'", null, null, "'SUBSTITUTE'", 
			"'SUBSTRING'", "'SUM'", "'SUMIF'", "'SUMPRODUCT'", "'SUMSQ'", "'SUMX2MY2'", 
			"'SUMX2PY2'", "'SUMXMY2'", "'SWITCH'", "'SYD'", "'T'", "'TAN'", "'TANH'", 
			null, "'TEXT'", "'TEXTTOBASE64'", "'TEXTTOBASE64URL'", "'TIME'", "'TIMESTAMP'", 
			"'TIMEVALUE'", null, "'TRIM'", null, null, "'TRUNC'", "'UNICHAR'", "'UNICODE'", 
			null, "'URLDECODE'", "'URLENCODE'", "'VALUE'", null, null, "'WEEKDAY'", 
			"'WEEKNUM'", "'WEIBULL'", "'WORKDAY'", "'XIRR'", "'XNPV'", "'XOR'", "'YEAR'", 
			"'YEARFRAC'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			"OPAND", "OPOR", "OPADD", "OPSUB", "OPMUL", "OPDIV", "OPMOD", "OPCAT", 
			"OPGE", "OPLE", "OPGT", "OPLT", "OPNE", "OPEQ", "NUM", "STRING", "ALGORITHMVERSION", 
			"FALSE", "NULL", "TRUE", "E", "GUID", "NOW", "PI", "RAND", "TODAY", "ABS", 
			"ACOS", "ACOSH", "ACOT", "ACOTH", "ADDDAYS", "ADDHOURS", "ADDMINUTES", 
			"ADDMONTHS", "ADDSECONDS", "ADDYEARS", "AND", "ARABIC", "ARRAY", "ASC", 
			"ASIN", "ASINH", "ATAN", "ATAN2", "ATANH", "AVEDEV", "AVERAGE", "AVERAGEIF", 
			"BASE64TOTEXT", "BASE64URLTOTEXT", "BESSELI", "BESSELJ", "BESSELK", "BESSELY", 
			"BETADIST", "BETAINV", "BIN2DEC", "BIN2HEX", "BIN2OCT", "BINOMDIST", 
			"CEILING", "CHAR", "CLEAN", "CODE", "COMBIN", "CONCATENATE", "CORREL", 
			"COS", "COSH", "COT", "COTH", "COUNT", "COUNTIF", "COVAR", "COVARIANCES", 
			"CSC", "CSCH", "DATE", "DATEDIF", "DATEVALUE", "DAY", "DAYS", "DAYS360", 
			"DB", "DDB", "DEC2BIN", "DEC2HEX", "DEC2OCT", "DEGREES", "DELTA", "DEVSQ", 
			"EDATE", "ENDSWITH", "EOMONTH", "ERF", "ERFC", "ERROR", "EVEN", "EXACT", 
			"EXP", "EXPONDIST", "FACT", "FACTDOUBLE", "FDIST", "FIND", "FINV", "FISHER", 
			"FISHERINV", "FIXED", "FLOOR", "FORECAST", "FV", "GAMMADIST", "GAMMAINV", 
			"GAMMALN", "GCD", "GEOMEAN", "GESTEP", "HARMEAN", "HAS", "HASVALUE", 
			"HEX2BIN", "HEX2DEC", "HEX2OCT", "HMACMD5", "HMACSHA1", "HMACSHA256", 
			"HMACSHA512", "HOUR", "HTMLDECODE", "HTMLENCODE", "HYPGEOMDIST", "IF", 
			"IFERROR", "IFS", "INDEXOF", "INT", "INTERCEPT", "IPMT", "IRR", "ISERROR", 
			"ISEVEN", "ISLOGICAL", "ISNONTEXT", "ISNULL", "ISNULLOREMPTY", "ISNULLORERROR", 
			"ISNULLORWHITESPACE", "ISNUMBER", "ISODD", "ISREGEX", "ISTEXT", "JIS", 
			"JOIN", "JSON", "LARGE", "LASTINDEXOF", "LCM", "LEFT", "LEN", "LN", "LOG", 
			"LOG10", "LOGINV", "LOGNORMDIST", "LOOKCEILING", "LOOKFLOOR", "LOWER", 
			"MAX", "MD5", "MEDIAN", "MID", "MIN", "MINUTE", "MIRR", "MOD", "MODE", 
			"MONTH", "MROUND", "MULTINOMIAL", "NEGBINOMDIST", "NETWORKDAYS", "NORMDIST", 
			"NORMINV", "NORMSDIST", "NORMSINV", "NOT", "NPER", "NPV", "OCT2BIN", 
			"OCT2DEC", "OCT2HEX", "ODD", "OR", "PARAM", "PEARSON", "PERCENTILE", 
			"PERCENTRANK", "PERMUT", "PMT", "POISSON", "POWER", "PPMT", "PRODUCT", 
			"PROPER", "PV", "QUARTILE", "QUOTIENT", "RADIANS", "RANDBETWEEN", "RANK", 
			"RATE", "REGEX", "REGEXREPLACE", "REMOVEEND", "REMOVESTART", "REPLACE", 
			"REPT", "RIGHT", "RMB", "ROMAN", "ROUND", "ROUNDDOWN", "ROUNDUP", "SEARCH", 
			"SEC", "SECH", "SECOND", "SERIESSUM", "SHA1", "SHA256", "SHA512", "SIGN", 
			"SIN", "SINH", "SLN", "SLOPE", "SMALL", "SPLIT", "SQRT", "SQRTPI", "STARTSWITH", 
			"STDEV", "STDEVP", "SUBSTITUTE", "SUBSTRING", "SUM", "SUMIF", "SUMPRODUCT", 
			"SUMSQ", "SUMX2MY2", "SUMX2PY2", "SUMXMY2", "SWITCH", "SYD", "T", "TAN", 
			"TANH", "TDIST", "TEXT", "TEXTTOBASE64", "TEXTTOBASE64URL", "TIME", "TIMESTAMP", 
			"TIMEVALUE", "TINV", "TRIM", "TRIMEND", "TRIMSTART", "TRUNC", "UNICHAR", 
			"UNICODE", "UPPER", "URLDECODE", "URLENCODE", "VALUE", "VAR", "VARP", 
			"WEEKDAY", "WEEKNUM", "WEIBULL", "WORKDAY", "XIRR", "XNPV", "XOR", "YEAR", 
			"YEARFRAC", "PARAMETER", "WS", "COMMENT", "LINE_COMMENT"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "math.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public mathParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgContext extends MyRuleContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public TerminalNode EOF() { return getToken(mathParser.EOF, 0); }
		public ProgContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_prog; }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitProg(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ProgContext prog() throws RecognitionException {
		ProgContext _localctx = new ProgContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_prog);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(8);
			expr(0);
			setState(9);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExprContext extends MyRuleContext {
		public ExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expr; }
	 
		public ExprContext() { }
		public void copyFrom(ExprContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Function_funContext extends ExprContext {
		public Token f;
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode ERROR() { return getToken(mathParser.ERROR, 0); }
		public TerminalNode YEAR() { return getToken(mathParser.YEAR, 0); }
		public TerminalNode MONTH() { return getToken(mathParser.MONTH, 0); }
		public TerminalNode DAY() { return getToken(mathParser.DAY, 0); }
		public TerminalNode HOUR() { return getToken(mathParser.HOUR, 0); }
		public TerminalNode MINUTE() { return getToken(mathParser.MINUTE, 0); }
		public TerminalNode SECOND() { return getToken(mathParser.SECOND, 0); }
		public TerminalNode ISNUMBER() { return getToken(mathParser.ISNUMBER, 0); }
		public TerminalNode ISTEXT() { return getToken(mathParser.ISTEXT, 0); }
		public TerminalNode ISNONTEXT() { return getToken(mathParser.ISNONTEXT, 0); }
		public TerminalNode ISLOGICAL() { return getToken(mathParser.ISLOGICAL, 0); }
		public TerminalNode ISEVEN() { return getToken(mathParser.ISEVEN, 0); }
		public TerminalNode ISODD() { return getToken(mathParser.ISODD, 0); }
		public TerminalNode NOT() { return getToken(mathParser.NOT, 0); }
		public TerminalNode ABS() { return getToken(mathParser.ABS, 0); }
		public TerminalNode SIGN() { return getToken(mathParser.SIGN, 0); }
		public TerminalNode SQRT() { return getToken(mathParser.SQRT, 0); }
		public TerminalNode INT() { return getToken(mathParser.INT, 0); }
		public TerminalNode DEGREES() { return getToken(mathParser.DEGREES, 0); }
		public TerminalNode RADIANS() { return getToken(mathParser.RADIANS, 0); }
		public TerminalNode COS() { return getToken(mathParser.COS, 0); }
		public TerminalNode COSH() { return getToken(mathParser.COSH, 0); }
		public TerminalNode SIN() { return getToken(mathParser.SIN, 0); }
		public TerminalNode SINH() { return getToken(mathParser.SINH, 0); }
		public TerminalNode TAN() { return getToken(mathParser.TAN, 0); }
		public TerminalNode TANH() { return getToken(mathParser.TANH, 0); }
		public TerminalNode COT() { return getToken(mathParser.COT, 0); }
		public TerminalNode COTH() { return getToken(mathParser.COTH, 0); }
		public TerminalNode CSC() { return getToken(mathParser.CSC, 0); }
		public TerminalNode CSCH() { return getToken(mathParser.CSCH, 0); }
		public TerminalNode SEC() { return getToken(mathParser.SEC, 0); }
		public TerminalNode SECH() { return getToken(mathParser.SECH, 0); }
		public TerminalNode ACOS() { return getToken(mathParser.ACOS, 0); }
		public TerminalNode ACOSH() { return getToken(mathParser.ACOSH, 0); }
		public TerminalNode ASIN() { return getToken(mathParser.ASIN, 0); }
		public TerminalNode ASINH() { return getToken(mathParser.ASINH, 0); }
		public TerminalNode ATAN() { return getToken(mathParser.ATAN, 0); }
		public TerminalNode ATANH() { return getToken(mathParser.ATANH, 0); }
		public TerminalNode ACOT() { return getToken(mathParser.ACOT, 0); }
		public TerminalNode ACOTH() { return getToken(mathParser.ACOTH, 0); }
		public TerminalNode EVEN() { return getToken(mathParser.EVEN, 0); }
		public TerminalNode ODD() { return getToken(mathParser.ODD, 0); }
		public TerminalNode FACT() { return getToken(mathParser.FACT, 0); }
		public TerminalNode FACTDOUBLE() { return getToken(mathParser.FACTDOUBLE, 0); }
		public TerminalNode EXP() { return getToken(mathParser.EXP, 0); }
		public TerminalNode LN() { return getToken(mathParser.LN, 0); }
		public TerminalNode LOG10() { return getToken(mathParser.LOG10, 0); }
		public TerminalNode SQRTPI() { return getToken(mathParser.SQRTPI, 0); }
		public TerminalNode ERF() { return getToken(mathParser.ERF, 0); }
		public TerminalNode ERFC() { return getToken(mathParser.ERFC, 0); }
		public TerminalNode ARABIC() { return getToken(mathParser.ARABIC, 0); }
		public TerminalNode ASC() { return getToken(mathParser.ASC, 0); }
		public TerminalNode JIS() { return getToken(mathParser.JIS, 0); }
		public TerminalNode CHAR() { return getToken(mathParser.CHAR, 0); }
		public TerminalNode CLEAN() { return getToken(mathParser.CLEAN, 0); }
		public TerminalNode CODE() { return getToken(mathParser.CODE, 0); }
		public TerminalNode UNICHAR() { return getToken(mathParser.UNICHAR, 0); }
		public TerminalNode UNICODE() { return getToken(mathParser.UNICODE, 0); }
		public TerminalNode LEN() { return getToken(mathParser.LEN, 0); }
		public TerminalNode LOWER() { return getToken(mathParser.LOWER, 0); }
		public TerminalNode PROPER() { return getToken(mathParser.PROPER, 0); }
		public TerminalNode TRIM() { return getToken(mathParser.TRIM, 0); }
		public TerminalNode UPPER() { return getToken(mathParser.UPPER, 0); }
		public TerminalNode VALUE() { return getToken(mathParser.VALUE, 0); }
		public TerminalNode TIMEVALUE() { return getToken(mathParser.TIMEVALUE, 0); }
		public TerminalNode NORMSDIST() { return getToken(mathParser.NORMSDIST, 0); }
		public TerminalNode NORMSINV() { return getToken(mathParser.NORMSINV, 0); }
		public TerminalNode FISHER() { return getToken(mathParser.FISHER, 0); }
		public TerminalNode FISHERINV() { return getToken(mathParser.FISHERINV, 0); }
		public TerminalNode GAMMALN() { return getToken(mathParser.GAMMALN, 0); }
		public TerminalNode URLENCODE() { return getToken(mathParser.URLENCODE, 0); }
		public TerminalNode URLDECODE() { return getToken(mathParser.URLDECODE, 0); }
		public TerminalNode HTMLENCODE() { return getToken(mathParser.HTMLENCODE, 0); }
		public TerminalNode HTMLDECODE() { return getToken(mathParser.HTMLDECODE, 0); }
		public TerminalNode BASE64TOTEXT() { return getToken(mathParser.BASE64TOTEXT, 0); }
		public TerminalNode BASE64URLTOTEXT() { return getToken(mathParser.BASE64URLTOTEXT, 0); }
		public TerminalNode TEXTTOBASE64() { return getToken(mathParser.TEXTTOBASE64, 0); }
		public TerminalNode TEXTTOBASE64URL() { return getToken(mathParser.TEXTTOBASE64URL, 0); }
		public TerminalNode ISNULLOREMPTY() { return getToken(mathParser.ISNULLOREMPTY, 0); }
		public TerminalNode ISNULLORWHITESPACE() { return getToken(mathParser.ISNULLORWHITESPACE, 0); }
		public TerminalNode JSON() { return getToken(mathParser.JSON, 0); }
		public TerminalNode T() { return getToken(mathParser.T, 0); }
		public TerminalNode RMB() { return getToken(mathParser.RMB, 0); }
		public TerminalNode MD5() { return getToken(mathParser.MD5, 0); }
		public TerminalNode SHA1() { return getToken(mathParser.SHA1, 0); }
		public TerminalNode SHA256() { return getToken(mathParser.SHA256, 0); }
		public TerminalNode SHA512() { return getToken(mathParser.SHA512, 0); }
		public TerminalNode DEC2BIN() { return getToken(mathParser.DEC2BIN, 0); }
		public TerminalNode DEC2HEX() { return getToken(mathParser.DEC2HEX, 0); }
		public TerminalNode DEC2OCT() { return getToken(mathParser.DEC2OCT, 0); }
		public TerminalNode HEX2BIN() { return getToken(mathParser.HEX2BIN, 0); }
		public TerminalNode HEX2DEC() { return getToken(mathParser.HEX2DEC, 0); }
		public TerminalNode HEX2OCT() { return getToken(mathParser.HEX2OCT, 0); }
		public TerminalNode OCT2BIN() { return getToken(mathParser.OCT2BIN, 0); }
		public TerminalNode OCT2DEC() { return getToken(mathParser.OCT2DEC, 0); }
		public TerminalNode OCT2HEX() { return getToken(mathParser.OCT2HEX, 0); }
		public TerminalNode BIN2OCT() { return getToken(mathParser.BIN2OCT, 0); }
		public TerminalNode BIN2DEC() { return getToken(mathParser.BIN2DEC, 0); }
		public TerminalNode BIN2HEX() { return getToken(mathParser.BIN2HEX, 0); }
		public TerminalNode ISERROR() { return getToken(mathParser.ISERROR, 0); }
		public TerminalNode ISNULL() { return getToken(mathParser.ISNULL, 0); }
		public TerminalNode ISNULLORERROR() { return getToken(mathParser.ISNULLORERROR, 0); }
		public TerminalNode TRUNC() { return getToken(mathParser.TRUNC, 0); }
		public TerminalNode ROUND() { return getToken(mathParser.ROUND, 0); }
		public TerminalNode CEILING() { return getToken(mathParser.CEILING, 0); }
		public TerminalNode FLOOR() { return getToken(mathParser.FLOOR, 0); }
		public TerminalNode LOG() { return getToken(mathParser.LOG, 0); }
		public TerminalNode DELTA() { return getToken(mathParser.DELTA, 0); }
		public TerminalNode GESTEP() { return getToken(mathParser.GESTEP, 0); }
		public TerminalNode ROMAN() { return getToken(mathParser.ROMAN, 0); }
		public TerminalNode LEFT() { return getToken(mathParser.LEFT, 0); }
		public TerminalNode RIGHT() { return getToken(mathParser.RIGHT, 0); }
		public TerminalNode SEARCH() { return getToken(mathParser.SEARCH, 0); }
		public TerminalNode WEEKDAY() { return getToken(mathParser.WEEKDAY, 0); }
		public TerminalNode WEEKNUM() { return getToken(mathParser.WEEKNUM, 0); }
		public TerminalNode IRR() { return getToken(mathParser.IRR, 0); }
		public TerminalNode TRIMSTART() { return getToken(mathParser.TRIMSTART, 0); }
		public TerminalNode TRIMEND() { return getToken(mathParser.TRIMEND, 0); }
		public TerminalNode TIMESTAMP() { return getToken(mathParser.TIMESTAMP, 0); }
		public TerminalNode PARAM() { return getToken(mathParser.PARAM, 0); }
		public TerminalNode DATEVALUE() { return getToken(mathParser.DATEVALUE, 0); }
		public TerminalNode DAYS360() { return getToken(mathParser.DAYS360, 0); }
		public TerminalNode NETWORKDAYS() { return getToken(mathParser.NETWORKDAYS, 0); }
		public TerminalNode WORKDAY() { return getToken(mathParser.WORKDAY, 0); }
		public TerminalNode AVERAGEIF() { return getToken(mathParser.AVERAGEIF, 0); }
		public TerminalNode SUMIF() { return getToken(mathParser.SUMIF, 0); }
		public TerminalNode XIRR() { return getToken(mathParser.XIRR, 0); }
		public TerminalNode IF() { return getToken(mathParser.IF, 0); }
		public TerminalNode IFERROR() { return getToken(mathParser.IFERROR, 0); }
		public TerminalNode TIME() { return getToken(mathParser.TIME, 0); }
		public TerminalNode YEARFRAC() { return getToken(mathParser.YEARFRAC, 0); }
		public TerminalNode SUBSTRING() { return getToken(mathParser.SUBSTRING, 0); }
		public TerminalNode STARTSWITH() { return getToken(mathParser.STARTSWITH, 0); }
		public TerminalNode ENDSWITH() { return getToken(mathParser.ENDSWITH, 0); }
		public TerminalNode FIND() { return getToken(mathParser.FIND, 0); }
		public TerminalNode RANK() { return getToken(mathParser.RANK, 0); }
		public TerminalNode SUBSTITUTE() { return getToken(mathParser.SUBSTITUTE, 0); }
		public TerminalNode REPLACE() { return getToken(mathParser.REPLACE, 0); }
		public TerminalNode DB() { return getToken(mathParser.DB, 0); }
		public TerminalNode DDB() { return getToken(mathParser.DDB, 0); }
		public TerminalNode FIXED() { return getToken(mathParser.FIXED, 0); }
		public TerminalNode REMOVESTART() { return getToken(mathParser.REMOVESTART, 0); }
		public TerminalNode REMOVEEND() { return getToken(mathParser.REMOVEEND, 0); }
		public TerminalNode ARRAY() { return getToken(mathParser.ARRAY, 0); }
		public TerminalNode AND() { return getToken(mathParser.AND, 0); }
		public TerminalNode OR() { return getToken(mathParser.OR, 0); }
		public TerminalNode XOR() { return getToken(mathParser.XOR, 0); }
		public TerminalNode GCD() { return getToken(mathParser.GCD, 0); }
		public TerminalNode LCM() { return getToken(mathParser.LCM, 0); }
		public TerminalNode MULTINOMIAL() { return getToken(mathParser.MULTINOMIAL, 0); }
		public TerminalNode PRODUCT() { return getToken(mathParser.PRODUCT, 0); }
		public TerminalNode SUMSQ() { return getToken(mathParser.SUMSQ, 0); }
		public TerminalNode SUMPRODUCT() { return getToken(mathParser.SUMPRODUCT, 0); }
		public TerminalNode CONCATENATE() { return getToken(mathParser.CONCATENATE, 0); }
		public TerminalNode MAX() { return getToken(mathParser.MAX, 0); }
		public TerminalNode MEDIAN() { return getToken(mathParser.MEDIAN, 0); }
		public TerminalNode MIN() { return getToken(mathParser.MIN, 0); }
		public TerminalNode MODE() { return getToken(mathParser.MODE, 0); }
		public TerminalNode AVERAGE() { return getToken(mathParser.AVERAGE, 0); }
		public TerminalNode GEOMEAN() { return getToken(mathParser.GEOMEAN, 0); }
		public TerminalNode HARMEAN() { return getToken(mathParser.HARMEAN, 0); }
		public TerminalNode COUNT() { return getToken(mathParser.COUNT, 0); }
		public TerminalNode SUM() { return getToken(mathParser.SUM, 0); }
		public TerminalNode AVEDEV() { return getToken(mathParser.AVEDEV, 0); }
		public TerminalNode STDEV() { return getToken(mathParser.STDEV, 0); }
		public TerminalNode STDEVP() { return getToken(mathParser.STDEVP, 0); }
		public TerminalNode DEVSQ() { return getToken(mathParser.DEVSQ, 0); }
		public TerminalNode VAR() { return getToken(mathParser.VAR, 0); }
		public TerminalNode VARP() { return getToken(mathParser.VARP, 0); }
		public TerminalNode NPV() { return getToken(mathParser.NPV, 0); }
		public TerminalNode QUOTIENT() { return getToken(mathParser.QUOTIENT, 0); }
		public TerminalNode MOD() { return getToken(mathParser.MOD, 0); }
		public TerminalNode COMBIN() { return getToken(mathParser.COMBIN, 0); }
		public TerminalNode PERMUT() { return getToken(mathParser.PERMUT, 0); }
		public TerminalNode ATAN2() { return getToken(mathParser.ATAN2, 0); }
		public TerminalNode ROUNDDOWN() { return getToken(mathParser.ROUNDDOWN, 0); }
		public TerminalNode ROUNDUP() { return getToken(mathParser.ROUNDUP, 0); }
		public TerminalNode MROUND() { return getToken(mathParser.MROUND, 0); }
		public TerminalNode RANDBETWEEN() { return getToken(mathParser.RANDBETWEEN, 0); }
		public TerminalNode POWER() { return getToken(mathParser.POWER, 0); }
		public TerminalNode BESSELI() { return getToken(mathParser.BESSELI, 0); }
		public TerminalNode BESSELJ() { return getToken(mathParser.BESSELJ, 0); }
		public TerminalNode BESSELK() { return getToken(mathParser.BESSELK, 0); }
		public TerminalNode BESSELY() { return getToken(mathParser.BESSELY, 0); }
		public TerminalNode SUMX2MY2() { return getToken(mathParser.SUMX2MY2, 0); }
		public TerminalNode SUMX2PY2() { return getToken(mathParser.SUMX2PY2, 0); }
		public TerminalNode SUMXMY2() { return getToken(mathParser.SUMXMY2, 0); }
		public TerminalNode EXACT() { return getToken(mathParser.EXACT, 0); }
		public TerminalNode REPT() { return getToken(mathParser.REPT, 0); }
		public TerminalNode TEXT() { return getToken(mathParser.TEXT, 0); }
		public TerminalNode DAYS() { return getToken(mathParser.DAYS, 0); }
		public TerminalNode EDATE() { return getToken(mathParser.EDATE, 0); }
		public TerminalNode EOMONTH() { return getToken(mathParser.EOMONTH, 0); }
		public TerminalNode QUARTILE() { return getToken(mathParser.QUARTILE, 0); }
		public TerminalNode LARGE() { return getToken(mathParser.LARGE, 0); }
		public TerminalNode SMALL() { return getToken(mathParser.SMALL, 0); }
		public TerminalNode PERCENTILE() { return getToken(mathParser.PERCENTILE, 0); }
		public TerminalNode PERCENTRANK() { return getToken(mathParser.PERCENTRANK, 0); }
		public TerminalNode COVAR() { return getToken(mathParser.COVAR, 0); }
		public TerminalNode COVARIANCES() { return getToken(mathParser.COVARIANCES, 0); }
		public TerminalNode TINV() { return getToken(mathParser.TINV, 0); }
		public TerminalNode REGEX() { return getToken(mathParser.REGEX, 0); }
		public TerminalNode ISREGEX() { return getToken(mathParser.ISREGEX, 0); }
		public TerminalNode HMACMD5() { return getToken(mathParser.HMACMD5, 0); }
		public TerminalNode HMACSHA1() { return getToken(mathParser.HMACSHA1, 0); }
		public TerminalNode HMACSHA256() { return getToken(mathParser.HMACSHA256, 0); }
		public TerminalNode HMACSHA512() { return getToken(mathParser.HMACSHA512, 0); }
		public TerminalNode SPLIT() { return getToken(mathParser.SPLIT, 0); }
		public TerminalNode LOOKCEILING() { return getToken(mathParser.LOOKCEILING, 0); }
		public TerminalNode LOOKFLOOR() { return getToken(mathParser.LOOKFLOOR, 0); }
		public TerminalNode ADDYEARS() { return getToken(mathParser.ADDYEARS, 0); }
		public TerminalNode ADDMONTHS() { return getToken(mathParser.ADDMONTHS, 0); }
		public TerminalNode ADDDAYS() { return getToken(mathParser.ADDDAYS, 0); }
		public TerminalNode ADDHOURS() { return getToken(mathParser.ADDHOURS, 0); }
		public TerminalNode ADDMINUTES() { return getToken(mathParser.ADDMINUTES, 0); }
		public TerminalNode ADDSECONDS() { return getToken(mathParser.ADDSECONDS, 0); }
		public TerminalNode HAS() { return getToken(mathParser.HAS, 0); }
		public TerminalNode HASVALUE() { return getToken(mathParser.HASVALUE, 0); }
		public TerminalNode INTERCEPT() { return getToken(mathParser.INTERCEPT, 0); }
		public TerminalNode SLOPE() { return getToken(mathParser.SLOPE, 0); }
		public TerminalNode CORREL() { return getToken(mathParser.CORREL, 0); }
		public TerminalNode PEARSON() { return getToken(mathParser.PEARSON, 0); }
		public TerminalNode COUNTIF() { return getToken(mathParser.COUNTIF, 0); }
		public TerminalNode FORECAST() { return getToken(mathParser.FORECAST, 0); }
		public TerminalNode NORMINV() { return getToken(mathParser.NORMINV, 0); }
		public TerminalNode BETADIST() { return getToken(mathParser.BETADIST, 0); }
		public TerminalNode BETAINV() { return getToken(mathParser.BETAINV, 0); }
		public TerminalNode EXPONDIST() { return getToken(mathParser.EXPONDIST, 0); }
		public TerminalNode FDIST() { return getToken(mathParser.FDIST, 0); }
		public TerminalNode FINV() { return getToken(mathParser.FINV, 0); }
		public TerminalNode GAMMAINV() { return getToken(mathParser.GAMMAINV, 0); }
		public TerminalNode LOGINV() { return getToken(mathParser.LOGINV, 0); }
		public TerminalNode XNPV() { return getToken(mathParser.XNPV, 0); }
		public TerminalNode MIRR() { return getToken(mathParser.MIRR, 0); }
		public TerminalNode SLN() { return getToken(mathParser.SLN, 0); }
		public TerminalNode MID() { return getToken(mathParser.MID, 0); }
		public TerminalNode DATEDIF() { return getToken(mathParser.DATEDIF, 0); }
		public TerminalNode REGEXREPLACE() { return getToken(mathParser.REGEXREPLACE, 0); }
		public TerminalNode LOGNORMDIST() { return getToken(mathParser.LOGNORMDIST, 0); }
		public TerminalNode NEGBINOMDIST() { return getToken(mathParser.NEGBINOMDIST, 0); }
		public TerminalNode POISSON() { return getToken(mathParser.POISSON, 0); }
		public TerminalNode TDIST() { return getToken(mathParser.TDIST, 0); }
		public TerminalNode NORMDIST() { return getToken(mathParser.NORMDIST, 0); }
		public TerminalNode BINOMDIST() { return getToken(mathParser.BINOMDIST, 0); }
		public TerminalNode GAMMADIST() { return getToken(mathParser.GAMMADIST, 0); }
		public TerminalNode HYPGEOMDIST() { return getToken(mathParser.HYPGEOMDIST, 0); }
		public TerminalNode WEIBULL() { return getToken(mathParser.WEIBULL, 0); }
		public TerminalNode SYD() { return getToken(mathParser.SYD, 0); }
		public TerminalNode SERIESSUM() { return getToken(mathParser.SERIESSUM, 0); }
		public TerminalNode PMT() { return getToken(mathParser.PMT, 0); }
		public TerminalNode PV() { return getToken(mathParser.PV, 0); }
		public TerminalNode FV() { return getToken(mathParser.FV, 0); }
		public TerminalNode NPER() { return getToken(mathParser.NPER, 0); }
		public TerminalNode RATE() { return getToken(mathParser.RATE, 0); }
		public TerminalNode DATE() { return getToken(mathParser.DATE, 0); }
		public TerminalNode PPMT() { return getToken(mathParser.PPMT, 0); }
		public TerminalNode IPMT() { return getToken(mathParser.IPMT, 0); }
		public TerminalNode INDEXOF() { return getToken(mathParser.INDEXOF, 0); }
		public TerminalNode LASTINDEXOF() { return getToken(mathParser.LASTINDEXOF, 0); }
		public TerminalNode JOIN() { return getToken(mathParser.JOIN, 0); }
		public TerminalNode IFS() { return getToken(mathParser.IFS, 0); }
		public TerminalNode SWITCH() { return getToken(mathParser.SWITCH, 0); }
		public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
		public Function_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitFunction_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Or_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode OPOR() { return getToken(mathParser.OPOR, 0); }
		public Or_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitOr_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class IF_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public IF_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitIF_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Judge_funContext extends ExprContext {
		public Token op;
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode OPGT() { return getToken(mathParser.OPGT, 0); }
		public TerminalNode OPGE() { return getToken(mathParser.OPGE, 0); }
		public TerminalNode OPLT() { return getToken(mathParser.OPLT, 0); }
		public TerminalNode OPLE() { return getToken(mathParser.OPLE, 0); }
		public TerminalNode OPNE() { return getToken(mathParser.OPNE, 0); }
		public TerminalNode OPEQ() { return getToken(mathParser.OPEQ, 0); }
		public Judge_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitJudge_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Percentage_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public TerminalNode OPMOD() { return getToken(mathParser.OPMOD, 0); }
		public Percentage_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitPercentage_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class STRING_funContext extends ExprContext {
		public TerminalNode STRING() { return getToken(mathParser.STRING, 0); }
		public STRING_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitSTRING_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class AddSub_funContext extends ExprContext {
		public Token op;
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode OPADD() { return getToken(mathParser.OPADD, 0); }
		public TerminalNode OPSUB() { return getToken(mathParser.OPSUB, 0); }
		public TerminalNode OPCAT() { return getToken(mathParser.OPCAT, 0); }
		public AddSub_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitAddSub_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ArrayJson_funContext extends ExprContext {
		public List<ArrayJsonContext> arrayJson() {
			return getRuleContexts(ArrayJsonContext.class);
		}
		public ArrayJsonContext arrayJson(int i) {
			return getRuleContext(ArrayJsonContext.class,i);
		}
		public ArrayJson_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitArrayJson_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GetJsonValue_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public Parameter2Context parameter2() {
			return getRuleContext(Parameter2Context.class,0);
		}
		public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
		public GetJsonValue_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitGetJsonValue_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class And_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode OPAND() { return getToken(mathParser.OPAND, 0); }
		public And_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitAnd_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Array_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public Array_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitArray_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class MulDiv_funContext extends ExprContext {
		public Token op;
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public TerminalNode OPMUL() { return getToken(mathParser.OPMUL, 0); }
		public TerminalNode OPDIV() { return getToken(mathParser.OPDIV, 0); }
		public TerminalNode OPMOD() { return getToken(mathParser.OPMOD, 0); }
		public MulDiv_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitMulDiv_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NOT_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public NOT_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitNOT_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class CONST2_funContext extends ExprContext {
		public Token f;
		public TerminalNode TRUE() { return getToken(mathParser.TRUE, 0); }
		public TerminalNode FALSE() { return getToken(mathParser.FALSE, 0); }
		public TerminalNode ALGORITHMVERSION() { return getToken(mathParser.ALGORITHMVERSION, 0); }
		public TerminalNode NULL() { return getToken(mathParser.NULL, 0); }
		public CONST2_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitCONST2_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class Bracket_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public Bracket_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitBracket_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class CONST_funContext extends ExprContext {
		public Token f;
		public TerminalNode RAND() { return getToken(mathParser.RAND, 0); }
		public TerminalNode NOW() { return getToken(mathParser.NOW, 0); }
		public TerminalNode TODAY() { return getToken(mathParser.TODAY, 0); }
		public TerminalNode GUID() { return getToken(mathParser.GUID, 0); }
		public TerminalNode E() { return getToken(mathParser.E, 0); }
		public TerminalNode PI() { return getToken(mathParser.PI, 0); }
		public CONST_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitCONST_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NUM_funContext extends ExprContext {
		public TerminalNode NUM() { return getToken(mathParser.NUM, 0); }
		public TerminalNode OPSUB() { return getToken(mathParser.OPSUB, 0); }
		public NUM_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitNUM_fun(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class PARAMETER_funContext extends ExprContext {
		public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
		public PARAMETER_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitPARAMETER_fun(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ExprContext expr() throws RecognitionException {
		return expr(0);
	}

	private ExprContext expr(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExprContext _localctx = new ExprContext(_ctx, _parentState);
		ExprContext _prevctx = _localctx;
		int _startState = 2;
		enterRecursionRule(_localctx, 2, RULE_expr, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(72);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
			case 1:
				{
				_localctx = new Bracket_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(12);
				match(T__0);
				setState(13);
				expr(0);
				setState(14);
				match(T__1);
				}
				break;
			case 2:
				{
				_localctx = new NUM_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(17);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==OPSUB) {
					{
					setState(16);
					match(OPSUB);
					}
				}

				setState(19);
				match(NUM);
				}
				break;
			case 3:
				{
				_localctx = new STRING_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(20);
				match(STRING);
				}
				break;
			case 4:
				{
				_localctx = new PARAMETER_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(21);
				match(PARAMETER);
				}
				break;
			case 5:
				{
				_localctx = new NOT_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(22);
				match(T__6);
				setState(23);
				expr(14);
				}
				break;
			case 6:
				{
				_localctx = new CONST2_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(24);
				((CONST2_funContext)_localctx).f = _input.LT(1);
				_la = _input.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 4026531840L) != 0)) ) {
					((CONST2_funContext)_localctx).f = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(27);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
				case 1:
					{
					setState(25);
					match(T__0);
					setState(26);
					match(T__1);
					}
					break;
				}
				}
				break;
			case 7:
				{
				_localctx = new CONST_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(29);
				((CONST_funContext)_localctx).f = _input.LT(1);
				_la = _input.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 270582939648L) != 0)) ) {
					((CONST_funContext)_localctx).f = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(30);
				match(T__0);
				setState(31);
				match(T__1);
				}
				break;
			case 8:
				{
				_localctx = new Function_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(32);
				((Function_funContext)_localctx).f = _input.LT(1);
				_la = _input.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & -274877906944L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 35184372088831L) != 0)) ) {
					((Function_funContext)_localctx).f = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(33);
				match(T__0);
				setState(34);
				expr(0);
				setState(39);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__3) {
					{
					{
					setState(35);
					match(T__3);
					setState(36);
					expr(0);
					}
					}
					setState(41);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(42);
				match(T__1);
				}
				break;
			case 9:
				{
				_localctx = new ArrayJson_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(44);
				match(T__9);
				setState(45);
				arrayJson();
				setState(50);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(46);
						match(T__3);
						setState(47);
						arrayJson();
						}
						} 
					}
					setState(52);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
				}
				setState(54);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__3) {
					{
					setState(53);
					match(T__3);
					}
				}

				setState(56);
				match(T__10);
				}
				break;
			case 10:
				{
				_localctx = new Array_funContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(58);
				match(T__4);
				setState(59);
				expr(0);
				setState(64);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(60);
						match(T__3);
						setState(61);
						expr(0);
						}
						} 
					}
					setState(66);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
				}
				setState(68);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__3) {
					{
					setState(67);
					match(T__3);
					}
				}

				setState(70);
				match(T__5);
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(128);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,13,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(126);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
					case 1:
						{
						_localctx = new MulDiv_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(74);
						if (!(precpred(_ctx, 12))) throw new FailedPredicateException(this, "precpred(_ctx, 12)");
						setState(75);
						((MulDiv_funContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 458752L) != 0)) ) {
							((MulDiv_funContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(76);
						expr(13);
						}
						break;
					case 2:
						{
						_localctx = new AddSub_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(77);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(78);
						((AddSub_funContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 573440L) != 0)) ) {
							((AddSub_funContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(79);
						expr(12);
						}
						break;
					case 3:
						{
						_localctx = new Judge_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(80);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(81);
						((Judge_funContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 15728640L) != 0)) ) {
							((Judge_funContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(82);
						expr(11);
						}
						break;
					case 4:
						{
						_localctx = new Judge_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(83);
						if (!(precpred(_ctx, 9))) throw new FailedPredicateException(this, "precpred(_ctx, 9)");
						setState(84);
						((Judge_funContext)_localctx).op = _input.LT(1);
						_la = _input.LA(1);
						if ( !(_la==OPNE || _la==OPEQ) ) {
							((Judge_funContext)_localctx).op = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(85);
						expr(10);
						}
						break;
					case 5:
						{
						_localctx = new And_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(86);
						if (!(precpred(_ctx, 8))) throw new FailedPredicateException(this, "precpred(_ctx, 8)");
						setState(87);
						match(OPAND);
						setState(88);
						expr(9);
						}
						break;
					case 6:
						{
						_localctx = new Or_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(89);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(90);
						match(OPOR);
						setState(91);
						expr(8);
						}
						break;
					case 7:
						{
						_localctx = new IF_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(92);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(93);
						match(T__7);
						setState(94);
						expr(0);
						setState(95);
						match(T__8);
						setState(96);
						expr(7);
						}
						break;
					case 8:
						{
						_localctx = new Function_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(98);
						if (!(precpred(_ctx, 16))) throw new FailedPredicateException(this, "precpred(_ctx, 16)");
						setState(99);
						match(T__2);
						setState(100);
						((Function_funContext)_localctx).f = _input.LT(1);
						_la = _input.LA(1);
						if ( !(((((_la - 43)) & ~0x3f) == 0 && ((1L << (_la - 43)) & 4613374868287651903L) != 0) || ((((_la - 111)) & ~0x3f) == 0 && ((1L << (_la - 111)) & 3870831098982301697L) != 0) || ((((_la - 180)) & ~0x3f) == 0 && ((1L << (_la - 180)) & -3450918398844730283L) != 0) || ((((_la - 244)) & ~0x3f) == 0 && ((1L << (_la - 244)) & 90105231584543873L) != 0)) ) {
							((Function_funContext)_localctx).f = (Token)_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(101);
						match(T__0);
						setState(110);
						_errHandler.sync(this);
						_la = _input.LA(1);
						if ((((_la) & ~0x3f) == 0 && ((1L << _la) & -67074910L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 35184372088831L) != 0)) {
							{
							setState(102);
							expr(0);
							setState(107);
							_errHandler.sync(this);
							_la = _input.LA(1);
							while (_la==T__3) {
								{
								{
								setState(103);
								match(T__3);
								setState(104);
								expr(0);
								}
								}
								setState(109);
								_errHandler.sync(this);
								_la = _input.LA(1);
							}
							}
						}

						setState(112);
						match(T__1);
						}
						break;
					case 9:
						{
						_localctx = new GetJsonValue_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(113);
						if (!(precpred(_ctx, 15))) throw new FailedPredicateException(this, "precpred(_ctx, 15)");
						setState(122);
						_errHandler.sync(this);
						switch (_input.LA(1)) {
						case T__4:
							{
							setState(114);
							match(T__4);
							setState(117);
							_errHandler.sync(this);
							switch ( getInterpreter().adaptivePredict(_input,10,_ctx) ) {
							case 1:
								{
								setState(115);
								match(PARAMETER);
								}
								break;
							case 2:
								{
								setState(116);
								expr(0);
								}
								break;
							}
							setState(119);
							match(T__5);
							}
							break;
						case T__2:
							{
							setState(120);
							match(T__2);
							setState(121);
							parameter2();
							}
							break;
						default:
							throw new NoViableAltException(this);
						}
						}
						break;
					case 10:
						{
						_localctx = new Percentage_funContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(124);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(125);
						match(OPMOD);
						}
						break;
					}
					} 
				}
				setState(130);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,13,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ArrayJsonContext extends MyRuleContext {
		public Token key;
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public TerminalNode NUM() { return getToken(mathParser.NUM, 0); }
		public TerminalNode STRING() { return getToken(mathParser.STRING, 0); }
		public Parameter2Context parameter2() {
			return getRuleContext(Parameter2Context.class,0);
		}
		public ArrayJsonContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayJson; }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitArrayJson(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ArrayJsonContext arrayJson() throws RecognitionException {
		ArrayJsonContext _localctx = new ArrayJsonContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_arrayJson);
		int _la;
		try {
			setState(138);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NUM:
			case STRING:
				enterOuterAlt(_localctx, 1);
				{
				setState(131);
				((ArrayJsonContext)_localctx).key = _input.LT(1);
				_la = _input.LA(1);
				if ( !(_la==NUM || _la==STRING) ) {
					((ArrayJsonContext)_localctx).key = (Token)_errHandler.recoverInline(this);
				}
				else {
					if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
					_errHandler.reportMatch(this);
					consume();
				}
				setState(132);
				match(T__8);
				setState(133);
				expr(0);
				}
				break;
			case ALGORITHMVERSION:
			case FALSE:
			case NULL:
			case TRUE:
			case E:
			case GUID:
			case NOW:
			case PI:
			case RAND:
			case TODAY:
			case ABS:
			case ACOS:
			case ACOSH:
			case ACOT:
			case ACOTH:
			case ADDDAYS:
			case ADDHOURS:
			case ADDMINUTES:
			case ADDMONTHS:
			case ADDSECONDS:
			case ADDYEARS:
			case AND:
			case ARABIC:
			case ARRAY:
			case ASC:
			case ASIN:
			case ASINH:
			case ATAN:
			case ATAN2:
			case ATANH:
			case AVEDEV:
			case AVERAGE:
			case AVERAGEIF:
			case BASE64TOTEXT:
			case BASE64URLTOTEXT:
			case BESSELI:
			case BESSELJ:
			case BESSELK:
			case BESSELY:
			case BETADIST:
			case BETAINV:
			case BIN2DEC:
			case BIN2HEX:
			case BIN2OCT:
			case BINOMDIST:
			case CEILING:
			case CHAR:
			case CLEAN:
			case CODE:
			case COMBIN:
			case CONCATENATE:
			case CORREL:
			case COS:
			case COSH:
			case COT:
			case COTH:
			case COUNT:
			case COUNTIF:
			case COVAR:
			case COVARIANCES:
			case CSC:
			case CSCH:
			case DATE:
			case DATEDIF:
			case DATEVALUE:
			case DAY:
			case DAYS:
			case DAYS360:
			case DB:
			case DDB:
			case DEC2BIN:
			case DEC2HEX:
			case DEC2OCT:
			case DEGREES:
			case DELTA:
			case DEVSQ:
			case EDATE:
			case ENDSWITH:
			case EOMONTH:
			case ERF:
			case ERFC:
			case ERROR:
			case EVEN:
			case EXACT:
			case EXP:
			case EXPONDIST:
			case FACT:
			case FACTDOUBLE:
			case FDIST:
			case FIND:
			case FINV:
			case FISHER:
			case FISHERINV:
			case FIXED:
			case FLOOR:
			case FORECAST:
			case FV:
			case GAMMADIST:
			case GAMMAINV:
			case GAMMALN:
			case GCD:
			case GEOMEAN:
			case GESTEP:
			case HARMEAN:
			case HAS:
			case HASVALUE:
			case HEX2BIN:
			case HEX2DEC:
			case HEX2OCT:
			case HMACMD5:
			case HMACSHA1:
			case HMACSHA256:
			case HMACSHA512:
			case HOUR:
			case HTMLDECODE:
			case HTMLENCODE:
			case HYPGEOMDIST:
			case IF:
			case IFERROR:
			case IFS:
			case INDEXOF:
			case INT:
			case INTERCEPT:
			case IPMT:
			case IRR:
			case ISERROR:
			case ISEVEN:
			case ISLOGICAL:
			case ISNONTEXT:
			case ISNULL:
			case ISNULLOREMPTY:
			case ISNULLORERROR:
			case ISNULLORWHITESPACE:
			case ISNUMBER:
			case ISODD:
			case ISREGEX:
			case ISTEXT:
			case JIS:
			case JOIN:
			case JSON:
			case LARGE:
			case LASTINDEXOF:
			case LCM:
			case LEFT:
			case LEN:
			case LN:
			case LOG:
			case LOG10:
			case LOGINV:
			case LOGNORMDIST:
			case LOOKCEILING:
			case LOOKFLOOR:
			case LOWER:
			case MAX:
			case MD5:
			case MEDIAN:
			case MID:
			case MIN:
			case MINUTE:
			case MIRR:
			case MOD:
			case MODE:
			case MONTH:
			case MROUND:
			case MULTINOMIAL:
			case NEGBINOMDIST:
			case NETWORKDAYS:
			case NORMDIST:
			case NORMINV:
			case NORMSDIST:
			case NORMSINV:
			case NOT:
			case NPER:
			case NPV:
			case OCT2BIN:
			case OCT2DEC:
			case OCT2HEX:
			case ODD:
			case OR:
			case PARAM:
			case PEARSON:
			case PERCENTILE:
			case PERCENTRANK:
			case PERMUT:
			case PMT:
			case POISSON:
			case POWER:
			case PPMT:
			case PRODUCT:
			case PROPER:
			case PV:
			case QUARTILE:
			case QUOTIENT:
			case RADIANS:
			case RANDBETWEEN:
			case RANK:
			case RATE:
			case REGEX:
			case REGEXREPLACE:
			case REMOVEEND:
			case REMOVESTART:
			case REPLACE:
			case REPT:
			case RIGHT:
			case RMB:
			case ROMAN:
			case ROUND:
			case ROUNDDOWN:
			case ROUNDUP:
			case SEARCH:
			case SEC:
			case SECH:
			case SECOND:
			case SERIESSUM:
			case SHA1:
			case SHA256:
			case SHA512:
			case SIGN:
			case SIN:
			case SINH:
			case SLN:
			case SLOPE:
			case SMALL:
			case SPLIT:
			case SQRT:
			case SQRTPI:
			case STARTSWITH:
			case STDEV:
			case STDEVP:
			case SUBSTITUTE:
			case SUBSTRING:
			case SUM:
			case SUMIF:
			case SUMPRODUCT:
			case SUMSQ:
			case SUMX2MY2:
			case SUMX2PY2:
			case SUMXMY2:
			case SWITCH:
			case SYD:
			case T:
			case TAN:
			case TANH:
			case TDIST:
			case TEXT:
			case TEXTTOBASE64:
			case TEXTTOBASE64URL:
			case TIME:
			case TIMESTAMP:
			case TIMEVALUE:
			case TINV:
			case TRIM:
			case TRIMEND:
			case TRIMSTART:
			case TRUNC:
			case UNICHAR:
			case UNICODE:
			case UPPER:
			case URLDECODE:
			case URLENCODE:
			case VALUE:
			case VAR:
			case VARP:
			case WEEKDAY:
			case WEEKNUM:
			case WEIBULL:
			case WORKDAY:
			case XIRR:
			case XNPV:
			case XOR:
			case YEAR:
			case YEARFRAC:
			case PARAMETER:
				enterOuterAlt(_localctx, 2);
				{
				setState(134);
				parameter2();
				setState(135);
				match(T__8);
				setState(136);
				expr(0);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Parameter2Context extends MyRuleContext {
		public TerminalNode E() { return getToken(mathParser.E, 0); }
		public TerminalNode PI() { return getToken(mathParser.PI, 0); }
		public TerminalNode TRUE() { return getToken(mathParser.TRUE, 0); }
		public TerminalNode FALSE() { return getToken(mathParser.FALSE, 0); }
		public TerminalNode NULL() { return getToken(mathParser.NULL, 0); }
		public TerminalNode IF() { return getToken(mathParser.IF, 0); }
		public TerminalNode IFS() { return getToken(mathParser.IFS, 0); }
		public TerminalNode SWITCH() { return getToken(mathParser.SWITCH, 0); }
		public TerminalNode IFERROR() { return getToken(mathParser.IFERROR, 0); }
		public TerminalNode AND() { return getToken(mathParser.AND, 0); }
		public TerminalNode OR() { return getToken(mathParser.OR, 0); }
		public TerminalNode XOR() { return getToken(mathParser.XOR, 0); }
		public TerminalNode NOT() { return getToken(mathParser.NOT, 0); }
		public TerminalNode ISNUMBER() { return getToken(mathParser.ISNUMBER, 0); }
		public TerminalNode ISTEXT() { return getToken(mathParser.ISTEXT, 0); }
		public TerminalNode ISERROR() { return getToken(mathParser.ISERROR, 0); }
		public TerminalNode ISNONTEXT() { return getToken(mathParser.ISNONTEXT, 0); }
		public TerminalNode ISLOGICAL() { return getToken(mathParser.ISLOGICAL, 0); }
		public TerminalNode ISEVEN() { return getToken(mathParser.ISEVEN, 0); }
		public TerminalNode ISODD() { return getToken(mathParser.ISODD, 0); }
		public TerminalNode ISNULL() { return getToken(mathParser.ISNULL, 0); }
		public TerminalNode ISNULLORERROR() { return getToken(mathParser.ISNULLORERROR, 0); }
		public TerminalNode DEC2BIN() { return getToken(mathParser.DEC2BIN, 0); }
		public TerminalNode DEC2HEX() { return getToken(mathParser.DEC2HEX, 0); }
		public TerminalNode DEC2OCT() { return getToken(mathParser.DEC2OCT, 0); }
		public TerminalNode HEX2BIN() { return getToken(mathParser.HEX2BIN, 0); }
		public TerminalNode HEX2DEC() { return getToken(mathParser.HEX2DEC, 0); }
		public TerminalNode HEX2OCT() { return getToken(mathParser.HEX2OCT, 0); }
		public TerminalNode OCT2BIN() { return getToken(mathParser.OCT2BIN, 0); }
		public TerminalNode OCT2DEC() { return getToken(mathParser.OCT2DEC, 0); }
		public TerminalNode OCT2HEX() { return getToken(mathParser.OCT2HEX, 0); }
		public TerminalNode BIN2OCT() { return getToken(mathParser.BIN2OCT, 0); }
		public TerminalNode BIN2DEC() { return getToken(mathParser.BIN2DEC, 0); }
		public TerminalNode BIN2HEX() { return getToken(mathParser.BIN2HEX, 0); }
		public TerminalNode ABS() { return getToken(mathParser.ABS, 0); }
		public TerminalNode QUOTIENT() { return getToken(mathParser.QUOTIENT, 0); }
		public TerminalNode MOD() { return getToken(mathParser.MOD, 0); }
		public TerminalNode SIGN() { return getToken(mathParser.SIGN, 0); }
		public TerminalNode SQRT() { return getToken(mathParser.SQRT, 0); }
		public TerminalNode TRUNC() { return getToken(mathParser.TRUNC, 0); }
		public TerminalNode INT() { return getToken(mathParser.INT, 0); }
		public TerminalNode GCD() { return getToken(mathParser.GCD, 0); }
		public TerminalNode LCM() { return getToken(mathParser.LCM, 0); }
		public TerminalNode COMBIN() { return getToken(mathParser.COMBIN, 0); }
		public TerminalNode PERMUT() { return getToken(mathParser.PERMUT, 0); }
		public TerminalNode DEGREES() { return getToken(mathParser.DEGREES, 0); }
		public TerminalNode RADIANS() { return getToken(mathParser.RADIANS, 0); }
		public TerminalNode COS() { return getToken(mathParser.COS, 0); }
		public TerminalNode COSH() { return getToken(mathParser.COSH, 0); }
		public TerminalNode SIN() { return getToken(mathParser.SIN, 0); }
		public TerminalNode SINH() { return getToken(mathParser.SINH, 0); }
		public TerminalNode TAN() { return getToken(mathParser.TAN, 0); }
		public TerminalNode TANH() { return getToken(mathParser.TANH, 0); }
		public TerminalNode COT() { return getToken(mathParser.COT, 0); }
		public TerminalNode COTH() { return getToken(mathParser.COTH, 0); }
		public TerminalNode CSC() { return getToken(mathParser.CSC, 0); }
		public TerminalNode CSCH() { return getToken(mathParser.CSCH, 0); }
		public TerminalNode SEC() { return getToken(mathParser.SEC, 0); }
		public TerminalNode SECH() { return getToken(mathParser.SECH, 0); }
		public TerminalNode ACOS() { return getToken(mathParser.ACOS, 0); }
		public TerminalNode ACOSH() { return getToken(mathParser.ACOSH, 0); }
		public TerminalNode ASIN() { return getToken(mathParser.ASIN, 0); }
		public TerminalNode ASINH() { return getToken(mathParser.ASINH, 0); }
		public TerminalNode ATAN() { return getToken(mathParser.ATAN, 0); }
		public TerminalNode ATANH() { return getToken(mathParser.ATANH, 0); }
		public TerminalNode ACOT() { return getToken(mathParser.ACOT, 0); }
		public TerminalNode ACOTH() { return getToken(mathParser.ACOTH, 0); }
		public TerminalNode ATAN2() { return getToken(mathParser.ATAN2, 0); }
		public TerminalNode ROUND() { return getToken(mathParser.ROUND, 0); }
		public TerminalNode ROUNDDOWN() { return getToken(mathParser.ROUNDDOWN, 0); }
		public TerminalNode ROUNDUP() { return getToken(mathParser.ROUNDUP, 0); }
		public TerminalNode CEILING() { return getToken(mathParser.CEILING, 0); }
		public TerminalNode FLOOR() { return getToken(mathParser.FLOOR, 0); }
		public TerminalNode EVEN() { return getToken(mathParser.EVEN, 0); }
		public TerminalNode ODD() { return getToken(mathParser.ODD, 0); }
		public TerminalNode MROUND() { return getToken(mathParser.MROUND, 0); }
		public TerminalNode RAND() { return getToken(mathParser.RAND, 0); }
		public TerminalNode RANDBETWEEN() { return getToken(mathParser.RANDBETWEEN, 0); }
		public TerminalNode FACT() { return getToken(mathParser.FACT, 0); }
		public TerminalNode FACTDOUBLE() { return getToken(mathParser.FACTDOUBLE, 0); }
		public TerminalNode POWER() { return getToken(mathParser.POWER, 0); }
		public TerminalNode EXP() { return getToken(mathParser.EXP, 0); }
		public TerminalNode LN() { return getToken(mathParser.LN, 0); }
		public TerminalNode LOG() { return getToken(mathParser.LOG, 0); }
		public TerminalNode LOG10() { return getToken(mathParser.LOG10, 0); }
		public TerminalNode MULTINOMIAL() { return getToken(mathParser.MULTINOMIAL, 0); }
		public TerminalNode PRODUCT() { return getToken(mathParser.PRODUCT, 0); }
		public TerminalNode SQRTPI() { return getToken(mathParser.SQRTPI, 0); }
		public TerminalNode ERF() { return getToken(mathParser.ERF, 0); }
		public TerminalNode ERFC() { return getToken(mathParser.ERFC, 0); }
		public TerminalNode BESSELI() { return getToken(mathParser.BESSELI, 0); }
		public TerminalNode BESSELJ() { return getToken(mathParser.BESSELJ, 0); }
		public TerminalNode BESSELK() { return getToken(mathParser.BESSELK, 0); }
		public TerminalNode BESSELY() { return getToken(mathParser.BESSELY, 0); }
		public TerminalNode DELTA() { return getToken(mathParser.DELTA, 0); }
		public TerminalNode GESTEP() { return getToken(mathParser.GESTEP, 0); }
		public TerminalNode SUMSQ() { return getToken(mathParser.SUMSQ, 0); }
		public TerminalNode SUMPRODUCT() { return getToken(mathParser.SUMPRODUCT, 0); }
		public TerminalNode SUMX2MY2() { return getToken(mathParser.SUMX2MY2, 0); }
		public TerminalNode SUMX2PY2() { return getToken(mathParser.SUMX2PY2, 0); }
		public TerminalNode SUMXMY2() { return getToken(mathParser.SUMXMY2, 0); }
		public TerminalNode ARABIC() { return getToken(mathParser.ARABIC, 0); }
		public TerminalNode ROMAN() { return getToken(mathParser.ROMAN, 0); }
		public TerminalNode SERIESSUM() { return getToken(mathParser.SERIESSUM, 0); }
		public TerminalNode RANK() { return getToken(mathParser.RANK, 0); }
		public TerminalNode FORECAST() { return getToken(mathParser.FORECAST, 0); }
		public TerminalNode INTERCEPT() { return getToken(mathParser.INTERCEPT, 0); }
		public TerminalNode SLOPE() { return getToken(mathParser.SLOPE, 0); }
		public TerminalNode CORREL() { return getToken(mathParser.CORREL, 0); }
		public TerminalNode PEARSON() { return getToken(mathParser.PEARSON, 0); }
		public TerminalNode ASC() { return getToken(mathParser.ASC, 0); }
		public TerminalNode JIS() { return getToken(mathParser.JIS, 0); }
		public TerminalNode CHAR() { return getToken(mathParser.CHAR, 0); }
		public TerminalNode CLEAN() { return getToken(mathParser.CLEAN, 0); }
		public TerminalNode CODE() { return getToken(mathParser.CODE, 0); }
		public TerminalNode UNICHAR() { return getToken(mathParser.UNICHAR, 0); }
		public TerminalNode UNICODE() { return getToken(mathParser.UNICODE, 0); }
		public TerminalNode CONCATENATE() { return getToken(mathParser.CONCATENATE, 0); }
		public TerminalNode EXACT() { return getToken(mathParser.EXACT, 0); }
		public TerminalNode FIND() { return getToken(mathParser.FIND, 0); }
		public TerminalNode FIXED() { return getToken(mathParser.FIXED, 0); }
		public TerminalNode LEFT() { return getToken(mathParser.LEFT, 0); }
		public TerminalNode LEN() { return getToken(mathParser.LEN, 0); }
		public TerminalNode LOWER() { return getToken(mathParser.LOWER, 0); }
		public TerminalNode MID() { return getToken(mathParser.MID, 0); }
		public TerminalNode PROPER() { return getToken(mathParser.PROPER, 0); }
		public TerminalNode REPLACE() { return getToken(mathParser.REPLACE, 0); }
		public TerminalNode REPT() { return getToken(mathParser.REPT, 0); }
		public TerminalNode RIGHT() { return getToken(mathParser.RIGHT, 0); }
		public TerminalNode RMB() { return getToken(mathParser.RMB, 0); }
		public TerminalNode SEARCH() { return getToken(mathParser.SEARCH, 0); }
		public TerminalNode SUBSTITUTE() { return getToken(mathParser.SUBSTITUTE, 0); }
		public TerminalNode T() { return getToken(mathParser.T, 0); }
		public TerminalNode TEXT() { return getToken(mathParser.TEXT, 0); }
		public TerminalNode TRIM() { return getToken(mathParser.TRIM, 0); }
		public TerminalNode UPPER() { return getToken(mathParser.UPPER, 0); }
		public TerminalNode VALUE() { return getToken(mathParser.VALUE, 0); }
		public TerminalNode URLENCODE() { return getToken(mathParser.URLENCODE, 0); }
		public TerminalNode URLDECODE() { return getToken(mathParser.URLDECODE, 0); }
		public TerminalNode HTMLENCODE() { return getToken(mathParser.HTMLENCODE, 0); }
		public TerminalNode HTMLDECODE() { return getToken(mathParser.HTMLDECODE, 0); }
		public TerminalNode BASE64TOTEXT() { return getToken(mathParser.BASE64TOTEXT, 0); }
		public TerminalNode BASE64URLTOTEXT() { return getToken(mathParser.BASE64URLTOTEXT, 0); }
		public TerminalNode TEXTTOBASE64() { return getToken(mathParser.TEXTTOBASE64, 0); }
		public TerminalNode TEXTTOBASE64URL() { return getToken(mathParser.TEXTTOBASE64URL, 0); }
		public TerminalNode REGEX() { return getToken(mathParser.REGEX, 0); }
		public TerminalNode REGEXREPLACE() { return getToken(mathParser.REGEXREPLACE, 0); }
		public TerminalNode ISREGEX() { return getToken(mathParser.ISREGEX, 0); }
		public TerminalNode MD5() { return getToken(mathParser.MD5, 0); }
		public TerminalNode SHA1() { return getToken(mathParser.SHA1, 0); }
		public TerminalNode SHA256() { return getToken(mathParser.SHA256, 0); }
		public TerminalNode SHA512() { return getToken(mathParser.SHA512, 0); }
		public TerminalNode HMACMD5() { return getToken(mathParser.HMACMD5, 0); }
		public TerminalNode HMACSHA1() { return getToken(mathParser.HMACSHA1, 0); }
		public TerminalNode HMACSHA256() { return getToken(mathParser.HMACSHA256, 0); }
		public TerminalNode HMACSHA512() { return getToken(mathParser.HMACSHA512, 0); }
		public TerminalNode DATEVALUE() { return getToken(mathParser.DATEVALUE, 0); }
		public TerminalNode TIMEVALUE() { return getToken(mathParser.TIMEVALUE, 0); }
		public TerminalNode DATE() { return getToken(mathParser.DATE, 0); }
		public TerminalNode TIME() { return getToken(mathParser.TIME, 0); }
		public TerminalNode NOW() { return getToken(mathParser.NOW, 0); }
		public TerminalNode TODAY() { return getToken(mathParser.TODAY, 0); }
		public TerminalNode YEAR() { return getToken(mathParser.YEAR, 0); }
		public TerminalNode MONTH() { return getToken(mathParser.MONTH, 0); }
		public TerminalNode DAY() { return getToken(mathParser.DAY, 0); }
		public TerminalNode HOUR() { return getToken(mathParser.HOUR, 0); }
		public TerminalNode MINUTE() { return getToken(mathParser.MINUTE, 0); }
		public TerminalNode SECOND() { return getToken(mathParser.SECOND, 0); }
		public TerminalNode WEEKDAY() { return getToken(mathParser.WEEKDAY, 0); }
		public TerminalNode WEEKNUM() { return getToken(mathParser.WEEKNUM, 0); }
		public TerminalNode DATEDIF() { return getToken(mathParser.DATEDIF, 0); }
		public TerminalNode DAYS() { return getToken(mathParser.DAYS, 0); }
		public TerminalNode DAYS360() { return getToken(mathParser.DAYS360, 0); }
		public TerminalNode EDATE() { return getToken(mathParser.EDATE, 0); }
		public TerminalNode EOMONTH() { return getToken(mathParser.EOMONTH, 0); }
		public TerminalNode NETWORKDAYS() { return getToken(mathParser.NETWORKDAYS, 0); }
		public TerminalNode WORKDAY() { return getToken(mathParser.WORKDAY, 0); }
		public TerminalNode YEARFRAC() { return getToken(mathParser.YEARFRAC, 0); }
		public TerminalNode MAX() { return getToken(mathParser.MAX, 0); }
		public TerminalNode MEDIAN() { return getToken(mathParser.MEDIAN, 0); }
		public TerminalNode MIN() { return getToken(mathParser.MIN, 0); }
		public TerminalNode QUARTILE() { return getToken(mathParser.QUARTILE, 0); }
		public TerminalNode MODE() { return getToken(mathParser.MODE, 0); }
		public TerminalNode LARGE() { return getToken(mathParser.LARGE, 0); }
		public TerminalNode SMALL() { return getToken(mathParser.SMALL, 0); }
		public TerminalNode PERCENTILE() { return getToken(mathParser.PERCENTILE, 0); }
		public TerminalNode PERCENTRANK() { return getToken(mathParser.PERCENTRANK, 0); }
		public TerminalNode AVERAGE() { return getToken(mathParser.AVERAGE, 0); }
		public TerminalNode AVERAGEIF() { return getToken(mathParser.AVERAGEIF, 0); }
		public TerminalNode GEOMEAN() { return getToken(mathParser.GEOMEAN, 0); }
		public TerminalNode HARMEAN() { return getToken(mathParser.HARMEAN, 0); }
		public TerminalNode COUNT() { return getToken(mathParser.COUNT, 0); }
		public TerminalNode COUNTIF() { return getToken(mathParser.COUNTIF, 0); }
		public TerminalNode SUM() { return getToken(mathParser.SUM, 0); }
		public TerminalNode SUMIF() { return getToken(mathParser.SUMIF, 0); }
		public TerminalNode AVEDEV() { return getToken(mathParser.AVEDEV, 0); }
		public TerminalNode STDEV() { return getToken(mathParser.STDEV, 0); }
		public TerminalNode STDEVP() { return getToken(mathParser.STDEVP, 0); }
		public TerminalNode COVAR() { return getToken(mathParser.COVAR, 0); }
		public TerminalNode COVARIANCES() { return getToken(mathParser.COVARIANCES, 0); }
		public TerminalNode DEVSQ() { return getToken(mathParser.DEVSQ, 0); }
		public TerminalNode VAR() { return getToken(mathParser.VAR, 0); }
		public TerminalNode VARP() { return getToken(mathParser.VARP, 0); }
		public TerminalNode NORMDIST() { return getToken(mathParser.NORMDIST, 0); }
		public TerminalNode NORMINV() { return getToken(mathParser.NORMINV, 0); }
		public TerminalNode NORMSDIST() { return getToken(mathParser.NORMSDIST, 0); }
		public TerminalNode NORMSINV() { return getToken(mathParser.NORMSINV, 0); }
		public TerminalNode BETADIST() { return getToken(mathParser.BETADIST, 0); }
		public TerminalNode BETAINV() { return getToken(mathParser.BETAINV, 0); }
		public TerminalNode BINOMDIST() { return getToken(mathParser.BINOMDIST, 0); }
		public TerminalNode EXPONDIST() { return getToken(mathParser.EXPONDIST, 0); }
		public TerminalNode FDIST() { return getToken(mathParser.FDIST, 0); }
		public TerminalNode FINV() { return getToken(mathParser.FINV, 0); }
		public TerminalNode GAMMADIST() { return getToken(mathParser.GAMMADIST, 0); }
		public TerminalNode GAMMAINV() { return getToken(mathParser.GAMMAINV, 0); }
		public TerminalNode GAMMALN() { return getToken(mathParser.GAMMALN, 0); }
		public TerminalNode HYPGEOMDIST() { return getToken(mathParser.HYPGEOMDIST, 0); }
		public TerminalNode LOGINV() { return getToken(mathParser.LOGINV, 0); }
		public TerminalNode LOGNORMDIST() { return getToken(mathParser.LOGNORMDIST, 0); }
		public TerminalNode NEGBINOMDIST() { return getToken(mathParser.NEGBINOMDIST, 0); }
		public TerminalNode POISSON() { return getToken(mathParser.POISSON, 0); }
		public TerminalNode TDIST() { return getToken(mathParser.TDIST, 0); }
		public TerminalNode TINV() { return getToken(mathParser.TINV, 0); }
		public TerminalNode WEIBULL() { return getToken(mathParser.WEIBULL, 0); }
		public TerminalNode FISHER() { return getToken(mathParser.FISHER, 0); }
		public TerminalNode FISHERINV() { return getToken(mathParser.FISHERINV, 0); }
		public TerminalNode PMT() { return getToken(mathParser.PMT, 0); }
		public TerminalNode PPMT() { return getToken(mathParser.PPMT, 0); }
		public TerminalNode IPMT() { return getToken(mathParser.IPMT, 0); }
		public TerminalNode PV() { return getToken(mathParser.PV, 0); }
		public TerminalNode FV() { return getToken(mathParser.FV, 0); }
		public TerminalNode NPER() { return getToken(mathParser.NPER, 0); }
		public TerminalNode RATE() { return getToken(mathParser.RATE, 0); }
		public TerminalNode NPV() { return getToken(mathParser.NPV, 0); }
		public TerminalNode XNPV() { return getToken(mathParser.XNPV, 0); }
		public TerminalNode IRR() { return getToken(mathParser.IRR, 0); }
		public TerminalNode MIRR() { return getToken(mathParser.MIRR, 0); }
		public TerminalNode XIRR() { return getToken(mathParser.XIRR, 0); }
		public TerminalNode SLN() { return getToken(mathParser.SLN, 0); }
		public TerminalNode DB() { return getToken(mathParser.DB, 0); }
		public TerminalNode DDB() { return getToken(mathParser.DDB, 0); }
		public TerminalNode SYD() { return getToken(mathParser.SYD, 0); }
		public TerminalNode ARRAY() { return getToken(mathParser.ARRAY, 0); }
		public TerminalNode INDEXOF() { return getToken(mathParser.INDEXOF, 0); }
		public TerminalNode LASTINDEXOF() { return getToken(mathParser.LASTINDEXOF, 0); }
		public TerminalNode SPLIT() { return getToken(mathParser.SPLIT, 0); }
		public TerminalNode JOIN() { return getToken(mathParser.JOIN, 0); }
		public TerminalNode SUBSTRING() { return getToken(mathParser.SUBSTRING, 0); }
		public TerminalNode STARTSWITH() { return getToken(mathParser.STARTSWITH, 0); }
		public TerminalNode ENDSWITH() { return getToken(mathParser.ENDSWITH, 0); }
		public TerminalNode HAS() { return getToken(mathParser.HAS, 0); }
		public TerminalNode HASVALUE() { return getToken(mathParser.HASVALUE, 0); }
		public TerminalNode GUID() { return getToken(mathParser.GUID, 0); }
		public TerminalNode ERROR() { return getToken(mathParser.ERROR, 0); }
		public TerminalNode TRIMSTART() { return getToken(mathParser.TRIMSTART, 0); }
		public TerminalNode TRIMEND() { return getToken(mathParser.TRIMEND, 0); }
		public TerminalNode ISNULLOREMPTY() { return getToken(mathParser.ISNULLOREMPTY, 0); }
		public TerminalNode ISNULLORWHITESPACE() { return getToken(mathParser.ISNULLORWHITESPACE, 0); }
		public TerminalNode REMOVESTART() { return getToken(mathParser.REMOVESTART, 0); }
		public TerminalNode REMOVEEND() { return getToken(mathParser.REMOVEEND, 0); }
		public TerminalNode JSON() { return getToken(mathParser.JSON, 0); }
		public TerminalNode LOOKCEILING() { return getToken(mathParser.LOOKCEILING, 0); }
		public TerminalNode LOOKFLOOR() { return getToken(mathParser.LOOKFLOOR, 0); }
		public TerminalNode ADDYEARS() { return getToken(mathParser.ADDYEARS, 0); }
		public TerminalNode ADDMONTHS() { return getToken(mathParser.ADDMONTHS, 0); }
		public TerminalNode ADDDAYS() { return getToken(mathParser.ADDDAYS, 0); }
		public TerminalNode ADDHOURS() { return getToken(mathParser.ADDHOURS, 0); }
		public TerminalNode ADDMINUTES() { return getToken(mathParser.ADDMINUTES, 0); }
		public TerminalNode ADDSECONDS() { return getToken(mathParser.ADDSECONDS, 0); }
		public TerminalNode TIMESTAMP() { return getToken(mathParser.TIMESTAMP, 0); }
		public TerminalNode ALGORITHMVERSION() { return getToken(mathParser.ALGORITHMVERSION, 0); }
		public TerminalNode PARAM() { return getToken(mathParser.PARAM, 0); }
		public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
		public Parameter2Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameter2; }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof mathVisitor ) return ((mathVisitor<? extends T>)visitor).visitParameter2(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Parameter2Context parameter2() throws RecognitionException {
		Parameter2Context _localctx = new Parameter2Context(_ctx, getState());
		enterRule(_localctx, 6, RULE_parameter2);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(140);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & -268435456L) != 0) || ((((_la - 64)) & ~0x3f) == 0 && ((1L << (_la - 64)) & -1L) != 0) || ((((_la - 128)) & ~0x3f) == 0 && ((1L << (_la - 128)) & -1L) != 0) || ((((_la - 192)) & ~0x3f) == 0 && ((1L << (_la - 192)) & -1L) != 0) || ((((_la - 256)) & ~0x3f) == 0 && ((1L << (_la - 256)) & 35184372088831L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1:
			return expr_sempred((ExprContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expr_sempred(ExprContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 12);
		case 1:
			return precpred(_ctx, 11);
		case 2:
			return precpred(_ctx, 10);
		case 3:
			return precpred(_ctx, 9);
		case 4:
			return precpred(_ctx, 8);
		case 5:
			return precpred(_ctx, 7);
		case 6:
			return precpred(_ctx, 6);
		case 7:
			return precpred(_ctx, 16);
		case 8:
			return precpred(_ctx, 15);
		case 9:
			return precpred(_ctx, 13);
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u0001\u012f\u008f\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001"+
		"\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0001\u0000\u0001\u0000"+
		"\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0003\u0001\u0012\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0003\u0001"+
		"\u001c\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0005\u0001&\b\u0001\n\u0001\f\u0001"+
		")\t\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0005\u00011\b\u0001\n\u0001\f\u00014\t\u0001\u0001\u0001"+
		"\u0003\u00017\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0005\u0001?\b\u0001\n\u0001\f\u0001B\t\u0001"+
		"\u0001\u0001\u0003\u0001E\b\u0001\u0001\u0001\u0001\u0001\u0003\u0001"+
		"I\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0005\u0001j\b\u0001\n\u0001\f\u0001m\t\u0001"+
		"\u0003\u0001o\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0003\u0001v\b\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0003\u0001{\b\u0001\u0001\u0001\u0001\u0001\u0005\u0001\u007f\b\u0001"+
		"\n\u0001\f\u0001\u0082\t\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001"+
		"\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0003\u0002\u008b\b\u0002\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0000\u0001\u0002\u0004\u0000\u0002\u0004"+
		"\u0006\u0000\n\u0001\u0000\u001c\u001f\u0001\u0000 %\u0001\u0000&\u012c"+
		"\u0001\u0000\u0010\u0012\u0002\u0000\u000e\u000f\u0013\u0013\u0001\u0000"+
		"\u0014\u0017\u0001\u0000\u0018\u0019 \u0000+0\\]iioo\u0084\u0085\u008d"+
		"\u008d\u0094\u0095\u0099\u0099\u009b\u00a1\u00a3\u00a4\u00a6\u00a7\u00a9"+
		"\u00a9\u00ab\u00ac\u00b4\u00b4\u00b6\u00b6\u00b8\u00b8\u00ba\u00ba\u00be"+
		"\u00be\u00e1\u00e5\u00e7\u00e8\u00f0\u00f0\u00f2\u00f4\u00fb\u00fb\u00fe"+
		"\u00fe\u0102\u0102\u010c\u010c\u0110\u0110\u0114\u0115\u0117\u0119\u011d"+
		"\u0120\u012a\u012a\u012c\u012c\u0001\u0000\u001a\u001b\u0001\u0000\u001c"+
		"\u012c\u00a9\u0000\b\u0001\u0000\u0000\u0000\u0002H\u0001\u0000\u0000"+
		"\u0000\u0004\u008a\u0001\u0000\u0000\u0000\u0006\u008c\u0001\u0000\u0000"+
		"\u0000\b\t\u0003\u0002\u0001\u0000\t\n\u0005\u0000\u0000\u0001\n\u0001"+
		"\u0001\u0000\u0000\u0000\u000b\f\u0006\u0001\uffff\uffff\u0000\f\r\u0005"+
		"\u0001\u0000\u0000\r\u000e\u0003\u0002\u0001\u0000\u000e\u000f\u0005\u0002"+
		"\u0000\u0000\u000fI\u0001\u0000\u0000\u0000\u0010\u0012\u0005\u000f\u0000"+
		"\u0000\u0011\u0010\u0001\u0000\u0000\u0000\u0011\u0012\u0001\u0000\u0000"+
		"\u0000\u0012\u0013\u0001\u0000\u0000\u0000\u0013I\u0005\u001a\u0000\u0000"+
		"\u0014I\u0005\u001b\u0000\u0000\u0015I\u0005\u012c\u0000\u0000\u0016\u0017"+
		"\u0005\u0007\u0000\u0000\u0017I\u0003\u0002\u0001\u000e\u0018\u001b\u0007"+
		"\u0000\u0000\u0000\u0019\u001a\u0005\u0001\u0000\u0000\u001a\u001c\u0005"+
		"\u0002\u0000\u0000\u001b\u0019\u0001\u0000\u0000\u0000\u001b\u001c\u0001"+
		"\u0000\u0000\u0000\u001cI\u0001\u0000\u0000\u0000\u001d\u001e\u0007\u0001"+
		"\u0000\u0000\u001e\u001f\u0005\u0001\u0000\u0000\u001fI\u0005\u0002\u0000"+
		"\u0000 !\u0007\u0002\u0000\u0000!\"\u0005\u0001\u0000\u0000\"\'\u0003"+
		"\u0002\u0001\u0000#$\u0005\u0004\u0000\u0000$&\u0003\u0002\u0001\u0000"+
		"%#\u0001\u0000\u0000\u0000&)\u0001\u0000\u0000\u0000\'%\u0001\u0000\u0000"+
		"\u0000\'(\u0001\u0000\u0000\u0000(*\u0001\u0000\u0000\u0000)\'\u0001\u0000"+
		"\u0000\u0000*+\u0005\u0002\u0000\u0000+I\u0001\u0000\u0000\u0000,-\u0005"+
		"\n\u0000\u0000-2\u0003\u0004\u0002\u0000./\u0005\u0004\u0000\u0000/1\u0003"+
		"\u0004\u0002\u00000.\u0001\u0000\u0000\u000014\u0001\u0000\u0000\u0000"+
		"20\u0001\u0000\u0000\u000023\u0001\u0000\u0000\u000036\u0001\u0000\u0000"+
		"\u000042\u0001\u0000\u0000\u000057\u0005\u0004\u0000\u000065\u0001\u0000"+
		"\u0000\u000067\u0001\u0000\u0000\u000078\u0001\u0000\u0000\u000089\u0005"+
		"\u000b\u0000\u00009I\u0001\u0000\u0000\u0000:;\u0005\u0005\u0000\u0000"+
		";@\u0003\u0002\u0001\u0000<=\u0005\u0004\u0000\u0000=?\u0003\u0002\u0001"+
		"\u0000><\u0001\u0000\u0000\u0000?B\u0001\u0000\u0000\u0000@>\u0001\u0000"+
		"\u0000\u0000@A\u0001\u0000\u0000\u0000AD\u0001\u0000\u0000\u0000B@\u0001"+
		"\u0000\u0000\u0000CE\u0005\u0004\u0000\u0000DC\u0001\u0000\u0000\u0000"+
		"DE\u0001\u0000\u0000\u0000EF\u0001\u0000\u0000\u0000FG\u0005\u0006\u0000"+
		"\u0000GI\u0001\u0000\u0000\u0000H\u000b\u0001\u0000\u0000\u0000H\u0011"+
		"\u0001\u0000\u0000\u0000H\u0014\u0001\u0000\u0000\u0000H\u0015\u0001\u0000"+
		"\u0000\u0000H\u0016\u0001\u0000\u0000\u0000H\u0018\u0001\u0000\u0000\u0000"+
		"H\u001d\u0001\u0000\u0000\u0000H \u0001\u0000\u0000\u0000H,\u0001\u0000"+
		"\u0000\u0000H:\u0001\u0000\u0000\u0000I\u0080\u0001\u0000\u0000\u0000"+
		"JK\n\f\u0000\u0000KL\u0007\u0003\u0000\u0000L\u007f\u0003\u0002\u0001"+
		"\rMN\n\u000b\u0000\u0000NO\u0007\u0004\u0000\u0000O\u007f\u0003\u0002"+
		"\u0001\fPQ\n\n\u0000\u0000QR\u0007\u0005\u0000\u0000R\u007f\u0003\u0002"+
		"\u0001\u000bST\n\t\u0000\u0000TU\u0007\u0006\u0000\u0000U\u007f\u0003"+
		"\u0002\u0001\nVW\n\b\u0000\u0000WX\u0005\f\u0000\u0000X\u007f\u0003\u0002"+
		"\u0001\tYZ\n\u0007\u0000\u0000Z[\u0005\r\u0000\u0000[\u007f\u0003\u0002"+
		"\u0001\b\\]\n\u0006\u0000\u0000]^\u0005\b\u0000\u0000^_\u0003\u0002\u0001"+
		"\u0000_`\u0005\t\u0000\u0000`a\u0003\u0002\u0001\u0007a\u007f\u0001\u0000"+
		"\u0000\u0000bc\n\u0010\u0000\u0000cd\u0005\u0003\u0000\u0000de\u0007\u0007"+
		"\u0000\u0000en\u0005\u0001\u0000\u0000fk\u0003\u0002\u0001\u0000gh\u0005"+
		"\u0004\u0000\u0000hj\u0003\u0002\u0001\u0000ig\u0001\u0000\u0000\u0000"+
		"jm\u0001\u0000\u0000\u0000ki\u0001\u0000\u0000\u0000kl\u0001\u0000\u0000"+
		"\u0000lo\u0001\u0000\u0000\u0000mk\u0001\u0000\u0000\u0000nf\u0001\u0000"+
		"\u0000\u0000no\u0001\u0000\u0000\u0000op\u0001\u0000\u0000\u0000p\u007f"+
		"\u0005\u0002\u0000\u0000qz\n\u000f\u0000\u0000ru\u0005\u0005\u0000\u0000"+
		"sv\u0005\u012c\u0000\u0000tv\u0003\u0002\u0001\u0000us\u0001\u0000\u0000"+
		"\u0000ut\u0001\u0000\u0000\u0000vw\u0001\u0000\u0000\u0000w{\u0005\u0006"+
		"\u0000\u0000xy\u0005\u0003\u0000\u0000y{\u0003\u0006\u0003\u0000zr\u0001"+
		"\u0000\u0000\u0000zx\u0001\u0000\u0000\u0000{\u007f\u0001\u0000\u0000"+
		"\u0000|}\n\r\u0000\u0000}\u007f\u0005\u0012\u0000\u0000~J\u0001\u0000"+
		"\u0000\u0000~M\u0001\u0000\u0000\u0000~P\u0001\u0000\u0000\u0000~S\u0001"+
		"\u0000\u0000\u0000~V\u0001\u0000\u0000\u0000~Y\u0001\u0000\u0000\u0000"+
		"~\\\u0001\u0000\u0000\u0000~b\u0001\u0000\u0000\u0000~q\u0001\u0000\u0000"+
		"\u0000~|\u0001\u0000\u0000\u0000\u007f\u0082\u0001\u0000\u0000\u0000\u0080"+
		"~\u0001\u0000\u0000\u0000\u0080\u0081\u0001\u0000\u0000\u0000\u0081\u0003"+
		"\u0001\u0000\u0000\u0000\u0082\u0080\u0001\u0000\u0000\u0000\u0083\u0084"+
		"\u0007\b\u0000\u0000\u0084\u0085\u0005\t\u0000\u0000\u0085\u008b\u0003"+
		"\u0002\u0001\u0000\u0086\u0087\u0003\u0006\u0003\u0000\u0087\u0088\u0005"+
		"\t\u0000\u0000\u0088\u0089\u0003\u0002\u0001\u0000\u0089\u008b\u0001\u0000"+
		"\u0000\u0000\u008a\u0083\u0001\u0000\u0000\u0000\u008a\u0086\u0001\u0000"+
		"\u0000\u0000\u008b\u0005\u0001\u0000\u0000\u0000\u008c\u008d\u0007\t\u0000"+
		"\u0000\u008d\u0007\u0001\u0000\u0000\u0000\u000f\u0011\u001b\'26@DHkn"+
		"uz~\u0080\u008a";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}