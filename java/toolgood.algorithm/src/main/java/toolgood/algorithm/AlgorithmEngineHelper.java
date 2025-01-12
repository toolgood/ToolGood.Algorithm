package toolgood.algorithm;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.CharUtil;
import toolgood.algorithm.internals.DiyNameVisitor;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.internals.MyFunction;
import toolgood.algorithm.internals.MyParameter;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;
import toolgood.algorithm.unitConversion.AreaConverter;
import toolgood.algorithm.unitConversion.DistanceConverter;
import toolgood.algorithm.unitConversion.MassConverter;
import toolgood.algorithm.unitConversion.VolumeConverter;

import java.math.BigDecimal;
import java.util.HashSet;
import java.util.Set;
import java.util.function.Function;

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
            lexerSet.add("ADDYEARS");
            lexerSet.add("ADDMONTHS");
            lexerSet.add("ADDDAYS");
            lexerSet.add("ADDHOURS");
            lexerSet.add("ADDMINUTES");
            lexerSet.add("ADDSECONDS");
            lexerSet.add("TIMESTAMP");

            lexerSet.add("HAS");
            lexerSet.add("HASKEY");
            lexerSet.add("CONTAINS");
            lexerSet.add("CONTAINSKEY");
            lexerSet.add("HASVALUE");
            lexerSet.add("CONTAINSVALUE");
            lexerSet.add("PARAM");
            lexerSet.add("PARAMETER");
            lexerSet.add("GETPARAMETER");
            lexerSet.add("ERROR");
            _lexerSet = lexerSet;
        }
        return _lexerSet;
    }

    /**
     * 是否与内置关键字相同
     *
     * @param parameter
     * @return
     */
    public static boolean IsKeywords(String parameter) {
        Set<String> lexerSet = GetLexerSet();
        return lexerSet.contains(CharUtil.StandardString(parameter));
    }

    /**
     * 获取 DIY 名称
     *
     * @param exp
     * @return
     * @throws Exception
     */
    public static DiyNameInfo GetDiyNames(String exp) throws Exception {
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

    /**
     * 单位转换
     *
     * @param src
     * @param oldSrcUnit
     * @param oldTarUnit
     * @return
     * @throws Exception
     */
    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit) throws Exception {
        return UnitConversion(src, oldSrcUnit, oldTarUnit, null);
    }

    /**
     * 单位转换
     *
     * @param src
     * @param oldSrcUnit
     * @param oldTarUnit
     * @param name
     * @return
     * @throws Exception
     */
    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit, String name)
            throws Exception {
        if (oldSrcUnit == null || oldSrcUnit.equals("") || oldTarUnit == null || oldTarUnit.equals("")) {
            return src;
        }
        oldSrcUnit = oldSrcUnit.replaceAll("[\\s \\(\\)（）\\[\\]<>]", "");
        if (oldSrcUnit.equals(oldTarUnit)) {
            return src;
        }
        if (DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
            DistanceConverter c = new DistanceConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
            MassConverter c = new MassConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
            AreaConverter c = new AreaConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
            VolumeConverter c = new VolumeConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (name == null || name.equals("")) {
            throw new Exception("The input item has different units and cannot be converted from [" + oldSrcUnit
                    + "] to [" + oldTarUnit + "]");
        }
        throw new Exception("The input item [" + name + "] has different units and cannot be converted from ["
                + oldSrcUnit + "] to [" + oldTarUnit + "]");
    }

    /**
     * 解析
     * 
     * @param exp
     * @return
     * @throws Exception
     */
    public static mathParser.ProgContext Parse(String exp) throws Exception {
        if (null == exp || exp.equals("")) {
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
        return context;
    }

    /**
     * 执行
     * 
     * @param context
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context) {
        return Evaluate(context, null, null, true, false, DistanceUnitType.M, AreaUnitType.M2, VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter) {
        return Evaluate(context, GetParameter, null, true, false, DistanceUnitType.M, AreaUnitType.M2,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, true, false, DistanceUnitType.M, AreaUnitType.M2,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, UseExcelIndex, false, DistanceUnitType.M,
                AreaUnitType.M2,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @param UseLocalTime
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex, boolean UseLocalTime) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, UseExcelIndex, UseLocalTime, DistanceUnitType.M,
                AreaUnitType.M2,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @param UseLocalTime
     * @param DistanceUnit
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex, boolean UseLocalTime,
            DistanceUnitType DistanceUnit) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, UseExcelIndex, UseLocalTime, DistanceUnit,
                AreaUnitType.M2,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @param UseLocalTime
     * @param DistanceUnit
     * @param AreaUnit
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex, boolean UseLocalTime,
            DistanceUnitType DistanceUnit, AreaUnitType AreaUnit) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, UseExcelIndex, UseLocalTime, DistanceUnit,
                AreaUnit,
                VolumeUnitType.M3,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @param UseLocalTime
     * @param DistanceUnit
     * @param AreaUnit
     * @param VolumeUnit
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex, boolean UseLocalTime,
            DistanceUnitType DistanceUnit, AreaUnitType AreaUnit, VolumeUnitType VolumeUnit) {
        return Evaluate(context, GetParameter, ExecuteDiyFunction, UseExcelIndex, UseLocalTime, DistanceUnit,
                AreaUnit,
                VolumeUnit,
                MassUnitType.KG);
    }

    /**
     * 执行
     * 
     * @param context
     * @param GetParameter
     * @param ExecuteDiyFunction
     * @param UseExcelIndex
     * @param UseLocalTime
     * @param DistanceUnit
     * @param AreaUnit
     * @param VolumeUnit
     * @param MassUnit
     * @return
     */
    public static Operand Evaluate(mathParser.ProgContext context, Function<MyParameter, Operand> GetParameter,
            Function<MyFunction, Operand> ExecuteDiyFunction, boolean UseExcelIndex, boolean UseLocalTime,
            DistanceUnitType DistanceUnit, AreaUnitType AreaUnit, VolumeUnitType VolumeUnit, MassUnitType MassUnit) {
        MathVisitor visitor = new MathVisitor();
        if (GetParameter != null) {
            visitor.GetParameter = GetParameter;
        }
        if (ExecuteDiyFunction != null) {
            visitor.DiyFunction = ExecuteDiyFunction;
        }
        visitor.excelIndex = UseExcelIndex ? 1 : 0;
        visitor.useLocalTime = UseLocalTime;
        visitor.MassUnit = MassUnit;
        visitor.DistanceUnit = DistanceUnit;
        visitor.AreaUnit = AreaUnit;
        visitor.VolumeUnit = VolumeUnit;
        return visitor.visit(context);
    }

}
