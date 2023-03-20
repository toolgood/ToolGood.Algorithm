using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 算法引擎助手
    /// </summary>
    public static class AlgorithmEngineHelper
    {
        private static HashSet<string> _lexerSet;

        private static HashSet<string> GetLexerSet()
        {
            if (_lexerSet == null) {
                var lexerSet = new HashSet<string>();
                lexerSet.Add("NULL");
                lexerSet.Add("IF");
                lexerSet.Add("IFERROR");
                lexerSet.Add("ISNUMBER");
                lexerSet.Add("ISTEXT");
                lexerSet.Add("ISERROR");
                lexerSet.Add("ISNONTEXT");
                lexerSet.Add("ISLOGICAL");
                lexerSet.Add("ISEVEN");
                lexerSet.Add("ISODD");
                lexerSet.Add("ISNULL");
                lexerSet.Add("ISNULLORERROR");
                lexerSet.Add("AND");
                lexerSet.Add("OR");
                lexerSet.Add("NOT");
                lexerSet.Add("TRUE");
                lexerSet.Add("FALSE");
                lexerSet.Add("E");
                lexerSet.Add("PI");
                lexerSet.Add("DEC2BIN");
                lexerSet.Add("DEC2HEX");
                lexerSet.Add("DEC2OCT");
                lexerSet.Add("HEX2BIN");
                lexerSet.Add("HEX2DEC");
                lexerSet.Add("HEX2OCT");
                lexerSet.Add("OCT2BIN");
                lexerSet.Add("OCT2DEC");
                lexerSet.Add("OCT2HEX");
                lexerSet.Add("BIN2OCT");
                lexerSet.Add("BIN2DEC");
                lexerSet.Add("BIN2HEX");
                lexerSet.Add("ABS");
                lexerSet.Add("QUOTIENT");
                lexerSet.Add("MOD");
                lexerSet.Add("SIGN");
                lexerSet.Add("SQRT");
                lexerSet.Add("TRUNC");
                lexerSet.Add("INT");
                lexerSet.Add("GCD");
                lexerSet.Add("LCM");
                lexerSet.Add("COMBIN");
                lexerSet.Add("PERMUT");
                lexerSet.Add("DEGREES");
                lexerSet.Add("RADIANS");
                lexerSet.Add("COS");
                lexerSet.Add("COSH");
                lexerSet.Add("SIN");
                lexerSet.Add("SINH");
                lexerSet.Add("TAN");
                lexerSet.Add("TANH");
                lexerSet.Add("ACOS");
                lexerSet.Add("ACOSH");
                lexerSet.Add("ASIN");
                lexerSet.Add("ASINH");
                lexerSet.Add("ATAN");
                lexerSet.Add("ATANH");
                lexerSet.Add("ATAN2");
                lexerSet.Add("ROUND");
                lexerSet.Add("ROUNDDOWN");
                lexerSet.Add("ROUNDUP");
                lexerSet.Add("CEILING");
                lexerSet.Add("FLOOR");
                lexerSet.Add("EVEN");
                lexerSet.Add("ODD");
                lexerSet.Add("MROUND");
                lexerSet.Add("RAND");
                lexerSet.Add("RANDBETWEEN");
                lexerSet.Add("FACT");
                lexerSet.Add("FACTDOUBLE");
                lexerSet.Add("POWER");
                lexerSet.Add("EXP");
                lexerSet.Add("LN");
                lexerSet.Add("LOG");
                lexerSet.Add("LOG10");
                lexerSet.Add("MULTINOMIAL");
                lexerSet.Add("PRODUCT");
                lexerSet.Add("SQRTPI");
                lexerSet.Add("SUMSQ");
                lexerSet.Add("ASC");
                lexerSet.Add("JIS");
                lexerSet.Add("WIDECHAR");
                lexerSet.Add("CHAR");
                lexerSet.Add("CLEAN");
                lexerSet.Add("CODE");
                lexerSet.Add("CONCATENATE");
                lexerSet.Add("EXACT");
                lexerSet.Add("FIND");
                lexerSet.Add("FIXED");
                lexerSet.Add("LEFT");
                lexerSet.Add("LEN");
                lexerSet.Add("LOWER");
                lexerSet.Add("TOLOWER");
                lexerSet.Add("MID");
                lexerSet.Add("PROPER");
                lexerSet.Add("REPLACE");
                lexerSet.Add("REPT");
                lexerSet.Add("RIGHT");
                lexerSet.Add("RMB");
                lexerSet.Add("SEARCH");
                lexerSet.Add("SUBSTITUTE");
                lexerSet.Add("T");
                lexerSet.Add("TEXT");
                lexerSet.Add("TRIM");
                lexerSet.Add("UPPER");
                lexerSet.Add("TOUPPER");
                lexerSet.Add("VALUE");
                lexerSet.Add("DATEVALUE");
                lexerSet.Add("TIMEVALUE");
                lexerSet.Add("DATE");
                lexerSet.Add("TIME");
                lexerSet.Add("NOW");
                lexerSet.Add("TODAY");
                lexerSet.Add("YEAR");
                lexerSet.Add("MONTH");
                lexerSet.Add("DAY");
                lexerSet.Add("HOUR");
                lexerSet.Add("MINUTE");
                lexerSet.Add("SECOND");
                lexerSet.Add("WEEKDAY");
                lexerSet.Add("DATEDIF");
                lexerSet.Add("DAYS360");
                lexerSet.Add("EDATE");
                lexerSet.Add("EOMONTH");
                lexerSet.Add("NETWORKDAYS");
                lexerSet.Add("WORKDAY");
                lexerSet.Add("WEEKNUM");
                lexerSet.Add("MAX");
                lexerSet.Add("MEDIAN");
                lexerSet.Add("MIN");
                lexerSet.Add("QUARTILE");
                lexerSet.Add("MODE");
                lexerSet.Add("LARGE");
                lexerSet.Add("SMALL");
                lexerSet.Add("PERCENTILE");
                lexerSet.Add("PERCENTRANK");
                lexerSet.Add("AVERAGE");
                lexerSet.Add("AVERAGEIF");
                lexerSet.Add("GEOMEAN");
                lexerSet.Add("HARMEAN");
                lexerSet.Add("COUNT");
                lexerSet.Add("COUNTIF");
                lexerSet.Add("SUM");
                lexerSet.Add("SUMIF");
                lexerSet.Add("AVEDEV");
                lexerSet.Add("STDEV");
                lexerSet.Add("STDEVP");
                lexerSet.Add("DEVSQ");
                lexerSet.Add("VAR");
                lexerSet.Add("VARP");
                lexerSet.Add("NORMDIST");
                lexerSet.Add("NORMINV");
                lexerSet.Add("NORMSDIST");
                lexerSet.Add("NORMSINV");
                lexerSet.Add("BETADIST");
                lexerSet.Add("BETAINV");
                lexerSet.Add("BINOMDIST");
                lexerSet.Add("EXPONDIST");
                lexerSet.Add("FDIST");
                lexerSet.Add("FINV");
                lexerSet.Add("FISHER");
                lexerSet.Add("FISHERINV");
                lexerSet.Add("GAMMADIST");
                lexerSet.Add("GAMMAINV");
                lexerSet.Add("GAMMALN");
                lexerSet.Add("HYPGEOMDIST");
                lexerSet.Add("LOGINV");
                lexerSet.Add("LOGNORMDIST");
                lexerSet.Add("NEGBINOMDIST");
                lexerSet.Add("POISSON");
                lexerSet.Add("TDIST");
                lexerSet.Add("TINV");
                lexerSet.Add("WEIBULL");
                lexerSet.Add("URLENCODE");
                lexerSet.Add("URLDECODE");
                lexerSet.Add("HTMLENCODE");
                lexerSet.Add("HTMLDECODE");
                lexerSet.Add("BASE64TOTEXT");
                lexerSet.Add("BASE64URLTOTEXT");
                lexerSet.Add("TEXTTOBASE64");
                lexerSet.Add("TEXTTOBASE64URL");
                lexerSet.Add("REGEX");
                lexerSet.Add("REGEXREPALCE");
                lexerSet.Add("ISREGEX");
                lexerSet.Add("ISMATCH");
                lexerSet.Add("GUID");
                lexerSet.Add("MD5");
                lexerSet.Add("SHA1");
                lexerSet.Add("SHA256");
                lexerSet.Add("SHA512");
                lexerSet.Add("CRC32");
                lexerSet.Add("HMACMD5");
                lexerSet.Add("HMACSHA1");
                lexerSet.Add("HMACSHA256");
                lexerSet.Add("HMACSHA512");
                lexerSet.Add("TRIMSTART");
                lexerSet.Add("LTRIM");
                lexerSet.Add("TRIMEND");
                lexerSet.Add("RTRIM");
                lexerSet.Add("INDEXOF");
                lexerSet.Add("LASTINDEXOF");
                lexerSet.Add("SPLIT");
                lexerSet.Add("JOIN");
                lexerSet.Add("SUBSTRING");
                lexerSet.Add("STARTSWITH");
                lexerSet.Add("ENDSWITH");
                lexerSet.Add("ISNULLOREMPTY");
                lexerSet.Add("ISNULLORWHITESPACE");
                lexerSet.Add("REMOVESTART");
                lexerSet.Add("REMOVEEND");
                lexerSet.Add("JSON");
                lexerSet.Add("VLOOKUP");
                lexerSet.Add("LOOKUP");
                lexerSet.Add("ARRAY");

				lexerSet.Add("ADDYEARS");
				lexerSet.Add("ADDMONTHS");
				lexerSet.Add("ADDDAYS");
				lexerSet.Add("ADDHOURS");
				lexerSet.Add("ADDMINUTES");
				lexerSet.Add("ADDSECONDS");
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


    }
}
