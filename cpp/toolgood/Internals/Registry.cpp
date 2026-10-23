// Registry.cpp - 函数注册表（由 csharp MathFunctionVisitor 的分派表自动生成）
#include "Registry.h"

#include "Functions/Compare/Compare.h"
#include "Functions/Csharp/Csharp.h"
#include "Functions/CsharpSecurity/CsharpSecurity.h"
#include "Functions/CsharpWeb/CsharpWeb.h"
#include "Functions/DateTimes/DateTimes.h"
#include "Functions/Financial/Financial.h"
#include "Functions/Flow/Flow.h"
#include "Functions/MathBase/MathBase.h"
#include "Functions/MathSum/MathSum.h"
#include "Functions/MathSum2/MathSum2.h"
#include "Functions/MathTransformation/MathTransformation.h"
#include "Functions/MathTrigonometric/MathTrigonometric.h"
#include "Functions/Operator/Operator.h"
#include "Functions/String/String.h"
#include "Functions/Value/Value.h"

namespace toolgood {

static void RegisterGroup_compare() {
}

static void RegisterGroup_csharp() {
    RegisterFunction("ENDSWITH", [](const std::vector<Function>& a) { return NewFunction_ENDSWITH(a); });
    RegisterFunction("HAS", [](const std::vector<Function>& a) { return NewFunction_HAS(a); });
    RegisterFunction("HASVALUE", [](const std::vector<Function>& a) { return NewFunction_HASVALUE(a); });
    RegisterFunction("INDEXOF", [](const std::vector<Function>& a) { return NewFunction_INDEXOF(a); });
    RegisterFunction("ISREGEX", [](const std::vector<Function>& a) { return NewFunction_ISREGEX(a); });
    RegisterFunction("JOIN", [](const std::vector<Function>& a) { return NewFunction_JOIN(a); });
    RegisterFunction("LASTINDEXOF", [](const std::vector<Function>& a) { return NewFunction_LASTINDEXOF(a); });
    RegisterFunction("LOOKCEILING", [](const std::vector<Function>& a) { return NewFunction_LOOKCEILING(a); });
    RegisterFunction("LOOKFLOOR", [](const std::vector<Function>& a) { return NewFunction_LOOKFLOOR(a); });
    RegisterFunction("REGEX", [](const std::vector<Function>& a) { return NewFunction_REGEX(a); });
    RegisterFunction("REGEXREPLACE", [](const std::vector<Function>& a) { return NewFunction_REGEXREPLACE(a); });
    RegisterFunction("REMOVEEND", [](const std::vector<Function>& a) { return NewFunction_REMOVEEND(a); });
    RegisterFunction("REMOVESTART", [](const std::vector<Function>& a) { return NewFunction_REMOVESTART(a); });
    RegisterFunction("SPLIT", [](const std::vector<Function>& a) { return NewFunction_SPLIT(a); });
    RegisterFunction("STARTSWITH", [](const std::vector<Function>& a) { return NewFunction_STARTSWITH(a); });
    RegisterFunction("SUBSTRING", [](const std::vector<Function>& a) { return NewFunction_SUBSTRING(a); });
    RegisterFunction("TRIMEND", [](const std::vector<Function>& a) { return NewFunction_TRIMEND(a); });
    RegisterFunction("TRIMSTART", [](const std::vector<Function>& a) { return NewFunction_TRIMSTART(a); });
}

static void RegisterGroup_csharpsecurity() {
    RegisterFunction("HMACMD5", [](const std::vector<Function>& a) { return NewFunction_HMACMD5(a); });
    RegisterFunction("HMACSHA1", [](const std::vector<Function>& a) { return NewFunction_HMACSHA1(a); });
    RegisterFunction("HMACSHA256", [](const std::vector<Function>& a) { return NewFunction_HMACSHA256(a); });
    RegisterFunction("HMACSHA512", [](const std::vector<Function>& a) { return NewFunction_HMACSHA512(a); });
    RegisterFunction("MD5", [](const std::vector<Function>& a) { return NewFunction_MD5(a); });
    RegisterFunction("SHA1", [](const std::vector<Function>& a) { return NewFunction_SHA1(a); });
    RegisterFunction("SHA256", [](const std::vector<Function>& a) { return NewFunction_SHA256(a); });
    RegisterFunction("SHA512", [](const std::vector<Function>& a) { return NewFunction_SHA512(a); });
}

static void RegisterGroup_csharpweb() {
    RegisterFunction("BASE64TOTEXT", [](const std::vector<Function>& a) { return NewFunction_BASE64TOTEXT(a); });
    RegisterFunction("BASE64URLTOTEXT", [](const std::vector<Function>& a) { return NewFunction_BASE64URLTOTEXT(a); });
    RegisterFunction("HTMLDECODE", [](const std::vector<Function>& a) { return NewFunction_HTMLDECODE(a); });
    RegisterFunction("HTMLENCODE", [](const std::vector<Function>& a) { return NewFunction_HTMLENCODE(a); });
    RegisterFunction("TEXTTOBASE64", [](const std::vector<Function>& a) { return NewFunction_TEXTTOBASE64(a); });
    RegisterFunction("TEXTTOBASE64URL", [](const std::vector<Function>& a) { return NewFunction_TEXTTOBASE64URL(a); });
    RegisterFunction("URLDECODE", [](const std::vector<Function>& a) { return NewFunction_URLDECODE(a); });
    RegisterFunction("URLENCODE", [](const std::vector<Function>& a) { return NewFunction_URLENCODE(a); });
}

static void RegisterGroup_datetimes() {
    RegisterFunction("ADDDAYS", [](const std::vector<Function>& a) { return NewFunction_ADDDAYS(a); });
    RegisterFunction("ADDHOURS", [](const std::vector<Function>& a) { return NewFunction_ADDHOURS(a); });
    RegisterFunction("ADDMINUTES", [](const std::vector<Function>& a) { return NewFunction_ADDMINUTES(a); });
    RegisterFunction("ADDMONTHS", [](const std::vector<Function>& a) { return NewFunction_ADDMONTHS(a); });
    RegisterFunction("ADDSECONDS", [](const std::vector<Function>& a) { return NewFunction_ADDSECONDS(a); });
    RegisterFunction("ADDYEARS", [](const std::vector<Function>& a) { return NewFunction_ADDYEARS(a); });
    RegisterFunction("DATE", [](const std::vector<Function>& a) { return NewFunction_DATE(a); });
    RegisterFunction("DATEDIF", [](const std::vector<Function>& a) { return NewFunction_DATEDIF(a); });
    RegisterFunction("DATEVALUE", [](const std::vector<Function>& a) { return NewFunction_DATEVALUE(a); });
    RegisterFunction("DAY", [](const std::vector<Function>& a) { return NewFunction_DAY(a); });
    RegisterFunction("DAYS", [](const std::vector<Function>& a) { return NewFunction_DAYS(a); });
    RegisterFunction("DAYS360", [](const std::vector<Function>& a) { return NewFunction_DAYS360(a); });
    RegisterFunction("EDATE", [](const std::vector<Function>& a) { return NewFunction_EDATE(a); });
    RegisterFunction("EOMONTH", [](const std::vector<Function>& a) { return NewFunction_EOMONTH(a); });
    RegisterFunction("HOUR", [](const std::vector<Function>& a) { return NewFunction_HOUR(a); });
    RegisterFunction("MINUTE", [](const std::vector<Function>& a) { return NewFunction_MINUTE(a); });
    RegisterFunction("MONTH", [](const std::vector<Function>& a) { return NewFunction_MONTH(a); });
    RegisterFunction("NETWORKDAYS", [](const std::vector<Function>& a) { return NewFunction_NETWORKDAYS(a); });
    RegisterFunction("SECOND", [](const std::vector<Function>& a) { return NewFunction_SECOND(a); });
    RegisterFunction("TIME", [](const std::vector<Function>& a) { return NewFunction_TIME(a); });
    RegisterFunction("TIMESTAMP", [](const std::vector<Function>& a) { return NewFunction_TIMESTAMP(a); });
    RegisterFunction("TIMEVALUE", [](const std::vector<Function>& a) { return NewFunction_TIMEVALUE(a); });
    RegisterFunction("WEEKDAY", [](const std::vector<Function>& a) { return NewFunction_WEEKDAY(a); });
    RegisterFunction("WEEKNUM", [](const std::vector<Function>& a) { return NewFunction_WEEKNUM(a); });
    RegisterFunction("WORKDAY", [](const std::vector<Function>& a) { return NewFunction_WORKDAY(a); });
    RegisterFunction("YEAR", [](const std::vector<Function>& a) { return NewFunction_YEAR(a); });
    RegisterFunction("YEARFRAC", [](const std::vector<Function>& a) { return NewFunction_YEARFRAC(a); });
}

static void RegisterGroup_financial() {
    RegisterFunction("DB", [](const std::vector<Function>& a) { return NewFunction_DB(a); });
    RegisterFunction("DDB", [](const std::vector<Function>& a) { return NewFunction_DDB(a); });
    RegisterFunction("FV", [](const std::vector<Function>& a) { return NewFunction_FV(a); });
    RegisterFunction("IPMT", [](const std::vector<Function>& a) { return NewFunction_IPMT(a); });
    RegisterFunction("IRR", [](const std::vector<Function>& a) { return NewFunction_IRR(a); });
    RegisterFunction("MIRR", [](const std::vector<Function>& a) { return NewFunction_MIRR(a); });
    RegisterFunction("NPER", [](const std::vector<Function>& a) { return NewFunction_NPER(a); });
    RegisterFunction("NPV", [](const std::vector<Function>& a) { return NewFunction_NPV(a); });
    RegisterFunction("PMT", [](const std::vector<Function>& a) { return NewFunction_PMT(a); });
    RegisterFunction("PPMT", [](const std::vector<Function>& a) { return NewFunction_PPMT(a); });
    RegisterFunction("PV", [](const std::vector<Function>& a) { return NewFunction_PV(a); });
    RegisterFunction("RATE", [](const std::vector<Function>& a) { return NewFunction_RATE(a); });
    RegisterFunction("SLN", [](const std::vector<Function>& a) { return NewFunction_SLN(a); });
    RegisterFunction("SYD", [](const std::vector<Function>& a) { return NewFunction_SYD(a); });
    RegisterFunction("XIRR", [](const std::vector<Function>& a) { return NewFunction_XIRR(a); });
    RegisterFunction("XNPV", [](const std::vector<Function>& a) { return NewFunction_XNPV(a); });
}

static void RegisterGroup_flow() {
    RegisterFunction("IF", [](const std::vector<Function>& a) { return NewFunction_IF(a); });
    RegisterFunction("IFERROR", [](const std::vector<Function>& a) { return NewFunction_IFERROR(a); });
    RegisterFunction("IFS", [](const std::vector<Function>& a) { return NewFunction_IFS(a); });
    RegisterFunction("ISERROR", [](const std::vector<Function>& a) { return NewFunction_ISERROR(a); });
    RegisterFunction("ISEVEN", [](const std::vector<Function>& a) { return NewFunction_ISEVEN(a); });
    RegisterFunction("ISLOGICAL", [](const std::vector<Function>& a) { return NewFunction_ISLOGICAL(a); });
    RegisterFunction("ISNONTEXT", [](const std::vector<Function>& a) { return NewFunction_ISNONTEXT(a); });
    RegisterFunction("ISNULL", [](const std::vector<Function>& a) { return NewFunction_ISNULL(a); });
    RegisterFunction("ISNULLOREMPTY", [](const std::vector<Function>& a) { return NewFunction_ISNULLOREMPTY(a); });
    RegisterFunction("ISNULLORERROR", [](const std::vector<Function>& a) { return NewFunction_ISNULLORERROR(a); });
    RegisterFunction("ISNULLORWHITESPACE", [](const std::vector<Function>& a) { return NewFunction_ISNULLORWHITESPACE(a); });
    RegisterFunction("ISNUMBER", [](const std::vector<Function>& a) { return NewFunction_ISNUMBER(a); });
    RegisterFunction("ISODD", [](const std::vector<Function>& a) { return NewFunction_ISODD(a); });
    RegisterFunction("ISTEXT", [](const std::vector<Function>& a) { return NewFunction_ISTEXT(a); });
    RegisterFunction("NOT", [](const std::vector<Function>& a) { return NewFunction_NOT(a); });
    RegisterFunction("SWITCH", [](const std::vector<Function>& a) { return NewFunction_SWITCH(a); });
}

static void RegisterGroup_mathbase() {
    RegisterFunction("ABS", [](const std::vector<Function>& a) { return NewFunction_ABS(a); });
    RegisterFunction("CEILING", [](const std::vector<Function>& a) { return NewFunction_CEILING(a); });
    RegisterFunction("COMBIN", [](const std::vector<Function>& a) { return NewFunction_COMBIN(a); });
    RegisterFunction("DELTA", [](const std::vector<Function>& a) { return NewFunction_DELTA(a); });
    RegisterFunction("EVEN", [](const std::vector<Function>& a) { return NewFunction_EVEN(a); });
    RegisterFunction("EXP", [](const std::vector<Function>& a) { return NewFunction_EXP(a); });
    RegisterFunction("FACT", [](const std::vector<Function>& a) { return NewFunction_FACT(a); });
    RegisterFunction("FACTDOUBLE", [](const std::vector<Function>& a) { return NewFunction_FACTDOUBLE(a); });
    RegisterFunction("FIXED", [](const std::vector<Function>& a) { return NewFunction_FIXED(a); });
    RegisterFunction("FLOOR", [](const std::vector<Function>& a) { return NewFunction_FLOOR(a); });
    RegisterFunction("GCD", [](const std::vector<Function>& a) { return NewFunction_GCD(a); });
    RegisterFunction("GESTEP", [](const std::vector<Function>& a) { return NewFunction_GESTEP(a); });
    RegisterFunction("INT", [](const std::vector<Function>& a) { return NewFunction_INT(a); });
    RegisterFunction("LCM", [](const std::vector<Function>& a) { return NewFunction_LCM(a); });
    RegisterFunction("LN", [](const std::vector<Function>& a) { return NewFunction_LN(a); });
    RegisterFunction("LOG", [](const std::vector<Function>& a) { return NewFunction_LOG(a); });
    RegisterFunction("LOG10", [](const std::vector<Function>& a) { return NewFunction_LOG10(a); });
    RegisterFunction("MROUND", [](const std::vector<Function>& a) { return NewFunction_MROUND(a); });
    RegisterFunction("MULTINOMIAL", [](const std::vector<Function>& a) { return NewFunction_MULTINOMIAL(a); });
    RegisterFunction("ODD", [](const std::vector<Function>& a) { return NewFunction_ODD(a); });
    RegisterFunction("PERMUT", [](const std::vector<Function>& a) { return NewFunction_PERMUT(a); });
    RegisterFunction("POWER", [](const std::vector<Function>& a) { return NewFunction_POWER(a); });
    RegisterFunction("PRODUCT", [](const std::vector<Function>& a) { return NewFunction_PRODUCT(a); });
    RegisterFunction("QUOTIENT", [](const std::vector<Function>& a) { return NewFunction_QUOTIENT(a); });
    RegisterFunction("RANDBETWEEN", [](const std::vector<Function>& a) { return NewFunction_RANDBETWEEN(a); });
    RegisterFunction("ROUND", [](const std::vector<Function>& a) { return NewFunction_ROUND(a); });
    RegisterFunction("ROUNDDOWN", [](const std::vector<Function>& a) { return NewFunction_ROUNDDOWN(a); });
    RegisterFunction("ROUNDUP", [](const std::vector<Function>& a) { return NewFunction_ROUNDUP(a); });
    RegisterFunction("SIGN", [](const std::vector<Function>& a) { return NewFunction_SIGN(a); });
    RegisterFunction("SQRT", [](const std::vector<Function>& a) { return NewFunction_SQRT(a); });
    RegisterFunction("SQRTPI", [](const std::vector<Function>& a) { return NewFunction_SQRTPI(a); });
    RegisterFunction("TRUNC", [](const std::vector<Function>& a) { return NewFunction_TRUNC(a); });
}

static void RegisterGroup_mathsum() {
    RegisterFunction("AVEDEV", [](const std::vector<Function>& a) { return NewFunction_AVEDEV(a); });
    RegisterFunction("AVERAGE", [](const std::vector<Function>& a) { return NewFunction_AVERAGE(a); });
    RegisterFunction("AVERAGEIF", [](const std::vector<Function>& a) { return NewFunction_AVERAGEIF(a); });
    RegisterFunction("CORREL", [](const std::vector<Function>& a) { return NewFunction_CORREL(a); });
    RegisterFunction("COUNT", [](const std::vector<Function>& a) { return NewFunction_COUNT(a); });
    RegisterFunction("COUNTIF", [](const std::vector<Function>& a) { return NewFunction_COUNTIF(a); });
    RegisterFunction("COVAR", [](const std::vector<Function>& a) { return NewFunction_COVAR(a); });
    RegisterFunction("COVARIANCES", [](const std::vector<Function>& a) { return NewFunction_COVARIANCES(a); });
    RegisterFunction("DEVSQ", [](const std::vector<Function>& a) { return NewFunction_DEVSQ(a); });
    RegisterFunction("FORECAST", [](const std::vector<Function>& a) { return NewFunction_FORECAST(a); });
    RegisterFunction("GEOMEAN", [](const std::vector<Function>& a) { return NewFunction_GEOMEAN(a); });
    RegisterFunction("HARMEAN", [](const std::vector<Function>& a) { return NewFunction_HARMEAN(a); });
    RegisterFunction("INTERCEPT", [](const std::vector<Function>& a) { return NewFunction_INTERCEPT(a); });
    RegisterFunction("LARGE", [](const std::vector<Function>& a) { return NewFunction_LARGE(a); });
    RegisterFunction("MAX", [](const std::vector<Function>& a) { return NewFunction_MAX(a); });
    RegisterFunction("MEDIAN", [](const std::vector<Function>& a) { return NewFunction_MEDIAN(a); });
    RegisterFunction("MIN", [](const std::vector<Function>& a) { return NewFunction_MIN(a); });
    RegisterFunction("MODE", [](const std::vector<Function>& a) { return NewFunction_MODE(a); });
    RegisterFunction("PEARSON", [](const std::vector<Function>& a) { return NewFunction_PEARSON(a); });
    RegisterFunction("PERCENTILE", [](const std::vector<Function>& a) { return NewFunction_PERCENTILE(a); });
    RegisterFunction("PERCENTRANK", [](const std::vector<Function>& a) { return NewFunction_PERCENTRANK(a); });
    RegisterFunction("POISSON", [](const std::vector<Function>& a) { return NewFunction_POISSON(a); });
    RegisterFunction("QUARTILE", [](const std::vector<Function>& a) { return NewFunction_QUARTILE(a); });
    RegisterFunction("RANK", [](const std::vector<Function>& a) { return NewFunction_RANK(a); });
    RegisterFunction("SERIESSUM", [](const std::vector<Function>& a) { return NewFunction_SERIESSUM(a); });
    RegisterFunction("SLOPE", [](const std::vector<Function>& a) { return NewFunction_SLOPE(a); });
    RegisterFunction("SMALL", [](const std::vector<Function>& a) { return NewFunction_SMALL(a); });
    RegisterFunction("STDEV", [](const std::vector<Function>& a) { return NewFunction_STDEV(a); });
    RegisterFunction("STDEVP", [](const std::vector<Function>& a) { return NewFunction_STDEVP(a); });
    RegisterFunction("SUM", [](const std::vector<Function>& a) { return NewFunction_SUM(a); });
    RegisterFunction("SUMIF", [](const std::vector<Function>& a) { return NewFunction_SUMIF(a); });
    RegisterFunction("SUMPRODUCT", [](const std::vector<Function>& a) { return NewFunction_SUMPRODUCT(a); });
    RegisterFunction("SUMSQ", [](const std::vector<Function>& a) { return NewFunction_SUMSQ(a); });
    RegisterFunction("SUMX2MY2", [](const std::vector<Function>& a) { return NewFunction_SUMX2MY2(a); });
    RegisterFunction("SUMX2PY2", [](const std::vector<Function>& a) { return NewFunction_SUMX2PY2(a); });
    RegisterFunction("SUMXMY2", [](const std::vector<Function>& a) { return NewFunction_SUMXMY2(a); });
    RegisterFunction("VAR", [](const std::vector<Function>& a) { return NewFunction_VAR(a); });
    RegisterFunction("VARP", [](const std::vector<Function>& a) { return NewFunction_VARP(a); });
}

static void RegisterGroup_mathsum2() {
    RegisterFunction("BESSELI", [](const std::vector<Function>& a) { return NewFunction_BESSELI(a); });
    RegisterFunction("BESSELJ", [](const std::vector<Function>& a) { return NewFunction_BESSELJ(a); });
    RegisterFunction("BESSELK", [](const std::vector<Function>& a) { return NewFunction_BESSELK(a); });
    RegisterFunction("BESSELY", [](const std::vector<Function>& a) { return NewFunction_BESSELY(a); });
    RegisterFunction("BETADIST", [](const std::vector<Function>& a) { return NewFunction_BETADIST(a); });
    RegisterFunction("BETAINV", [](const std::vector<Function>& a) { return NewFunction_BETAINV(a); });
    RegisterFunction("BINOMDIST", [](const std::vector<Function>& a) { return NewFunction_BINOMDIST(a); });
    RegisterFunction("ERF", [](const std::vector<Function>& a) { return NewFunction_ERF(a); });
    RegisterFunction("ERFC", [](const std::vector<Function>& a) { return NewFunction_ERFC(a); });
    RegisterFunction("EXPONDIST", [](const std::vector<Function>& a) { return NewFunction_EXPONDIST(a); });
    RegisterFunction("FDIST", [](const std::vector<Function>& a) { return NewFunction_FDIST(a); });
    RegisterFunction("FINV", [](const std::vector<Function>& a) { return NewFunction_FINV(a); });
    RegisterFunction("FISHER", [](const std::vector<Function>& a) { return NewFunction_FISHER(a); });
    RegisterFunction("FISHERINV", [](const std::vector<Function>& a) { return NewFunction_FISHERINV(a); });
    RegisterFunction("GAMMADIST", [](const std::vector<Function>& a) { return NewFunction_GAMMADIST(a); });
    RegisterFunction("GAMMAINV", [](const std::vector<Function>& a) { return NewFunction_GAMMAINV(a); });
    RegisterFunction("GAMMALN", [](const std::vector<Function>& a) { return NewFunction_GAMMALN(a); });
    RegisterFunction("HYPGEOMDIST", [](const std::vector<Function>& a) { return NewFunction_HYPGEOMDIST(a); });
    RegisterFunction("LOGINV", [](const std::vector<Function>& a) { return NewFunction_LOGINV(a); });
    RegisterFunction("LOGNORMDIST", [](const std::vector<Function>& a) { return NewFunction_LOGNORMDIST(a); });
    RegisterFunction("NEGBINOMDIST", [](const std::vector<Function>& a) { return NewFunction_NEGBINOMDIST(a); });
    RegisterFunction("NORMDIST", [](const std::vector<Function>& a) { return NewFunction_NORMDIST(a); });
    RegisterFunction("NORMINV", [](const std::vector<Function>& a) { return NewFunction_NORMINV(a); });
    RegisterFunction("NORMSDIST", [](const std::vector<Function>& a) { return NewFunction_NORMSDIST(a); });
    RegisterFunction("NORMSINV", [](const std::vector<Function>& a) { return NewFunction_NORMSINV(a); });
    RegisterFunction("TDIST", [](const std::vector<Function>& a) { return NewFunction_TDIST(a); });
    RegisterFunction("TINV", [](const std::vector<Function>& a) { return NewFunction_TINV(a); });
    RegisterFunction("WEIBULL", [](const std::vector<Function>& a) { return NewFunction_WEIBULL(a); });
}

static void RegisterGroup_mathtransformation() {
    RegisterFunction("ARABIC", [](const std::vector<Function>& a) { return NewFunction_ARABIC(a); });
    RegisterFunction("BIN2DEC", [](const std::vector<Function>& a) { return NewFunction_BIN2DEC(a); });
    RegisterFunction("BIN2HEX", [](const std::vector<Function>& a) { return NewFunction_BIN2HEX(a); });
    RegisterFunction("BIN2OCT", [](const std::vector<Function>& a) { return NewFunction_BIN2OCT(a); });
    RegisterFunction("DEC2BIN", [](const std::vector<Function>& a) { return NewFunction_DEC2BIN(a); });
    RegisterFunction("DEC2HEX", [](const std::vector<Function>& a) { return NewFunction_DEC2HEX(a); });
    RegisterFunction("DEC2OCT", [](const std::vector<Function>& a) { return NewFunction_DEC2OCT(a); });
    RegisterFunction("HEX2BIN", [](const std::vector<Function>& a) { return NewFunction_HEX2BIN(a); });
    RegisterFunction("HEX2DEC", [](const std::vector<Function>& a) { return NewFunction_HEX2DEC(a); });
    RegisterFunction("HEX2OCT", [](const std::vector<Function>& a) { return NewFunction_HEX2OCT(a); });
    RegisterFunction("OCT2BIN", [](const std::vector<Function>& a) { return NewFunction_OCT2BIN(a); });
    RegisterFunction("OCT2DEC", [](const std::vector<Function>& a) { return NewFunction_OCT2DEC(a); });
    RegisterFunction("OCT2HEX", [](const std::vector<Function>& a) { return NewFunction_OCT2HEX(a); });
    RegisterFunction("ROMAN", [](const std::vector<Function>& a) { return NewFunction_ROMAN(a); });
}

static void RegisterGroup_mathtrigonometric() {
    RegisterFunction("ACOS", [](const std::vector<Function>& a) { return NewFunction_ACOS(a); });
    RegisterFunction("ACOSH", [](const std::vector<Function>& a) { return NewFunction_ACOSH(a); });
    RegisterFunction("ACOT", [](const std::vector<Function>& a) { return NewFunction_ACOT(a); });
    RegisterFunction("ACOTH", [](const std::vector<Function>& a) { return NewFunction_ACOTH(a); });
    RegisterFunction("ASIN", [](const std::vector<Function>& a) { return NewFunction_ASIN(a); });
    RegisterFunction("ASINH", [](const std::vector<Function>& a) { return NewFunction_ASINH(a); });
    RegisterFunction("ATAN", [](const std::vector<Function>& a) { return NewFunction_ATAN(a); });
    RegisterFunction("ATAN2", [](const std::vector<Function>& a) { return NewFunction_ATAN2(a); });
    RegisterFunction("ATANH", [](const std::vector<Function>& a) { return NewFunction_ATANH(a); });
    RegisterFunction("COS", [](const std::vector<Function>& a) { return NewFunction_COS(a); });
    RegisterFunction("COSH", [](const std::vector<Function>& a) { return NewFunction_COSH(a); });
    RegisterFunction("COT", [](const std::vector<Function>& a) { return NewFunction_COT(a); });
    RegisterFunction("COTH", [](const std::vector<Function>& a) { return NewFunction_COTH(a); });
    RegisterFunction("CSC", [](const std::vector<Function>& a) { return NewFunction_CSC(a); });
    RegisterFunction("CSCH", [](const std::vector<Function>& a) { return NewFunction_CSCH(a); });
    RegisterFunction("DEGREES", [](const std::vector<Function>& a) { return NewFunction_DEGREES(a); });
    RegisterFunction("RADIANS", [](const std::vector<Function>& a) { return NewFunction_RADIANS(a); });
    RegisterFunction("SEC", [](const std::vector<Function>& a) { return NewFunction_SEC(a); });
    RegisterFunction("SECH", [](const std::vector<Function>& a) { return NewFunction_SECH(a); });
    RegisterFunction("SIN", [](const std::vector<Function>& a) { return NewFunction_SIN(a); });
    RegisterFunction("SINH", [](const std::vector<Function>& a) { return NewFunction_SINH(a); });
    RegisterFunction("TAN", [](const std::vector<Function>& a) { return NewFunction_TAN(a); });
    RegisterFunction("TANH", [](const std::vector<Function>& a) { return NewFunction_TANH(a); });
}

static void RegisterGroup_op() {
    RegisterFunction("AND", [](const std::vector<Function>& a) { return NewFunction_AND_N(a); });
    RegisterFunction("MOD", [](const std::vector<Function>& a) { return NewFunction_Mod(a); });
    RegisterFunction("OR", [](const std::vector<Function>& a) { return NewFunction_OR_N(a); });
    RegisterFunction("XOR", [](const std::vector<Function>& a) { return NewFunction_XOR(a); });
}

static void RegisterGroup_str() {
    RegisterFunction("ASC", [](const std::vector<Function>& a) { return NewFunction_ASC(a); });
    RegisterFunction("CHAR", [](const std::vector<Function>& a) { return NewFunction_CHAR(a); });
    RegisterFunction("CLEAN", [](const std::vector<Function>& a) { return NewFunction_CLEAN(a); });
    RegisterFunction("CODE", [](const std::vector<Function>& a) { return NewFunction_CODE(a); });
    RegisterFunction("CONCATENATE", [](const std::vector<Function>& a) { return NewFunction_CONCATENATE(a); });
    RegisterFunction("EXACT", [](const std::vector<Function>& a) { return NewFunction_EXACT(a); });
    RegisterFunction("FIND", [](const std::vector<Function>& a) { return NewFunction_FIND(a); });
    RegisterFunction("JIS", [](const std::vector<Function>& a) { return NewFunction_JIS(a); });
    RegisterFunction("LEFT", [](const std::vector<Function>& a) { return NewFunction_LEFT(a); });
    RegisterFunction("LEN", [](const std::vector<Function>& a) { return NewFunction_LEN(a); });
    RegisterFunction("LOWER", [](const std::vector<Function>& a) { return NewFunction_LOWER(a); });
    RegisterFunction("MID", [](const std::vector<Function>& a) { return NewFunction_MID(a); });
    RegisterFunction("PROPER", [](const std::vector<Function>& a) { return NewFunction_PROPER(a); });
    RegisterFunction("REPLACE", [](const std::vector<Function>& a) { return NewFunction_REPLACE(a); });
    RegisterFunction("REPT", [](const std::vector<Function>& a) { return NewFunction_REPT(a); });
    RegisterFunction("RIGHT", [](const std::vector<Function>& a) { return NewFunction_RIGHT(a); });
    RegisterFunction("RMB", [](const std::vector<Function>& a) { return NewFunction_RMB(a); });
    RegisterFunction("SEARCH", [](const std::vector<Function>& a) { return NewFunction_SEARCH(a); });
    RegisterFunction("SUBSTITUTE", [](const std::vector<Function>& a) { return NewFunction_SUBSTITUTE(a); });
    RegisterFunction("T", [](const std::vector<Function>& a) { return NewFunction_T(a); });
    RegisterFunction("TEXT", [](const std::vector<Function>& a) { return NewFunction_TEXT(a); });
    RegisterFunction("TRIM", [](const std::vector<Function>& a) { return NewFunction_TRIM(a); });
    RegisterFunction("UNICHAR", [](const std::vector<Function>& a) { return NewFunction_UNICHAR(a); });
    RegisterFunction("UNICODE", [](const std::vector<Function>& a) { return NewFunction_UNICODE(a); });
    RegisterFunction("UPPER", [](const std::vector<Function>& a) { return NewFunction_UPPER(a); });
    RegisterFunction("VALUE", [](const std::vector<Function>& a) { return NewFunction_VALUE(a); });
}

static void RegisterGroup_value() {
    RegisterFunction("ARRAY", [](const std::vector<Function>& a) { return NewFunction_ARRAY(a); });
    RegisterFunction("ERROR", [](const std::vector<Function>& a) { return NewFunction_ERROR(a); });
    RegisterFunction("JSON", [](const std::vector<Function>& a) { return NewFunction_JSON(a); });
    RegisterFunction("PARAM", [](const std::vector<Function>& a) { return NewFunction_PARAM(a); });
}

void RegisterAllFunctions() {
    static bool done = false;
    if (done) return;
    done = true;
    RegisterGroup_compare();
    RegisterGroup_csharp();
    RegisterGroup_csharpsecurity();
    RegisterGroup_csharpweb();
    RegisterGroup_datetimes();
    RegisterGroup_financial();
    RegisterGroup_flow();
    RegisterGroup_mathbase();
    RegisterGroup_mathsum();
    RegisterGroup_mathsum2();
    RegisterGroup_mathtransformation();
    RegisterGroup_mathtrigonometric();
    RegisterGroup_op();
    RegisterGroup_str();
    RegisterGroup_value();
}

}  // namespace toolgood
