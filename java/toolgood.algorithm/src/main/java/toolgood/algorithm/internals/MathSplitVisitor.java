package toolgood.algorithm.internals;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import toolgood.algorithm.math.mathParser2;
import toolgood.algorithm.math.mathVisitor;

import java.util.ArrayList;

public class MathSplitVisitor extends AbstractParseTreeVisitor<ConditionTree> implements mathVisitor<ConditionTree> {
    @Override
    public ConditionTree visitProg(mathParser2.ProgContext context) {
        return visit(context.expr());
    }

    @Override
    public ConditionTree visitAndOr_fun(mathParser2.AndOr_funContext context) {
        ConditionTree tree = new ConditionTree();
        tree.Nodes = new ArrayList<>();
        String t = context.op.getText();
        if (CharUtil.Equals(t, "&&", "and")) {
            tree.Type = ConditionTreeType.And;
        } else {
            tree.Type = ConditionTreeType.Or;
        }
        tree.Nodes.add(this.visit(context.expr(0)));
        tree.Nodes.add(this.visit(context.expr(1)));
        return tree;
    }

    @Override
    public ConditionTree visitBracket_fun(mathParser2.Bracket_funContext context) {
        return visit(context.expr());
    }

    public ConditionTree visit_fun(ParserRuleContext context) {
        ConditionTree tree = new ConditionTree();
        tree.Type = ConditionTreeType.String;
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.ConditionString = context.getText();
        return tree;
    }

    @Override
    public ConditionTree visitCEILING_fun(mathParser2.CEILING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFACT_fun(mathParser2.FACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREGEXREPALCE_fun(mathParser2.REGEXREPALCE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAddSub_fun(mathParser2.AddSub_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVERAGEIF_fun(mathParser2.AVERAGEIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRIGHT_fun(mathParser2.RIGHT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2BIN_fun(mathParser2.OCT2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitQUARTILE_fun(mathParser2.QUARTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFINV_fun(mathParser2.FINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNOT_fun(mathParser2.NOT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDAYS360_fun(mathParser2.DAYS360_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEEKNUM_fun(mathParser2.WEEKNUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPOISSON_fun(mathParser2.POISSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISREGEX_fun(mathParser2.ISREGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERCENTILE_fun(mathParser2.PERCENTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA256_fun(mathParser2.SHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHYPGEOMDIST_fun(mathParser2.HYPGEOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERMUT_fun(mathParser2.PERMUT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIMSTART_fun(mathParser2.TRIMSTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRMB_fun(mathParser2.RMB_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2HEX_fun(mathParser2.DEC2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCLEAN_fun(mathParser2.CLEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOWER_fun(mathParser2.LOWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOR_fun(mathParser2.OR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMSINV_fun(mathParser2.NORMSINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLEFT_fun(mathParser2.LEFT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISEVEN_fun(mathParser2.ISEVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOGINV_fun(mathParser2.LOGINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWORKDAY_fun(mathParser2.WORKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISERROR_fun(mathParser2.ISERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBIN2DEC_fun(mathParser2.BIN2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJIS_fun(mathParser2.JIS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCRC32_fun(mathParser2.CRC32_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLCM_fun(mathParser2.LCM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHARMEAN_fun(mathParser2.HARMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMINV_fun(mathParser2.NORMINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMAINV_fun(mathParser2.GAMMAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSQRT_fun(mathParser2.SQRT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEGREES_fun(mathParser2.DEGREES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMROUND_fun(mathParser2.MROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATEDIF_fun(mathParser2.DATEDIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIMEND_fun(mathParser2.TRIMEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISLOGICAL_fun(mathParser2.ISLOGICAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitINT_fun(mathParser2.INT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUMIF_fun(mathParser2.SUMIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2OCT_fun(mathParser2.HEX2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPI_fun(mathParser2.PI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitYEAR_fun(mathParser2.YEAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSQRTPI_fun(mathParser2.SQRTPI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCONCATENATE_fun(mathParser2.CONCATENATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOUNT_fun(mathParser2.COUNT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFALSE_fun(mathParser2.FALSE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHTMLENCODE_fun(mathParser2.HTMLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBASE64URLTOTEXT_fun(mathParser2.BASE64URLTOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOG10_fun(mathParser2.LOG10_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISTEXT_fun(mathParser2.ISTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNEGBINOMDIST_fun(mathParser2.NEGBINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNETWORKDAYS_fun(mathParser2.NETWORKDAYS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFACTDOUBLE_fun(mathParser2.FACTDOUBLE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTIMEVALUE_fun(mathParser2.TIMEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVEDEV_fun(mathParser2.AVEDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGUID_fun(mathParser2.GUID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJSON_fun(mathParser2.JSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFIXED_fun(mathParser2.FIXED_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGetJsonValue_fun(mathParser2.GetJsonValue_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTINV_fun(mathParser2.TINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEDATE_fun(mathParser2.EDATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGEOMEAN_fun(mathParser2.GEOMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVAR_fun(mathParser2.VAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSIGN_fun(mathParser2.SIGN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEOMONTH_fun(mathParser2.EOMONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFLOOR_fun(mathParser2.FLOOR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHOUR_fun(mathParser2.HOUR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLEN_fun(mathParser2.LEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitACOS_fun(mathParser2.ACOS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLORWHITESPACE_fun(mathParser2.ISNULLORWHITESPACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNUM_fun(mathParser2.NUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOSH_fun(mathParser2.COSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitQUOTIENT_fun(mathParser2.QUOTIENT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2DEC_fun(mathParser2.OCT2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSEARCH_fun(mathParser2.SEARCH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUNDUP_fun(mathParser2.ROUNDUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOMBIN_fun(mathParser2.COMBIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCODE_fun(mathParser2.CODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASINH_fun(mathParser2.ASINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSIN_fun(mathParser2.SIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUBSTRING_fun(mathParser2.SUBSTRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRANDBETWEEN_fun(mathParser2.RANDBETWEEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVERAGE_fun(mathParser2.AVERAGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOG_fun(mathParser2.LOG_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA512_fun(mathParser2.HMACSHA512_funContext context) {
        return visit_fun(context);
    }


    @Override
    public ConditionTree visitSTDEVP_fun(mathParser2.STDEVP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitArray_fun(mathParser2.Array_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUND_fun(mathParser2.ROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXP_fun(mathParser2.EXP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOUNTIF_fun(mathParser2.COUNTIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVARP_fun(mathParser2.VARP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREMOVEEND_fun(mathParser2.REMOVEEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATE_fun(mathParser2.DATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPARAMETER_fun(mathParser2.PARAMETER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSPLIT_fun(mathParser2.SPLIT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitURLDECODE_fun(mathParser2.URLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLARGE_fun(mathParser2.LARGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVALUE_fun(mathParser2.VALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDAY_fun(mathParser2.DAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEIBULL_fun(mathParser2.WEIBULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA256_fun(mathParser2.HMACSHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBINOMDIST_fun(mathParser2.BINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJudge_fun(mathParser2.Judge_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEVSQ_fun(mathParser2.DEVSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMODE_fun(mathParser2.MODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBETAINV_fun(mathParser2.BETAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMAX_fun(mathParser2.MAX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMINUTE_fun(mathParser2.MINUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTAN_fun(mathParser2.TAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitIFERROR_fun(mathParser2.IFERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFDIST_fun(mathParser2.FDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitINDEXOF_fun(mathParser2.INDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitUPPER_fun(mathParser2.UPPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHTMLDECODE_fun(mathParser2.HTMLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXPONDIST_fun(mathParser2.EXPONDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2BIN_fun(mathParser2.DEC2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2DEC_fun(mathParser2.HEX2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSMALL_fun(mathParser2.SMALL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitODD_fun(mathParser2.ODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXTTOBASE64_fun(mathParser2.TEXTTOBASE64_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMID_fun(mathParser2.MID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERCENTRANK_fun(mathParser2.PERCENTRANK_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTDEV_fun(mathParser2.STDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMSDIST_fun(mathParser2.NORMSDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNUMBER_fun(mathParser2.ISNUMBER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLASTINDEXOF_fun(mathParser2.LASTINDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMOD_fun(mathParser2.MOD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCHAR_fun(mathParser2.CHAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREGEX_fun(mathParser2.REGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXTTOBASE64URL_fun(mathParser2.TEXTTOBASE64URL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMD5_fun(mathParser2.MD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREPLACE_fun(mathParser2.REPLACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitACOSH_fun(mathParser2.ACOSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISODD_fun(mathParser2.ISODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASC_fun(mathParser2.ASC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOS_fun(mathParser2.COS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLN_fun(mathParser2.LN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTRING_fun(mathParser2.STRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACMD5_fun(mathParser2.HMACMD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPRODUCT_fun(mathParser2.PRODUCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXACT_fun(mathParser2.EXACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUMSQ_fun(mathParser2.SUMSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUM_fun(mathParser2.SUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSECOND_fun(mathParser2.SECOND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMADIST_fun(mathParser2.GAMMADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2HEX_fun(mathParser2.OCT2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTODAY_fun(mathParser2.TODAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitATAN_fun(mathParser2.ATAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitE_fun(mathParser2.E_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIM_fun(mathParser2.TRIM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRADIANS_fun(mathParser2.RADIANS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMALN_fun(mathParser2.GAMMALN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXT_fun(mathParser2.TEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFISHER_fun(mathParser2.FISHER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAND_fun(mathParser2.AND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBIN2HEX_fun(mathParser2.BIN2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMULTINOMIAL_fun(mathParser2.MULTINOMIAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMONTH_fun(mathParser2.MONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitURLENCODE_fun(mathParser2.URLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMDIST_fun(mathParser2.NORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA1_fun(mathParser2.HMACSHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitENDSWITH_fun(mathParser2.ENDSWITH_funContext context) {
        return visit_fun(context);
    }


    @Override
    public ConditionTree visitBETADIST_fun(mathParser2.BETADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitATANH_fun(mathParser2.ATANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNOW_fun(mathParser2.NOW_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMEDIAN_fun(mathParser2.MEDIAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPOWER_fun(mathParser2.POWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2OCT_fun(mathParser2.DEC2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPROPER_fun(mathParser2.PROPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRUNC_fun(mathParser2.TRUNC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGCD_fun(mathParser2.GCD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTANH_fun(mathParser2.TANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2BIN_fun(mathParser2.HEX2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSINH_fun(mathParser2.SINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA512_fun(mathParser2.SHA512_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMIN_fun(mathParser2.MIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNONTEXT_fun(mathParser2.ISNONTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitABS_fun(mathParser2.ABS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUNDDOWN_fun(mathParser2.ROUNDDOWN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitIF_fun(mathParser2.IF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJOIN_fun(mathParser2.JOIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFIND_fun(mathParser2.FIND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUBSTITUTE_fun(mathParser2.SUBSTITUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREPT_fun(mathParser2.REPT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASIN_fun(mathParser2.ASIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMulDiv_fun(mathParser2.MulDiv_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREMOVESTART_fun(mathParser2.REMOVESTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitT_fun(mathParser2.T_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEEKDAY_fun(mathParser2.WEEKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBIN2OCT_fun(mathParser2.BIN2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBASE64TOTEXT_fun(mathParser2.BASE64TOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTDIST_fun(mathParser2.TDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATEVALUE_fun(mathParser2.DATEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTARTSWITH_fun(mathParser2.STARTSWITH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEVEN_fun(mathParser2.EVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOGNORMDIST_fun(mathParser2.LOGNORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLOREMPTY_fun(mathParser2.ISNULLOREMPTY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRUE_fun(mathParser2.TRUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFISHERINV_fun(mathParser2.FISHERINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA1_fun(mathParser2.SHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTIME_fun(mathParser2.TIME_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitATAN2_fun(mathParser2.ATAN2_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRAND_fun(mathParser2.RAND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitParameter2(mathParser2.Parameter2Context context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitExpr2_fun(mathParser2.Expr2_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVLOOKUP_fun(mathParser2.VLOOKUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOOKUP_fun(mathParser2.LOOKUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNULL_fun(mathParser2.NULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULL_fun(mathParser2.ISNULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLORERROR_fun(mathParser2.ISNULLORERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDiyFunction_fun(mathParser2.DiyFunction_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPercentage_fun(mathParser2.Percentage_funContext context) {
        return visit_fun(context);
    }
}
