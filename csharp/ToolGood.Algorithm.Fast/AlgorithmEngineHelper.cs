using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.UnitConversion;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 算法引擎助手
    /// </summary>
    public static class AlgorithmEngineHelper
    {
        private static HashSet<string> _lexerSet;
        private static Regex unitRegex;

        private static HashSet<string> GetLexerSet()
        {
            if (_lexerSet == null) {
                var lexerSet = new HashSet<string> {
                    "NULL",
                    "IF",
                    "IFERROR",
                    "ISNUMBER",
                    "ISTEXT",
                    "ISERROR",
                    "ISNONTEXT",
                    "ISLOGICAL",
                    "ISEVEN",
                    "ISODD",
                    "ISNULL",
                    "ISNULLORERROR",
                    "AND",
                    "OR",
                    "NOT",
                    "TRUE",
                    "FALSE",
                    "E",
                    "PI",
                    "DEC2BIN",
                    "DEC2HEX",
                    "DEC2OCT",
                    "HEX2BIN",
                    "HEX2DEC",
                    "HEX2OCT",
                    "OCT2BIN",
                    "OCT2DEC",
                    "OCT2HEX",
                    "BIN2OCT",
                    "BIN2DEC",
                    "BIN2HEX",
                    "ABS",
                    "QUOTIENT",
                    "MOD",
                    "SIGN",
                    "SQRT",
                    "TRUNC",
                    "INT",
                    "GCD",
                    "LCM",
                    "COMBIN",
                    "PERMUT",
                    "DEGREES",
                    "RADIANS",
                    "COS",
                    "COSH",
                    "SIN",
                    "SINH",
                    "TAN",
                    "TANH",
                    "ACOS",
                    "ACOSH",
                    "ASIN",
                    "ASINH",
                    "ATAN",
                    "ATANH",
                    "ATAN2",
                    "ROUND",
                    "ROUNDDOWN",
                    "ROUNDUP",
                    "CEILING",
                    "FLOOR",
                    "EVEN",
                    "ODD",
                    "MROUND",
                    "RAND",
                    "RANDBETWEEN",
                    "FACT",
                    "FACTDOUBLE",
                    "POWER",
                    "EXP",
                    "LN",
                    "LOG",
                    "LOG10",
                    "MULTINOMIAL",
                    "PRODUCT",
                    "SQRTPI",
                    "SUMSQ",
                    "ASC",
                    "JIS",
                    "WIDECHAR",
                    "CHAR",
                    "CLEAN",
                    "CODE",
                    "CONCATENATE",
                    "EXACT",
                    "FIND",
                    "FIXED",
                    "LEFT",
                    "LEN",
                    "LOWER",
                    "TOLOWER",
                    "MID",
                    "PROPER",
                    "REPLACE",
                    "REPT",
                    "RIGHT",
                    "RMB",
                    "SEARCH",
                    "SUBSTITUTE",
                    "T",
                    "TEXT",
                    "TRIM",
                    "UPPER",
                    "TOUPPER",
                    "VALUE",
                    "DATEVALUE",
                    "TIMEVALUE",
                    "DATE",
                    "TIME",
                    "NOW",
                    "TODAY",
                    "YEAR",
                    "MONTH",
                    "DAY",
                    "HOUR",
                    "MINUTE",
                    "SECOND",
                    "WEEKDAY",
                    "DATEDIF",
                    "DAYS360",
                    "EDATE",
                    "EOMONTH",
                    "NETWORKDAYS",
                    "WORKDAY",
                    "WEEKNUM",
                    "MAX",
                    "MEDIAN",
                    "MIN",
                    "QUARTILE",
                    "MODE",
                    "LARGE",
                    "SMALL",
                    "PERCENTILE",
                    "PERCENTILE.INC",
                    "PERCENTRANK",
                    "PERCENTRANK.INC",
                    "AVERAGE",
                    "AVERAGEIF",
                    "GEOMEAN",
                    "HARMEAN",
                    "COUNT",
                    "COUNTIF",
                    "SUM",
                    "SUMIF",
                    "AVEDEV",
                    "STDEV",
                    "STDEV.S",
                    "STDEVP",
                    "STDEV.P",
                    "COVAR",
                    "COVARIANCE.P",
                    "COVARIANCE.S'",
                    "DEVSQ",
                    "VAR",
                    "VAR.S",
                    "VARP",
                    "VAR.P",
                    "NORMDIST",
                    "NORM.DIST",
                    "NORMINV",
                    "NORM.INV",
                    "NORMSDIST",
                    "NORM.S.DIST",
                    "NORMSINV",
                    "NORM.S.INV",
                    "BETADIST",
                    "BETA.DIST",
                    "BETAINV",
                    "BETA.INV",
                    "BINOMDIST",
                    "BINOM.DIST",
                    "EXPONDIST",
                    "EXPON.DIST",
                    "FDIST",
                    "F.DIST",
                    "FINV",
                    "F.INV",
                    "FISHER",
                    "FISHERINV",
                    "GAMMADIST",
                    "GAMMA.DIST",
                    "GAMMAINV",
                    "GAMMA.INV",
                    "GAMMALN",
                    "GAMMALN.PRECISE",
                    "HYPGEOMDIST",
                    "HYPGEOM.DIST",
                    "LOGINV",
                    "LOGNORM.INV",
                    "LOGNORMDIST",
                    "LOGNORM.DIST",
                    "NEGBINOMDIST",
                    "NEGBINOM.DIST",
                    "POISSON",
                    "POISSON.DIST",
                    "TDIST",
                    "T.DIST",
                    "TINV",
                    "T.INV",
                    "WEIBULL",
                    "URLENCODE",
                    "URLDECODE",
                    "HTMLENCODE",
                    "HTMLDECODE",
                    "BASE64TOTEXT",
                    "BASE64URLTOTEXT",
                    "TEXTTOBASE64",
                    "TEXTTOBASE64URL",
                    "REGEX",
                    "REGEXREPALCE",
                    "ISREGEX",
                    "ISMATCH",
                    "GUID",
                    "MD5",
                    "SHA1",
                    "SHA256",
                    "SHA512",
                    "CRC32",
                    "HMACMD5",
                    "HMACSHA1",
                    "HMACSHA256",
                    "HMACSHA512",
                    "TRIMSTART",
                    "LTRIM",
                    "TRIMEND",
                    "RTRIM",
                    "INDEXOF",
                    "LASTINDEXOF",
                    "SPLIT",
                    "JOIN",
                    "SUBSTRING",
                    "STARTSWITH",
                    "ENDSWITH",
                    "ISNULLOREMPTY",
                    "ISNULLORWHITESPACE",
                    "REMOVESTART",
                    "REMOVEEND",
                    "JSON",
                    "VLOOKUP",
                    "LOOKUP",
                    "ARRAY",

                    "ADDYEARS",
                    "ADDMONTHS",
                    "ADDDAYS",
                    "ADDHOURS",
                    "ADDMINUTES",
                    "ADDSECONDS",
                    "TIMESTAMP",

                    "HAS",
                    "HASKEY",
                    "CONTAINS",
                    "CONTAINSKEY",
                    "HASVALUE",
                    "CONTAINSVALUE",
                    "PARAM",
                    "PARAMETER",
                    "GETPARAMETER",
                    "ERROR"
                };

                _lexerSet = lexerSet;
            }
            return _lexerSet;
        }

        /// <summary>
        /// 是否与内置关键字相同
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool IsKeywords(string parameter)
        {
            var lexerSet = GetLexerSet();
            return lexerSet.Contains(CharUtil.StandardString(parameter));
        }

        /// <summary>
        /// 获取 DIY 名称
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DiyNameInfo GetDiyNames(String exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) {
                throw new Exception("Parameter exp invalid !");
            }
            var stream = new AntlrCharStream(new AntlrInputStream(exp));
            var lexer = new mathLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new mathParser(tokens);
            var antlrErrorListener = new AntlrErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(antlrErrorListener);

            var context = parser.prog();
            if (antlrErrorListener.IsError) {
                throw new Exception(antlrErrorListener.ErrorMsg);
            }
            DiyNameVisitor visitor = new DiyNameVisitor();
            visitor.Visit(context);
            return visitor.diy;
        }

        /// <summary>
        /// 单位转换
        /// </summary>
        /// <param name="src"></param>
        /// <param name="oldSrcUnit"></param>
        /// <param name="oldTarUnit"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static decimal UnitConversion(decimal src, string oldSrcUnit, string oldTarUnit, string name = null)
        {
            if (string.IsNullOrWhiteSpace(oldSrcUnit) || string.IsNullOrWhiteSpace(oldTarUnit)) { return src; }
            if (unitRegex == null) {
                unitRegex = new Regex(@"[\s \(\)（）\[\]<>]", RegexOptions.Compiled);
            }
            oldSrcUnit = unitRegex.Replace(oldSrcUnit, "");
            if (oldSrcUnit == oldTarUnit) { return src; }

            if (DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new DistanceConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new MassConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new AreaConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new VolumeConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (string.IsNullOrEmpty(name)) {
                throw new Exception($"The input item has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
            }
            throw new Exception($"The input item [{name}] has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
        }

  
     
    }
}