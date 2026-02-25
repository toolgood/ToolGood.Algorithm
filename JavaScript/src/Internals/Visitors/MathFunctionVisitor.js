import mathVisitor from '../../math/mathVisitor.js';
import { Function_Mul } from '../Functions/Operator/Function_Mul.js';
import { Function_Div } from '../Functions/Operator/Function_Div.js';
import { Function_Mod } from '../Functions/Operator/Function_Mod.js';
import { Function_Add } from '../Functions/Operator/Function_Add.js';
import { Function_Sub } from '../Functions/Operator/Function_Sub.js';
import { Function_Connect } from '../Functions/Operator/Function_Connect.js';
import { Function_EQ } from '../Functions/Compare/Function_EQ.js';
import { Function_LT } from '../Functions/Compare/Function_LT.js';
import { Function_LE } from '../Functions/Compare/Function_LE.js';
import { Function_GT } from '../Functions/Compare/Function_GT.js';
import { Function_GE } from '../Functions/Compare/Function_GE.js';
import { Function_NE } from '../Functions/Compare/Function_NE.js';
import { Function_AND } from '../Functions/Operator/Function_AND.js';
import { Function_OR } from '../Functions/Operator/Function_OR.js';
import { Function_AND_N } from '../Functions/Operator/Function_AND_N.js';
import { Function_OR_N } from '../Functions/Operator/Function_OR_N.js';
import { Function_NOT } from '../Functions/Flow/Function_NOT.js';
import { Function_Value } from '../Functions/Value/Function_Value.js';

import { Function_VALUE } from '../Functions/String/Function_VALUE.js';
import { Function_ABS } from '../Functions/MathBase/Function_ABS.js';
import { Function_QUOTIENT } from '../Functions/MathBase/Function_QUOTIENT.js';
import { Function_SIGN } from '../Functions/MathBase/Function_SIGN.js';
import { Function_SQRT } from '../Functions/MathBase/Function_SQRT.js';
import { Function_TRUNC } from '../Functions/MathBase/Function_TRUNC.js';
import { Function_GCD } from '../Functions/MathBase/Function_GCD.js';
import { Function_LCM } from '../Functions/MathBase/Function_LCM.js';
import { Function_COMBIN } from '../Functions/MathBase/Function_COMBIN.js';
import { Function_PERMUT } from '../Functions/MathBase/Function_PERMUT.js';
import { Function_Percentage } from '../Functions/MathBase/Function_Percentage.js';
import { Function_DEGREES } from '../Functions/MathTrigonometric/Function_DEGREES.js';
import { Function_RADIANS } from '../Functions/MathTrigonometric/Function_RADIANS.js';
import { Function_COS } from '../Functions/MathTrigonometric/Function_COS.js';
import { Function_COSH } from '../Functions/MathTrigonometric/Function_COSH.js';
import { Function_SIN } from '../Functions/MathTrigonometric/Function_SIN.js';
import { Function_SINH } from '../Functions/MathTrigonometric/Function_SINH.js';
import { Function_TAN } from '../Functions/MathTrigonometric/Function_TAN.js';
import { Function_TANH } from '../Functions/MathTrigonometric/Function_TANH.js';
import { Function_ACOS } from '../Functions/MathTrigonometric/Function_ACOS.js';
import { Function_ACOSH } from '../Functions/MathTrigonometric/Function_ACOSH.js';
import { Function_ASIN } from '../Functions/MathTrigonometric/Function_ASIN.js';
import { Function_ASINH } from '../Functions/MathTrigonometric/Function_ASINH.js';
import { Function_ATAN } from '../Functions/MathTrigonometric/Function_ATAN.js';
import { Function_ATANH } from '../Functions/MathTrigonometric/Function_ATANH.js';
import { Function_ATAN2 } from '../Functions/MathTrigonometric/Function_ATAN2.js';
import { Function_FIXED } from '../Functions/MathBase/Function_FIXED.js';
import { Function_BIN2OCT } from '../Functions/MathTransformation/Function_BIN2OCT.js';
import { Function_BIN2DEC } from '../Functions/MathTransformation/Function_BIN2DEC.js';
import { Function_BIN2HEX } from '../Functions/MathTransformation/Function_BIN2HEX.js';
import { Function_OCT2BIN } from '../Functions/MathTransformation/Function_OCT2BIN.js';
import { Function_OCT2DEC } from '../Functions/MathTransformation/Function_OCT2DEC.js';
import { Function_OCT2HEX } from '../Functions/MathTransformation/Function_OCT2HEX.js';
import { Function_DEC2BIN } from '../Functions/MathTransformation/Function_DEC2BIN.js';
import { Function_DEC2OCT } from '../Functions/MathTransformation/Function_DEC2OCT.js';
import { Function_DEC2HEX } from '../Functions/MathTransformation/Function_DEC2HEX.js';
import { Function_HEX2BIN } from '../Functions/MathTransformation/Function_HEX2BIN.js';
import { Function_HEX2OCT } from '../Functions/MathTransformation/Function_HEX2OCT.js';
import { Function_HEX2DEC } from '../Functions/MathTransformation/Function_HEX2DEC.js';
import { Function_ROUND } from '../Functions/MathBase/Function_ROUND.js';
import { Function_ROUNDDOWN } from '../Functions/MathBase/Function_ROUNDDOWN.js';
import { Function_ROUNDUP } from '../Functions/MathBase/Function_ROUNDUP.js';
import { Function_CEILING } from '../Functions/MathBase/Function_CEILING.js';
import { Function_FLOOR } from '../Functions/MathBase/Function_FLOOR.js';
import { Function_EVEN } from '../Functions/MathBase/Function_EVEN.js';
import { Function_ODD } from '../Functions/MathBase/Function_ODD.js';
import { Function_MROUND } from '../Functions/MathBase/Function_MROUND.js';
import { Function_RAND } from '../Functions/MathBase/Function_RAND.js';
import { Function_RANDBETWEEN } from '../Functions/MathBase/Function_RANDBETWEEN.js';
import { Function_COVARIANCES } from '../Functions/MathSum/Function_COVARIANCES.js';
import { Function_COVAR } from '../Functions/MathSum/Function_COVAR.js';
import { Function_FACT } from '../Functions/MathBase/Function_FACT.js';
import { Function_FACTDOUBLE } from '../Functions/MathBase/Function_FACTDOUBLE.js';
import { Function_POWER } from '../Functions/MathBase/Function_POWER.js';
import { Function_EXP } from '../Functions/MathBase/Function_EXP.js';
import { Function_LN } from '../Functions/MathBase/Function_LN.js';
import { Function_LOG } from '../Functions/MathBase/Function_LOG.js';
import { Function_LOG10 } from '../Functions/MathBase/Function_LOG10.js';
import { Function_MULTINOMIAL } from '../Functions/MathBase/Function_MULTINOMIAL.js';
import { Function_PRODUCT } from '../Functions/MathBase/Function_PRODUCT.js';
import { Function_SQRTPI } from '../Functions/MathBase/Function_SQRTPI.js';
import { Function_SUMSQ } from '../Functions/MathSum/Function_SUMSQ.js';
import { Function_ASC } from '../Functions/String/Function_ASC.js';
import { Function_JIS } from '../Functions/String/Function_JIS.js';
import { Function_CHAR } from '../Functions/String/Function_CHAR.js';
import { Function_CLEAN } from '../Functions/String/Function_CLEAN.js';
import { Function_CODE } from '../Functions/String/Function_CODE.js';
import { Function_CONCATENATE } from '../Functions/String/Function_CONCATENATE.js';
import { Function_EXACT } from '../Functions/String/Function_EXACT.js';
import { Function_FIND } from '../Functions/String/Function_FIND.js';
import { Function_LEFT } from '../Functions/String/Function_LEFT.js';
import { Function_LEN } from '../Functions/String/Function_LEN.js';
import { Function_LOWER } from '../Functions/String/Function_LOWER.js';
import { Function_MID } from '../Functions/String/Function_MID.js';
import { Function_PROPER } from '../Functions/String/Function_PROPER.js';
import { Function_REPLACE } from '../Functions/String/Function_REPLACE.js';
import { Function_REPT } from '../Functions/String/Function_REPT.js';
import { Function_RIGHT } from '../Functions/String/Function_RIGHT.js';
import { Function_RMB } from '../Functions/String/Function_RMB.js';
import { Function_SEARCH } from '../Functions/String/Function_SEARCH.js';
import { Function_SUBSTITUTE } from '../Functions/String/Function_SUBSTITUTE.js';
import { Function_T } from '../Functions/String/Function_T.js';
import { Function_TEXT } from '../Functions/String/Function_TEXT.js';
import { Function_TRIM } from '../Functions/String/Function_TRIM.js';
import { Function_UPPER } from '../Functions/String/Function_UPPER.js';
import { Function_DATEVALUE } from '../Functions/DateTimes/Function_DATEVALUE.js';
import { Function_TIMESTAMP } from '../Functions/DateTimes/Function_TIMESTAMP.js';
import { Function_TIMEVALUE } from '../Functions/DateTimes/Function_TIMEVALUE.js';
import { Function_DATE } from '../Functions/DateTimes/Function_DATE.js';
import { Function_TIME } from '../Functions/DateTimes/Function_TIME.js';
import { Function_NOW } from '../Functions/DateTimes/Function_NOW.js';
import { Function_TODAY } from '../Functions/DateTimes/Function_TODAY.js';
import { Function_YEAR } from '../Functions/DateTimes/Function_YEAR.js';
import { Function_MONTH } from '../Functions/DateTimes/Function_MONTH.js';
import { Function_DAY } from '../Functions/DateTimes/Function_DAY.js';
import { Function_HOUR } from '../Functions/DateTimes/Function_HOUR.js';
import { Function_MINUTE } from '../Functions/DateTimes/Function_MINUTE.js';
import { Function_SECOND } from '../Functions/DateTimes/Function_SECOND.js';
import { Function_WEEKDAY } from '../Functions/DateTimes/Function_WEEKDAY.js';
import { Function_DATEDIF } from '../Functions/DateTimes/Function_DATEDIF.js';
import { Function_DAYS360 } from '../Functions/DateTimes/Function_DAYS360.js';
import { Function_EDATE } from '../Functions/DateTimes/Function_EDATE.js';
import { Function_EOMONTH } from '../Functions/DateTimes/Function_EOMONTH.js';
import { Function_NETWORKDAYS } from '../Functions/DateTimes/Function_NETWORKDAYS.js';
import { Function_WORKDAY } from '../Functions/DateTimes/Function_WORKDAY.js';
import { Function_WEEKNUM } from '../Functions/DateTimes/Function_WEEKNUM.js';
import { Function_ADDMONTHS } from '../Functions/DateTimes/Function_ADDMONTHS.js';
import { Function_ADDYEARS } from '../Functions/DateTimes/Function_ADDYEARS.js';
import { Function_ADDSECONDS } from '../Functions/DateTimes/Function_ADDSECONDS.js';
import { Function_ADDMINUTES } from '../Functions/DateTimes/Function_ADDMINUTES.js';
import { Function_ADDDAYS } from '../Functions/DateTimes/Function_ADDDAYS.js';
import { Function_ADDHOURS } from '../Functions/DateTimes/Function_ADDHOURS.js';
import { Function_MAX } from '../Functions/MathSum/Function_MAX.js';
import { Function_MEDIAN } from '../Functions/MathSum/Function_MEDIAN.js';
import { Function_MIN } from '../Functions/MathSum/Function_MIN.js';
import { Function_QUARTILE } from '../Functions/MathSum/Function_QUARTILE.js';
import { Function_MODE } from '../Functions/MathSum/Function_MODE.js';
import { Function_LARGE } from '../Functions/MathSum/Function_LARGE.js';
import { Function_SMALL } from '../Functions/MathSum/Function_SMALL.js';
import { Function_PERCENTILE } from '../Functions/MathSum/Function_PERCENTILE.js';
import { Function_PERCENTRANK } from '../Functions/MathSum/Function_PERCENTRANK.js';
import { Function_AVERAGE } from '../Functions/MathSum/Function_AVERAGE.js';
import { Function_AVERAGEIF } from '../Functions/MathSum/Function_AVERAGEIF.js';
import { Function_GEOMEAN } from '../Functions/MathSum/Function_GEOMEAN.js';
import { Function_HARMEAN } from '../Functions/MathSum/Function_HARMEAN.js';
import { Function_COUNT } from '../Functions/MathSum/Function_COUNT.js';
import { Function_COUNTIF } from '../Functions/MathSum/Function_COUNTIF.js';
import { Function_SUM } from '../Functions/MathSum/Function_SUM.js';
import { Function_SUMIF } from '../Functions/MathSum/Function_SUMIF.js';
import { Function_AVEDEV } from '../Functions/MathSum/Function_AVEDEV.js';
import { Function_STDEV } from '../Functions/MathSum/Function_STDEV.js';
import { Function_STDEVP } from '../Functions/MathSum/Function_STDEVP.js';
import { Function_DEVSQ } from '../Functions/MathSum/Function_DEVSQ.js';
import { Function_VAR } from '../Functions/MathSum/Function_VAR.js';
import { Function_VARP } from '../Functions/MathSum/Function_VARP.js';
import { Function_NORMDIST } from '../Functions/MathSum/Function_NORMDIST.js';
import { Function_NORMINV } from '../Functions/MathSum/Function_NORMINV.js';
import { Function_NORMSDIST } from '../Functions/MathSum/Function_NORMSDIST.js';
import { Function_NORMSINV } from '../Functions/MathSum/Function_NORMSINV.js';
import { Function_BETADIST } from '../Functions/MathSum/Function_BETADIST.js';
import { Function_BETAINV } from '../Functions/MathSum/Function_BETAINV.js';
import { Function_BINOMDIST } from '../Functions/MathSum/Function_BINOMDIST.js';
import { Function_EXPONDIST } from '../Functions/MathSum/Function_EXPONDIST.js';
import { Function_FDIST } from '../Functions/MathSum/Function_FDIST.js';
import { Function_FINV } from '../Functions/MathSum/Function_FINV.js';
import { Function_FISHER } from '../Functions/MathSum/Function_FISHER.js';
import { Function_FISHERINV } from '../Functions/MathSum/Function_FISHERINV.js';
import { Function_GAMMADIST } from '../Functions/MathSum/Function_GAMMADIST.js';
import { Function_GAMMAINV } from '../Functions/MathSum/Function_GAMMAINV.js';
import { Function_GAMMALN } from '../Functions/MathSum/Function_GAMMALN.js';
import { Function_HYPGEOMDIST } from '../Functions/MathSum/Function_HYPGEOMDIST.js';
import { Function_LOGINV } from '../Functions/MathSum/Function_LOGINV.js';
import { Function_LOGNORMDIST } from '../Functions/MathSum/Function_LOGNORMDIST.js';
import { Function_NEGBINOMDIST } from '../Functions/MathSum/Function_NEGBINOMDIST.js';
import { Function_POISSON } from '../Functions/MathSum/Function_POISSON.js';
import { Function_TDIST } from '../Functions/MathSum/Function_TDIST.js';
import { Function_TINV } from '../Functions/MathSum/Function_TINV.js';
import { Function_WEIBULL } from '../Functions/MathSum/Function_WEIBULL.js';
import { Function_URLENCODE } from '../Functions/CsharpWeb/Function_URLENCODE.js';
import { Function_URLDECODE } from '../Functions/CsharpWeb/Function_URLDECODE.js';
import { Function_HTMLENCODE } from '../Functions/CsharpWeb/Function_HTMLENCODE.js';
import { Function_HTMLDECODE } from '../Functions/CsharpWeb/Function_HTMLDECODE.js';
import { Function_BASE64TOTEXT } from '../Functions/CsharpWeb/Function_BASE64TOTEXT.js';
import { Function_BASE64URLTOTEXT } from '../Functions/CsharpWeb/Function_BASE64URLTOTEXT.js';
import { Function_TEXTTOBASE64 } from '../Functions/CsharpWeb/Function_TEXTTOBASE64.js';
import { Function_TEXTTOBASE64URL } from '../Functions/CsharpWeb/Function_TEXTTOBASE64URL.js';
import { Function_REGEX } from '../Functions/Csharp/Function_REGEX.js';
import { Function_REGEXREPALCE } from '../Functions/Csharp/Function_REGEXREPALCE.js';
import { Function_ISREGEX } from '../Functions/Csharp/Function_ISREGEX.js';
import { Function_GUID } from '../Functions/Csharp/Function_GUID.js';
import { Function_MD5 } from '../Functions/CsharpSecurity/Function_MD5.js';
import { Function_SHA1 } from '../Functions/CsharpSecurity/Function_SHA1.js';
import { Function_SHA256 } from '../Functions/CsharpSecurity/Function_SHA256.js';
import { Function_SHA512 } from '../Functions/CsharpSecurity/Function_SHA512.js';
import { Function_HMACMD5 } from '../Functions/CsharpSecurity/Function_HMACMD5.js';
import { Function_HMACSHA1 } from '../Functions/CsharpSecurity/Function_HMACSHA1.js';
import { Function_HMACSHA256 } from '../Functions/CsharpSecurity/Function_HMACSHA256.js';
import { Function_HMACSHA512 } from '../Functions/CsharpSecurity/Function_HMACSHA512.js';
import { Function_STARTSWITH } from '../Functions/Csharp/Function_STARTSWITH.js';
import { Function_ENDSWITH } from '../Functions/Csharp/Function_ENDSWITH.js';
import { Function_INDEXOF } from '../Functions/Csharp/Function_INDEXOF.js';
import { Function_LASTINDEXOF } from '../Functions/Csharp/Function_LASTINDEXOF.js';
import { Function_SPLIT } from '../Functions/Csharp/Function_SPLIT.js';
import { Function_SUBSTRING } from '../Functions/Csharp/Function_SUBSTRING.js';
import { Function_TRIMSTART } from '../Functions/Csharp/Function_TRIMSTART.js';
import { Function_TRIMEND } from '../Functions/Csharp/Function_TRIMEND.js';
import { Function_REMOVESTART } from '../Functions/Csharp/Function_REMOVESTART.js';
import { Function_REMOVEEND } from '../Functions/Csharp/Function_REMOVEEND.js';
import { Function_JOIN } from '../Functions/Csharp/Function_JOIN.js';
import { Function_LOOKCEILING } from '../Functions/Csharp/Function_LOOKCEILING.js';
import { Function_LOOKFLOOR } from '../Functions/Csharp/Function_LOOKFLOOR.js';
import { Function_HAS } from '../Functions/Csharp/Function_HAS.js';
import { Function_HASVALUE } from '../Functions/Csharp/Function_HASVALUE.js';
import { Function_IF } from '../Functions/Flow/Function_IF.js';
import { Function_IFERROR } from '../Functions/Flow/Function_IFERROR.js';
import { Function_ISERROR } from '../Functions/Flow/Function_ISERROR.js';
import { Function_ISNULL } from '../Functions/Flow/Function_ISNULL.js';
import { Function_ISNULLORERROR } from '../Functions/Flow/Function_ISNULLORERROR.js';
import { Function_ISEVEN } from '../Functions/Flow/Function_ISEVEN.js';
import { Function_ISLOGICAL } from '../Functions/Flow/Function_ISLOGICAL.js';
import { Function_ISODD } from '../Functions/Flow/Function_ISODD.js';
import { Function_ISNONTEXT } from '../Functions/Flow/Function_ISNONTEXT.js';
import { Function_ISNULLOREMPTY } from '../Functions/Flow/Function_ISNULLOREMPTY.js';
import { Function_ISNULLORWHITESPACE } from '../Functions/Flow/Function_ISNULLORWHITESPACE.js';
import { Function_ISNUMBER } from '../Functions/Flow/Function_ISNUMBER.js';
import { Function_ISTEXT } from '../Functions/Flow/Function_ISTEXT.js';
import { Function_Array } from '../Functions/Value/Function_Array.js';
import { Function_JSON } from '../Functions/Value/Function_JSON.js';
import { Function_ArrayJson } from '../Functions/Value/Function_ArrayJson.js';
import { Function_ArrayJsonItem } from '../Functions/Value/Function_ArrayJsonItem.js';
import { Function_GetJsonValue } from '../Functions/Value/Function_GetJsonValue.js';
import { Function_NUM } from '../Functions/Value/Function_NUM.js';
import { Function_PARAM } from '../Functions/Value/Function_PARAM.js';
import { Function_PARAMETER } from '../Functions/Value/Function_PARAMETER.js';
import { Function_DiyFunction } from '../Functions/Value/Function_DiyFunction.js';
import { Function_ERROR } from '../Functions/Value/Function_ERROR.js';
import { CharUtil } from './CharUtil.js';
import { Operand } from '../../Operand.js';

const funcDict = {
    "IF": (args) => new Function_IF(args),
    "IFERROR": (args) => new Function_IFERROR(args),
    "ISNUMBER": (args) => new Function_ISNUMBER(args),
    "ISTEXT": (args) => new Function_ISTEXT(args),
    "ISERROR": (args) => new Function_ISERROR(args),
    "ISNONTEXT": (args) => new Function_ISNONTEXT(args),
    "ISLOGICAL": (args) => new Function_ISLOGICAL(args),
    "ISEVEN": (args) => new Function_ISEVEN(args),
    "ISODD": (args) => new Function_ISODD(args),
    "ISNULL": (args) => new Function_ISNULL(args),
    "ISNULLORERROR": (args) => new Function_ISNULLORERROR(args),
    "AND": (args) => new Function_AND(args),
    "OR": (args) => new Function_OR(args),
    "NOT": (args) => new Function_NOT(args),
    "DEC2BIN": (args) => new Function_DEC2BIN(args),
    "DEC2HEX": (args) => new Function_DEC2HEX(args),
    "DEC2OCT": (args) => new Function_DEC2OCT(args),
    "HEX2BIN": (args) => new Function_HEX2BIN(args),
    "HEX2DEC": (args) => new Function_HEX2DEC(args),
    "HEX2OCT": (args) => new Function_HEX2OCT(args),
    "OCT2BIN": (args) => new Function_OCT2BIN(args),
    "OCT2DEC": (args) => new Function_OCT2DEC(args),
    "OCT2HEX": (args) => new Function_OCT2HEX(args),
    "BIN2OCT": (args) => new Function_BIN2OCT(args),
    "BIN2DEC": (args) => new Function_BIN2DEC(args),
    "BIN2HEX": (args) => new Function_BIN2HEX(args),
    "ABS": (args) => new Function_ABS(args),
    "QUOTIENT": (args) => new Function_QUOTIENT(args),
    "MOD": (args) => new Function_Mod(args),
    "SIGN": (args) => new Function_SIGN(args),
    "SQRT": (args) => new Function_SQRT(args),
    "TRUNC": (args) => new Function_TRUNC(args),
    "INT": (args) => new Function_TRUNC(args),
    "GCD": (args) => new Function_GCD(args),
    "LCM": (args) => new Function_LCM(args),
    "COMBIN": (args) => new Function_COMBIN(args),
    "PERMUT": (args) => new Function_PERMUT(args),
    "DEGREES": (args) => new Function_DEGREES(args),
    "RADIANS": (args) => new Function_RADIANS(args),
    "COS": (args) => new Function_COS(args),
    "COSH": (args) => new Function_COSH(args),
    "SIN": (args) => new Function_SIN(args),
    "SINH": (args) => new Function_SINH(args),
    "TAN": (args) => new Function_TAN(args),
    "TANH": (args) => new Function_TANH(args),
    "ACOS": (args) => new Function_ACOS(args),
    "ACOSH": (args) => new Function_ACOSH(args),
    "ASIN": (args) => new Function_ASIN(args),
    "ASINH": (args) => new Function_ASINH(args),
    "ATAN": (args) => new Function_ATAN(args),
    "ATANH": (args) => new Function_ATANH(args),
    "ATAN2": (args) => new Function_ATAN2(args),
    "ROUND": (args) => new Function_ROUND(args),
    "ROUNDDOWN": (args) => new Function_ROUNDDOWN(args),
    "ROUNDUP": (args) => new Function_ROUNDUP(args),
    "CEILING": (args) => new Function_CEILING(args),
    "FLOOR": (args) => new Function_FLOOR(args),
    "EVEN": (args) => new Function_EVEN(args),
    "ODD": (args) => new Function_ODD(args),
    "MROUND": (args) => new Function_MROUND(args),
    "RANDBETWEEN": (args) => new Function_RANDBETWEEN(args),
    "FACT": (args) => new Function_FACT(args),
    "FACTDOUBLE": (args) => new Function_FACTDOUBLE(args),
    "POWER": (args) => new Function_POWER(args),
    "EXP": (args) => new Function_EXP(args),
    "LN": (args) => new Function_LN(args),
    "LOG": (args) => new Function_LOG(args),
    "LOG10": (args) => new Function_LOG10(args),
    "MULTINOMIAL": (args) => new Function_MULTINOMIAL(args),
    "PRODUCT": (args) => new Function_PRODUCT(args),
    "SQRTPI": (args) => new Function_SQRTPI(args),
    "SUMSQ": (args) => new Function_SUMSQ(args),
    "ASC": (args) => new Function_ASC(args),
    "JIS": (args) => new Function_JIS(args),
    "WIDECHAR": (args) => new Function_JIS(args),
    "CHAR": (args) => new Function_CHAR(args),
    "CLEAN": (args) => new Function_CLEAN(args),
    "CODE": (args) => new Function_CODE(args),
    "CONCATENATE": (args) => new Function_CONCATENATE(args),
    "EXACT": (args) => new Function_EXACT(args),
    "FIND": (args) => new Function_FIND(args),
    "FIXED": (args) => new Function_FIXED(args),
    "LEFT": (args) => new Function_LEFT(args),
    "LEN": (args) => new Function_LEN(args),
    "LOWER": (args) => new Function_LOWER(args),
    "TOLOWER": (args) => new Function_LOWER(args),
    "MID": (args) => new Function_MID(args),
    "PROPER": (args) => new Function_PROPER(args),
    "REPLACE": (args) => new Function_REPLACE(args),
    "REPT": (args) => new Function_REPT(args),
    "RIGHT": (args) => new Function_RIGHT(args),
    "RMB": (args) => new Function_RMB(args),
    "SEARCH": (args) => new Function_SEARCH(args),
    "SUBSTITUTE": (args) => new Function_SUBSTITUTE(args),
    "T": (args) => new Function_T(args),
    "TEXT": (args) => new Function_TEXT(args),
    "TRIM": (args) => new Function_TRIM(args),
    "UPPER": (args) => new Function_UPPER(args),
    "TOUPPER": (args) => new Function_UPPER(args),
    "VALUE": (args) => new Function_VALUE(args),
    "DATEVALUE": (args) => new Function_DATEVALUE(args),
    "TIMEVALUE": (args) => new Function_TIMEVALUE(args),
    "DATE": (args) => new Function_DATE(args),
    "TIME": (args) => new Function_TIME(args),
    "YEAR": (args) => new Function_YEAR(args),
    "MONTH": (args) => new Function_MONTH(args),
    "DAY": (args) => new Function_DAY(args),
    "HOUR": (args) => new Function_HOUR(args),
    "MINUTE": (args) => new Function_MINUTE(args),
    "SECOND": (args) => new Function_SECOND(args),
    "WEEKDAY": (args) => new Function_WEEKDAY(args),
    "DATEDIF": (args) => new Function_DATEDIF(args),
    "DAYS360": (args) => new Function_DAYS360(args),
    "EDATE": (args) => new Function_EDATE(args),
    "EOMONTH": (args) => new Function_EOMONTH(args),
    "NETWORKDAYS": (args) => new Function_NETWORKDAYS(args),
    "WORKDAY": (args) => new Function_WORKDAY(args),
    "WEEKNUM": (args) => new Function_WEEKNUM(args),
    "MAX": (args) => new Function_MAX(args),
    "MEDIAN": (args) => new Function_MEDIAN(args),
    "MIN": (args) => new Function_MIN(args),
    "QUARTILE": (args) => new Function_QUARTILE(args),
    "MODE": (args) => new Function_MODE(args),
    "LARGE": (args) => new Function_LARGE(args),
    "SMALL": (args) => new Function_SMALL(args),
    "PERCENTILE": (args) => new Function_PERCENTILE(args),
    "PERCENTILE.INC": (args) => new Function_PERCENTILE(args),
    "PERCENTRANK": (args) => new Function_PERCENTRANK(args),
    "PERCENTRANK.INC": (args) => new Function_PERCENTRANK(args),
    "AVERAGE": (args) => new Function_AVERAGE(args),
    "AVERAGEIF": (args) => new Function_AVERAGEIF(args),
    "GEOMEAN": (args) => new Function_GEOMEAN(args),
    "HARMEAN": (args) => new Function_HARMEAN(args),
    "COUNT": (args) => new Function_COUNT(args),
    "COUNTIF": (args) => new Function_COUNTIF(args),
    "SUM": (args) => new Function_SUM(args),
    "SUMIF": (args) => new Function_SUMIF(args),
    "AVEDEV": (args) => new Function_AVEDEV(args),
    "STDEV": (args) => new Function_STDEV(args),
    "STDEV.S": (args) => new Function_STDEV(args),
    "STDEVP": (args) => new Function_STDEVP(args),
    "STDEV.P": (args) => new Function_STDEVP(args),
    "COVAR": (args) => new Function_COVAR(args),
    "COVARIANCE.P": (args) => new Function_COVAR(args),
    "COVARIANCE.S": (args) => new Function_COVARIANCES(args),
    "DEVSQ": (args) => new Function_DEVSQ(args),
    "VAR": (args) => new Function_VAR(args),
    "VAR.S": (args) => new Function_VAR(args),
    "VARP": (args) => new Function_VARP(args),
    "VAR.P": (args) => new Function_VARP(args),
    "NORMDIST": (args) => new Function_NORMDIST(args),
    "NORM.DIST": (args) => new Function_NORMDIST(args),
    "NORMINV": (args) => new Function_NORMINV(args),
    "NORM.INV": (args) => new Function_NORMINV(args),
    "NORMSDIST": (args) => new Function_NORMSDIST(args),
    "NORM.S.DIST": (args) => new Function_NORMSDIST(args),
    "NORMSINV": (args) => new Function_NORMSINV(args),
    "NORM.S.INV": (args) => new Function_NORMSINV(args),
    "BETADIST": (args) => new Function_BETADIST(args),
    "BETA.DIST": (args) => new Function_BETADIST(args),
    "BETAINV": (args) => new Function_BETAINV(args),
    "BETA.INV": (args) => new Function_BETAINV(args),
    "BINOMDIST": (args) => new Function_BINOMDIST(args),
    "BINOM.DIST": (args) => new Function_BINOMDIST(args),
    "EXPONDIST": (args) => new Function_EXPONDIST(args),
    "EXPON.DIST": (args) => new Function_EXPONDIST(args),
    "FDIST": (args) => new Function_FDIST(args),
    "F.DIST": (args) => new Function_FDIST(args),
    "FINV": (args) => new Function_FINV(args),
    "F.INV": (args) => new Function_FINV(args),
    "FISHER": (args) => new Function_FISHER(args),
    "FISHERINV": (args) => new Function_FISHERINV(args),
    "GAMMADIST": (args) => new Function_GAMMADIST(args),
    "GAMMA.DIST": (args) => new Function_GAMMADIST(args),
    "GAMMAINV": (args) => new Function_GAMMAINV(args),
    "GAMMA.INV": (args) => new Function_GAMMAINV(args),
    "GAMMALN": (args) => new Function_GAMMALN(args),
    "GAMMALN.PRECISE": (args) => new Function_GAMMALN(args),
    "HYPGEOMDIST": (args) => new Function_HYPGEOMDIST(args),
    "HYPGEOM.DIST": (args) => new Function_HYPGEOMDIST(args),
    "LOGINV": (args) => new Function_LOGINV(args),
    "LOGNORM.INV": (args) => new Function_LOGINV(args),
    "LOGNORMDIST": (args) => new Function_LOGNORMDIST(args),
    "LOGNORM.DIST": (args) => new Function_LOGNORMDIST(args),
    "NEGBINOMDIST": (args) => new Function_NEGBINOMDIST(args),
    "NEGBINOM.DIST": (args) => new Function_NEGBINOMDIST(args),
    "POISSON": (args) => new Function_POISSON(args),
    "POISSON.DIST": (args) => new Function_POISSON(args),
    "TDIST": (args) => new Function_TDIST(args),
    "T.DIST": (args) => new Function_TDIST(args),
    "TINV": (args) => new Function_TINV(args),
    "T.INV": (args) => new Function_TINV(args),
    "WEIBULL": (args) => new Function_WEIBULL(args),
    "URLENCODE": (args) => new Function_URLENCODE(args),
    "URLDECODE": (args) => new Function_URLDECODE(args),
    "HTMLENCODE": (args) => new Function_HTMLENCODE(args),
    "HTMLDECODE": (args) => new Function_HTMLDECODE(args),
    "BASE64TOTEXT": (args) => new Function_BASE64TOTEXT(args),
    "BASE64URLTOTEXT": (args) => new Function_BASE64URLTOTEXT(args),
    "TEXTTOBASE64": (args) => new Function_TEXTTOBASE64(args),
    "TEXTTOBASE64URL": (args) => new Function_TEXTTOBASE64URL(args),
    "REGEX": (args) => new Function_REGEX(args),
    "REGEXREPALCE": (args) => new Function_REGEXREPALCE(args),
    "ISREGEX": (args) => new Function_ISREGEX(args),
    "ISMATCH": (args) => new Function_ISREGEX(args),
    "MD5": (args) => new Function_MD5(args),
    "SHA1": (args) => new Function_SHA1(args),
    "SHA256": (args) => new Function_SHA256(args),
    "SHA512": (args) => new Function_SHA512(args),
    "HMACMD5": (args) => new Function_HMACMD5(args),
    "HMACSHA1": (args) => new Function_HMACSHA1(args),
    "HMACSHA256": (args) => new Function_HMACSHA256(args),
    "HMACSHA512": (args) => new Function_HMACSHA512(args),
    "TRIMSTART": (args) => new Function_TRIMSTART(args),
    "LTRIM": (args) => new Function_TRIMSTART(args),
    "TRIMEND": (args) => new Function_TRIMEND(args),
    "RTRIM": (args) => new Function_TRIMEND(args),
    "INDEXOF": (args) => new Function_INDEXOF(args),
    "LASTINDEXOF": (args) => new Function_LASTINDEXOF(args),
    "SPLIT": (args) => new Function_SPLIT(args),
    "JOIN": (args) => new Function_JOIN(args),
    "SUBSTRING": (args) => new Function_SUBSTRING(args),
    "STARTSWITH": (args) => new Function_STARTSWITH(args),
    "ENDSWITH": (args) => new Function_ENDSWITH(args),
    "ISNULLOREMPTY": (args) => new Function_ISNULLOREMPTY(args),
    "ISNULLORWHITESPACE": (args) => new Function_ISNULLORWHITESPACE(args),
    "REMOVESTART": (args) => new Function_REMOVESTART(args),
    "REMOVEEND": (args) => new Function_REMOVEEND(args),
    "JSON": (args) => new Function_JSON(args),
    "LOOKCEILING": (args) => new Function_LOOKCEILING(args),
    "LOOKFLOOR": (args) => new Function_LOOKFLOOR(args),
    "ARRAY": (args) => new Function_Array(args),
    "ADDYEARS": (args) => new Function_ADDYEARS(args),
    "ADDMONTHS": (args) => new Function_ADDMONTHS(args),
    "ADDDAYS": (args) => new Function_ADDDAYS(args),
    "ADDHOURS": (args) => new Function_ADDHOURS(args),
    "ADDMINUTES": (args) => new Function_ADDMINUTES(args),
    "ADDSECONDS": (args) => new Function_ADDSECONDS(args),
    "TIMESTAMP": (args) => new Function_TIMESTAMP(args),
    "HAS": (args) => new Function_HAS(args),
    "HASKEY": (args) => new Function_HAS(args),
    "CONTAINS": (args) => new Function_HAS(args),
    "CONTAINSKEY": (args) => new Function_HAS(args),
    "HASVALUE": (args) => new Function_HASVALUE(args),
    "CONTAINSVALUE": (args) => new Function_HASVALUE(args),
    "PARAM": (args) => new Function_PARAM(args),
    "PARAMETER": (args) => new Function_PARAM(args),
    "GETPARAMETER": (args) => new Function_PARAM(args),
    "ERROR": (args) => new Function_ERROR(args),
};

function findFunction(funcName) {
    return funcDict[funcName];
}

class MathFunctionVisitor {
    static getFuncDict() {
        return funcDict;
    }

    vN(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return args;
    }

    v1(context) {
        return context.expr().accept(this);
    }

    visitProg(context) {
        return this.v1(context);
    }

    visitMulDiv_fun(context) {
        let funcs = this.vN(context);
        let t = context.op.text;
        if (CharUtil.equals(t, '*')) {
            return new Function_Mul(funcs);
        } else if (CharUtil.equals(t, '/')) {
            return new Function_Div(funcs);
        }
        return new Function_Mod(funcs);
    }

    visitAddSub_fun(context) {
        let funcs = this.vN(context);
        let t = context.op.text;
        if (CharUtil.equals(t, '&')) {
            return new Function_Connect(funcs);
        } else if (CharUtil.equals(t, '+')) {
            return new Function_Add(funcs);
        }
        return new Function_Sub(funcs);
    }

    visitJudge_fun(context) {
        let funcs = this.vN(context);
        let Type = context.op.text;
        if (CharUtil.equals4(Type, "=", "==", "===")) {
            return new Function_EQ(funcs);
        } else if (CharUtil.equals(Type, "<")) {
            return new Function_LT(funcs);
        } else if (CharUtil.equals(Type, "<=")) {
            return new Function_LE(funcs);
        } else if (CharUtil.equals(Type, ">")) {
            return new Function_GT(funcs);
        } else if (CharUtil.equals(Type, ">=")) {
            return new Function_GE(funcs);
        }
        return new Function_NE(funcs);
    }

    visitAndOr_fun(context) {
        let funcs = this.vN(context);
        let t = context.op.text;
        if (CharUtil.equals(t, "&&")) {
            return new Function_AND(funcs);
        }
        return new Function_OR(funcs);
    }

    visitDiyFunction_fun(context) {
        let t = CharUtil.standardString(context.f.text);
        if (t === "E") {
            return new Function_Value(Operand.Create(Math.E), "E");
        } else if (t === "PI") {
            return new Function_Value(Operand.Create(Math.PI), "PI");
        } else if (t === "TRUE" || t === "YES" || t === "是" || t === "有") {
            return new Function_Value(Operand.True, t);
        } else if (t === "FALSE" || t === "NO" || t === "不是" || t === "否" || t === "没有" || t === "无") {
            return new Function_Value(Operand.False, t);
        } else if (t === "ALGORITHMVERSION" || t === "ENGINEVERSION") {
            return new Function_Value(Operand.Version, t);
        } else if (t === "RAND") {
            return new Function_RAND();
        } else if (t === "GUID") {
            return new Function_GUID();
        } else if (t === "NOW") {
            return new Function_NOW();
        } else if (t === "TODAY") {
            return new Function_TODAY();
        }

        let funcs = this.vN(context);
        let func = findFunction(t);
        if (func) {
            return func(funcs);
        }
        return new Function_DiyFunction(t, funcs);
    }

    visitPARAMETER_fun(context) {
        let node = context.PARAMETER();
        let t = CharUtil.standardString(node.text);
        if (t === "E") {
            return new Function_Value(Operand.Create(Math.E), "E");
        } else if (t === "PI") {
            return new Function_Value(Operand.Create(Math.PI), "PI");
        } else if (t === "TRUE" || t === "YES" || t === "是" || t === "有") {
            return new Function_Value(Operand.True, t);
        } else if (t === "FALSE" || t === "NO" || t === "不是" || t === "否" || t === "没有" || t === "无") {
            return new Function_Value(Operand.False, t);
        } else if (t === "ALGORITHMVERSION" || t === "ENGINEVERSION") {
            return new Function_Value(Operand.Version, t);
        }
        return new Function_PARAMETER(node.text);
    }

    visitPercentage_fun(context) {
        let args1 = this.v1(context);
        return new Function_Percentage(args1);
    }

    visitSTRING_fun(context) {
        let opd = context.getText();
        let sb = [];
        let index = 1;
        while (index < opd.length - 1) {
            let c = opd[index++];
            if (c === '\\') {
                let c2 = opd[index++];
                if (c2 === 'n') sb.push('\n');
                else if (c2 === 'r') sb.push('\r');
                else if (c2 === 't') sb.push('\t');
                else if (c2 === '0') sb.push('\0');
                else if (c2 === 'v') sb.push('\v');
                else if (c2 === 'a') sb.push('\a');
                else if (c2 === 'b') sb.push('\b');
                else if (c2 === 'f') sb.push('\f');
                else sb.push(opd[index++]);
            } else {
                sb.push(c);
            }
        }
        return new Function_Value(Operand.Create(sb.join('')));
    }

    visitArrayJson_fun(context) {
        let exprs = context.arrayJson();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_ArrayJson(args);
    }

    visitGetJsonValue_fun(context) {
        let funcs = this.vN(context);
        if (context.p != null) {
            let keyName = context.p.text.trim().replace(/^["'"\s\t\r\n\f]+|["'"\s\t\r\n\f]+$/g, '');
            let op = new Function_Value(Operand.Create(keyName), keyName);
            return new Function_GetJsonValue(funcs[0], op);
        }
        if (context.PARAMETER() != null) {
            let op = new Function_PARAMETER(context.PARAMETER().text);
            return new Function_GetJsonValue(funcs[0], op);
        }
        return new Function_GetJsonValue(funcs[0], funcs[1]);
    }

    visitArray_fun(context) {
        let funcs = this.vN(context);
        return new Function_Array(funcs);
    }

    visitNOT_fun(context) {
        let args1 = this.v1(context);
        return new Function_NOT(args1);
    }

    visitBracket_fun(context) {
        return this.v1(context);
    }

    visitNUM_fun(context) {
        let numText = context.num().text;
        let d = parseFloat(numText);
        if (context.unit == null) {
            return new Function_Value(Operand.Create(d), numText);
        }
        let unit = context.unit.text;
        return new Function_Num(d, unit);
    }

    visitNULL_fun(context) {
        return new Function_Value(Operand.CreateNull(), "NULL");
    }

    visitNum(context) {
        let d = parseFloat(context.getText());
        return new Function_Value(Operand.Create(d), context.getText());
    }

    visitArrayJson(context) {
        let keyName = null;
        if (context.key) {
            let stringContext = context.key.text.replace(/^["'"\s\t\r\n\f]+|["'"\s\t\r\n\f]+$/g, '');
            keyName = stringContext;
        }
        let f = this.v1(context);
        return new Function_ArrayJsonItem(keyName, f);
    }

    visitIF_fun(context) {
        let funcs = this.vN(context);
        return new Function_IF(funcs);
    }
}

export { MathFunctionVisitor, funcDict, findFunction };
