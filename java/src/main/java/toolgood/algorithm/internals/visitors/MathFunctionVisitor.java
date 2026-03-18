package toolgood.algorithm.internals.visitors;

import java.math.BigDecimal;
import java.text.NumberFormat;
import java.util.List;
import java.util.Locale;

import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.compare.*;
import toolgood.algorithm.internals.functions.csharp.*;
import toolgood.algorithm.internals.functions.csharpsecurity.*;
import toolgood.algorithm.internals.functions.csharpweb.*; 
import toolgood.algorithm.internals.functions.datetimes.*;
import toolgood.algorithm.internals.functions.financial.*; 
import toolgood.algorithm.internals.functions.flow.*; 
import toolgood.algorithm.internals.functions.mathbase.*; 
import toolgood.algorithm.internals.functions.mathsum2.*;
import toolgood.algorithm.internals.functions.mathsum.*;
import toolgood.algorithm.internals.functions.mathtransformation.*;
import toolgood.algorithm.internals.functions.mathtrigonometric.*;
import toolgood.algorithm.internals.functions.operator.*; 
import toolgood.algorithm.internals.functions.string.*; 
import toolgood.algorithm.internals.functions.value.*;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ExprContext;
import toolgood.algorithm.math.mathVisitor;
import toolgood.algorithm.system.MathEx;

public final class MathFunctionVisitor extends AbstractParseTreeVisitor<FunctionBase> implements mathVisitor<FunctionBase> {
    private FunctionBase[] VisitExprs(List<ExprContext> exprs) {
        FunctionBase[] list = new FunctionBase[exprs.size()];
        for (int i = 0; i < exprs.size(); i++) {
            list[i] = exprs.get(i).accept(this);
        }
        return list;
    }

    @Override
    public FunctionBase visitProg(mathParser.ProgContext ctx) {
        return ctx.expr().accept(this);
    }

    @Override
    public FunctionBase visitMulDiv_fun(mathParser.MulDiv_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        Token t = ctx.op;
        if (CharUtil.Equals(t.getText(), '*')) {
            return new Function_Mul(funcs);
        } else if (CharUtil.Equals(t.getText(), '/')) {
            return new Function_Div(funcs);
        }
        return new Function_Mod(funcs);
    }

    @Override
    public FunctionBase visitAddSub_fun(mathParser.AddSub_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        Token t = ctx.op;
        if (CharUtil.Equals(t.getText(), '&')) {
            return new Function_Connect(funcs);
        } else if (CharUtil.Equals(t.getText(), '+')) {
            return new Function_Add(funcs);
        }
        return new Function_Sub(funcs);
    }

    @Override
    public FunctionBase visitJudge_fun(mathParser.Judge_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        String type = ctx.op.getText();
        if (CharUtil.Equals(type, "=", "==", "===")) {
            return new Function_EQ(funcs);
        } else if (CharUtil.Equals(type, "<")) {
            return new Function_LT(funcs);
        } else if (CharUtil.Equals(type, "<=")) {
            return new Function_LE(funcs);
        } else if (CharUtil.Equals(type, ">")) {
            return new Function_GT(funcs);
        } else if (CharUtil.Equals(type, ">=")) {
            return new Function_GE(funcs);
        }
        return new Function_NE(funcs);
    }

    @Override
    public FunctionBase visitAndOr_fun(mathParser.AndOr_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        Token t = ctx.op;
        if (CharUtil.Equals(t.getText(), "&&")) {
            return new Function_AND(funcs);
        }
        return new Function_OR(funcs);
    }

    @Override
    public FunctionBase visitIF_fun(mathParser.IF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_IF(funcs);
    }

    @Override
    public FunctionBase visitIFS_fun(mathParser.IFS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_IFS(funcs);
    }

    @Override
    public FunctionBase visitSWITCH_fun(mathParser.SWITCH_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SWITCH(funcs);
    }

    @Override
    public FunctionBase visitIFERROR_fun(mathParser.IFERROR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_IFERROR(funcs);
    }

    @Override
    public FunctionBase visitISNUMBER_fun(mathParser.ISNUMBER_funContext ctx) {
        FunctionBase args1 = this.visit(ctx.expr());
        return new Function_ISNUMBER(args1);
    }

    @Override
    public FunctionBase visitISTEXT_fun(mathParser.ISTEXT_funContext ctx) {
        FunctionBase args1 = this.visit(ctx.expr());
        return new Function_ISTEXT(args1);
    }

    @Override
    public FunctionBase visitISERROR_fun(mathParser.ISERROR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ISERROR(funcs);
    }

    @Override
    public FunctionBase visitISNULL_fun(mathParser.ISNULL_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ISNULL(funcs);
    }

    @Override
    public FunctionBase visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ISNULLORERROR(funcs);
    }

    @Override
    public FunctionBase visitISEVEN_fun(mathParser.ISEVEN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISEVEN(args1);
    }

    @Override
    public FunctionBase visitISLOGICAL_fun(mathParser.ISLOGICAL_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISLOGICAL(args1);
    }

    @Override
    public FunctionBase visitISODD_fun(mathParser.ISODD_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISODD(args1);
    }

    @Override
    public FunctionBase visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISNONTEXT(args1);
    }

    @Override
    public FunctionBase visitAND_fun(mathParser.AND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_AND_N(funcs);
    }

    @Override
    public FunctionBase visitOR_fun(mathParser.OR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_OR_N(funcs);
    }

    @Override
    public FunctionBase visitXOR_fun(mathParser.XOR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_XOR(funcs);
    }

    @Override
    public FunctionBase visitNOT_fun(mathParser.NOT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_NOT(args1);
    }

    @Override
    public FunctionBase visitTRUE_fun(mathParser.TRUE_funContext ctx) {
        return new Function_ValueBoolean(true);
    }

    @Override
    public FunctionBase visitFALSE_fun(mathParser.FALSE_funContext ctx) {
        return new Function_ValueBoolean(false);
    }

    @Override
    public FunctionBase visitE_fun(mathParser.E_funContext ctx) {
        return new Function_ValueNumber(Operand.Create(MathEx.E), "E");
    }

    @Override
    public FunctionBase visitPI_fun(mathParser.PI_funContext ctx) {
        return new Function_ValueNumber(Operand.Create(MathEx.PI), "PI");
    }

    @Override
    public FunctionBase visitABS_fun(mathParser.ABS_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ABS(args1);
    }

    @Override
    public FunctionBase visitQUOTIENT_fun(mathParser.QUOTIENT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_QUOTIENT(funcs);
    }

    @Override
    public FunctionBase visitMOD_fun(mathParser.MOD_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_Mod(funcs[0], funcs[1]);
    }

    @Override
    public FunctionBase visitSIGN_fun(mathParser.SIGN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SIGN(args1);
    }

    @Override
    public FunctionBase visitSQRT_fun(mathParser.SQRT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SQRT(args1);
    }

    @Override
    public FunctionBase visitTRUNC_fun(mathParser.TRUNC_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TRUNC(args1);
    }

    @Override
    public FunctionBase visitINT_fun(mathParser.INT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TRUNC(args1);
    }

    @Override
    public FunctionBase visitGCD_fun(mathParser.GCD_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_GCD(funcs);
    }

    @Override
    public FunctionBase visitLCM_fun(mathParser.LCM_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LCM(funcs);
    }

    @Override
    public FunctionBase visitCOMBIN_fun(mathParser.COMBIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_COMBIN(funcs);
    }

    @Override
    public FunctionBase visitPERMUT_fun(mathParser.PERMUT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PERMUT(funcs);
    }

    @Override
    public FunctionBase visitPercentage_fun(mathParser.Percentage_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_Percentage(args1);
    }

    @Override
    public FunctionBase visitDEGREES_fun(mathParser.DEGREES_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_DEGREES(args1);
    }

    @Override
    public FunctionBase visitRADIANS_fun(mathParser.RADIANS_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_RADIANS(args1);
    }

    @Override
    public FunctionBase visitCOS_fun(mathParser.COS_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_COS(args1);
    }

    @Override
    public FunctionBase visitCOSH_fun(mathParser.COSH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_COSH(args1);
    }

    @Override
    public FunctionBase visitSIN_fun(mathParser.SIN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SIN(args1);
    }

    @Override
    public FunctionBase visitSINH_fun(mathParser.SINH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SINH(args1);
    }

    @Override
    public FunctionBase visitTAN_fun(mathParser.TAN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TAN(args1);
    }

    @Override
    public FunctionBase visitTANH_fun(mathParser.TANH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TANH(args1);
    }

    @Override
    public FunctionBase visitCOT_fun(mathParser.COT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_COT(args1);
    }

    @Override
    public FunctionBase visitCOTH_fun(mathParser.COTH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_COTH(args1);
    }

    @Override
    public FunctionBase visitCSC_fun(mathParser.CSC_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_CSC(args1);
    }

    @Override
    public FunctionBase visitCSCH_fun(mathParser.CSCH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_CSCH(args1);
    }

    @Override
    public FunctionBase visitSEC_fun(mathParser.SEC_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SEC(args1);
    }

    @Override
    public FunctionBase visitSECH_fun(mathParser.SECH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SECH(args1);
    }

    @Override
    public FunctionBase visitACOS_fun(mathParser.ACOS_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ACOS(args1);
    }

    @Override
    public FunctionBase visitACOSH_fun(mathParser.ACOSH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ACOSH(args1);
    }

    @Override
    public FunctionBase visitASIN_fun(mathParser.ASIN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ASIN(args1);
    }

    @Override
    public FunctionBase visitASINH_fun(mathParser.ASINH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ASINH(args1);
    }

    @Override
    public FunctionBase visitATAN_fun(mathParser.ATAN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ATAN(args1);
    }

    @Override
    public FunctionBase visitATANH_fun(mathParser.ATANH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ATANH(args1);
    }

    @Override
    public FunctionBase visitACOT_fun(mathParser.ACOT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ACOT(args1);
    }

    @Override
    public FunctionBase visitACOTH_fun(mathParser.ACOTH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ACOTH(args1);
    }

    @Override
    public FunctionBase visitATAN2_fun(mathParser.ATAN2_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ATAN2(funcs);
    }

    @Override
    public FunctionBase visitFIXED_fun(mathParser.FIXED_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FIXED(funcs);
    }

    @Override
    public FunctionBase visitBIN2OCT_fun(mathParser.BIN2OCT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BIN2OCT(funcs);
    }

    @Override
    public FunctionBase visitBIN2DEC_fun(mathParser.BIN2DEC_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BIN2DEC(funcs);
    }

    @Override
    public FunctionBase visitBIN2HEX_fun(mathParser.BIN2HEX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BIN2HEX(funcs);
    }

    @Override
    public FunctionBase visitOCT2BIN_fun(mathParser.OCT2BIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_OCT2BIN(funcs);
    }

    @Override
    public FunctionBase visitOCT2DEC_fun(mathParser.OCT2DEC_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_OCT2DEC(funcs);
    }

    @Override
    public FunctionBase visitOCT2HEX_fun(mathParser.OCT2HEX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_OCT2HEX(funcs);
    }

    @Override
    public FunctionBase visitDEC2BIN_fun(mathParser.DEC2BIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DEC2BIN(funcs);
    }

    @Override
    public FunctionBase visitDEC2OCT_fun(mathParser.DEC2OCT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DEC2OCT(funcs);
    }

    @Override
    public FunctionBase visitDEC2HEX_fun(mathParser.DEC2HEX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DEC2HEX(funcs);
    }

    @Override
    public FunctionBase visitHEX2BIN_fun(mathParser.HEX2BIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HEX2BIN(funcs);
    }

    @Override
    public FunctionBase visitHEX2OCT_fun(mathParser.HEX2OCT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HEX2OCT(funcs);
    }

    @Override
    public FunctionBase visitHEX2DEC_fun(mathParser.HEX2DEC_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HEX2DEC(funcs);
    }

    @Override
    public FunctionBase visitROUND_fun(mathParser.ROUND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ROUND(funcs);
    }

    @Override
    public FunctionBase visitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ROUNDDOWN(funcs);
    }

    @Override
    public FunctionBase visitROUNDUP_fun(mathParser.ROUNDUP_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ROUNDUP(funcs);
    }

    @Override
    public FunctionBase visitCEILING_fun(mathParser.CEILING_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_CEILING(funcs);
    }

    @Override
    public FunctionBase visitFLOOR_fun(mathParser.FLOOR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FLOOR(funcs);
    }

    @Override
    public FunctionBase visitEVEN_fun(mathParser.EVEN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_EVEN(args1);
    }

    @Override
    public FunctionBase visitODD_fun(mathParser.ODD_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ODD(args1);
    }

    @Override
    public FunctionBase visitMROUND_fun(mathParser.MROUND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MROUND(funcs);
    }

    @Override
    public FunctionBase visitRAND_fun(mathParser.RAND_funContext ctx) {
        return new Function_RAND();
    }

    @Override
    public FunctionBase visitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_RANDBETWEEN(funcs);
    }

    @Override
    public FunctionBase visitCOVARIANCES_fun(mathParser.COVARIANCES_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_COVARIANCES(funcs);
    }

    @Override
    public FunctionBase visitCOVAR_fun(mathParser.COVAR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_COVAR(funcs);
    }

    @Override
    public FunctionBase visitFACT_fun(mathParser.FACT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_FACT(args1);
    }

    @Override
    public FunctionBase visitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_FACTDOUBLE(args1);
    }

    @Override
    public FunctionBase visitPOWER_fun(mathParser.POWER_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_POWER(funcs);
    }

    @Override
    public FunctionBase visitEXP_fun(mathParser.EXP_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_EXP(args1);
    }

    @Override
    public FunctionBase visitLN_fun(mathParser.LN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_LN(args1);
    }

    @Override
    public FunctionBase visitLOG_fun(mathParser.LOG_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LOG(funcs);
    }

    @Override
    public FunctionBase visitLOG10_fun(mathParser.LOG10_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_LOG10(args1);
    }

    @Override
    public FunctionBase visitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MULTINOMIAL(funcs);
    }

    @Override
    public FunctionBase visitPRODUCT_fun(mathParser.PRODUCT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PRODUCT(funcs);
    }

    @Override
    public FunctionBase visitSQRTPI_fun(mathParser.SQRTPI_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SQRTPI(args1);
    }

    @Override
    public FunctionBase visitERF_fun(mathParser.ERF_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ERF(args1);
    }

    @Override
    public FunctionBase visitERFC_fun(mathParser.ERFC_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ERFC(args1);
    }

    @Override
    public FunctionBase visitBESSELI_fun(mathParser.BESSELI_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BESSELI(funcs);
    }

    @Override
    public FunctionBase visitBESSELJ_fun(mathParser.BESSELJ_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BESSELJ(funcs);
    }

    @Override
    public FunctionBase visitBESSELK_fun(mathParser.BESSELK_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BESSELK(funcs);
    }

    @Override
    public FunctionBase visitBESSELY_fun(mathParser.BESSELY_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BESSELY(funcs);
    }

    @Override
    public FunctionBase visitDELTA_fun(mathParser.DELTA_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DELTA(funcs);
    }

    @Override
    public FunctionBase visitGESTEP_fun(mathParser.GESTEP_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_GESTEP(funcs);
    }

    @Override
    public FunctionBase visitSUMSQ_fun(mathParser.SUMSQ_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMSQ(funcs);
    }

    @Override
    public FunctionBase visitSUMPRODUCT_fun(mathParser.SUMPRODUCT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMPRODUCT(funcs);
    }

    @Override
    public FunctionBase visitSUMX2MY2_fun(mathParser.SUMX2MY2_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMX2MY2(funcs);
    }

    @Override
    public FunctionBase visitSUMX2PY2_fun(mathParser.SUMX2PY2_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMX2PY2(funcs);
    }

    @Override
    public FunctionBase visitSUMXMY2_fun(mathParser.SUMXMY2_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMXMY2(funcs);
    }

    @Override
    public FunctionBase visitARABIC_fun(mathParser.ARABIC_funContext ctx) {
        FunctionBase func = visit(ctx.expr());
        return new Function_ARABIC(new FunctionBase[]{func});
    }

    @Override
    public FunctionBase visitROMAN_fun(mathParser.ROMAN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ROMAN(funcs);
    }

    @Override
    public FunctionBase visitSERIESSUM_fun(mathParser.SERIESSUM_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SERIESSUM(funcs);
    }

    @Override
    public FunctionBase visitRANK_fun(mathParser.RANK_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_RANK(funcs);
    }

    @Override
    public FunctionBase visitFORECAST_fun(mathParser.FORECAST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FORECAST(funcs);
    }

    @Override
    public FunctionBase visitINTERCEPT_fun(mathParser.INTERCEPT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_INTERCEPT(funcs);
    }

    @Override
    public FunctionBase visitSLOPE_fun(mathParser.SLOPE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SLOPE(funcs);
    }

    @Override
    public FunctionBase visitCORREL_fun(mathParser.CORREL_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_CORREL(funcs);
    }

    @Override
    public FunctionBase visitPEARSON_fun(mathParser.PEARSON_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PEARSON(funcs);
    }

    @Override
    public FunctionBase visitYEARFRAC_fun(mathParser.YEARFRAC_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_YEARFRAC(funcs);
    }

    @Override
    public FunctionBase visitASC_fun(mathParser.ASC_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ASC(args1);
    }

    @Override
    public FunctionBase visitJIS_fun(mathParser.JIS_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_JIS(args1);
    }

    @Override
    public FunctionBase visitCHAR_fun(mathParser.CHAR_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_CHAR(args1);
    }

    @Override
    public FunctionBase visitCLEAN_fun(mathParser.CLEAN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_CLEAN(args1);
    }

    @Override
    public FunctionBase visitCODE_fun(mathParser.CODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_CODE(args1);
    }

    @Override
    public FunctionBase visitUNICHAR_fun(mathParser.UNICHAR_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_UNICHAR(args1);
    }

    @Override
    public FunctionBase visitUNICODE_fun(mathParser.UNICODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_UNICODE(args1);
    }

    @Override
    public FunctionBase visitCONCATENATE_fun(mathParser.CONCATENATE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_CONCATENATE(funcs);
    }

    @Override
    public FunctionBase visitEXACT_fun(mathParser.EXACT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_EXACT(funcs);
    }

    @Override
    public FunctionBase visitFIND_fun(mathParser.FIND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FIND(funcs);
    }

    @Override
    public FunctionBase visitLEFT_fun(mathParser.LEFT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LEFT(funcs);
    }

    @Override
    public FunctionBase visitLEN_fun(mathParser.LEN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_LEN(args1);
    }

    @Override
    public FunctionBase visitLOWER_fun(mathParser.LOWER_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_LOWER(args1);
    }

    @Override
    public FunctionBase visitMID_fun(mathParser.MID_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MID(funcs);
    }

    @Override
    public FunctionBase visitPROPER_fun(mathParser.PROPER_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_PROPER(args1);
    }

    @Override
    public FunctionBase visitREPLACE_fun(mathParser.REPLACE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REPLACE(funcs);
    }

    @Override
    public FunctionBase visitREPT_fun(mathParser.REPT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REPT(funcs);
    }

    @Override
    public FunctionBase visitRIGHT_fun(mathParser.RIGHT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_RIGHT(funcs);
    }

    @Override
    public FunctionBase visitRMB_fun(mathParser.RMB_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_RMB(args1);
    }

    @Override
    public FunctionBase visitSEARCH_fun(mathParser.SEARCH_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SEARCH(funcs);
    }

    @Override
    public FunctionBase visitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUBSTITUTE(funcs);
    }

    @Override
    public FunctionBase visitT_fun(mathParser.T_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_T(args1);
    }

    @Override
    public FunctionBase visitTEXT_fun(mathParser.TEXT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TEXT(funcs);
    }

    @Override
    public FunctionBase visitTRIM_fun(mathParser.TRIM_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TRIM(args1);
    }

    @Override
    public FunctionBase visitUPPER_fun(mathParser.UPPER_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_UPPER(args1);
    }

    @Override
    public FunctionBase visitVALUE_fun(mathParser.VALUE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_VALUE(args1);
    }

    @Override
    public FunctionBase visitDATEVALUE_fun(mathParser.DATEVALUE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DATEVALUE(funcs);
    }

    @Override
    public FunctionBase visitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TIMESTAMP(funcs);
    }

    @Override
    public FunctionBase visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TIMEVALUE(args1);
    }

    @Override
    public FunctionBase visitDATE_fun(mathParser.DATE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DATE(funcs);
    }

    @Override
    public FunctionBase visitTIME_fun(mathParser.TIME_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TIME(funcs);
    }

    @Override
    public FunctionBase visitNOW_fun(mathParser.NOW_funContext ctx) {
        return new Function_NOW();
    }

    @Override
    public FunctionBase visitTODAY_fun(mathParser.TODAY_funContext ctx) {
        return new Function_TODAY();
    }

    @Override
    public FunctionBase visitYEAR_fun(mathParser.YEAR_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_YEAR(args1);
    }

    @Override
    public FunctionBase visitMONTH_fun(mathParser.MONTH_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_MONTH(args1);
    }

    @Override
    public FunctionBase visitDAY_fun(mathParser.DAY_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_DAY(args1);
    }

    @Override
    public FunctionBase visitHOUR_fun(mathParser.HOUR_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_HOUR(args1);
    }

    @Override
    public FunctionBase visitMINUTE_fun(mathParser.MINUTE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_MINUTE(args1);
    }

    @Override
    public FunctionBase visitSECOND_fun(mathParser.SECOND_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SECOND(args1);
    }

    @Override
    public FunctionBase visitWEEKDAY_fun(mathParser.WEEKDAY_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_WEEKDAY(funcs);
    }

    @Override
    public FunctionBase visitDATEDIF_fun(mathParser.DATEDIF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DATEDIF(funcs);
    }

    @Override
    public FunctionBase visitDAYS_fun(mathParser.DAYS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DAYS(funcs);
    }

    @Override
    public FunctionBase visitDAYS360_fun(mathParser.DAYS360_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DAYS360(funcs);
    }

    @Override
    public FunctionBase visitEDATE_fun(mathParser.EDATE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_EDATE(funcs);
    }

    @Override
    public FunctionBase visitEOMONTH_fun(mathParser.EOMONTH_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_EOMONTH(funcs);
    }

    @Override
    public FunctionBase visitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NETWORKDAYS(funcs);
    }

    @Override
    public FunctionBase visitWORKDAY_fun(mathParser.WORKDAY_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_WORKDAY(funcs);
    }

    @Override
    public FunctionBase visitWEEKNUM_fun(mathParser.WEEKNUM_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_WEEKNUM(funcs);
    }

    @Override
    public FunctionBase visitADDMONTHS_fun(mathParser.ADDMONTHS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDMONTHS(funcs);
    }

    @Override
    public FunctionBase visitADDYEARS_fun(mathParser.ADDYEARS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDYEARS(funcs);
    }

    @Override
    public FunctionBase visitADDSECONDS_fun(mathParser.ADDSECONDS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDSECONDS(funcs);
    }

    @Override
    public FunctionBase visitADDMINUTES_fun(mathParser.ADDMINUTES_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDMINUTES(funcs);
    }

    @Override
    public FunctionBase visitADDDAYS_fun(mathParser.ADDDAYS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDDAYS(funcs);
    }

    @Override
    public FunctionBase visitADDHOURS_fun(mathParser.ADDHOURS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ADDHOURS(funcs);
    }

    @Override
    public FunctionBase visitMAX_fun(mathParser.MAX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MAX(funcs);
    }

    @Override
    public FunctionBase visitMEDIAN_fun(mathParser.MEDIAN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MEDIAN(funcs);
    }

    @Override
    public FunctionBase visitMIN_fun(mathParser.MIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MIN(funcs);
    }

    @Override
    public FunctionBase visitQUARTILE_fun(mathParser.QUARTILE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_QUARTILE(funcs);
    }

    @Override
    public FunctionBase visitMODE_fun(mathParser.MODE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MODE(funcs);
    }

    @Override
    public FunctionBase visitLARGE_fun(mathParser.LARGE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LARGE(funcs);
    }

    @Override
    public FunctionBase visitSMALL_fun(mathParser.SMALL_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SMALL(funcs);
    }

    @Override
    public FunctionBase visitPERCENTILE_fun(mathParser.PERCENTILE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PERCENTILE(funcs);
    }

    @Override
    public FunctionBase visitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PERCENTRANK(funcs);
    }

    @Override
    public FunctionBase visitAVERAGE_fun(mathParser.AVERAGE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_AVERAGE(funcs);
    }

    @Override
    public FunctionBase visitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_AVERAGEIF(funcs);
    }

    @Override
    public FunctionBase visitGEOMEAN_fun(mathParser.GEOMEAN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_GEOMEAN(funcs);
    }

    @Override
    public FunctionBase visitHARMEAN_fun(mathParser.HARMEAN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HARMEAN(funcs);
    }

    @Override
    public FunctionBase visitCOUNT_fun(mathParser.COUNT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_COUNT(funcs);
    }

    @Override
    public FunctionBase visitCOUNTIF_fun(mathParser.COUNTIF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_COUNTIF(funcs);
    }

    @Override
    public FunctionBase visitSUM_fun(mathParser.SUM_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUM(funcs);
    }

    @Override
    public FunctionBase visitSUMIF_fun(mathParser.SUMIF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUMIF(funcs);
    }

    @Override
    public FunctionBase visitAVEDEV_fun(mathParser.AVEDEV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_AVEDEV(funcs);
    }

    @Override
    public FunctionBase visitSTDEV_fun(mathParser.STDEV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_STDEV(funcs);
    }

    @Override
    public FunctionBase visitSTDEVP_fun(mathParser.STDEVP_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_STDEVP(funcs);
    }

    @Override
    public FunctionBase visitDEVSQ_fun(mathParser.DEVSQ_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DEVSQ(funcs);
    }

    @Override
    public FunctionBase visitVAR_fun(mathParser.VAR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_VAR(funcs);
    }

    @Override
    public FunctionBase visitVARP_fun(mathParser.VARP_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_VARP(funcs);
    }

    @Override
    public FunctionBase visitNORMDIST_fun(mathParser.NORMDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NORMDIST(funcs);
    }

    @Override
    public FunctionBase visitNORMINV_fun(mathParser.NORMINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NORMINV(funcs);
    }

    @Override
    public FunctionBase visitNORMSDIST_fun(mathParser.NORMSDIST_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_NORMSDIST(args1);
    }

    @Override
    public FunctionBase visitNORMSINV_fun(mathParser.NORMSINV_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_NORMSINV(args1);
    }

    @Override
    public FunctionBase visitBETADIST_fun(mathParser.BETADIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BETADIST(funcs);
    }

    @Override
    public FunctionBase visitBETAINV_fun(mathParser.BETAINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BETAINV(funcs);
    }

    @Override
    public FunctionBase visitBINOMDIST_fun(mathParser.BINOMDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_BINOMDIST(funcs);
    }

    @Override
    public FunctionBase visitEXPONDIST_fun(mathParser.EXPONDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_EXPONDIST(funcs);
    }

    @Override
    public FunctionBase visitFDIST_fun(mathParser.FDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FDIST(funcs);
    }

    @Override
    public FunctionBase visitFINV_fun(mathParser.FINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FINV(funcs);
    }

    @Override
    public FunctionBase visitFISHER_fun(mathParser.FISHER_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_FISHER(args1);
    }

    @Override
    public FunctionBase visitFISHERINV_fun(mathParser.FISHERINV_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_FISHERINV(args1);
    }

    @Override
    public FunctionBase visitGAMMADIST_fun(mathParser.GAMMADIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_GAMMADIST(funcs);
    }

    @Override
    public FunctionBase visitGAMMAINV_fun(mathParser.GAMMAINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_GAMMAINV(funcs);
    }

    @Override
    public FunctionBase visitGAMMALN_fun(mathParser.GAMMALN_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_GAMMALN(args1);
    }

    @Override
    public FunctionBase visitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HYPGEOMDIST(funcs);
    }

    @Override
    public FunctionBase visitLOGINV_fun(mathParser.LOGINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LOGINV(funcs);
    }

    @Override
    public FunctionBase visitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LOGNORMDIST(funcs);
    }

    @Override
    public FunctionBase visitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NEGBINOMDIST(funcs);
    }

    @Override
    public FunctionBase visitPOISSON_fun(mathParser.POISSON_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_POISSON(funcs);
    }

    @Override
    public FunctionBase visitTDIST_fun(mathParser.TDIST_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TDIST(funcs);
    }

    @Override
    public FunctionBase visitTINV_fun(mathParser.TINV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TINV(funcs);
    }

    @Override
    public FunctionBase visitWEIBULL_fun(mathParser.WEIBULL_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_WEIBULL(funcs);
    }

    @Override
    public FunctionBase visitPMT_fun(mathParser.PMT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PMT(funcs);
    }

    @Override
    public FunctionBase visitPPMT_fun(mathParser.PPMT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PPMT(funcs);
    }

    @Override
    public FunctionBase visitIPMT_fun(mathParser.IPMT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_IPMT(funcs);
    }

    @Override
    public FunctionBase visitPV_fun(mathParser.PV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PV(funcs);
    }

    @Override
    public FunctionBase visitFV_fun(mathParser.FV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_FV(funcs);
    }

    @Override
    public FunctionBase visitNPER_fun(mathParser.NPER_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NPER(funcs);
    }

    @Override
    public FunctionBase visitRATE_fun(mathParser.RATE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_RATE(funcs);
    }

    @Override
    public FunctionBase visitNPV_fun(mathParser.NPV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_NPV(funcs);
    }

    @Override
    public FunctionBase visitXNPV_fun(mathParser.XNPV_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_XNPV(funcs);
    }

    @Override
    public FunctionBase visitIRR_fun(mathParser.IRR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_IRR(funcs);
    }

    @Override
    public FunctionBase visitMIRR_fun(mathParser.MIRR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_MIRR(funcs);
    }

    @Override
    public FunctionBase visitXIRR_fun(mathParser.XIRR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_XIRR(funcs);
    }

    @Override
    public FunctionBase visitSLN_fun(mathParser.SLN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SLN(funcs);
    }

    @Override
    public FunctionBase visitDB_fun(mathParser.DB_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DB(funcs);
    }

    @Override
    public FunctionBase visitDDB_fun(mathParser.DDB_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DDB(funcs);
    }

    @Override
    public FunctionBase visitSYD_fun(mathParser.SYD_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SYD(funcs);
    }

    @Override
    public FunctionBase visitURLENCODE_fun(mathParser.URLENCODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_URLENCODE(args1);
    }

    @Override
    public FunctionBase visitURLDECODE_fun(mathParser.URLDECODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_URLDECODE(args1);
    }

    @Override
    public FunctionBase visitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_HTMLENCODE(args1);
    }

    @Override
    public FunctionBase visitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_HTMLDECODE(args1);
    }

    @Override
    public FunctionBase visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_BASE64TOTEXT(args1);
    }

    @Override
    public FunctionBase visitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_BASE64URLTOTEXT(args1);
    }

    @Override
    public FunctionBase visitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TEXTTOBASE64(args1);
    }

    @Override
    public FunctionBase visitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_TEXTTOBASE64URL(args1);
    }

    @Override
    public FunctionBase visitREGEX_fun(mathParser.REGEX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REGEX(funcs);
    }

    @Override
    public FunctionBase visitREGEXREPLACE_fun(mathParser.REGEXREPLACE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REGEXREPLACE(funcs);
    }

    @Override
    public FunctionBase visitISREGEX_fun(mathParser.ISREGEX_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ISREGEX(funcs);
    }

    @Override
    public FunctionBase visitGUID_fun(mathParser.GUID_funContext ctx) {
        return new Function_GUID();
    }

    @Override
    public FunctionBase visitMD5_fun(mathParser.MD5_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_MD5(args1);
    }

    @Override
    public FunctionBase visitSHA1_fun(mathParser.SHA1_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SHA1(args1);
    }

    @Override
    public FunctionBase visitSHA256_fun(mathParser.SHA256_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SHA256(args1);
    }

    @Override
    public FunctionBase visitSHA512_fun(mathParser.SHA512_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_SHA512(args1);
    }

    @Override
    public FunctionBase visitHMACMD5_fun(mathParser.HMACMD5_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HMACMD5(funcs);
    }

    @Override
    public FunctionBase visitHMACSHA1_fun(mathParser.HMACSHA1_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HMACSHA1(funcs);
    }

    @Override
    public FunctionBase visitHMACSHA256_fun(mathParser.HMACSHA256_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HMACSHA256(funcs);
    }

    @Override
    public FunctionBase visitHMACSHA512_fun(mathParser.HMACSHA512_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HMACSHA512(funcs);
    }

    @Override
    public FunctionBase visitTRIMSTART_fun(mathParser.TRIMSTART_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TRIMSTART(funcs);
    }

    @Override
    public FunctionBase visitTRIMEND_fun(mathParser.TRIMEND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_TRIMEND(funcs);
    }

    @Override
    public FunctionBase visitINDEXOF_fun(mathParser.INDEXOF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_INDEXOF(funcs);
    }

    @Override
    public FunctionBase visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LASTINDEXOF(funcs);
    }

    @Override
    public FunctionBase visitSPLIT_fun(mathParser.SPLIT_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SPLIT(funcs);
    }

    @Override
    public FunctionBase visitJOIN_fun(mathParser.JOIN_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_JOIN(funcs);
    }

    @Override
    public FunctionBase visitSUBSTRING_fun(mathParser.SUBSTRING_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_SUBSTRING(funcs);
    }

    @Override
    public FunctionBase visitSTARTSWITH_fun(mathParser.STARTSWITH_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_STARTSWITH(funcs);
    }

    @Override
    public FunctionBase visitENDSWITH_fun(mathParser.ENDSWITH_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_ENDSWITH(funcs);
    }

    @Override
    public FunctionBase visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISNULLOREMPTY(args1);
    }

    @Override
    public FunctionBase visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ISNULLORWHITESPACE(args1);
    }

    @Override
    public FunctionBase visitREMOVESTART_fun(mathParser.REMOVESTART_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REMOVESTART(funcs);
    }

    @Override
    public FunctionBase visitREMOVEEND_fun(mathParser.REMOVEEND_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_REMOVEEND(funcs);
    }

    @Override
    public FunctionBase visitJSON_fun(mathParser.JSON_funContext ctx) {
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_JSON(args1);
    }

    @Override
    public FunctionBase visitLOOKFLOOR_fun(mathParser.LOOKFLOOR_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LOOKFLOOR(funcs);
    }

    @Override
    public FunctionBase visitLOOKCEILING_fun(mathParser.LOOKCEILING_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_LOOKCEILING(funcs);
    }

    @Override
    public FunctionBase visitArray_fun(mathParser.Array_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_Array(funcs);
    }

    @Override
    public FunctionBase visitBracket_fun(mathParser.Bracket_funContext ctx) {
        return ctx.expr().accept(this);
    }

    @Override
    public FunctionBase visitNUM_fun(mathParser.NUM_funContext ctx) {
        String text = ctx.num().getText();
        BigDecimal d = new BigDecimal(text);
        if (ctx.unit == null) {
            return new Function_ValueNumber(Operand.Create(d), text);
        }
        String unit = ctx.unit.getText();
        return new Function_Number(d, unit);
    }

    @Override
    public FunctionBase visitNum(mathParser.NumContext ctx) {
        String text = ctx.getText();
        BigDecimal d = new BigDecimal(text);
        return new Function_ValueNumber(Operand.Create(d), text);
    }

    @Override
    public FunctionBase visitSTRING_fun(mathParser.STRING_funContext ctx) {
        String opd = ctx.getText();
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
    public FunctionBase visitNULL_fun(mathParser.NULL_funContext ctx) {
        return new Function_NULL();
    }

    @Override
    public FunctionBase visitPARAMETER_fun(mathParser.PARAMETER_funContext ctx) {
        TerminalNode node = ctx.PARAMETER();
        return new Function_PARAMETER(node.getText());
    }

    @Override
    public FunctionBase visitParameter2(mathParser.Parameter2Context ctx) {
        return new Function_ValueText(Operand.Create(ctx.getChild(0).getText()));
    }

    @Override
    public FunctionBase visitGetJsonValue_fun(mathParser.GetJsonValue_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        if (ctx.PARAMETER() != null) {
            FunctionBase op = new Function_PARAMETER(ctx.PARAMETER().getText());
            return new Function_GetJsonValue(funcs[0], op);
        }
        if (ctx.parameter2() != null) {
            FunctionBase op = ctx.parameter2().accept(this);
            return new Function_GetJsonValue(funcs[0], op);
        }
        return new Function_GetJsonValue(funcs[0], funcs[1]);
    }

    @Override
    public FunctionBase visitDiyFunction_fun(mathParser.DiyFunction_funContext ctx) {
        String funName = ctx.PARAMETER().getText();
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_DiyFunction(funName, funcs);
    }

    @Override
    public FunctionBase visitPARAM_fun(mathParser.PARAM_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_PARAM(funcs);
    }

    @Override
    public FunctionBase visitHAS_fun(mathParser.HAS_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HAS(funcs);
    }

    @Override
    public FunctionBase visitHASVALUE_fun(mathParser.HASVALUE_funContext ctx) {
        FunctionBase[] funcs = VisitExprs(ctx.expr());
        return new Function_HASVALUE(funcs);
    }

    @Override
    public FunctionBase visitArrayJson_fun(mathParser.ArrayJson_funContext ctx) {
        List<mathParser.ArrayJsonContext> exprs = ctx.arrayJson();
        FunctionBase[] args = new FunctionBase[exprs.size()];
        for (int i = 0; i < exprs.size(); i++) {
            args[i] = exprs.get(i).accept(this);
        }
        return new Function_ArrayJson(args);
    }

    @Override
    public FunctionBase visitArrayJson(mathParser.ArrayJsonContext ctx) {
        String keyName = null;
        if (ctx.key != null) {
            keyName = ctx.key.getText().trim();
            if (keyName.length() >= 2) {
                char firstChar = keyName.charAt(0);
                if ((firstChar == '"' || firstChar == '\'') && keyName.charAt(keyName.length() - 1) == firstChar) {
                    keyName = keyName.substring(1, keyName.length() - 1);
                }
            }
        } else if (ctx.parameter2() != null) {
            keyName = ctx.parameter2().getText();
        }
        FunctionBase f = ctx.expr().accept(this);
        return new Function_ArrayJsonItem(keyName, f);
    }

    @Override
    public FunctionBase visitERROR_fun(mathParser.ERROR_funContext ctx) {
        if (ctx.expr() == null) {
            return new Function_ERROR(null);
        }
        FunctionBase args1 = ctx.expr().accept(this);
        return new Function_ERROR(args1);
    }

    @Override
    public FunctionBase visitVersion_fun(mathParser.Version_funContext ctx) {
        return new Function_ValueText(Operand.Version, "ALGORITHMVERSION");
    }
}
