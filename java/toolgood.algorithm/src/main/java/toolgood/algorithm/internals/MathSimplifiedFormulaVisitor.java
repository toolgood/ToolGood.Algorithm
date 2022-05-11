package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;

import org.antlr.v4.runtime.tree.ErrorNode;
import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser.*;

public class MathSimplifiedFormulaVisitor extends MathVisitor {
    private int inFunctionCount = 0;

    @Override
    public Operand visitProg(ProgContext context) {
        return super.visitProg(context);
    }

    @Override
    public Operand visitAddSub_fun(AddSub_funContext context) {
        if (inFunctionCount > 0) {
            return super.visitAddSub_fun(context);
        }

        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("");
        Operand secondValue = args.get(1).ToText("");
        final String t = context.op.getText();

        return Operand.Create(firstValue.TextValue() +" "+ t +" "+ secondValue.TextValue());
    }

    @Override
    public Operand visitMulDiv_fun(MulDiv_funContext context) {
        if (inFunctionCount > 0) {
            return super.visitMulDiv_fun(context);
        }
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("");
        Operand secondValue = args.get(1).ToText("");
        final String t = context.op.getText();

        return Operand.Create(firstValue.TextValue() +" "+ t +" "+ secondValue.TextValue());
    }

    @Override
    public Operand visitBracket_fun(Bracket_funContext context) {
        if (inFunctionCount > 0) {
            return super.visitBracket_fun(context);
        }
        Operand firstValue = visit(context.expr()).ToText("");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create("(" + firstValue.TextValue() + ")");
    }

    @Override
    public Operand visitABS_fun(ABS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitABS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitACOSH_fun(ACOSH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitACOSH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitACOS_fun(ACOS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitACOS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitAND_fun(AND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitAND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitASC_fun(ASC_funContext context) {
        inFunctionCount++;
        Operand r = super.visitASC_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitASINH_fun(ASINH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitASINH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitASIN_fun(ASIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitASIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitATAN2_fun(ATAN2_funContext context) {
        inFunctionCount++;
        Operand r = super.visitATAN2_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitATANH_fun(ATANH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitATANH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitATAN_fun(ATAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitATAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitAVEDEV_fun(AVEDEV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitAVEDEV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitAVERAGEIF_fun(AVERAGEIF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitAVERAGEIF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitAVERAGE_fun(AVERAGE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitAVERAGE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitAndOr_fun(AndOr_funContext context) {
        inFunctionCount++;
        Operand r = super.visitAndOr_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitArray_fun(Array_funContext context) {
        inFunctionCount++;
        Operand r = super.visitArray_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBASE64TOTEXT_fun(BASE64TOTEXT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBASE64TOTEXT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBASE64URLTOTEXT_fun(BASE64URLTOTEXT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBASE64URLTOTEXT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBETADIST_fun(BETADIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBETADIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBETAINV_fun(BETAINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBETAINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBIN2DEC_fun(BIN2DEC_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBIN2DEC_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBIN2HEX_fun(BIN2HEX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBIN2HEX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBIN2OCT_fun(BIN2OCT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBIN2OCT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitBINOMDIST_fun(BINOMDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitBINOMDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCEILING_fun(CEILING_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCEILING_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCHAR_fun(CHAR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCHAR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCLEAN_fun(CLEAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCLEAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCODE_fun(CODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCOMBIN_fun(COMBIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCOMBIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCONCATENATE_fun(CONCATENATE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCONCATENATE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCOSH_fun(COSH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCOSH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCOS_fun(COS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCOS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCOUNTIF_fun(COUNTIF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCOUNTIF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCOUNT_fun(COUNT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCOUNT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitCRC32_fun(CRC32_funContext context) {
        inFunctionCount++;
        Operand r = super.visitCRC32_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDATEDIF_fun(DATEDIF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDATEDIF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDATEVALUE_fun(DATEVALUE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDATEVALUE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDATE_fun(DATE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDATE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDAYS360_fun(DAYS360_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDAYS360_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDAY_fun(DAY_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDAY_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDEC2BIN_fun(DEC2BIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDEC2BIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDEC2HEX_fun(DEC2HEX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDEC2HEX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDEC2OCT_fun(DEC2OCT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDEC2OCT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDEGREES_fun(DEGREES_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDEGREES_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDEVSQ_fun(DEVSQ_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDEVSQ_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitDiyFunction_fun(DiyFunction_funContext context) {
        inFunctionCount++;
        Operand r = super.visitDiyFunction_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEDATE_fun(EDATE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEDATE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitENDSWITH_fun(ENDSWITH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitENDSWITH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEOMONTH_fun(EOMONTH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEOMONTH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEVEN_fun(EVEN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEVEN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEXACT_fun(EXACT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEXACT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEXPONDIST_fun(EXPONDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEXPONDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitEXP_fun(EXP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitEXP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitE_fun(E_funContext context) {
        inFunctionCount++;
        Operand r = super.visitE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFACTDOUBLE_fun(FACTDOUBLE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFACTDOUBLE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFACT_fun(FACT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFACT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFALSE_fun(FALSE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFALSE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFDIST_fun(FDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFIND_fun(FIND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFIND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFINV_fun(FINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFISHERINV_fun(FISHERINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFISHERINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFISHER_fun(FISHER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFISHER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFIXED_fun(FIXED_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFIXED_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitFLOOR_fun(FLOOR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitFLOOR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGAMMADIST_fun(GAMMADIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGAMMADIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGAMMAINV_fun(GAMMAINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGAMMAINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGAMMALN_fun(GAMMALN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGAMMALN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGCD_fun(GCD_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGCD_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGEOMEAN_fun(GEOMEAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGEOMEAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGUID_fun(GUID_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGUID_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitGetJsonValue_fun(GetJsonValue_funContext context) {
        inFunctionCount++;
        Operand r = super.visitGetJsonValue_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHARMEAN_fun(HARMEAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHARMEAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHEX2BIN_fun(HEX2BIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHEX2BIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHEX2DEC_fun(HEX2DEC_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHEX2DEC_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHEX2OCT_fun(HEX2OCT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHEX2OCT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHMACMD5_fun(HMACMD5_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHMACMD5_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHMACSHA1_fun(HMACSHA1_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHMACSHA1_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHMACSHA256_fun(HMACSHA256_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHMACSHA256_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHMACSHA512_fun(HMACSHA512_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHMACSHA512_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHOUR_fun(HOUR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHOUR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHTMLDECODE_fun(HTMLDECODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHTMLDECODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHTMLENCODE_fun(HTMLENCODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHTMLENCODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitHYPGEOMDIST_fun(HYPGEOMDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitHYPGEOMDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitIFERROR_fun(IFERROR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitIFERROR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitIF_fun(IF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitIF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitINDEXOF_fun(INDEXOF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitINDEXOF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitINT_fun(INT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitINT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISERROR_fun(ISERROR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISERROR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISEVEN_fun(ISEVEN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISEVEN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISLOGICAL_fun(ISLOGICAL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISLOGICAL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNONTEXT_fun(ISNONTEXT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNONTEXT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNULLOREMPTY_fun(ISNULLOREMPTY_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNULLOREMPTY_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNULLORERROR_fun(ISNULLORERROR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNULLORERROR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNULLORWHITESPACE_fun(ISNULLORWHITESPACE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNULLORWHITESPACE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNULL_fun(ISNULL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNULL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISNUMBER_fun(ISNUMBER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISNUMBER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISODD_fun(ISODD_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISODD_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISREGEX_fun(ISREGEX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISREGEX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitISTEXT_fun(ISTEXT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitISTEXT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitJIS_fun(JIS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitJIS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitJOIN_fun(JOIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitJOIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitJSON_fun(JSON_funContext context) {
        inFunctionCount++;
        Operand r = super.visitJSON_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitJudge_fun(Judge_funContext context) {
        inFunctionCount++;
        Operand r = super.visitJudge_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLARGE_fun(LARGE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLARGE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLASTINDEXOF_fun(LASTINDEXOF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLASTINDEXOF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLCM_fun(LCM_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLCM_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLEFT_fun(LEFT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLEFT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLEN_fun(LEN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLEN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLN_fun(LN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOG10_fun(LOG10_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOG10_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOGINV_fun(LOGINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOGINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOGNORMDIST_fun(LOGNORMDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOGNORMDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOG_fun(LOG_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOG_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOOKUP_fun(LOOKUP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOOKUP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitLOWER_fun(LOWER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitLOWER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMAX_fun(MAX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMAX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMD5_fun(MD5_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMD5_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMEDIAN_fun(MEDIAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMEDIAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMID_fun(MID_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMID_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMINUTE_fun(MINUTE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMINUTE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMIN_fun(MIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMODE_fun(MODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMOD_fun(MOD_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMOD_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMONTH_fun(MONTH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMONTH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMROUND_fun(MROUND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMROUND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitMULTINOMIAL_fun(MULTINOMIAL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitMULTINOMIAL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNEGBINOMDIST_fun(NEGBINOMDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNEGBINOMDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNETWORKDAYS_fun(NETWORKDAYS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNETWORKDAYS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNORMDIST_fun(NORMDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNORMDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNORMINV_fun(NORMINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNORMINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNORMSDIST_fun(NORMSDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNORMSDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNORMSINV_fun(NORMSINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNORMSINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNOT_fun(NOT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNOT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNOW_fun(NOW_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNOW_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNULL_fun(NULL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNULL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitNUM_fun(NUM_funContext context) {
        inFunctionCount++;
        Operand r = super.visitNUM_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitOCT2BIN_fun(OCT2BIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitOCT2BIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitOCT2DEC_fun(OCT2DEC_funContext context) {
        inFunctionCount++;
        Operand r = super.visitOCT2DEC_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitOCT2HEX_fun(OCT2HEX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitOCT2HEX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitODD_fun(ODD_funContext context) {
        inFunctionCount++;
        Operand r = super.visitODD_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitOR_fun(OR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitOR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPARAMETER_fun(PARAMETER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPARAMETER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPERCENTILE_fun(PERCENTILE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPERCENTILE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPERCENTRANK_fun(PERCENTRANK_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPERCENTRANK_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPERMUT_fun(PERMUT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPERMUT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPI_fun(PI_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPI_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPOISSON_fun(POISSON_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPOISSON_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPOWER_fun(POWER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPOWER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPRODUCT_fun(PRODUCT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPRODUCT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPROPER_fun(PROPER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPROPER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitParameter2(Parameter2Context context) {
        inFunctionCount++;
        Operand r = super.visitParameter2(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitPercentage_fun(Percentage_funContext context) {
        inFunctionCount++;
        Operand r = super.visitPercentage_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitQUARTILE_fun(QUARTILE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitQUARTILE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitQUOTIENT_fun(QUOTIENT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitQUOTIENT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitRADIANS_fun(RADIANS_funContext context) {
        inFunctionCount++;
        Operand r = super.visitRADIANS_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitRANDBETWEEN_fun(RANDBETWEEN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitRANDBETWEEN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitRAND_fun(RAND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitRAND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREGEXREPALCE_fun(REGEXREPALCE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREGEXREPALCE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREGEX_fun(REGEX_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREGEX_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREMOVEEND_fun(REMOVEEND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREMOVEEND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREMOVESTART_fun(REMOVESTART_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREMOVESTART_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREPLACE_fun(REPLACE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREPLACE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitREPT_fun(REPT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitREPT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitRIGHT_fun(RIGHT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitRIGHT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitRMB_fun(RMB_funContext context) {
        inFunctionCount++;
        Operand r = super.visitRMB_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitROUNDDOWN_fun(ROUNDDOWN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitROUNDDOWN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitROUNDUP_fun(ROUNDUP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitROUNDUP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitROUND_fun(ROUND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitROUND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSEARCH_fun(SEARCH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSEARCH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSECOND_fun(SECOND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSECOND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSHA1_fun(SHA1_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSHA1_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSHA256_fun(SHA256_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSHA256_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSHA512_fun(SHA512_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSHA512_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSIGN_fun(SIGN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSIGN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSINH_fun(SINH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSINH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSIN_fun(SIN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSIN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSMALL_fun(SMALL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSMALL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSPLIT_fun(SPLIT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSPLIT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSQRTPI_fun(SQRTPI_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSQRTPI_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSQRT_fun(SQRT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSQRT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSTARTSWITH_fun(STARTSWITH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSTARTSWITH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSTDEVP_fun(STDEVP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSTDEVP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSTDEV_fun(STDEV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSTDEV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSTRING_fun(STRING_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSTRING_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSUBSTITUTE_fun(SUBSTITUTE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSUBSTITUTE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSUBSTRING_fun(SUBSTRING_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSUBSTRING_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSUMIF_fun(SUMIF_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSUMIF_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSUMSQ_fun(SUMSQ_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSUMSQ_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitSUM_fun(SUM_funContext context) {
        inFunctionCount++;
        Operand r = super.visitSUM_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTANH_fun(TANH_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTANH_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTAN_fun(TAN_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTAN_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTDIST_fun(TDIST_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTDIST_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTEXTTOBASE64URL_fun(TEXTTOBASE64URL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTEXTTOBASE64URL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTEXTTOBASE64_fun(TEXTTOBASE64_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTEXTTOBASE64_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTEXT_fun(TEXT_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTEXT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTIMEVALUE_fun(TIMEVALUE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTIMEVALUE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTIME_fun(TIME_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTIME_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTINV_fun(TINV_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTINV_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTODAY_fun(TODAY_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTODAY_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTRIMEND_fun(TRIMEND_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTRIMEND_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTRIMSTART_fun(TRIMSTART_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTRIMSTART_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTRIM_fun(TRIM_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTRIM_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTRUE_fun(TRUE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTRUE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitTRUNC_fun(TRUNC_funContext context) {
        inFunctionCount++;
        Operand r = super.visitTRUNC_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitT_fun(T_funContext context) {
        inFunctionCount++;
        Operand r = super.visitT_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitUPPER_fun(UPPER_funContext context) {
        inFunctionCount++;
        Operand r = super.visitUPPER_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitURLDECODE_fun(URLDECODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitURLDECODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitURLENCODE_fun(URLENCODE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitURLENCODE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitVALUE_fun(VALUE_funContext context) {
        inFunctionCount++;
        Operand r = super.visitVALUE_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitVARP_fun(VARP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitVARP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitVAR_fun(VAR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitVAR_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitVLOOKUP_fun(VLOOKUP_funContext context) {
        inFunctionCount++;
        Operand r = super.visitVLOOKUP_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitWEEKDAY_fun(WEEKDAY_funContext context) {
        inFunctionCount++;
        Operand r = super.visitWEEKDAY_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitWEEKNUM_fun(WEEKNUM_funContext context) {
        inFunctionCount++;
        Operand r = super.visitWEEKNUM_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitWEIBULL_fun(WEIBULL_funContext context) {
        inFunctionCount++;
        Operand r = super.visitWEIBULL_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitWORKDAY_fun(WORKDAY_funContext context) {
        inFunctionCount++;
        Operand r = super.visitWORKDAY_fun(context);
        inFunctionCount--;
        return r;
    }

    @Override
    public Operand visitYEAR_fun(YEAR_funContext context) {
        inFunctionCount++;
        Operand r = super.visitYEAR_fun(context);
        inFunctionCount--;
        return r;
    }

}