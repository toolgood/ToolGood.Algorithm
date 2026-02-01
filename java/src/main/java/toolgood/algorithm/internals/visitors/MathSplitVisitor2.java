package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathVisitor;
import toolgood.algorithm.internals.CalculateTree;

import java.util.ArrayList;
import java.util.List;

public class MathSplitVisitor2 extends AbstractParseTreeVisitor<CalculateTree> implements mathVisitor<CalculateTree> {
    @Override
    public CalculateTree visitProg(mathParser.ProgContext context) {
        return visit(context.expr());
    }

    @Override
    public CalculateTree visitBracket_fun(mathParser.Bracket_funContext context) {
        return visit(context.expr());
    }

    @Override
    public CalculateTree visitMulDiv_fun(mathParser.MulDiv_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.Nodes = new ArrayList<>();
        List<mathParser.ExprContext> exprs = context.expr();
        String t = context.op.getText();
        if (CharUtil.Equals(t, "*")) {
            tree.Type = CalculateTreeType.Mul;
        } else if (CharUtil.Equals(t, "/")) {
            tree.Type = CalculateTreeType.Div;
        } else {
            tree.Type = CalculateTreeType.Mod;
        }
        tree.Nodes.add(this.visit(exprs.get(0)));
        tree.Nodes.add(this.visit(exprs.get(1)));
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.ConditionString = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitAddSub_fun(mathParser.AddSub_funContext context) {
        CalculateTree tree = new CalculateTree();
        tree.Nodes = new ArrayList<>();
        List<mathParser.ExprContext> exprs = context.expr();
        String t = context.op.getText();
        if (CharUtil.Equals(t, "+")) {
            tree.Type = CalculateTreeType.Add;
        } else if (CharUtil.Equals(t, "-")) {
            tree.Type = CalculateTreeType.Sub;
        } else {
            tree.Type = CalculateTreeType.Connect;
        }
        tree.Nodes.add(this.visit(exprs.get(0)));
        tree.Nodes.add(this.visit(exprs.get(1)));
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.ConditionString = context.getText();
        return tree;
    }

    public CalculateTree visit_fun(ParserRuleContext context) {
        CalculateTree tree = new CalculateTree();
        tree.Start = context.start.getStartIndex();
        tree.End = context.stop.getStopIndex();
        tree.ConditionString = context.getText();
        return tree;
    }

    @Override
    public CalculateTree visitABS_fun(mathParser.ABS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitACOSH_fun(mathParser.ACOSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitACOS_fun(mathParser.ACOS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDDAYS_fun(mathParser.ADDDAYS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDHOURS_fun(mathParser.ADDHOURS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitADDYEARS_fun(mathParser.ADDYEARS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitAndOr_fun(mathParser.AndOr_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitAND_fun(mathParser.AND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitArrayJson(mathParser.ArrayJsonContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitArrayJson_fun(mathParser.ArrayJson_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitArray_fun(mathParser.Array_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitASC_fun(mathParser.ASC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitASINH_fun(mathParser.ASINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitASIN_fun(mathParser.ASIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitATAN2_fun(mathParser.ATAN2_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitATANH_fun(mathParser.ATANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitATAN_fun(mathParser.ATAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitAVEDEV_fun(mathParser.AVEDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitAVERAGE_fun(mathParser.AVERAGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBETADIST_fun(mathParser.BETADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBETAINV_fun(mathParser.BETAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBIN2DEC_fun(mathParser.BIN2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBIN2HEX_fun(mathParser.BIN2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBIN2OCT_fun(mathParser.BIN2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitBINOMDIST_fun(mathParser.BINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCEILING_fun(mathParser.CEILING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCHAR_fun(mathParser.CHAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCLEAN_fun(mathParser.CLEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCODE_fun(mathParser.CODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOMBIN_fun(mathParser.COMBIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCONCATENATE_fun(mathParser.CONCATENATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOSH_fun(mathParser.COSH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOS_fun(mathParser.COS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOUNTIF_fun(mathParser.COUNTIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOUNT_fun(mathParser.COUNT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitCOVAR_fun(mathParser.COVAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDATEDIF_fun(mathParser.DATEDIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDATEVALUE_fun(mathParser.DATEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDATE_fun(mathParser.DATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDAYS360_fun(mathParser.DAYS360_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDAY_fun(mathParser.DAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDEC2BIN_fun(mathParser.DEC2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDEC2HEX_fun(mathParser.DEC2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDEC2OCT_fun(mathParser.DEC2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDEGREES_fun(mathParser.DEGREES_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDEVSQ_fun(mathParser.DEVSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitDiyFunction_fun(mathParser.DiyFunction_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEDATE_fun(mathParser.EDATE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitENDSWITH_fun(mathParser.ENDSWITH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEOMONTH_fun(mathParser.EOMONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitERROR_fun(mathParser.ERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEVEN_fun(mathParser.EVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEXACT_fun(mathParser.EXACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEXPONDIST_fun(mathParser.EXPONDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitEXP_fun(mathParser.EXP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitE_fun(mathParser.E_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFACT_fun(mathParser.FACT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFALSE_fun(mathParser.FALSE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFDIST_fun(mathParser.FDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFIND_fun(mathParser.FIND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFINV_fun(mathParser.FINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFISHERINV_fun(mathParser.FISHERINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFISHER_fun(mathParser.FISHER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFIXED_fun(mathParser.FIXED_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitFLOOR_fun(mathParser.FLOOR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGAMMADIST_fun(mathParser.GAMMADIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGAMMAINV_fun(mathParser.GAMMAINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGAMMALN_fun(mathParser.GAMMALN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGCD_fun(mathParser.GCD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGEOMEAN_fun(mathParser.GEOMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGetJsonValue_fun(mathParser.GetJsonValue_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitGUID_fun(mathParser.GUID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHARMEAN_fun(mathParser.HARMEAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHASVALUE_fun(mathParser.HASVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHAS_fun(mathParser.HAS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHEX2BIN_fun(mathParser.HEX2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHEX2DEC_fun(mathParser.HEX2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHEX2OCT_fun(mathParser.HEX2OCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHMACMD5_fun(mathParser.HMACMD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHMACSHA1_fun(mathParser.HMACSHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHMACSHA256_fun(mathParser.HMACSHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHMACSHA512_fun(mathParser.HMACSHA512_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHOUR_fun(mathParser.HOUR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitIFERROR_fun(mathParser.IFERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitIF_fun(mathParser.IF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitINDEXOF_fun(mathParser.INDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitINT_fun(mathParser.INT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitIsError_fun(mathParser.IsError_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISEVEN_fun(mathParser.ISEVEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISNULL_fun(mathParser.ISNULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitIsNumber_fun(mathParser.IsNumber_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISODD_fun(mathParser.ISODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISREGEX_fun(mathParser.ISREGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitISTEXT_fun(mathParser.ISTEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitJIS_fun(mathParser.JIS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitJOIN_fun(mathParser.JOIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitJSON_fun(mathParser.JSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitJudge_fun(mathParser.Judge_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLARGE_fun(mathParser.LARGE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLCM_fun(mathParser.LCM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLEFT_fun(mathParser.LEFT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLEN_fun(mathParser.LEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLN_fun(mathParser.LN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOG10_fun(mathParser.LOG10_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOGINV_fun(mathParser.LOGINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOG_fun(mathParser.LOG_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOWER_fun(mathParser.LOWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMAX_fun(mathParser.MAX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMD5_fun(mathParser.MD5_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMEDIAN_fun(mathParser.MEDIAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMID_fun(mathParser.MID_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMINUTE_fun(mathParser.MINUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMIN_fun(mathParser.MIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMODE_fun(mathParser.MODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMOD_fun(mathParser.MOD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMONTH_fun(mathParser.MONTH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMROUND_fun(mathParser.MROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNORMDIST_fun(mathParser.NORMDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNORMINV_fun(mathParser.NORMINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNORMSDIST_fun(mathParser.NORMSDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNORMSINV_fun(mathParser.NORMSINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNOT_fun(mathParser.NOT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNOW_fun(mathParser.NOW_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNULL_fun(mathParser.NULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNum(mathParser.NumContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitNUM_fun(mathParser.NUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitOCT2BIN_fun(mathParser.OCT2BIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitOCT2DEC_fun(mathParser.OCT2DEC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitOCT2HEX_fun(mathParser.OCT2HEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitODD_fun(mathParser.ODD_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitOR_fun(mathParser.OR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitParameter2(mathParser.Parameter2Context context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPARAMETER_fun(mathParser.PARAMETER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPARAM_fun(mathParser.PARAM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPercentage_fun(mathParser.Percentage_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPERCENTILE_fun(mathParser.PERCENTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPERMUT_fun(mathParser.PERMUT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPI_fun(mathParser.PI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPOISSON_fun(mathParser.POISSON_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPOWER_fun(mathParser.POWER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPRODUCT_fun(mathParser.PRODUCT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitPROPER_fun(mathParser.PROPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitQUARTILE_fun(mathParser.QUARTILE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitQUOTIENT_fun(mathParser.QUOTIENT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitRADIANS_fun(mathParser.RADIANS_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitRAND_fun(mathParser.RAND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREGEX_fun(mathParser.REGEX_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREMOVEEND_fun(mathParser.REMOVEEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREMOVESTART_fun(mathParser.REMOVESTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREPLACE_fun(mathParser.REPLACE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitREPT_fun(mathParser.REPT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitRIGHT_fun(mathParser.RIGHT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitRMB_fun(mathParser.RMB_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitROUNDUP_fun(mathParser.ROUNDUP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitROUND_fun(mathParser.ROUND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSEARCH_fun(mathParser.SEARCH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSECOND_fun(mathParser.SECOND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSHA1_fun(mathParser.SHA1_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSHA256_fun(mathParser.SHA256_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSHA512_fun(mathParser.SHA512_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSIGN_fun(mathParser.SIGN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSINH_fun(mathParser.SINH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSIN_fun(mathParser.SIN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSMALL_fun(mathParser.SMALL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSPLIT_fun(mathParser.SPLIT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSQRTPI_fun(mathParser.SQRTPI_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSQRT_fun(mathParser.SQRT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSTDEVP_fun(mathParser.STDEVP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSTDEV_fun(mathParser.STDEV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSTRING_fun(mathParser.STRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSUBSTRING_fun(mathParser.SUBSTRING_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSUMIF_fun(mathParser.SUMIF_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSUMSQ_fun(mathParser.SUMSQ_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitSUM_fun(mathParser.SUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTANH_fun(mathParser.TANH_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTAN_fun(mathParser.TAN_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTDIST_fun(mathParser.TDIST_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTEXT_fun(mathParser.TEXT_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTIME_fun(mathParser.TIME_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTINV_fun(mathParser.TINV_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTODAY_fun(mathParser.TODAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTRIMEND_fun(mathParser.TRIMEND_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTRIMSTART_fun(mathParser.TRIMSTART_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTRIM_fun(mathParser.TRIM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTRUE_fun(mathParser.TRUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitTRUNC_fun(mathParser.TRUNC_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitT_fun(mathParser.T_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitUnit(mathParser.UnitContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitUPPER_fun(mathParser.UPPER_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitURLDECODE_fun(mathParser.URLDECODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitURLENCODE_fun(mathParser.URLENCODE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitVALUE_fun(mathParser.VALUE_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitVARP_fun(mathParser.VARP_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitVAR_fun(mathParser.VAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitVersion_fun(mathParser.Version_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitWEEKDAY_fun(mathParser.WEEKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitWEEKNUM_fun(mathParser.WEEKNUM_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitWEIBULL_fun(mathParser.WEIBULL_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitWORKDAY_fun(mathParser.WORKDAY_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitYEAR_fun(mathParser.YEAR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOOKFLOOR_fun(mathParser.LOOKFLOOR_funContext context) {
        return visit_fun(context);
    }

    @Override
    public CalculateTree visitLOOKCEILING_fun(mathParser.LOOKCEILING_funContext context) {
        return visit_fun(context);
    }
}
