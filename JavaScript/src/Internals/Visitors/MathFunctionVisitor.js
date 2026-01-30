/**
 * MathFunctionvisitor - 数学函数访问者
 */
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
import { Function_CRC32 } from '../Functions/CsharpSecurity/Function_CRC32.js';
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

class MathFunctionVisitor extends mathVisitor  {
    /**
     * 访问程序节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitProg(context) {
        return context.expr().accept(this);
    }

    /**
     * 访问乘除函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMulDiv_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let t = context.op.text;
        if (CharUtil.Equals(t, '*')) {
            return new Function_Mul(args1, args2);
        } else if (CharUtil.Equals(t, '/')) {
            return new Function_Div(args1, args2);
        }
        return new Function_Mod(args1, args2);
    }

    /**
     * 访问加减函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAddSub_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let t = context.op.text;
        if (CharUtil.Equals(t, '&')) {
            return new Function_Connect(args1, args2);
        } else if (CharUtil.Equals(t, '+')) {
            return new Function_Add(args1, args2);
        }
        return new Function_Sub(args1, args2);
    }

    /**
     * 访问判断函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitJudge_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);

        let Type = context.op.text;
        if (CharUtil.Equals4(Type, "=", "==", "===")) {
            return new Function_EQ(args1, args2);
        } else if (CharUtil.Equals(Type, "<")) {
            return new Function_LT(args1, args2);
        } else if (CharUtil.Equals(Type, "<=")) {
            return new Function_LE(args1, args2);
        } else if (CharUtil.Equals(Type, ">")) {
            return new Function_GT(args1, args2);
        } else if (CharUtil.Equals(Type, ">=")) {
            return new Function_GE(args1, args2);
        }
        return new Function_NE(args1, args2);
    }

    /**
     * 访问与或函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAndOr_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let t = context.op.text;
        if (CharUtil.Equals3(t, "&&", "AND")) {
            return new Function_AND(args1, args2);
        }
        return new Function_OR(args1, args2);
    }

    /**
     * 访问IF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitIF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 3) {
            let args3 = exprs[2].accept(this);
            return new Function_IF(args1, args2, args3);
        }
        return new Function_IF(args1, args2, null);
    }

    /**
     * 访问IFERROR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitIFERROR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_IFERROR(args1, args2, args3);
    }

    /**
     * 访问ISNUMBER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNUMBER_fun(context) {
        let args1 = this.visit(context.expr());
        return new Function_ISNUMBER(args1);
    }

    /**
     * 访问ISTEXT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISTEXT_fun(context) {
        let args1 = this.visit(context.expr());
        return new Function_ISTEXT(args1);
    }

    /**
     * 访问ISERROR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISERROR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 2) {
            let args2 = exprs[1].accept(this);
            return new Function_ISERROR(args1, args2);
        }
        return new Function_ISERROR(args1, null);
    }

    /**
     * 访问ISNULL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNULL_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 2) {
            let args2 = exprs[1].accept(this);
            return new Function_ISNULL(args1, args2);
        }
        return new Function_ISNULL(args1, null);
    }

    /**
     * 访问ISNULLORERROR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNULLORERROR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 2) {
            let args2 = exprs[1].accept(this);
            return new Function_ISNULLORERROR(args1, args2);
        }
        return new Function_ISNULLORERROR(args1, null);
    }

    /**
     * 访问ISEVEN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISEVEN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISEVEN(args1);
    }

    /**
     * 访问ISLOGICAL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISLOGICAL_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISLOGICAL(args1);
    }

    /**
     * 访问ISODD函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISODD_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISODD(args1);
    }

    /**
     * 访问ISNONTEXT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNONTEXT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISNONTEXT(args1);
    }

    /**
     * 访问AND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAND_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_AND_N(args);
    }

    /**
     * 访问OR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitOR_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_OR_N(args);
    }

    /**
     * 访问NOT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNOT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_NOT(args1);
    }

    /**
     * 访问TRUE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTRUE_fun(context) {
        return new Function_Value(Operand.True);
    }

    /**
     * 访问FALSE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFALSE_fun(context) {
        return new Function_Value(Operand.False);
    }

    /**
     * 访问E函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitE_fun(context) {
        return new Function_Value(Operand.Create(Math.E), "E");
    }

    /**
     * 访问PI函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPI_fun(context) {
        return new Function_Value(Operand.Create(Math.PI), "PI");
    }

    /**
     * 访问ABS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitABS_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ABS(args1);
    }

    /**
     * 访问QUOTIENT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitQUOTIENT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_QUOTIENT(args1, args2);
    }

    /**
     * 访问MOD函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMOD_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_Mod(args1, args2);
    }

    /**
     * 访问SIGN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSIGN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SIGN(args1);
    }

    /**
     * 访问SQRT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSQRT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SQRT(args1);
    }

    /**
     * 访问TRUNC函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTRUNC_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TRUNC(args1);
    }

    /**
     * 访问INT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitINT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TRUNC(args1);
    }

    /**
     * 访问GCD函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGCD_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_GCD(args);
    }

    /**
     * 访问LCM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLCM_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_LCM(args);
    }

    /**
     * 访问COMBIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOMBIN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_COMBIN(args1, args2);
    }

    /**
     * 访问PERMUT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPERMUT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_PERMUT(args1, args2);
    }

    /**
     * 访问Percentage函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPercentage_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_Percentage(args1);
    }

    /**
     * 访问DEGREES函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDEGREES_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_DEGREES(args1);
    }

    /**
     * 访问RADIANS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitRADIANS_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_RADIANS(args1);
    }

    /**
     * 访问COS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOS_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_COS(args1);
    }

    /**
     * 访问COSH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOSH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_COSH(args1);
    }

    /**
     * 访问SIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSIN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SIN(args1);
    }

    /**
     * 访问SINH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSINH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SINH(args1);
    }

    /**
     * 访问TAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTAN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TAN(args1);
    }

    /**
     * 访问TANH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTANH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TANH(args1);
    }

    /**
     * 访问ACOS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitACOS_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ACOS(args1);
    }

    /**
     * 访问ACOSH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitACOSH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ACOSH(args1);
    }

    /**
     * 访问ASIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitASIN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ASIN(args1);
    }

    /**
     * 访问ASINH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitASINH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ASINH(args1);
    }

    /**
     * 访问ATAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitATAN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ATAN(args1);
    }

    /**
     * 访问ATANH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitATANH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ATANH(args1);
    }

    /**
     * 访问ATAN2函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitATAN2_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ATAN2(args1, args2);
    }

    /**
     * 访问FIXED函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFIXED_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_FIXED(args1, null, null);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_FIXED(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_FIXED(args1, args2, args3);
    }

    /**
     * 访问BIN2OCT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBIN2OCT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_BIN2OCT(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_BIN2OCT(args1, args2);
    }

    /**
     * 访问BIN2DEC函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBIN2DEC_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_BIN2DEC(args1);
    }

    /**
     * 访问BIN2HEX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBIN2HEX_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_BIN2HEX(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_BIN2HEX(args1, args2);
    }

    /**
     * 访问OCT2BIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitOCT2BIN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_OCT2BIN(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_OCT2BIN(args1, args2);
    }

    /**
     * 访问OCT2DEC函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitOCT2DEC_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_OCT2DEC(args1);
    }

    /**
     * 访问OCT2HEX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitOCT2HEX_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_OCT2HEX(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_OCT2HEX(args1, args2);
    }

    /**
     * 访问DEC2BIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDEC2BIN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_DEC2BIN(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_DEC2BIN(args1, args2);
    }

    /**
     * 访问DEC2OCT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDEC2OCT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_DEC2OCT(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_DEC2OCT(args1, args2);
    }

    /**
     * 访问DEC2HEX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDEC2HEX_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_DEC2HEX(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_DEC2HEX(args1, args2);
    }

    /**
     * 访问HEX2BIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHEX2BIN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_HEX2BIN(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_HEX2BIN(args1, args2);
    }

    /**
     * 访问HEX2OCT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHEX2OCT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_HEX2OCT(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_HEX2OCT(args1, args2);
    }

    /**
     * 访问HEX2DEC函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHEX2DEC_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_HEX2DEC(args1);
    }

    /**
     * 访问ROUND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitROUND_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) {
            return new Function_ROUND(args1, null);
        }
        let args2 = exprs[1].accept(this);
        return new Function_ROUND(args1, args2);
    }

    /**
     * 访问ROUNDDOWN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitROUNDDOWN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ROUNDDOWN(args1, args2);
    }

    /**
     * 访问ROUNDUP函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitROUNDUP_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ROUNDUP(args1, args2);
    }

    /**
     * 访问CEILING函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCEILING_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1)
            return new Function_CEILING(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_CEILING(args1, args2);
    }

    /**
     * 访问FLOOR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFLOOR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1)
            return new Function_FLOOR(args1, null);

        let args2 = exprs[1].accept(this);
        return new Function_FLOOR(args1, args2);
    }

    /**
     * 访问EVEN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEVEN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_EVEN(args1);
    }

    /**
     * 访问ODD函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitODD_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ODD(args1);
    }

    /**
     * 访问MROUND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMROUND_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_MROUND(args1, args2);
    }

    /**
     * 访问RAND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitRAND_fun(context) {
        return new Function_RAND();
    }

    /**
     * 访问RANDBETWEEN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitRANDBETWEEN_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_RANDBETWEEN(args1, args2);
    }

    /**
     * 访问COVARIANCES函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOVARIANCES_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_COVARIANCES(args1, args2);
    }

    /**
     * 访问COVAR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOVAR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_COVAR(args1, args2);
    }

    /**
     * 访问FACT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFACT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_FACT(args1);
    }

    /**
     * 访问FACTDOUBLE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFACTDOUBLE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_FACTDOUBLE(args1);
    }

    /**
     * 访问POWER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPOWER_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_POWER(args1, args2);
    }

    /**
     * 访问EXP函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEXP_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_EXP(args1);
    }

    /**
     * 访问LN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_LN(args1);
    }

    /**
     * 访问LOG函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOG_fun(context) {
        let exprs = context.expr();

        let args1 = exprs[0].accept(this);
        if (exprs.length > 1) {
            let args2 = exprs[1].accept(this);
            return new Function_LOG(args1, args2);
        }
        return new Function_LOG(args1, null);
    }

    /**
     * 访问LOG10函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOG10_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_LOG(args1, null);
    }

    /**
     * 访问MULTINOMIAL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMULTINOMIAL_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_MULTINOMIAL(args);
    }

    /**
     * 访问PRODUCT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPRODUCT_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_PRODUCT(args);
    }

    /**
     * 访问SQRTPI函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSQRTPI_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SQRTPI(args1);
    }

    /**
     * 访问SUMSQ函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSUMSQ_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_SUMSQ(args);
    }

    /**
     * 访问ASC函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitASC_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ASC(args1);
    }

    /**
     * 访问JIS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitJIS_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_JIS(args1);
    }

    /**
     * 访问CHAR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCHAR_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_CHAR(args1);
    }

    /**
     * 访问CLEAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCLEAN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_CLEAN(args1);
    }

    /**
     * 访问CODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCODE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_CODE(args1);
    }

    /**
     * 访问CONCATENATE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCONCATENATE_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_CONCATENATE(args);
    }

    /**
     * 访问EXACT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEXACT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_EXACT(args1, args2);
    }

    /**
     * 访问FIND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFIND_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);

        if (exprs.length === 2) {
            return new Function_FIND(args1, args2, null);
        }
        let count = exprs[2].accept(this);
        return new Function_FIND(args1, args2, count);
    }

    /**
     * 访问LEFT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLEFT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) {
            return new Function_LEFT(args1, null);
        }
        return new Function_LEFT(args1, exprs[1].accept(this));
    }

    /**
     * 访问LEN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLEN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_LEN(args1);
    }

    /**
     * 访问LOWER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOWER_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_LOWER(args1);
    }

    /**
     * 访问MID函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMID_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_MID(args1, args2, args3);
    }

    /**
     * 访问PROPER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPROPER_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_PROPER(args1);
    }

    /**
     * 访问REPLACE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREPLACE_fun(context) {
        let exprs = context.expr();

        let args1 = exprs[0].accept(this);
        if (exprs.length === 3) {
            let args22 = exprs[1].accept(this);
            let args32 = exprs[2].accept(this);
            return new Function_REPLACE(args1, args22, args32, null);
        }
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_REPLACE(args1, args2, args3, args4);
    }

    /**
     * 访问REPT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREPT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_REPT(args1, args2);
    }

    /**
     * 访问RIGHT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitRIGHT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);

        if (exprs.length === 1) {
            return new Function_RIGHT(args1);
        }
        let args2 = exprs[1].accept(this);
        return new Function_RIGHT(args1, args2);
    }

    /**
     * 访问RMB函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitRMB_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_RMB(args1);
    }

    /**
     * 访问SEARCH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSEARCH_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);

        if (exprs.length === 2) {
            return new Function_SEARCH(args1, args2, null);
        }
        let args3 = exprs[2].accept(this);
        return new Function_SEARCH(args1, args2, args3);
    }

    /**
     * 访问SUBSTITUTE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSUBSTITUTE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        if (exprs.length === 3) {
            return new Function_SUBSTITUTE(args1, args2, args3, null);
        }
        let args4 = exprs[3].accept(this);
        return new Function_SUBSTITUTE(args1, args2, args3, args4);
    }

    /**
     * 访问T函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitT_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_T(args1);
    }

    /**
     * 访问TEXT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTEXT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_TEXT(args1, args2);
    }

    /**
     * 访问TRIM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTRIM_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TRIM(args1);
    }

    /**
     * 访问UPPER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitUPPER_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_UPPER(args1);
    }

    /**
     * 访问VALUE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitVALUE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_VALUE(args1);
    }

    /**
     * 访问DATEVALUE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDATEVALUE_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_DATEVALUE(args);
    }

    /**
     * 访问TIMESTAMP函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTIMESTAMP_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_TIMESTAMP(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_TIMESTAMP(args1, args2);
    }

    /**
     * 访问TIMEVALUE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTIMEVALUE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_TIMEVALUE(args1);
    }

    /**
     * 访问DATE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDATE_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_DATE(args);
    }

    /**
     * 访问TIME函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTIME_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_TIME(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_TIME(args1, args2, args3);
    }

    /**
     * 访问NOW函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNOW_fun(context) {
        return new Function_NOW();
    }

    /**
     * 访问TODAY函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTODAY_fun(context) {
        return new Function_TODAY();
    }

    /**
     * 访问YEAR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitYEAR_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_YEAR(args1);
    }

    /**
     * 访问MONTH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMONTH_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_MONTH(args1);
    }

    /**
     * 访问DAY函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDAY_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_DAY(args1);
    }

    /**
     * 访问HOUR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHOUR_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_HOUR(args1);
    }

    /**
     * 访问MINUTE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMINUTE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_MINUTE(args1);
    }

    /**
     * 访问SECOND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSECOND_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_SECOND(args1);
    }

    /**
     * 访问WEEKDAY函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitWEEKDAY_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_WEEKDAY(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_WEEKDAY(args1, args2);
    }

    /**
     * 访问DATEDIF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDATEDIF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_DATEDIF(args1, args2, args3);
    }

    /**
     * 访问DAYS360函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDAYS360_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 3) {
            let args3 = exprs[2].accept(this);
            return new Function_DAYS360(args1, args2, args3);
        }
        return new Function_DAYS360(args1, args2, null);
    }

    /**
     * 访问EDATE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEDATE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_EDATE(args1, args2);
    }

    /**
     * 访问EOMONTH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEOMONTH_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_EOMONTH(args1, args2);
    }

    /**
     * 访问NETWORKDAYS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNETWORKDAYS_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_NETWORKDAYS(args);
    }

    /**
     * 访问WORKDAY函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitWORKDAY_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_WORKDAY(args);
    }

    /**
     * 访问WEEKNUM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitWEEKNUM_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_WEEKNUM(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_WEEKNUM(args1, args2);
    }

    /**
     * 访问ADDMONTHS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDMONTHS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDMONTHS(args1, args2);
    }

    /**
     * 访问ADDYEARS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDYEARS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDYEARS(args1, args2);
    }

    /**
     * 访问ADDSECONDS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDSECONDS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDSECONDS(args1, args2);
    }

    /**
     * 访问ADDMINUTES函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDMINUTES_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDMINUTES(args1, args2);
    }

    /**
     * 访问ADDDAYS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDDAYS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDDAYS(args1, args2);
    }

    /**
     * 访问ADDHOURS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitADDHOURS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ADDHOURS(args1, args2);
    }

    /**
     * 访问MAX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMAX_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_MAX(args);
    }

    /**
     * 访问MEDIAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMEDIAN_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_MEDIAN(args);
    }

    /**
     * 访问MIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMIN_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_MIN(args);
    }

    /**
     * 访问HMACSHA1函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHMACSHA1_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_HMACSHA1(args1, args2);
    }

    /**
     * 访问HMACSHA256函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHMACSHA256_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_HMACSHA256(args1, args2);
    }

    /**
     * 访问HMACSHA512函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHMACSHA512_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_HMACSHA512(args1, args2);
    }

    /**
     * 访问STARTSWITH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSTARTSWITH_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_STARTSWITH(args1, args2);
    }

    /**
     * 访问ENDSWITH函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitENDSWITH_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ENDSWITH(args1, args2);
    }

    /**
     * 访问INDEXOF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitINDEXOF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_INDEXOF(args1, args2);
    }

    /**
     * 访问LASTINDEXOF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLASTINDEXOF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_LASTINDEXOF(args1, args2);
    }

    /**
     * 访问SPLIT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSPLIT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_SPLIT(args1, args2);
    }

    /**
     * 访问SUBSTRING函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSUBSTRING_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_SUBSTRING(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_SUBSTRING(args1, args2, args3);
    }

    /**
     * 访问TRIMSTART函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTRIMSTART_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_TRIMSTART(args1);
        let args2 = exprs[1].accept(this);
        return new Function_TRIMSTART(args1, args2);
    }

    /**
     * 访问TRIMEND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTRIMEND_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_TRIMEND(args1);
        let args2 = exprs[1].accept(this);
        return new Function_TRIMEND(args1, args2);
    }

    /**
     * 访问REMOVESTART函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREMOVESTART_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_REMOVESTART(args1, args2);
    }

    /**
     * 访问REMOVEEND函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREMOVEEND_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_REMOVEEND(args1, args2);
    }

    /**
     * 访问JOIN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitJOIN_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_JOIN(args);
    }

    /**
     * 访问LOOKCEILING函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOOKCEILING_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_LOOKCEILING(args1, args2);
    }

    /**
     * 访问LOOKFLOOR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOOKFLOOR_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_LOOKFLOOR(args1, args2);
    }

    /**
     * 访问HAS函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHAS_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_HAS(args1, args2);
    }

    /**
     * 访问HASVALUE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHASVALUE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_HASVALUE(args1);
    }

    /**
     * 访问ISNULLOREMPTY函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNULLOREMPTY_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISNULLOREMPTY(args1);
    }

    /**
     * 访问ISNULLORWHITESPACE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISNULLORWHITESPACE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ISNULLORWHITESPACE(args1);
    }

    /**
     * 访问Array函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitArray_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_Array(args);
    }

    /**
     * 访问JSON函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitJSON_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_JSON(args1);
    }

    /**
     * 访问ArrayJson函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitArrayJson_fun(context) {
        let exprs = context.arrayJson();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_ArrayJson(args);
    }
    visitArrayJson(context) {
        let keyName = null;
        if (context.NUM) {
            let numContext = context.NUM();
            if (numContext) {
                let numText = numContext.getText ? numContext.getText() : numContext.text;
                let key = parseInt(numText);
                if (!isNaN(key)) {
                    keyName = key.toString();
                } else {
                    return new Function_Value(Operand.Error(`Json key '${numText}' is error!`));
                }
            }
        }
        if (context.STRING) {
            let stringContext = context.STRING();
            if (stringContext) {
                let opd = stringContext.getText ? stringContext.getText() : stringContext.text;
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
                keyName = sb.join('');
            }
        }
        if (context.parameter2) {
            let paramContext = context.parameter2();
            if (paramContext) {
                keyName = paramContext.getText ? paramContext.getText() : paramContext.text;
            }
        }
        let f = context.expr().accept(this);
        return new Function_ArrayJsonItem(keyName, f);
    }

    /**
     * 访问ArrayJsonItem函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitArrayJsonItem_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ArrayJsonItem(args1, args2);
    }

    /**
     * 访问GetJsonValue函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGetJsonValue_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_GetJsonValue(args1, args2);
    }

    /**
     * 访问NUM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNUM_fun(context) {
        let numText = context.num().getText ? context.num().getText() : context.num().text;
        let d = parseFloat(numText);
		if (!context.unit()) { return new Function_Value(Operand.Create(d), numText); }
		let unitText = context.unit().getText ? context.unit().getText() : context.unit().text;
		return new Function_NUM(d, unitText);
    }

    /**
     * 访问PARAM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPARAM_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_PARAM(args1);
    }

    /**
     * 访问PARAMETER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPARAMETER_fun(context) {
        let node = context.PARAMETER();
        return new Function_PARAMETER(node.getText());
    }

    /**
     * 访问NULL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNULL_fun(context) {
        return new Function_Value(Operand.CreateNull(), "NULL");
    }

    /**
     * 访问DiyFunction函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDiyFunction_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_DiyFunction(args1, args2);
    }

    /**
     * 访问ERROR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitERROR_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_ERROR(args1);
    }

    /**
     * 访问QUARTILE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitQUARTILE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_QUARTILE(args1, args2);
    }

    /**
     * 访问MODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMODE_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_MODE(args);
    }

    /**
     * 访问LARGE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLARGE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_LARGE(args1, args2);
    }

    /**
     * 访问SMALL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSMALL_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_SMALL(args1, args2);
    }

    /**
     * 访问PERCENTILE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPERCENTILE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_PERCENTILE(args1, args2);
    }

    /**
     * 访问PERCENTRANK函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPERCENTRANK_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_PERCENTRANK(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_PERCENTRANK(args1, args2, args3);
    }

    /**
     * 访问AVERAGE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAVERAGE_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_AVERAGE(args);
    }

    /**
     * 访问AVERAGEIF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAVERAGEIF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_AVERAGEIF(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_AVERAGEIF(args1, args2, args3);
    }

    /**
     * 访问GEOMEAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGEOMEAN_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_GEOMEAN(args);
    }

    /**
     * 访问HARMEAN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHARMEAN_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_HARMEAN(args);
    }

    /**
     * 访问COUNT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOUNT_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_COUNT(args);
    }

    /**
     * 访问COUNTIF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCOUNTIF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_COUNTIF(args1, args2);
    }

    /**
     * 访问SUM函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSUM_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_SUM(args);
    }

    /**
     * 访问SUMIF函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSUMIF_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_SUMIF(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_SUMIF(args1, args2, args3);
    }

    /**
     * 访问AVEDEV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitAVEDEV_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_AVEDEV(args);
    }

    /**
     * 访问STDEV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSTDEV_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_STDEV(args);
    }

    /**
     * 访问STDEVP函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSTDEVP_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_STDEVP(args);
    }

    /**
     * 访问DEVSQ函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitDEVSQ_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_DEVSQ(args);
    }

    /**
     * 访问VAR函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitVAR_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_VAR(args);
    }

    /**
     * 访问VARP函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitVARP_fun(context) {
        let exprs = context.expr();
        let args = new Array(exprs.length);
        for (let i = 0; i < exprs.length; i++) {
            args[i] = exprs[i].accept(this);
        }
        return new Function_VARP(args);
    }

    /**
     * 访问NORMDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNORMDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_NORMDIST(args1, args2, args3, args4);
    }

    /**
     * 访问NORMINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNORMINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_NORMINV(args1, args2, args3);
    }

    /**
     * 访问NORMSDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNORMSDIST_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_NORMSDIST(args1);
    }

    /**
     * 访问NORMSINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNORMSINV_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_NORMSINV(args1);
    }

    /**
     * 访问BETADIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBETADIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_BETADIST(args1, args2, args3);
    }

    /**
     * 访问BETAINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBETAINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_BETAINV(args1, args2, args3);
    }

    /**
     * 访问BINOMDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBINOMDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_BINOMDIST(args1, args2, args3, args4);
    }

    /**
     * 访问EXPONDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitEXPONDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_EXPONDIST(args1, args2, args3);
    }

    /**
     * 访问FDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_FDIST(args1, args2, args3);
    }

    /**
     * 访问FINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_FINV(args1, args2, args3);
    }

    /**
     * 访问FISHER函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFISHER_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_FISHER(args1);
    }

    /**
     * 访问FISHERINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitFISHERINV_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_FISHERINV(args1);
    }

    /**
     * 访问GAMMADIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGAMMADIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_GAMMADIST(args1, args2, args3, args4);
    }

    /**
     * 访问GAMMAINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGAMMAINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_GAMMAINV(args1, args2, args3);
    }

    /**
     * 访问GAMMALN函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGAMMALN_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_GAMMALN(args1);
    }

    /**
     * 访问HYPGEOMDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHYPGEOMDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_HYPGEOMDIST(args1, args2, args3, args4);
    }

    /**
     * 访问LOGINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOGINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_LOGINV(args1, args2, args3);
    }

    /**
     * 访问LOGNORMDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitLOGNORMDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_LOGNORMDIST(args1, args2, args3);
    }

    /**
     * 访问NEGBINOMDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitNEGBINOMDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_NEGBINOMDIST(args1, args2, args3);
    }

    /**
     * 访问POISSON函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitPOISSON_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_POISSON(args1, args2, args3);
    }

    /**
     * 访问TDIST函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTDIST_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_TDIST(args1, args2, args3);
    }

    /**
     * 访问TINV函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTINV_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_TINV(args1, args2);
    }

    /**
     * 访问WEIBULL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitWEIBULL_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        let args4 = exprs[3].accept(this);
        return new Function_WEIBULL(args1, args2, args3, args4);
    }

    /**
     * 访问URLENCODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitURLENCODE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_URLENCODE(args1);
    }

    /**
     * 访问URLDECODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitURLDECODE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_URLDECODE(args1);
    }

    /**
     * 访问HTMLENCODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHTMLENCODE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_HTMLENCODE(args1);
    }

    /**
     * 访问HTMLDECODE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHTMLDECODE_fun(context) {
        let args1 = context.expr().accept(this);
        return new Function_HTMLDECODE(args1);
    }

    /**
     * 访问BASE64TOTEXT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBASE64TOTEXT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_BASE64TOTEXT(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_BASE64TOTEXT(args1, args2);
    }

    /**
     * 访问BASE64URLTOTEXT函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitBASE64URLTOTEXT_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_BASE64URLTOTEXT(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_BASE64URLTOTEXT(args1, args2);
    }

    /**
     * 访问TEXTTOBASE64函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTEXTTOBASE64_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_TEXTTOBASE64(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_TEXTTOBASE64(args1, args2);
    }

    /**
     * 访问TEXTTOBASE64URL函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitTEXTTOBASE64URL_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_TEXTTOBASE64URL(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_TEXTTOBASE64URL(args1, args2);
    }

    /**
     * 访问REGEX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREGEX_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_REGEX(args1, args2);
    }

    /**
     * 访问REGEXREPALCE函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitREGEXREPALCE_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        let args3 = exprs[2].accept(this);
        return new Function_REGEXREPALCE(args1, args2, args3);
    }

    /**
     * 访问ISREGEX函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitISREGEX_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        return new Function_ISREGEX(args1, args2);
    }

    /**
     * 访问GUID函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitGUID_fun(context) {
        return new Function_GUID();
    }

    /**
     * 访问MD5函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitMD5_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_MD5(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_MD5(args1, args2);
    }

    /**
     * 访问SHA1函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSHA1_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_SHA1(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_SHA1(args1, args2);
    }

    /**
     * 访问SHA256函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSHA256_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_SHA256(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_SHA256(args1, args2);
    }

    /**
     * 访问SHA512函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitSHA512_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_SHA512(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_SHA512(args1, args2);
    }

    /**
     * 访问CRC32函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitCRC32_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        if (exprs.length === 1) return new Function_CRC32(args1, null);
        let args2 = exprs[1].accept(this);
        return new Function_CRC32(args1, args2);
    }

    /**
     * 访问HMACMD5函数节点
     * @param {Object} context - 上下文
     * @returns {FunctionBase}
     */
    visitHMACMD5_fun(context) {
        let exprs = context.expr();
        let args1 = exprs[0].accept(this);
        let args2 = exprs[1].accept(this);
        if (exprs.length === 2) return new Function_HMACMD5(args1, args2, null);
        let args3 = exprs[2].accept(this);
        return new Function_HMACMD5(args1, args2, args3);
    }
    visitBracket_fun(context) {
        return context.expr().accept(this);
    }

    visitSTRING_fun(context) {
        var opd = context.getText();
        var sb = [];
        let index = 1;
        while (index < opd.length - 1) {
            var c = opd[index++];
            if (c == '\\') {
                var c2 = opd[index++];
                if (c2 == 'n') sb.push('\n');
                else if (c2 == 'r') sb.push('\r');
                else if (c2 == 't') sb.push('\t');
                else if (c2 == '0') sb.push('\0');
                else if (c2 == 'v') sb.push('\v');
                else if (c2 == 'a') sb.push('\a');
                else if (c2 == 'b') sb.push('\b');
                else if (c2 == 'f') sb.push('\f');
                else sb.push(opd[index++]);
            } else {
                sb.push(c);
            }
        }
        return new Function_Value(Operand.Create(sb.join('')));
    }
    visitDiyFunction_fun(context) {
        let funName = context.PARAMETER().getText();
        let exprs = context.expr();
        let args = [];
        for (let i = 0; i < exprs.length; i++) {
            args.push(exprs[i].accept(this));
        }
        return new Function_DiyFunction(funName, args);
    }
}

export { MathFunctionVisitor };
