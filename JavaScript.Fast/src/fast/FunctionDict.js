import { Function_Mul } from '../Internals/Functions/Operator/Function_Mul.js';
import { Function_Div } from '../Internals/Functions/Operator/Function_Div.js';
import { Function_Mod } from '../Internals/Functions/Operator/Function_Mod.js';
import { Function_Add } from '../Internals/Functions/Operator/Function_Add.js';
import { Function_Sub } from '../Internals/Functions/Operator/Function_Sub.js';
import { Function_Connect } from '../Internals/Functions/Operator/Function_Connect.js';
import { Function_EQ } from '../Internals/Functions/Compare/Function_EQ.js';
import { Function_LT } from '../Internals/Functions/Compare/Function_LT.js';
import { Function_LE } from '../Internals/Functions/Compare/Function_LE.js';
import { Function_GT } from '../Internals/Functions/Compare/Function_GT.js';
import { Function_GE } from '../Internals/Functions/Compare/Function_GE.js';
import { Function_NE } from '../Internals/Functions/Compare/Function_NE.js';
import { Function_AND } from '../Internals/Functions/Operator/Function_AND.js';
import { Function_OR } from '../Internals/Functions/Operator/Function_OR.js';
import { Function_AND_N } from '../Internals/Functions/Operator/Function_AND_N.js';
import { Function_OR_N } from '../Internals/Functions/Operator/Function_OR_N.js';
import { Function_XOR } from '../Internals/Functions/Operator/Function_XOR.js';
import { Function_NOT } from '../Internals/Functions/Flow/Function_NOT.js';
import { Function_Value } from '../Internals/Functions/Value/Function_Value.js';

import { Function_VALUE } from '../Internals/Functions/String/Function_VALUE.js';
import { Function_ABS } from '../Internals/Functions/MathBase/Function_ABS.js';
import { Function_QUOTIENT } from '../Internals/Functions/MathBase/Function_QUOTIENT.js';
import { Function_SIGN } from '../Internals/Functions/MathBase/Function_SIGN.js';
import { Function_SQRT } from '../Internals/Functions/MathBase/Function_SQRT.js';
import { Function_TRUNC } from '../Internals/Functions/MathBase/Function_TRUNC.js';
import { Function_INT } from '../Internals/Functions/MathBase/Function_INT.js';
import { Function_GCD } from '../Internals/Functions/MathBase/Function_GCD.js';
import { Function_LCM } from '../Internals/Functions/MathBase/Function_LCM.js';
import { Function_COMBIN } from '../Internals/Functions/MathBase/Function_COMBIN.js';
import { Function_PERMUT } from '../Internals/Functions/MathBase/Function_PERMUT.js';
import { Function_Percentage } from '../Internals/Functions/MathBase/Function_Percentage.js';
import { Function_DEGREES } from '../Internals/Functions/MathTrigonometric/Function_DEGREES.js';
import { Function_RADIANS } from '../Internals/Functions/MathTrigonometric/Function_RADIANS.js';
import { Function_COS } from '../Internals/Functions/MathTrigonometric/Function_COS.js';
import { Function_COSH } from '../Internals/Functions/MathTrigonometric/Function_COSH.js';
import { Function_SIN } from '../Internals/Functions/MathTrigonometric/Function_SIN.js';
import { Function_SINH } from '../Internals/Functions/MathTrigonometric/Function_SINH.js';
import { Function_TAN } from '../Internals/Functions/MathTrigonometric/Function_TAN.js';
import { Function_TANH } from '../Internals/Functions/MathTrigonometric/Function_TANH.js';
import { Function_ACOS } from '../Internals/Functions/MathTrigonometric/Function_ACOS.js';
import { Function_ACOSH } from '../Internals/Functions/MathTrigonometric/Function_ACOSH.js';
import { Function_ASIN } from '../Internals/Functions/MathTrigonometric/Function_ASIN.js';
import { Function_ASINH } from '../Internals/Functions/MathTrigonometric/Function_ASINH.js';
import { Function_ATAN } from '../Internals/Functions/MathTrigonometric/Function_ATAN.js';
import { Function_ATANH } from '../Internals/Functions/MathTrigonometric/Function_ATANH.js';
import { Function_ATAN2 } from '../Internals/Functions/MathTrigonometric/Function_ATAN2.js';
import { Function_COT } from '../Internals/Functions/MathTrigonometric/Function_COT.js';
import { Function_COTH } from '../Internals/Functions/MathTrigonometric/Function_COTH.js';
import { Function_CSC } from '../Internals/Functions/MathTrigonometric/Function_CSC.js';
import { Function_CSCH } from '../Internals/Functions/MathTrigonometric/Function_CSCH.js';
import { Function_SEC } from '../Internals/Functions/MathTrigonometric/Function_SEC.js';
import { Function_SECH } from '../Internals/Functions/MathTrigonometric/Function_SECH.js';
import { Function_ACOT } from '../Internals/Functions/MathTrigonometric/Function_ACOT.js';
import { Function_ACOTH } from '../Internals/Functions/MathTrigonometric/Function_ACOTH.js';
import { Function_FIXED } from '../Internals/Functions/MathBase/Function_FIXED.js';
import { Function_BIN2OCT } from '../Internals/Functions/MathTransformation/Function_BIN2OCT.js';
import { Function_BIN2DEC } from '../Internals/Functions/MathTransformation/Function_BIN2DEC.js';
import { Function_BIN2HEX } from '../Internals/Functions/MathTransformation/Function_BIN2HEX.js';
import { Function_OCT2BIN } from '../Internals/Functions/MathTransformation/Function_OCT2BIN.js';
import { Function_OCT2DEC } from '../Internals/Functions/MathTransformation/Function_OCT2DEC.js';
import { Function_OCT2HEX } from '../Internals/Functions/MathTransformation/Function_OCT2HEX.js';
import { Function_DEC2BIN } from '../Internals/Functions/MathTransformation/Function_DEC2BIN.js';
import { Function_DEC2OCT } from '../Internals/Functions/MathTransformation/Function_DEC2OCT.js';
import { Function_DEC2HEX } from '../Internals/Functions/MathTransformation/Function_DEC2HEX.js';
import { Function_HEX2BIN } from '../Internals/Functions/MathTransformation/Function_HEX2BIN.js';
import { Function_HEX2OCT } from '../Internals/Functions/MathTransformation/Function_HEX2OCT.js';
import { Function_HEX2DEC } from '../Internals/Functions/MathTransformation/Function_HEX2DEC.js';
import { Function_ROUND } from '../Internals/Functions/MathBase/Function_ROUND.js';
import { Function_ROUNDDOWN } from '../Internals/Functions/MathBase/Function_ROUNDDOWN.js';
import { Function_ROUNDUP } from '../Internals/Functions/MathBase/Function_ROUNDUP.js';
import { Function_CEILING } from '../Internals/Functions/MathBase/Function_CEILING.js';
import { Function_FLOOR } from '../Internals/Functions/MathBase/Function_FLOOR.js';
import { Function_EVEN } from '../Internals/Functions/MathBase/Function_EVEN.js';
import { Function_ODD } from '../Internals/Functions/MathBase/Function_ODD.js';
import { Function_MROUND } from '../Internals/Functions/MathBase/Function_MROUND.js';
import { Function_RAND } from '../Internals/Functions/MathBase/Function_RAND.js';
import { Function_RANDBETWEEN } from '../Internals/Functions/MathBase/Function_RANDBETWEEN.js';
import { Function_COVARIANCES } from '../Internals/Functions/MathSum/Function_COVARIANCES.js';
import { Function_COVAR } from '../Internals/Functions/MathSum/Function_COVAR.js';
import { Function_FACT } from '../Internals/Functions/MathBase/Function_FACT.js';
import { Function_FACTDOUBLE } from '../Internals/Functions/MathBase/Function_FACTDOUBLE.js';
import { Function_POWER } from '../Internals/Functions/MathBase/Function_POWER.js';
import { Function_EXP } from '../Internals/Functions/MathBase/Function_EXP.js';
import { Function_LN } from '../Internals/Functions/MathBase/Function_LN.js';
import { Function_LOG } from '../Internals/Functions/MathBase/Function_LOG.js';
import { Function_LOG10 } from '../Internals/Functions/MathBase/Function_LOG10.js';
import { Function_MULTINOMIAL } from '../Internals/Functions/MathBase/Function_MULTINOMIAL.js';
import { Function_PRODUCT } from '../Internals/Functions/MathBase/Function_PRODUCT.js';
import { Function_SQRTPI } from '../Internals/Functions/MathBase/Function_SQRTPI.js';
import { Function_ERF } from '../Internals/Functions/MathBase/Function_ERF.js';
import { Function_ERFC } from '../Internals/Functions/MathBase/Function_ERFC.js';
import { Function_BESSELI } from '../Internals/Functions/MathBase/Function_BESSELI.js';
import { Function_BESSELJ } from '../Internals/Functions/MathBase/Function_BESSELJ.js';
import { Function_BESSELK } from '../Internals/Functions/MathBase/Function_BESSELK.js';
import { Function_BESSELY } from '../Internals/Functions/MathBase/Function_BESSELY.js';
import { Function_DELTA } from '../Internals/Functions/MathBase/Function_DELTA.js';
import { Function_GESTEP } from '../Internals/Functions/MathBase/Function_GESTEP.js';
import { Function_SUMSQ } from '../Internals/Functions/MathSum/Function_SUMSQ.js';
import { Function_SUMPRODUCT } from '../Internals/Functions/MathSum/Function_SUMPRODUCT.js';
import { Function_SUMX2MY2 } from '../Internals/Functions/MathSum/Function_SUMX2MY2.js';
import { Function_SUMX2PY2 } from '../Internals/Functions/MathSum/Function_SUMX2PY2.js';
import { Function_SUMXMY2 } from '../Internals/Functions/MathSum/Function_SUMXMY2.js';
import { Function_SERIESSUM } from '../Internals/Functions/MathSum/Function_SERIESSUM.js';
import { Function_ARABIC } from '../Internals/Functions/MathBase/Function_ARABIC.js';
import { Function_ROMAN } from '../Internals/Functions/MathBase/Function_ROMAN.js';
import { Function_RANK } from '../Internals/Functions/MathSum/Function_RANK.js';
import { Function_FORECAST } from '../Internals/Functions/MathSum/Function_FORECAST.js';
import { Function_INTERCEPT } from '../Internals/Functions/MathSum/Function_INTERCEPT.js';
import { Function_SLOPE } from '../Internals/Functions/MathSum/Function_SLOPE.js';
import { Function_CORREL } from '../Internals/Functions/MathSum/Function_CORREL.js';
import { Function_PEARSON } from '../Internals/Functions/MathSum/Function_PEARSON.js';
import { Function_YEARFRAC } from '../Internals/Functions/DateTimes/Function_YEARFRAC.js';
import { Function_PMT } from '../Internals/Functions/Financial/Function_PMT.js';
import { Function_PPMT } from '../Internals/Functions/Financial/Function_PPMT.js';
import { Function_IPMT } from '../Internals/Functions/Financial/Function_IPMT.js';
import { Function_PV } from '../Internals/Functions/Financial/Function_PV.js';
import { Function_FV } from '../Internals/Functions/Financial/Function_FV.js';
import { Function_NPER } from '../Internals/Functions/Financial/Function_NPER.js';
import { Function_RATE } from '../Internals/Functions/Financial/Function_RATE.js';
import { Function_NPV } from '../Internals/Functions/Financial/Function_NPV.js';
import { Function_XNPV } from '../Internals/Functions/Financial/Function_XNPV.js';
import { Function_IRR } from '../Internals/Functions/Financial/Function_IRR.js';
import { Function_MIRR } from '../Internals/Functions/Financial/Function_MIRR.js';
import { Function_XIRR } from '../Internals/Functions/Financial/Function_XIRR.js';
import { Function_SLN } from '../Internals/Functions/Financial/Function_SLN.js';
import { Function_SYD } from '../Internals/Functions/Financial/Function_SYD.js';
import { Function_DDB } from '../Internals/Functions/Financial/Function_DDB.js';
import { Function_DB } from '../Internals/Functions/Financial/Function_DB.js';
import { Function_ASC } from '../Internals/Functions/String/Function_ASC.js';
import { Function_JIS } from '../Internals/Functions/String/Function_JIS.js';
import { Function_CHAR } from '../Internals/Functions/String/Function_CHAR.js';
import { Function_CLEAN } from '../Internals/Functions/String/Function_CLEAN.js';
import { Function_CODE } from '../Internals/Functions/String/Function_CODE.js';
import { Function_CONCATENATE } from '../Internals/Functions/String/Function_CONCATENATE.js';
import { Function_EXACT } from '../Internals/Functions/String/Function_EXACT.js';
import { Function_FIND } from '../Internals/Functions/String/Function_FIND.js';
import { Function_LEFT } from '../Internals/Functions/String/Function_LEFT.js';
import { Function_LEN } from '../Internals/Functions/String/Function_LEN.js';
import { Function_LOWER } from '../Internals/Functions/String/Function_LOWER.js';
import { Function_MID } from '../Internals/Functions/String/Function_MID.js';
import { Function_PROPER } from '../Internals/Functions/String/Function_PROPER.js';
import { Function_REPLACE } from '../Internals/Functions/String/Function_REPLACE.js';
import { Function_REPT } from '../Internals/Functions/String/Function_REPT.js';
import { Function_RIGHT } from '../Internals/Functions/String/Function_RIGHT.js';
import { Function_RMB } from '../Internals/Functions/String/Function_RMB.js';
import { Function_SEARCH } from '../Internals/Functions/String/Function_SEARCH.js';
import { Function_SUBSTITUTE } from '../Internals/Functions/String/Function_SUBSTITUTE.js';
import { Function_T } from '../Internals/Functions/String/Function_T.js';
import { Function_TEXT } from '../Internals/Functions/String/Function_TEXT.js';
import { Function_TRIM } from '../Internals/Functions/String/Function_TRIM.js';
import { Function_UPPER } from '../Internals/Functions/String/Function_UPPER.js';
import { Function_UNICHAR } from '../Internals/Functions/String/Function_UNICHAR.js';
import { Function_UNICODE } from '../Internals/Functions/String/Function_UNICODE.js';
import { Function_DATEVALUE } from '../Internals/Functions/DateTimes/Function_DATEVALUE.js';
import { Function_TIMESTAMP } from '../Internals/Functions/DateTimes/Function_TIMESTAMP.js';
import { Function_TIMEVALUE } from '../Internals/Functions/DateTimes/Function_TIMEVALUE.js';
import { Function_DATE } from '../Internals/Functions/DateTimes/Function_DATE.js';
import { Function_TIME } from '../Internals/Functions/DateTimes/Function_TIME.js';
import { Function_NOW } from '../Internals/Functions/DateTimes/Function_NOW.js';
import { Function_TODAY } from '../Internals/Functions/DateTimes/Function_TODAY.js';
import { Function_YEAR } from '../Internals/Functions/DateTimes/Function_YEAR.js';
import { Function_MONTH } from '../Internals/Functions/DateTimes/Function_MONTH.js';
import { Function_DAY } from '../Internals/Functions/DateTimes/Function_DAY.js';
import { Function_HOUR } from '../Internals/Functions/DateTimes/Function_HOUR.js';
import { Function_MINUTE } from '../Internals/Functions/DateTimes/Function_MINUTE.js';
import { Function_SECOND } from '../Internals/Functions/DateTimes/Function_SECOND.js';
import { Function_WEEKDAY } from '../Internals/Functions/DateTimes/Function_WEEKDAY.js';
import { Function_DATEDIF } from '../Internals/Functions/DateTimes/Function_DATEDIF.js';
import { Function_DAYS360 } from '../Internals/Functions/DateTimes/Function_DAYS360.js';
import { Function_EDATE } from '../Internals/Functions/DateTimes/Function_EDATE.js';
import { Function_EOMONTH } from '../Internals/Functions/DateTimes/Function_EOMONTH.js';
import { Function_NETWORKDAYS } from '../Internals/Functions/DateTimes/Function_NETWORKDAYS.js';
import { Function_WORKDAY } from '../Internals/Functions/DateTimes/Function_WORKDAY.js';
import { Function_WEEKNUM } from '../Internals/Functions/DateTimes/Function_WEEKNUM.js';
import { Function_DAYS } from '../Internals/Functions/DateTimes/Function_DAYS.js';
import { Function_ADDMONTHS } from '../Internals/Functions/DateTimes/Function_ADDMONTHS.js';
import { Function_ADDYEARS } from '../Internals/Functions/DateTimes/Function_ADDYEARS.js';
import { Function_ADDSECONDS } from '../Internals/Functions/DateTimes/Function_ADDSECONDS.js';
import { Function_ADDMINUTES } from '../Internals/Functions/DateTimes/Function_ADDMINUTES.js';
import { Function_ADDDAYS } from '../Internals/Functions/DateTimes/Function_ADDDAYS.js';
import { Function_ADDHOURS } from '../Internals/Functions/DateTimes/Function_ADDHOURS.js';
import { Function_MAX } from '../Internals/Functions/MathSum/Function_MAX.js';
import { Function_MEDIAN } from '../Internals/Functions/MathSum/Function_MEDIAN.js';
import { Function_MIN } from '../Internals/Functions/MathSum/Function_MIN.js';
import { Function_QUARTILE } from '../Internals/Functions/MathSum/Function_QUARTILE.js';
import { Function_MODE } from '../Internals/Functions/MathSum/Function_MODE.js';
import { Function_LARGE } from '../Internals/Functions/MathSum/Function_LARGE.js';
import { Function_SMALL } from '../Internals/Functions/MathSum/Function_SMALL.js';
import { Function_PERCENTILE } from '../Internals/Functions/MathSum/Function_PERCENTILE.js';
import { Function_PERCENTRANK } from '../Internals/Functions/MathSum/Function_PERCENTRANK.js';
import { Function_AVERAGE } from '../Internals/Functions/MathSum/Function_AVERAGE.js';
import { Function_AVERAGEIF } from '../Internals/Functions/MathSum/Function_AVERAGEIF.js';
import { Function_GEOMEAN } from '../Internals/Functions/MathSum/Function_GEOMEAN.js';
import { Function_HARMEAN } from '../Internals/Functions/MathSum/Function_HARMEAN.js';
import { Function_COUNT } from '../Internals/Functions/MathSum/Function_COUNT.js';
import { Function_COUNTIF } from '../Internals/Functions/MathSum/Function_COUNTIF.js';
import { Function_SUM } from '../Internals/Functions/MathSum/Function_SUM.js';
import { Function_SUMIF } from '../Internals/Functions/MathSum/Function_SUMIF.js';
import { Function_AVEDEV } from '../Internals/Functions/MathSum/Function_AVEDEV.js';
import { Function_STDEV } from '../Internals/Functions/MathSum/Function_STDEV.js';
import { Function_STDEVP } from '../Internals/Functions/MathSum/Function_STDEVP.js';
import { Function_DEVSQ } from '../Internals/Functions/MathSum/Function_DEVSQ.js';
import { Function_VAR } from '../Internals/Functions/MathSum/Function_VAR.js';
import { Function_VARP } from '../Internals/Functions/MathSum/Function_VARP.js';
import { Function_NORMDIST } from '../Internals/Functions/MathSum/Function_NORMDIST.js';
import { Function_NORMINV } from '../Internals/Functions/MathSum/Function_NORMINV.js';
import { Function_NORMSDIST } from '../Internals/Functions/MathSum/Function_NORMSDIST.js';
import { Function_NORMSINV } from '../Internals/Functions/MathSum/Function_NORMSINV.js';
import { Function_BETADIST } from '../Internals/Functions/MathSum/Function_BETADIST.js';
import { Function_BETAINV } from '../Internals/Functions/MathSum/Function_BETAINV.js';
import { Function_BINOMDIST } from '../Internals/Functions/MathSum/Function_BINOMDIST.js';
import { Function_EXPONDIST } from '../Internals/Functions/MathSum/Function_EXPONDIST.js';
import { Function_FDIST } from '../Internals/Functions/MathSum/Function_FDIST.js';
import { Function_FINV } from '../Internals/Functions/MathSum/Function_FINV.js';
import { Function_FISHER } from '../Internals/Functions/MathSum/Function_FISHER.js';
import { Function_FISHERINV } from '../Internals/Functions/MathSum/Function_FISHERINV.js';
import { Function_GAMMADIST } from '../Internals/Functions/MathSum/Function_GAMMADIST.js';
import { Function_GAMMAINV } from '../Internals/Functions/MathSum/Function_GAMMAINV.js';
import { Function_GAMMALN } from '../Internals/Functions/MathSum/Function_GAMMALN.js';
import { Function_HYPGEOMDIST } from '../Internals/Functions/MathSum/Function_HYPGEOMDIST.js';
import { Function_LOGINV } from '../Internals/Functions/MathSum/Function_LOGINV.js';
import { Function_LOGNORMDIST } from '../Internals/Functions/MathSum/Function_LOGNORMDIST.js';
import { Function_NEGBINOMDIST } from '../Internals/Functions/MathSum/Function_NEGBINOMDIST.js';
import { Function_POISSON } from '../Internals/Functions/MathSum/Function_POISSON.js';
import { Function_TDIST } from '../Internals/Functions/MathSum/Function_TDIST.js';
import { Function_TINV } from '../Internals/Functions/MathSum/Function_TINV.js';
import { Function_WEIBULL } from '../Internals/Functions/MathSum/Function_WEIBULL.js';
import { Function_URLENCODE } from '../Internals/Functions/CsharpWeb/Function_URLENCODE.js';
import { Function_URLDECODE } from '../Internals/Functions/CsharpWeb/Function_URLDECODE.js';
import { Function_HTMLENCODE } from '../Internals/Functions/CsharpWeb/Function_HTMLENCODE.js';
import { Function_HTMLDECODE } from '../Internals/Functions/CsharpWeb/Function_HTMLDECODE.js';
import { Function_BASE64TOTEXT } from '../Internals/Functions/CsharpWeb/Function_BASE64TOTEXT.js';
import { Function_BASE64URLTOTEXT } from '../Internals/Functions/CsharpWeb/Function_BASE64URLTOTEXT.js';
import { Function_TEXTTOBASE64 } from '../Internals/Functions/CsharpWeb/Function_TEXTTOBASE64.js';
import { Function_TEXTTOBASE64URL } from '../Internals/Functions/CsharpWeb/Function_TEXTTOBASE64URL.js';
import { Function_REGEX } from '../Internals/Functions/Csharp/Function_REGEX.js';
import { Function_REGEXREPLACE } from '../Internals/Functions/Csharp/Function_REGEXREPLACE.js';
import { Function_ISREGEX } from '../Internals/Functions/Csharp/Function_ISREGEX.js';
import { Function_GUID } from '../Internals/Functions/Csharp/Function_GUID.js';
import { Function_MD5 } from '../Internals/Functions/CsharpSecurity/Function_MD5.js';
import { Function_SHA1 } from '../Internals/Functions/CsharpSecurity/Function_SHA1.js';
import { Function_SHA256 } from '../Internals/Functions/CsharpSecurity/Function_SHA256.js';
import { Function_SHA512 } from '../Internals/Functions/CsharpSecurity/Function_SHA512.js';
import { Function_HMACMD5 } from '../Internals/Functions/CsharpSecurity/Function_HMACMD5.js';
import { Function_HMACSHA1 } from '../Internals/Functions/CsharpSecurity/Function_HMACSHA1.js';
import { Function_HMACSHA256 } from '../Internals/Functions/CsharpSecurity/Function_HMACSHA256.js';
import { Function_HMACSHA512 } from '../Internals/Functions/CsharpSecurity/Function_HMACSHA512.js';
import { Function_STARTSWITH } from '../Internals/Functions/Csharp/Function_STARTSWITH.js';
import { Function_ENDSWITH } from '../Internals/Functions/Csharp/Function_ENDSWITH.js';
import { Function_INDEXOF } from '../Internals/Functions/Csharp/Function_INDEXOF.js';
import { Function_LASTINDEXOF } from '../Internals/Functions/Csharp/Function_LASTINDEXOF.js';
import { Function_SPLIT } from '../Internals/Functions/Csharp/Function_SPLIT.js';
import { Function_SUBSTRING } from '../Internals/Functions/Csharp/Function_SUBSTRING.js';
import { Function_TRIMSTART } from '../Internals/Functions/Csharp/Function_TRIMSTART.js';
import { Function_TRIMEND } from '../Internals/Functions/Csharp/Function_TRIMEND.js';
import { Function_REMOVESTART } from '../Internals/Functions/Csharp/Function_REMOVESTART.js';
import { Function_REMOVEEND } from '../Internals/Functions/Csharp/Function_REMOVEEND.js';
import { Function_JOIN } from '../Internals/Functions/Csharp/Function_JOIN.js';
import { Function_LOOKCEILING } from '../Internals/Functions/Csharp/Function_LOOKCEILING.js';
import { Function_LOOKFLOOR } from '../Internals/Functions/Csharp/Function_LOOKFLOOR.js';
import { Function_HAS } from '../Internals/Functions/Csharp/Function_HAS.js';
import { Function_HASVALUE } from '../Internals/Functions/Csharp/Function_HASVALUE.js';
import { Function_IF } from '../Internals/Functions/Flow/Function_IF.js';
import { Function_IFS } from '../Internals/Functions/Flow/Function_IFS.js';
import { Function_SWITCH } from '../Internals/Functions/Flow/Function_SWITCH.js';
import { Function_IFERROR } from '../Internals/Functions/Flow/Function_IFERROR.js';
import { Function_ISERROR } from '../Internals/Functions/Flow/Function_ISERROR.js';
import { Function_ISNULL } from '../Internals/Functions/Flow/Function_ISNULL.js';
import { Function_ISNULLORERROR } from '../Internals/Functions/Flow/Function_ISNULLORERROR.js';
import { Function_ISEVEN } from '../Internals/Functions/Flow/Function_ISEVEN.js';
import { Function_ISLOGICAL } from '../Internals/Functions/Flow/Function_ISLOGICAL.js';
import { Function_ISODD } from '../Internals/Functions/Flow/Function_ISODD.js';
import { Function_ISNONTEXT } from '../Internals/Functions/Flow/Function_ISNONTEXT.js';
import { Function_ISNULLOREMPTY } from '../Internals/Functions/Flow/Function_ISNULLOREMPTY.js';
import { Function_ISNULLORWHITESPACE } from '../Internals/Functions/Flow/Function_ISNULLORWHITESPACE.js';
import { Function_ISNUMBER } from '../Internals/Functions/Flow/Function_ISNUMBER.js';
import { Function_ISTEXT } from '../Internals/Functions/Flow/Function_ISTEXT.js';
import { Function_Array } from '../Internals/Functions/Value/Function_Array.js';
import { Function_JSON } from '../Internals/Functions/Value/Function_JSON.js';
import { Function_ArrayJson } from '../Internals/Functions/Value/Function_ArrayJson.js';
import { Function_ArrayJsonItem } from '../Internals/Functions/Value/Function_ArrayJsonItem.js';
import { Function_GetJsonValue } from '../Internals/Functions/Value/Function_GetJsonValue.js';
import { Function_NUM } from '../Internals/Functions/Value/Function_NUM.js';
import { Function_PARAM } from '../Internals/Functions/Value/Function_PARAM.js';
import { Function_PARAMETER } from '../Internals/Functions/Value/Function_PARAMETER.js';
import { Function_DiyFunction } from '../Internals/Functions/Value/Function_DiyFunction.js';
import { Function_ERROR } from '../Internals/Functions/Value/Function_ERROR.js';
import { CharUtil } from '../Internals/Visitors/CharUtil.js';
import { Operand } from '../Operand.js';

const funcDict = {
    "IF": (args) => new Function_IF(args),
    "IFS": (args) => new Function_IFS(args),
    "SWITCH": (args) => new Function_SWITCH(args),
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
    "AND": (args) => new Function_AND_N(args),
    "OR": (args) => new Function_OR_N(args),
    "XOR": (args) => new Function_XOR(args),
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
    "INT": (args) => new Function_INT(args),
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
    "COT": (args) => new Function_COT(args),
    "COTH": (args) => new Function_COTH(args),
    "CSC": (args) => new Function_CSC(args),
    "CSCH": (args) => new Function_CSCH(args),
    "SEC": (args) => new Function_SEC(args),
    "SECH": (args) => new Function_SECH(args),
    "ACOT": (args) => new Function_ACOT(args),
    "ACOTH": (args) => new Function_ACOTH(args),
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
    "ERF": (args) => new Function_ERF(args),
    "ERFC": (args) => new Function_ERFC(args),
    "BESSELI": (args) => new Function_BESSELI(args),
    "BESSELJ": (args) => new Function_BESSELJ(args),
    "BESSELK": (args) => new Function_BESSELK(args),
    "BESSELY": (args) => new Function_BESSELY(args),
    "DELTA": (args) => new Function_DELTA(args),
    "GESTEP": (args) => new Function_GESTEP(args),
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
    "UNICHAR": (args) => new Function_UNICHAR(args),
    "UNICODE": (args) => new Function_UNICODE(args),
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
    "DAYS": (args) => new Function_DAYS(args),
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
    "REGEXREPLACE": (args) => new Function_REGEXREPLACE(args),
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
    "SUMPRODUCT": (args) => new Function_SUMPRODUCT(args),
    "SUMX2MY2": (args) => new Function_SUMX2MY2(args),
    "SUMX2PY2": (args) => new Function_SUMX2PY2(args),
    "SUMXMY2": (args) => new Function_SUMXMY2(args),
    "SERIESSUM": (args) => new Function_SERIESSUM(args),
    "ARABIC": (args) => new Function_ARABIC(args),
    "ROMAN": (args) => new Function_ROMAN(args),
    "RANK": (args) => new Function_RANK(args),
    "FORECAST": (args) => new Function_FORECAST(args),
    "INTERCEPT": (args) => new Function_INTERCEPT(args),
    "SLOPE": (args) => new Function_SLOPE(args),
    "CORREL": (args) => new Function_CORREL(args),
    "PEARSON": (args) => new Function_PEARSON(args),
    "YEARFRAC": (args) => new Function_YEARFRAC(args),
    "PMT": (args) => new Function_PMT(args),
    "PPMT": (args) => new Function_PPMT(args),
    "IPMT": (args) => new Function_IPMT(args),
    "PV": (args) => new Function_PV(args),
    "FV": (args) => new Function_FV(args),
    "NPER": (args) => new Function_NPER(args),
    "RATE": (args) => new Function_RATE(args),
    "NPV": (args) => new Function_NPV(args),
    "XNPV": (args) => new Function_XNPV(args),
    "IRR": (args) => new Function_IRR(args),
    "MIRR": (args) => new Function_MIRR(args),
    "XIRR": (args) => new Function_XIRR(args),
    "SLN": (args) => new Function_SLN(args),
    "SYD": (args) => new Function_SYD(args),
    "DDB": (args) => new Function_DDB(args),
    "DB": (args) => new Function_DB(args),
};

function findFunction(funcName) {
    return funcDict[funcName];
}

export { funcDict, findFunction };
