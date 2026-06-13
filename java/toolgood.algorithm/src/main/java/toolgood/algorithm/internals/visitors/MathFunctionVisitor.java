package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.compare.*;
import toolgood.algorithm.internals.functions.csharp.*;
import toolgood.algorithm.internals.functions.csharpSecurity.*;
import toolgood.algorithm.internals.functions.csharpWeb.*;
import toolgood.algorithm.internals.functions.dateTimes.*;
import toolgood.algorithm.internals.functions.financial.*;
import toolgood.algorithm.internals.functions.flow.*;
import toolgood.algorithm.internals.functions.mathBase.*;
import toolgood.algorithm.internals.functions.mathSum.*;
import toolgood.algorithm.internals.functions.mathSum2.*;
import toolgood.algorithm.internals.functions.mathTransformation.*;
import toolgood.algorithm.internals.functions.mathTrigonometric.*;
import toolgood.algorithm.internals.functions.operator.*;
import toolgood.algorithm.internals.functions.string.*;
import toolgood.algorithm.internals.functions.value.*;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.math.mathVisitor;

import java.math.BigDecimal;
import java.util.List;

public class MathFunctionVisitor extends AbstractParseTreeVisitor<FunctionBase> implements mathVisitor<FunctionBase> {
    private FunctionBase[] VisitExprs(List<ExprContext> exprs) {
        FunctionBase[] list = new FunctionBase[exprs.size()];
        for (int i = 0; i < exprs.size(); i++) {
            list[i] = visit(exprs.get(i));
        }
        return list;
    }

    @Override
    public FunctionBase visitProg(mathParser.ProgContext context) {
        return visit(context.expr());
    }

    @Override
    public FunctionBase visitMulDiv_fun(MulDiv_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        int type = context.op.getType();
        if (type == mathParser.OPMUL) {
            return new Function_Mul(funcs);
        } else if (type == mathParser.OPDIV) {
            return new Function_Div(funcs);
        }
        return new Function_Mod(funcs);
    }

    @Override
    public FunctionBase visitAddSub_fun(AddSub_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        int type = context.op.getType();
        if (type == mathParser.OPADD) {
            return new Function_Add(funcs);
        } else if (type == mathParser.OPSUB) {
            return new Function_Sub(funcs);
        }
        return new Function_Connect(funcs);
    }

    @Override
    public FunctionBase visitJudge_fun(Judge_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        int type = context.op.getType();
        switch (type) {
            case mathParser.OPGT: return new Function_GT(funcs);
            case mathParser.OPGE: return new Function_GE(funcs);
            case mathParser.OPLT: return new Function_LT(funcs);
            case mathParser.OPLE: return new Function_LE(funcs);
            case mathParser.OPEQ: return new Function_EQ(funcs);
            case mathParser.OPNE:
            default: return new Function_NE(funcs);
        }
    }

    @Override
    public FunctionBase visitOr_fun(Or_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        return new Function_OR(funcs);
    }

    @Override
    public FunctionBase visitAnd_fun(And_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        return new Function_AND(funcs);
    }

    @Override
    public FunctionBase visitIF_fun(IF_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        return new Function_IF(funcs);
    }

    @Override
    public FunctionBase visitNOT_fun(NOT_funContext context) {
        FunctionBase args1 = visit(context.expr());
        return new Function_NOT(args1);
    }

    @Override
    public FunctionBase visitCONST2_fun(CONST2_funContext context) {
        int type = context.f.getType();
        switch (type) {
            case mathParser.TRUE: return new Function_ValueBoolean(true);
            case mathParser.FALSE: return new Function_ValueBoolean(false);
            case mathParser.ALGORITHMVERSION: return new Function_Version();
            case mathParser.NULL:
            default: return new Function_NULL();
        }
    }

    @Override
    public FunctionBase visitCONST_fun(CONST_funContext context) {
        int type = context.f.getType();
        switch (type) {
            case mathParser.E: return new Function_Number_E();
            case mathParser.PI: return new Function_Number_PI();
            case mathParser.RAND: return new Function_RAND();
            case mathParser.GUID: return new Function_GUID();
            case mathParser.NOW: return new Function_NOW();
            case mathParser.TODAY:
            default: return new Function_TODAY();
        }
    }

    @Override
    public FunctionBase visitPercentage_fun(Percentage_funContext context) {
        FunctionBase args1 = visit(context.expr());
        return new Function_Percentage(args1);
    }

    // region getValue
    @Override
    public FunctionBase visitArray_fun(Array_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        return new Function_ARRAY(funcs);
    }

    @Override
    public FunctionBase visitBracket_fun(Bracket_funContext context) {
        return visit(context.expr());
    }

    @Override
    public FunctionBase visitNUM_fun(NUM_funContext context) {
        String text = context.getText();
        try {
            BigDecimal d = new BigDecimal(text);
            return new Function_Number(Operand.Create(d));
        } catch (NumberFormatException e) {
            // try with unit
        }
        String txt;
        String unit;
        int len = text.length();
        if (len > 3 && text.charAt(len - 3) >= 'A') {
            txt = text.substring(0, len - 3);
            unit = text.substring(len - 3);
        } else if (len > 2 && text.charAt(len - 2) >= 'A') {
            txt = text.substring(0, len - 2);
            unit = text.substring(len - 2);
        } else {
            txt = text.substring(0, len - 1);
            unit = text.substring(len - 1);
        }
        BigDecimal d2 = new BigDecimal(txt);
        return new Function_Number2(d2, unit);
    }

    @Override
    public FunctionBase visitSTRING_fun(STRING_funContext context) {
        String opd = context.getText();
        StringBuilder sb = new StringBuilder(opd.length());
        int index = 1;
        while (index < opd.length() - 1) {
            char c = opd.charAt(index++);
            if (c == '\\') {
                char c2 = opd.charAt(index++);
                if (c2 == 'n') sb.append('\n');
                else if (c2 == 'r') sb.append('\r');
                else if (c2 == 't') sb.append('\t');
                else if (c2 == '0') sb.append('\0');
                else if (c2 == 'v') sb.append('\u000B');
                else if (c2 == 'a') sb.append('\u0007');
                else if (c2 == 'b') sb.append('\b');
                else if (c2 == 'f') sb.append('\f');
                else sb.append(c2);
            } else {
                sb.append(c);
            }
        }
        return new Function_ValueText(Operand.Create(sb.toString()));
    }

    @Override
    public FunctionBase visitPARAMETER_fun(PARAMETER_funContext context) {
        TerminalNode node = context.PARAMETER();
        return new Function_PARAMETER(node.getText());
    }

    @Override
    public FunctionBase visitParameter2(Parameter2Context context) {
        return new Function_ValueText(Operand.Create(context.children.get(0).getText()));
    }

    @Override
    public FunctionBase visitGetJsonValue_fun(GetJsonValue_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        if (context.PARAMETER() != null) {
            FunctionBase op = new Function_PARAMETER(context.PARAMETER().getText());
            return new Function_GetJsonValue(funcs[0], op);
        }
        if (context.parameter2() != null) {
            FunctionBase op = visit(context.parameter2());
            return new Function_GetJsonValue(funcs[0], op);
        }
        return new Function_GetJsonValue(funcs[0], funcs[1]);
    }

    @Override
    public FunctionBase visitArrayJson_fun(ArrayJson_funContext context) {
        List<ArrayJsonContext> exprs = context.arrayJson();
        FunctionBase[] args = new FunctionBase[exprs.size()];
        for (int i = 0; i < exprs.size(); i++) {
            args[i] = visit(exprs.get(i));
        }
        return new Function_ArrayJson(args);
    }

    @Override
    public FunctionBase visitArrayJson(ArrayJsonContext context) {
        String keyName = null;
        if (context.key != null) {
            keyName = context.key.getText().trim().replace("\"", "").replace("'", "").replace(" ", "").replace("\t", "").replace("\r", "").replace("\n", "").replace("\f", "");
        } else if (context.parameter2() != null) {
            keyName = context.parameter2().getText();
        }
        FunctionBase f = visit(context.expr());
        return new Function_ArrayJsonItem(keyName, f);
    }

    // endregion getValue

    @Override
    public FunctionBase visitFunction_fun(Function_funContext context) {
        FunctionBase[] funcs = VisitExprs(context.expr());
        int type = context.f.getType();
        switch (type) {
            case mathParser.ABS: return new Function_ABS(funcs);
            case mathParser.ACOS: return new Function_ACOS(funcs);
            case mathParser.ACOSH: return new Function_ACOSH(funcs);
            case mathParser.ACOT: return new Function_ACOT(funcs);
            case mathParser.ACOTH: return new Function_ACOTH(funcs);
            case mathParser.ADDDAYS: return new Function_ADDDAYS(funcs);
            case mathParser.ADDHOURS: return new Function_ADDHOURS(funcs);
            case mathParser.ADDMINUTES: return new Function_ADDMINUTES(funcs);
            case mathParser.ADDMONTHS: return new Function_ADDMONTHS(funcs);
            case mathParser.ADDSECONDS: return new Function_ADDSECONDS(funcs);
            case mathParser.ADDYEARS: return new Function_ADDYEARS(funcs);
            case mathParser.AND: return new Function_AND_N(funcs);
            case mathParser.ARABIC: return new Function_ARABIC(funcs);
            case mathParser.ARRAY: return new Function_ARRAY(funcs);
            case mathParser.ASC: return new Function_ASC(funcs);
            case mathParser.ASIN: return new Function_ASIN(funcs);
            case mathParser.ASINH: return new Function_ASINH(funcs);
            case mathParser.ATAN: return new Function_ATAN(funcs);
            case mathParser.ATAN2: return new Function_ATAN2(funcs);
            case mathParser.ATANH: return new Function_ATANH(funcs);
            case mathParser.AVEDEV: return new Function_AVEDEV(funcs);
            case mathParser.AVERAGE: return new Function_AVERAGE(funcs);
            case mathParser.AVERAGEIF: return new Function_AVERAGEIF(funcs);
            case mathParser.BASE64TOTEXT: return new Function_BASE64TOTEXT(funcs);
            case mathParser.BASE64URLTOTEXT: return new Function_BASE64URLTOTEXT(funcs);
            case mathParser.BESSELI: return new Function_BESSELI(funcs);
            case mathParser.BESSELJ: return new Function_BESSELJ(funcs);
            case mathParser.BESSELK: return new Function_BESSELK(funcs);
            case mathParser.BESSELY: return new Function_BESSELY(funcs);
            case mathParser.BETADIST: return new Function_BETADIST(funcs);
            case mathParser.BETAINV: return new Function_BETAINV(funcs);
            case mathParser.BIN2DEC: return new Function_BIN2DEC(funcs);
            case mathParser.BIN2HEX: return new Function_BIN2HEX(funcs);
            case mathParser.BIN2OCT: return new Function_BIN2OCT(funcs);
            case mathParser.BINOMDIST: return new Function_BINOMDIST(funcs);
            case mathParser.CEILING: return new Function_CEILING(funcs);
            case mathParser.CHAR: return new Function_CHAR(funcs);
            case mathParser.CLEAN: return new Function_CLEAN(funcs);
            case mathParser.CODE: return new Function_CODE(funcs);
            case mathParser.COMBIN: return new Function_COMBIN(funcs);
            case mathParser.CONCATENATE: return new Function_CONCATENATE(funcs);
            case mathParser.CORREL: return new Function_CORREL(funcs);
            case mathParser.COS: return new Function_COS(funcs);
            case mathParser.COSH: return new Function_COSH(funcs);
            case mathParser.COT: return new Function_COT(funcs);
            case mathParser.COTH: return new Function_COTH(funcs);
            case mathParser.COUNT: return new Function_COUNT(funcs);
            case mathParser.COUNTIF: return new Function_COUNTIF(funcs);
            case mathParser.COVAR: return new Function_COVAR(funcs);
            case mathParser.COVARIANCES: return new Function_COVARIANCES(funcs);
            case mathParser.CSC: return new Function_CSC(funcs);
            case mathParser.CSCH: return new Function_CSCH(funcs);
            case mathParser.DATE: return new Function_DATE(funcs);
            case mathParser.DATEDIF: return new Function_DATEDIF(funcs);
            case mathParser.DATEVALUE: return new Function_DATEVALUE(funcs);
            case mathParser.DAY: return new Function_DAY(funcs);
            case mathParser.DAYS: return new Function_DAYS(funcs);
            case mathParser.DAYS360: return new Function_DAYS360(funcs);
            case mathParser.DB: return new Function_DB(funcs);
            case mathParser.DDB: return new Function_DDB(funcs);
            case mathParser.DEC2BIN: return new Function_DEC2BIN(funcs);
            case mathParser.DEC2HEX: return new Function_DEC2HEX(funcs);
            case mathParser.DEC2OCT: return new Function_DEC2OCT(funcs);
            case mathParser.DEGREES: return new Function_DEGREES(funcs);
            case mathParser.DELTA: return new Function_DELTA(funcs);
            case mathParser.DEVSQ: return new Function_DEVSQ(funcs);
            case mathParser.EDATE: return new Function_EDATE(funcs);
            case mathParser.ENDSWITH: return new Function_ENDSWITH(funcs);
            case mathParser.EOMONTH: return new Function_EOMONTH(funcs);
            case mathParser.ERF: return new Function_ERF(funcs);
            case mathParser.ERFC: return new Function_ERFC(funcs);
            case mathParser.ERROR: return new Function_ERROR(funcs);
            case mathParser.EVEN: return new Function_EVEN(funcs);
            case mathParser.EXACT: return new Function_EXACT(funcs);
            case mathParser.EXP: return new Function_EXP(funcs);
            case mathParser.EXPONDIST: return new Function_EXPONDIST(funcs);
            case mathParser.FACT: return new Function_FACT(funcs);
            case mathParser.FACTDOUBLE: return new Function_FACTDOUBLE(funcs);
            case mathParser.FDIST: return new Function_FDIST(funcs);
            case mathParser.FIND: return new Function_FIND(funcs);
            case mathParser.FINV: return new Function_FINV(funcs);
            case mathParser.FISHER: return new Function_FISHER(funcs);
            case mathParser.FISHERINV: return new Function_FISHERINV(funcs);
            case mathParser.FIXED: return new Function_FIXED(funcs);
            case mathParser.FLOOR: return new Function_FLOOR(funcs);
            case mathParser.FORECAST: return new Function_FORECAST(funcs);
            case mathParser.FV: return new Function_FV(funcs);
            case mathParser.GAMMADIST: return new Function_GAMMADIST(funcs);
            case mathParser.GAMMAINV: return new Function_GAMMAINV(funcs);
            case mathParser.GAMMALN: return new Function_GAMMALN(funcs);
            case mathParser.GCD: return new Function_GCD(funcs);
            case mathParser.GEOMEAN: return new Function_GEOMEAN(funcs);
            case mathParser.GESTEP: return new Function_GESTEP(funcs);
            case mathParser.HARMEAN: return new Function_HARMEAN(funcs);
            case mathParser.HAS: return new Function_HAS(funcs);
            case mathParser.HASVALUE: return new Function_HASVALUE(funcs);
            case mathParser.HEX2BIN: return new Function_HEX2BIN(funcs);
            case mathParser.HEX2DEC: return new Function_HEX2DEC(funcs);
            case mathParser.HEX2OCT: return new Function_HEX2OCT(funcs);
            case mathParser.HMACMD5: return new Function_HMACMD5(funcs);
            case mathParser.HMACSHA1: return new Function_HMACSHA1(funcs);
            case mathParser.HMACSHA256: return new Function_HMACSHA256(funcs);
            case mathParser.HMACSHA512: return new Function_HMACSHA512(funcs);
            case mathParser.HOUR: return new Function_HOUR(funcs);
            case mathParser.HTMLDECODE: return new Function_HTMLDECODE(funcs);
            case mathParser.HTMLENCODE: return new Function_HTMLENCODE(funcs);
            case mathParser.HYPGEOMDIST: return new Function_HYPGEOMDIST(funcs);
            case mathParser.IF: return new Function_IF(funcs);
            case mathParser.IFERROR: return new Function_IFERROR(funcs);
            case mathParser.IFS: return new Function_IFS(funcs);
            case mathParser.INDEXOF: return new Function_INDEXOF(funcs);
            case mathParser.INT: return new Function_INT(funcs);
            case mathParser.INTERCEPT: return new Function_INTERCEPT(funcs);
            case mathParser.IPMT: return new Function_IPMT(funcs);
            case mathParser.IRR: return new Function_IRR(funcs);
            case mathParser.ISERROR: return new Function_ISERROR(funcs);
            case mathParser.ISEVEN: return new Function_ISEVEN(funcs);
            case mathParser.ISLOGICAL: return new Function_ISLOGICAL(funcs);
            case mathParser.ISNONTEXT: return new Function_ISNONTEXT(funcs);
            case mathParser.ISNULL: return new Function_ISNULL(funcs);
            case mathParser.ISNULLOREMPTY: return new Function_ISNULLOREMPTY(funcs);
            case mathParser.ISNULLORERROR: return new Function_ISNULLORERROR(funcs);
            case mathParser.ISNULLORWHITESPACE: return new Function_ISNULLORWHITESPACE(funcs);
            case mathParser.ISNUMBER: return new Function_ISNUMBER(funcs);
            case mathParser.ISODD: return new Function_ISODD(funcs);
            case mathParser.ISREGEX: return new Function_ISREGEX(funcs);
            case mathParser.ISTEXT: return new Function_ISTEXT(funcs);
            case mathParser.JIS: return new Function_JIS(funcs);
            case mathParser.JOIN: return new Function_JOIN(funcs);
            case mathParser.JSON: return new Function_JSON(funcs);
            case mathParser.LARGE: return new Function_LARGE(funcs);
            case mathParser.LASTINDEXOF: return new Function_LASTINDEXOF(funcs);
            case mathParser.LCM: return new Function_LCM(funcs);
            case mathParser.LEFT: return new Function_LEFT(funcs);
            case mathParser.LEN: return new Function_LEN(funcs);
            case mathParser.LN: return new Function_LN(funcs);
            case mathParser.LOG: return new Function_LOG(funcs);
            case mathParser.LOG10: return new Function_LOG10(funcs);
            case mathParser.LOGINV: return new Function_LOGINV(funcs);
            case mathParser.LOGNORMDIST: return new Function_LOGNORMDIST(funcs);
            case mathParser.LOOKCEILING: return new Function_LOOKCEILING(funcs);
            case mathParser.LOOKFLOOR: return new Function_LOOKFLOOR(funcs);
            case mathParser.LOWER: return new Function_LOWER(funcs);
            case mathParser.MAX: return new Function_MAX(funcs);
            case mathParser.MD5: return new Function_MD5(funcs);
            case mathParser.MEDIAN: return new Function_MEDIAN(funcs);
            case mathParser.MID: return new Function_MID(funcs);
            case mathParser.MIN: return new Function_MIN(funcs);
            case mathParser.MINUTE: return new Function_MINUTE(funcs);
            case mathParser.MIRR: return new Function_MIRR(funcs);
            case mathParser.MOD: return new Function_Mod(funcs);
            case mathParser.MODE: return new Function_MODE(funcs);
            case mathParser.MONTH: return new Function_MONTH(funcs);
            case mathParser.MROUND: return new Function_MROUND(funcs);
            case mathParser.MULTINOMIAL: return new Function_MULTINOMIAL(funcs);
            case mathParser.NEGBINOMDIST: return new Function_NEGBINOMDIST(funcs);
            case mathParser.NETWORKDAYS: return new Function_NETWORKDAYS(funcs);
            case mathParser.NORMDIST: return new Function_NORMDIST(funcs);
            case mathParser.NORMINV: return new Function_NORMINV(funcs);
            case mathParser.NORMSDIST: return new Function_NORMSDIST(funcs);
            case mathParser.NORMSINV: return new Function_NORMSINV(funcs);
            case mathParser.NOT: return new Function_NOT(funcs);
            case mathParser.NPER: return new Function_NPER(funcs);
            case mathParser.NPV: return new Function_NPV(funcs);
            case mathParser.OCT2BIN: return new Function_OCT2BIN(funcs);
            case mathParser.OCT2DEC: return new Function_OCT2DEC(funcs);
            case mathParser.OCT2HEX: return new Function_OCT2HEX(funcs);
            case mathParser.ODD: return new Function_ODD(funcs);
            case mathParser.OR: return new Function_OR_N(funcs);
            case mathParser.PARAM: return new Function_PARAM(funcs);
            case mathParser.PEARSON: return new Function_PEARSON(funcs);
            case mathParser.PERCENTILE: return new Function_PERCENTILE(funcs);
            case mathParser.PERCENTRANK: return new Function_PERCENTRANK(funcs);
            case mathParser.PERMUT: return new Function_PERMUT(funcs);
            case mathParser.PMT: return new Function_PMT(funcs);
            case mathParser.POISSON: return new Function_POISSON(funcs);
            case mathParser.POWER: return new Function_POWER(funcs);
            case mathParser.PPMT: return new Function_PPMT(funcs);
            case mathParser.PRODUCT: return new Function_PRODUCT(funcs);
            case mathParser.PROPER: return new Function_PROPER(funcs);
            case mathParser.PV: return new Function_PV(funcs);
            case mathParser.QUARTILE: return new Function_QUARTILE(funcs);
            case mathParser.QUOTIENT: return new Function_QUOTIENT(funcs);
            case mathParser.RADIANS: return new Function_RADIANS(funcs);
            case mathParser.RANDBETWEEN: return new Function_RANDBETWEEN(funcs);
            case mathParser.RANK: return new Function_RANK(funcs);
            case mathParser.RATE: return new Function_RATE(funcs);
            case mathParser.REGEX: return new Function_REGEX(funcs);
            case mathParser.REGEXREPLACE: return new Function_REGEXREPLACE(funcs);
            case mathParser.REMOVEEND: return new Function_REMOVEEND(funcs);
            case mathParser.REMOVESTART: return new Function_REMOVESTART(funcs);
            case mathParser.REPLACE: return new Function_REPLACE(funcs);
            case mathParser.REPT: return new Function_REPT(funcs);
            case mathParser.RIGHT: return new Function_RIGHT(funcs);
            case mathParser.RMB: return new Function_RMB(funcs);
            case mathParser.ROMAN: return new Function_ROMAN(funcs);
            case mathParser.ROUND: return new Function_ROUND(funcs);
            case mathParser.ROUNDDOWN: return new Function_ROUNDDOWN(funcs);
            case mathParser.ROUNDUP: return new Function_ROUNDUP(funcs);
            case mathParser.SEARCH: return new Function_SEARCH(funcs);
            case mathParser.SEC: return new Function_SEC(funcs);
            case mathParser.SECH: return new Function_SECH(funcs);
            case mathParser.SECOND: return new Function_SECOND(funcs);
            case mathParser.SERIESSUM: return new Function_SERIESSUM(funcs);
            case mathParser.SHA1: return new Function_SHA1(funcs);
            case mathParser.SHA256: return new Function_SHA256(funcs);
            case mathParser.SHA512: return new Function_SHA512(funcs);
            case mathParser.SIGN: return new Function_SIGN(funcs);
            case mathParser.SIN: return new Function_SIN(funcs);
            case mathParser.SINH: return new Function_SINH(funcs);
            case mathParser.SLN: return new Function_SLN(funcs);
            case mathParser.SLOPE: return new Function_SLOPE(funcs);
            case mathParser.SMALL: return new Function_SMALL(funcs);
            case mathParser.SPLIT: return new Function_SPLIT(funcs);
            case mathParser.SQRT: return new Function_SQRT(funcs);
            case mathParser.SQRTPI: return new Function_SQRTPI(funcs);
            case mathParser.STARTSWITH: return new Function_STARTSWITH(funcs);
            case mathParser.STDEV: return new Function_STDEV(funcs);
            case mathParser.STDEVP: return new Function_STDEVP(funcs);
            case mathParser.SUBSTITUTE: return new Function_SUBSTITUTE(funcs);
            case mathParser.SUBSTRING: return new Function_SUBSTRING(funcs);
            case mathParser.SUM: return new Function_SUM(funcs);
            case mathParser.SUMIF: return new Function_SUMIF(funcs);
            case mathParser.SUMPRODUCT: return new Function_SUMPRODUCT(funcs);
            case mathParser.SUMSQ: return new Function_SUMSQ(funcs);
            case mathParser.SUMX2MY2: return new Function_SUMX2MY2(funcs);
            case mathParser.SUMX2PY2: return new Function_SUMX2PY2(funcs);
            case mathParser.SUMXMY2: return new Function_SUMXMY2(funcs);
            case mathParser.SWITCH: return new Function_SWITCH(funcs);
            case mathParser.SYD: return new Function_SYD(funcs);
            case mathParser.T: return new Function_T(funcs);
            case mathParser.TAN: return new Function_TAN(funcs);
            case mathParser.TANH: return new Function_TANH(funcs);
            case mathParser.TDIST: return new Function_TDIST(funcs);
            case mathParser.TEXT: return new Function_TEXT(funcs);
            case mathParser.TEXTTOBASE64: return new Function_TEXTTOBASE64(funcs);
            case mathParser.TEXTTOBASE64URL: return new Function_TEXTTOBASE64URL(funcs);
            case mathParser.TIME: return new Function_TIME(funcs);
            case mathParser.TIMESTAMP: return new Function_TIMESTAMP(funcs);
            case mathParser.TIMEVALUE: return new Function_TIMEVALUE(funcs);
            case mathParser.TINV: return new Function_TINV(funcs);
            case mathParser.TRIM: return new Function_TRIM(funcs);
            case mathParser.TRIMEND: return new Function_TRIMEND(funcs);
            case mathParser.TRIMSTART: return new Function_TRIMSTART(funcs);
            case mathParser.TRUNC: return new Function_TRUNC(funcs);
            case mathParser.UNICHAR: return new Function_UNICHAR(funcs);
            case mathParser.UNICODE: return new Function_UNICODE(funcs);
            case mathParser.UPPER: return new Function_UPPER(funcs);
            case mathParser.URLDECODE: return new Function_URLDECODE(funcs);
            case mathParser.URLENCODE: return new Function_URLENCODE(funcs);
            case mathParser.VALUE: return new Function_VALUE(funcs);
            case mathParser.VAR: return new Function_VAR(funcs);
            case mathParser.VARP: return new Function_VARP(funcs);
            case mathParser.WEEKDAY: return new Function_WEEKDAY(funcs);
            case mathParser.WEEKNUM: return new Function_WEEKNUM(funcs);
            case mathParser.WEIBULL: return new Function_WEIBULL(funcs);
            case mathParser.WORKDAY: return new Function_WORKDAY(funcs);
            case mathParser.XIRR: return new Function_XIRR(funcs);
            case mathParser.XNPV: return new Function_XNPV(funcs);
            case mathParser.XOR: return new Function_XOR(funcs);
            case mathParser.YEAR: return new Function_YEAR(funcs);
            case mathParser.YEARFRAC: return new Function_YEARFRAC(funcs);
            case mathParser.PARAMETER:
            default:
                String funName = context.PARAMETER().getText();
                return new Function_DiyFunction(funName, funcs);
        }
    }
}
