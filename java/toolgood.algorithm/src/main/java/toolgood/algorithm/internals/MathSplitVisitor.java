package toolgood.algorithm.internals;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathVisitor;

import java.util.ArrayList;

public class MathSplitVisitor extends AbstractParseTreeVisitor<ConditionTree> implements mathVisitor<ConditionTree> {
    @Override
    public ConditionTree visitProg(mathParser.ProgContext context) {
        return visit(context.expr());
    }

    @Override
    public ConditionTree visitAndOr_fun(mathParser.AndOr_funContext context) {
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
    public ConditionTree visitBracket_fun(mathParser.Bracket_funContext context) {
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
    public ConditionTree visitCEILING_fun(mathParser.CEILING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFACT_fun(mathParser.FACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHASVALUE_fun(mathParser.HASVALUE_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitAddSub_fun(mathParser.AddSub_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPARAM_fun(mathParser.PARAM_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitRIGHT_fun(mathParser.RIGHT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2BIN_fun(mathParser.OCT2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitQUARTILE_fun(mathParser.QUARTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFINV_fun(mathParser.FINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNOT_fun(mathParser.NOT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDAYS360_fun(mathParser.DAYS360_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEEKNUM_fun(mathParser.WEEKNUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPOISSON_fun(mathParser.POISSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISREGEX_fun(mathParser.ISREGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERCENTILE_fun(mathParser.PERCENTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA256_fun(mathParser.SHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHAS_fun(mathParser.HAS_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERMUT_fun(mathParser.PERMUT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIMSTART_fun(mathParser.TRIMSTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRMB_fun(mathParser.RMB_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2HEX_fun(mathParser.DEC2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCLEAN_fun(mathParser.CLEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOWER_fun(mathParser.LOWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOR_fun(mathParser.OR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMSINV_fun(mathParser.NORMSINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLEFT_fun(mathParser.LEFT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISEVEN_fun(mathParser.ISEVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOGINV_fun(mathParser.LOGINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWORKDAY_fun(mathParser.WORKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISERROR_fun(mathParser.ISERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBIN2DEC_fun(mathParser.BIN2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJIS_fun(mathParser.JIS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCRC32_fun(mathParser.CRC32_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLCM_fun(mathParser.LCM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHARMEAN_fun(mathParser.HARMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMINV_fun(mathParser.NORMINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMAINV_fun(mathParser.GAMMAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSQRT_fun(mathParser.SQRT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEGREES_fun(mathParser.DEGREES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMROUND_fun(mathParser.MROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATEDIF_fun(mathParser.DATEDIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIMEND_fun(mathParser.TRIMEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitINT_fun(mathParser.INT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUMIF_fun(mathParser.SUMIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2OCT_fun(mathParser.HEX2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPI_fun(mathParser.PI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitYEAR_fun(mathParser.YEAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSQRTPI_fun(mathParser.SQRTPI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCONCATENATE_fun(mathParser.CONCATENATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOUNT_fun(mathParser.COUNT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFALSE_fun(mathParser.FALSE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOG10_fun(mathParser.LOG10_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISTEXT_fun(mathParser.ISTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVEDEV_fun(mathParser.AVEDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGUID_fun(mathParser.GUID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJSON_fun(mathParser.JSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFIXED_fun(mathParser.FIXED_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGetJsonValue_fun(mathParser.GetJsonValue_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTINV_fun(mathParser.TINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEDATE_fun(mathParser.EDATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGEOMEAN_fun(mathParser.GEOMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVAR_fun(mathParser.VAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSIGN_fun(mathParser.SIGN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEOMONTH_fun(mathParser.EOMONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFLOOR_fun(mathParser.FLOOR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHOUR_fun(mathParser.HOUR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLEN_fun(mathParser.LEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitACOS_fun(mathParser.ACOS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNUM_fun(mathParser.NUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOSH_fun(mathParser.COSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitQUOTIENT_fun(mathParser.QUOTIENT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2DEC_fun(mathParser.OCT2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSEARCH_fun(mathParser.SEARCH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUNDUP_fun(mathParser.ROUNDUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOMBIN_fun(mathParser.COMBIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCODE_fun(mathParser.CODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASINH_fun(mathParser.ASINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSIN_fun(mathParser.SIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUBSTRING_fun(mathParser.SUBSTRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAVERAGE_fun(mathParser.AVERAGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOG_fun(mathParser.LOG_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA512_fun(mathParser.HMACSHA512_funContext context) {
        return visit_fun(context);
    }


    @Override
    public ConditionTree visitSTDEVP_fun(mathParser.STDEVP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDYEARS_fun(mathParser.ADDYEARS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDSECONDS_fun(mathParser.ADDSECONDS_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitArray_fun(mathParser.Array_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUND_fun(mathParser.ROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXP_fun(mathParser.EXP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOUNTIF_fun(mathParser.COUNTIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVARP_fun(mathParser.VARP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREMOVEEND_fun(mathParser.REMOVEEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATE_fun(mathParser.DATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPARAMETER_fun(mathParser.PARAMETER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSPLIT_fun(mathParser.SPLIT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitURLDECODE_fun(mathParser.URLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLARGE_fun(mathParser.LARGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVALUE_fun(mathParser.VALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDAY_fun(mathParser.DAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEIBULL_fun(mathParser.WEIBULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA256_fun(mathParser.HMACSHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBINOMDIST_fun(mathParser.BINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJudge_fun(mathParser.Judge_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEVSQ_fun(mathParser.DEVSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMODE_fun(mathParser.MODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBETAINV_fun(mathParser.BETAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMAX_fun(mathParser.MAX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMINUTE_fun(mathParser.MINUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTAN_fun(mathParser.TAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitIFERROR_fun(mathParser.IFERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFDIST_fun(mathParser.FDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitINDEXOF_fun(mathParser.INDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitUPPER_fun(mathParser.UPPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXPONDIST_fun(mathParser.EXPONDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2BIN_fun(mathParser.DEC2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2DEC_fun(mathParser.HEX2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSMALL_fun(mathParser.SMALL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitODD_fun(mathParser.ODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMID_fun(mathParser.MID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTDEV_fun(mathParser.STDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMSDIST_fun(mathParser.NORMSDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNUMBER_fun(mathParser.ISNUMBER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMOD_fun(mathParser.MOD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCHAR_fun(mathParser.CHAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREGEX_fun(mathParser.REGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMD5_fun(mathParser.MD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREPLACE_fun(mathParser.REPLACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitACOSH_fun(mathParser.ACOSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISODD_fun(mathParser.ISODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASC_fun(mathParser.ASC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitCOS_fun(mathParser.COS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLN_fun(mathParser.LN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTRING_fun(mathParser.STRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACMD5_fun(mathParser.HMACMD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPRODUCT_fun(mathParser.PRODUCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEXACT_fun(mathParser.EXACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUMSQ_fun(mathParser.SUMSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUM_fun(mathParser.SUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSECOND_fun(mathParser.SECOND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMADIST_fun(mathParser.GAMMADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitOCT2HEX_fun(mathParser.OCT2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTODAY_fun(mathParser.TODAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitERROR_fun(mathParser.ERROR_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitATAN_fun(mathParser.ATAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitE_fun(mathParser.E_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRIM_fun(mathParser.TRIM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRADIANS_fun(mathParser.RADIANS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGAMMALN_fun(mathParser.GAMMALN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTEXT_fun(mathParser.TEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFISHER_fun(mathParser.FISHER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitAND_fun(mathParser.AND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitArrayJson_fun(mathParser.ArrayJson_funContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitBIN2HEX_fun(mathParser.BIN2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMONTH_fun(mathParser.MONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitURLENCODE_fun(mathParser.URLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNORMDIST_fun(mathParser.NORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHMACSHA1_fun(mathParser.HMACSHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitENDSWITH_fun(mathParser.ENDSWITH_funContext context) {
        return visit_fun(context);
    }


    @Override
    public ConditionTree visitBETADIST_fun(mathParser.BETADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitATANH_fun(mathParser.ATANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNOW_fun(mathParser.NOW_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMEDIAN_fun(mathParser.MEDIAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPOWER_fun(mathParser.POWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDEC2OCT_fun(mathParser.DEC2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPROPER_fun(mathParser.PROPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRUNC_fun(mathParser.TRUNC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitGCD_fun(mathParser.GCD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTANH_fun(mathParser.TANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitHEX2BIN_fun(mathParser.HEX2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSINH_fun(mathParser.SINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA512_fun(mathParser.SHA512_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMIN_fun(mathParser.MIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDDAYS_fun(mathParser.ADDDAYS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitABS_fun(mathParser.ABS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitIF_fun(mathParser.IF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitJOIN_fun(mathParser.JOIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFIND_fun(mathParser.FIND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREPT_fun(mathParser.REPT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitASIN_fun(mathParser.ASIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitMulDiv_fun(mathParser.MulDiv_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitREMOVESTART_fun(mathParser.REMOVESTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitT_fun(mathParser.T_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitWEEKDAY_fun(mathParser.WEEKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBIN2OCT_fun(mathParser.BIN2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTDIST_fun(mathParser.TDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDATEVALUE_fun(mathParser.DATEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitEVEN_fun(mathParser.EVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTRUE_fun(mathParser.TRUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitFISHERINV_fun(mathParser.FISHERINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitSHA1_fun(mathParser.SHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitTIME_fun(mathParser.TIME_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitATAN2_fun(mathParser.ATAN2_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitADDHOURS_fun(mathParser.ADDHOURS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitRAND_fun(mathParser.RAND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNum(mathParser.NumContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitUnit(mathParser.UnitContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitArrayJson(mathParser.ArrayJsonContext ctx) {
        return visit_fun(ctx);
    }

    @Override
    public ConditionTree visitParameter2(mathParser.Parameter2Context context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitVLOOKUP_fun(mathParser.VLOOKUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitLOOKUP_fun(mathParser.LOOKUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitNULL_fun(mathParser.NULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULL_fun(mathParser.ISNULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitDiyFunction_fun(mathParser.DiyFunction_funContext context) {
        return visit_fun(context);
    }

    @Override
    public ConditionTree visitPercentage_fun(mathParser.Percentage_funContext context) {
        return visit_fun(context);
    }
}
