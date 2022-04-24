package toolgood.algorithm;

import java.util.HashSet;
import java.util.Set;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.CharUtil;
import toolgood.algorithm.internals.DiyNameVisitor;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;

/**
 * 算法引擎助手
 */
public class AlgorithmEngineHelper {

    private static Set<String> _lexerSet;

    private static Set<String> GetLexerSet() {
        if (_lexerSet == null) {
            Set<String> lexerSet = new HashSet<String>();
            lexerSet.add("NULL");
            lexerSet.add("IF");
            lexerSet.add("IFERROR");
            lexerSet.add("ISNUMBER");
            lexerSet.add("ISTEXT");
            lexerSet.add("ISERROR");
            lexerSet.add("ISNONTEXT");
            lexerSet.add("ISLOGICAL");
            lexerSet.add("ISEVEN");
            lexerSet.add("ISODD");
            lexerSet.add("ISNULL");
            lexerSet.add("ISNULLORERROR");
            lexerSet.add("AND");
            lexerSet.add("OR");
            lexerSet.add("NOT");
            lexerSet.add("TRUE");
            lexerSet.add("FALSE");
            lexerSet.add("E");
            lexerSet.add("PI");
            lexerSet.add("DEC2BIN");
            lexerSet.add("DEC2HEX");
            lexerSet.add("DEC2OCT");
            lexerSet.add("HEX2BIN");
            lexerSet.add("HEX2DEC");
            lexerSet.add("HEX2OCT");
            lexerSet.add("OCT2BIN");
            lexerSet.add("OCT2DEC");
            lexerSet.add("OCT2HEX");
            lexerSet.add("BIN2OCT");
            lexerSet.add("BIN2DEC");
            lexerSet.add("BIN2HEX");
            lexerSet.add("ABS");
            lexerSet.add("QUOTIENT");
            lexerSet.add("MOD");
            lexerSet.add("SIGN");
            lexerSet.add("SQRT");
            lexerSet.add("TRUNC");
            lexerSet.add("INT");
            lexerSet.add("GCD");
            lexerSet.add("LCM");
            lexerSet.add("COMBIN");
            lexerSet.add("PERMUT");
            lexerSet.add("DEGREES");
            lexerSet.add("RADIANS");
            lexerSet.add("COS");
            lexerSet.add("COSH");
            lexerSet.add("SIN");
            lexerSet.add("SINH");
            lexerSet.add("TAN");
            lexerSet.add("TANH");
            lexerSet.add("ACOS");
            lexerSet.add("ACOSH");
            lexerSet.add("ASIN");
            lexerSet.add("ASINH");
            lexerSet.add("ATAN");
            lexerSet.add("ATANH");
            lexerSet.add("ATAN2");
            lexerSet.add("ROUND");
            lexerSet.add("ROUNDDOWN");
            lexerSet.add("ROUNDUP");
            lexerSet.add("CEILING");
            lexerSet.add("FLOOR");
            lexerSet.add("EVEN");
            lexerSet.add("ODD");
            lexerSet.add("MROUND");
            lexerSet.add("RAND");
            lexerSet.add("RANDBETWEEN");
            lexerSet.add("FACT");
            lexerSet.add("FACTDOUBLE");
            lexerSet.add("POWER");
            lexerSet.add("EXP");
            lexerSet.add("LN");
            lexerSet.add("LOG");
            lexerSet.add("LOG10");
            lexerSet.add("MULTINOMIAL");
            lexerSet.add("PRODUCT");
            lexerSet.add("SQRTPI");
            lexerSet.add("SUMSQ");
            lexerSet.add("ASC");
            lexerSet.add("JIS");
            lexerSet.add("WIDECHAR");
            lexerSet.add("CHAR");
            lexerSet.add("CLEAN");
            lexerSet.add("CODE");
            lexerSet.add("CONCATENATE");
            lexerSet.add("EXACT");
            lexerSet.add("FIND");
            lexerSet.add("FIXED");
            lexerSet.add("LEFT");
            lexerSet.add("LEN");
            lexerSet.add("LOWER");
            lexerSet.add("TOLOWER");
            lexerSet.add("MID");
            lexerSet.add("PROPER");
            lexerSet.add("REPLACE");
            lexerSet.add("REPT");
            lexerSet.add("RIGHT");
            lexerSet.add("RMB");
            lexerSet.add("SEARCH");
            lexerSet.add("SUBSTITUTE");
            lexerSet.add("T");
            lexerSet.add("TEXT");
            lexerSet.add("TRIM");
            lexerSet.add("UPPER");
            lexerSet.add("TOUPPER");
            lexerSet.add("VALUE");
            lexerSet.add("DATEVALUE");
            lexerSet.add("TIMEVALUE");
            lexerSet.add("DATE");
            lexerSet.add("TIME");
            lexerSet.add("NOW");
            lexerSet.add("TODAY");
            lexerSet.add("YEAR");
            lexerSet.add("MONTH");
            lexerSet.add("DAY");
            lexerSet.add("HOUR");
            lexerSet.add("MINUTE");
            lexerSet.add("SECOND");
            lexerSet.add("WEEKDAY");
            lexerSet.add("DATEDIF");
            lexerSet.add("DAYS360");
            lexerSet.add("EDATE");
            lexerSet.add("EOMONTH");
            lexerSet.add("NETWORKDAYS");
            lexerSet.add("WORKDAY");
            lexerSet.add("WEEKNUM");
            lexerSet.add("MAX");
            lexerSet.add("MEDIAN");
            lexerSet.add("MIN");
            lexerSet.add("QUARTILE");
            lexerSet.add("MODE");
            lexerSet.add("LARGE");
            lexerSet.add("SMALL");
            lexerSet.add("PERCENTILE");
            lexerSet.add("PERCENTRANK");
            lexerSet.add("AVERAGE");
            lexerSet.add("AVERAGEIF");
            lexerSet.add("GEOMEAN");
            lexerSet.add("HARMEAN");
            lexerSet.add("COUNT");
            lexerSet.add("COUNTIF");
            lexerSet.add("SUM");
            lexerSet.add("SUMIF");
            lexerSet.add("AVEDEV");
            lexerSet.add("STDEV");
            lexerSet.add("STDEVP");
            lexerSet.add("DEVSQ");
            lexerSet.add("VAR");
            lexerSet.add("VARP");
            lexerSet.add("NORMDIST");
            lexerSet.add("NORMINV");
            lexerSet.add("NORMSDIST");
            lexerSet.add("NORMSINV");
            lexerSet.add("BETADIST");
            lexerSet.add("BETAINV");
            lexerSet.add("BINOMDIST");
            lexerSet.add("EXPONDIST");
            lexerSet.add("FDIST");
            lexerSet.add("FINV");
            lexerSet.add("FISHER");
            lexerSet.add("FISHERINV");
            lexerSet.add("GAMMADIST");
            lexerSet.add("GAMMAINV");
            lexerSet.add("GAMMALN");
            lexerSet.add("HYPGEOMDIST");
            lexerSet.add("LOGINV");
            lexerSet.add("LOGNORMDIST");
            lexerSet.add("NEGBINOMDIST");
            lexerSet.add("POISSON");
            lexerSet.add("TDIST");
            lexerSet.add("TINV");
            lexerSet.add("WEIBULL");
            lexerSet.add("URLENCODE");
            lexerSet.add("URLDECODE");
            lexerSet.add("HTMLENCODE");
            lexerSet.add("HTMLDECODE");
            lexerSet.add("BASE64TOTEXT");
            lexerSet.add("BASE64URLTOTEXT");
            lexerSet.add("TEXTTOBASE64");
            lexerSet.add("TEXTTOBASE64URL");
            lexerSet.add("REGEX");
            lexerSet.add("REGEXREPALCE");
            lexerSet.add("ISREGEX");
            lexerSet.add("ISMATCH");
            lexerSet.add("GUID");
            lexerSet.add("MD5");
            lexerSet.add("SHA1");
            lexerSet.add("SHA256");
            lexerSet.add("SHA512");
            lexerSet.add("CRC32");
            lexerSet.add("HMACMD5");
            lexerSet.add("HMACSHA1");
            lexerSet.add("HMACSHA256");
            lexerSet.add("HMACSHA512");
            lexerSet.add("TRIMSTART");
            lexerSet.add("LTRIM");
            lexerSet.add("TRIMEND");
            lexerSet.add("RTRIM");
            lexerSet.add("INDEXOF");
            lexerSet.add("LASTINDEXOF");
            lexerSet.add("SPLIT");
            lexerSet.add("JOIN");
            lexerSet.add("SUBSTRING");
            lexerSet.add("STARTSWITH");
            lexerSet.add("ENDSWITH");
            lexerSet.add("ISNULLOREMPTY");
            lexerSet.add("ISNULLORWHITESPACE");
            lexerSet.add("REMOVESTART");
            lexerSet.add("REMOVEEND");
            lexerSet.add("JSON");
            lexerSet.add("VLOOKUP");
            lexerSet.add("LOOKUP");
            lexerSet.add("ARRAY");
            _lexerSet = lexerSet;
        }
        return _lexerSet;
    }

    /// <summary>
    /// 是否与内置关键字相同
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public boolean IsKeywords(String parameter) {
        Set<String> lexerSet = GetLexerSet();
        return lexerSet.contains(CharUtil.StandardString(parameter));
    }

    public DiyNameInfo GetDiyNames(String exp) throws Exception {
        if (exp == null || exp.equals("")) {
            throw new Exception("Parameter exp invalid !");
        }
        final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        final mathLexer lexer = new mathLexer(stream);
        final CommonTokenStream tokens = new CommonTokenStream(lexer);
        final mathParser parser = new mathParser(tokens);
        final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);
        final ProgContext context = parser.prog();

        if (antlrErrorListener.IsError) {
            throw new Exception(antlrErrorListener.ErrorMsg);
        }

        final DiyNameVisitor visitor = new DiyNameVisitor();
        visitor.visit(context);
        return visitor.diy;
    }

}
