package toolgood.algorithm.internals;

import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.DiyNameInfo;
import toolgood.algorithm.math.mathVisitor;

public class DiyNameVisitor extends AbstractParseTreeVisitor<Object> implements mathVisitor<Object> {
    public DiyNameInfo diy = new DiyNameInfo();

    @Override
    public Object visitPARAMETER_fun(PARAMETER_funContext context) {
        TerminalNode node = context.PARAMETER();
        if (node != null) {
            diy.Parameters.add(node.getText());
        }
        node = context.PARAMETER2();
        if (node != null) {
            String str = node.getText();
            if (str.startsWith("@")) {
                diy.Parameters.add(str.substring(1));
            } else if ((str.startsWith("【") && str.endsWith("】"))
                    || (str.startsWith("[") && str.endsWith("]"))
                    || (str.startsWith("#") && str.endsWith("#"))) {
                diy.Parameters.add(str.substring(1, str.length() - 1));

            } else {
                diy.Parameters.add(str);
            }
            return null;
        }
        return visitChildren(context);
    }
    @Override
    public Object visitDiyFunction_fun(DiyFunction_funContext context) {
        diy.Functions.add(context.PARAMETER().getText());
        return visitChildren(context);
    }

    @Override
    public Object visitParameter2(Parameter2Context context) {

        return visitChildren(context);
    }



    @Override
    public Object visitProg(ProgContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitCEILING_fun(CEILING_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFACT_fun(FACT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREGEXREPALCE_fun(REGEXREPALCE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHASVALUE_fun(HASVALUE_funContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitAddSub_fun(AddSub_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitAVERAGEIF_fun(AVERAGEIF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPARAM_fun(PARAM_funContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitRIGHT_fun(RIGHT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitOCT2BIN_fun(OCT2BIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitQUARTILE_fun(QUARTILE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFINV_fun(FINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNOT_fun(NOT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDAYS360_fun(DAYS360_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitWEEKNUM_fun(WEEKNUM_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPOISSON_fun(POISSON_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISREGEX_fun(ISREGEX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPERCENTILE_fun(PERCENTILE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSHA256_fun(SHA256_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHAS_fun(HAS_funContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitHYPGEOMDIST_fun(HYPGEOMDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPERMUT_fun(PERMUT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTRIMSTART_fun(TRIMSTART_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitRMB_fun(RMB_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDEC2HEX_fun(DEC2HEX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCLEAN_fun(CLEAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOWER_fun(LOWER_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitOR_fun(OR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitADDMONTHS_fun(ADDMONTHS_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitNORMSINV_fun(NORMSINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLEFT_fun(LEFT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISEVEN_fun(ISEVEN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOGINV_fun(LOGINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitWORKDAY_fun(WORKDAY_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitIsError_fun(IsError_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBIN2DEC_fun(BIN2DEC_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitJIS_fun(JIS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCRC32_fun(CRC32_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLCM_fun(LCM_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHARMEAN_fun(HARMEAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNORMINV_fun(NORMINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGAMMAINV_fun(GAMMAINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSQRT_fun(SQRT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDEGREES_fun(DEGREES_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMROUND_fun(MROUND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDATEDIF_fun(DATEDIF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTRIMEND_fun(TRIMEND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISLOGICAL_fun(ISLOGICAL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitINT_fun(INT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSUMIF_fun(SUMIF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHEX2OCT_fun(HEX2OCT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPI_fun(PI_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitYEAR_fun(YEAR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSQRTPI_fun(SQRTPI_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCONCATENATE_fun(CONCATENATE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCOUNT_fun(COUNT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFALSE_fun(FALSE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHTMLENCODE_fun(HTMLENCODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBASE64URLTOTEXT_fun(BASE64URLTOTEXT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOG10_fun(LOG10_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISTEXT_fun(ISTEXT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNEGBINOMDIST_fun(NEGBINOMDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNETWORKDAYS_fun(NETWORKDAYS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFACTDOUBLE_fun(FACTDOUBLE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTIMEVALUE_fun(TIMEVALUE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitAVEDEV_fun(AVEDEV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGUID_fun(GUID_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitJSON_fun(JSON_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFIXED_fun(FIXED_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGetJsonValue_fun(GetJsonValue_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTINV_fun(TINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEDATE_fun(EDATE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGEOMEAN_fun(GEOMEAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitVAR_fun(VAR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSIGN_fun(SIGN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEOMONTH_fun(EOMONTH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFLOOR_fun(FLOOR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHOUR_fun(HOUR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLEN_fun(LEN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitACOS_fun(ACOS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISNULLORWHITESPACE_fun(ISNULLORWHITESPACE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNUM_fun(NUM_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCOSH_fun(COSH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitQUOTIENT_fun(QUOTIENT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitOCT2DEC_fun(OCT2DEC_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSEARCH_fun(SEARCH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitROUNDUP_fun(ROUNDUP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCOMBIN_fun(COMBIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCODE_fun(CODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitASINH_fun(ASINH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSIN_fun(SIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSUBSTRING_fun(SUBSTRING_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitRANDBETWEEN_fun(RANDBETWEEN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitAVERAGE_fun(AVERAGE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOG_fun(LOG_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHMACSHA512_fun(HMACSHA512_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitAndOr_fun(AndOr_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSTDEVP_fun(STDEVP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitADDYEARS_fun(ADDYEARS_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitADDSECONDS_fun(ADDSECONDS_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitArray_fun(Array_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitROUND_fun(ROUND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEXP_fun(EXP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCOUNTIF_fun(COUNTIF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitVARP_fun(VARP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREMOVEEND_fun(REMOVEEND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDATE_fun(DATE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSPLIT_fun(SPLIT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitURLDECODE_fun(URLDECODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLARGE_fun(LARGE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTIMESTAMP_fun(TIMESTAMP_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitVALUE_fun(VALUE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDAY_fun(DAY_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitWEIBULL_fun(WEIBULL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHMACSHA256_fun(HMACSHA256_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBINOMDIST_fun(BINOMDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitJudge_fun(Judge_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDEVSQ_fun(DEVSQ_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMODE_fun(MODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBETAINV_fun(BETAINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMAX_fun(MAX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMINUTE_fun(MINUTE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTAN_fun(TAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitIFERROR_fun(IFERROR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFDIST_fun(FDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitINDEXOF_fun(INDEXOF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitUPPER_fun(UPPER_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHTMLDECODE_fun(HTMLDECODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEXPONDIST_fun(EXPONDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDEC2BIN_fun(DEC2BIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHEX2DEC_fun(HEX2DEC_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSMALL_fun(SMALL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitODD_fun(ODD_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTEXTTOBASE64_fun(TEXTTOBASE64_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMID_fun(MID_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPERCENTRANK_fun(PERCENTRANK_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSTDEV_fun(STDEV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNORMSDIST_fun(NORMSDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitIsNumber_fun(IsNumber_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLASTINDEXOF_fun(LASTINDEXOF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMOD_fun(MOD_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCHAR_fun(CHAR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREGEX_fun(REGEX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTEXTTOBASE64URL_fun(TEXTTOBASE64URL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMD5_fun(MD5_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREPLACE_fun(REPLACE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitACOSH_fun(ACOSH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISODD_fun(ISODD_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitASC_fun(ASC_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitCOS_fun(COS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLN_fun(LN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSTRING_fun(STRING_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHMACMD5_fun(HMACMD5_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPRODUCT_fun(PRODUCT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEXACT_fun(EXACT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitADDMINUTES_fun(ADDMINUTES_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitSUMSQ_fun(SUMSQ_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSUM_fun(SUM_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSECOND_fun(SECOND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGAMMADIST_fun(GAMMADIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitOCT2HEX_fun(OCT2HEX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTODAY_fun(TODAY_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitERROR_fun(ERROR_funContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitATAN_fun(ATAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitE_fun(E_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTRIM_fun(TRIM_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitRADIANS_fun(RADIANS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGAMMALN_fun(GAMMALN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTEXT_fun(TEXT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFISHER_fun(FISHER_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitAND_fun(AND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitArrayJson_fun(ArrayJson_funContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitBIN2HEX_fun(BIN2HEX_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMULTINOMIAL_fun(MULTINOMIAL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMONTH_fun(MONTH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitURLENCODE_fun(URLENCODE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNORMDIST_fun(NORMDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHMACSHA1_fun(HMACSHA1_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitENDSWITH_fun(ENDSWITH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBracket_fun(Bracket_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBETADIST_fun(BETADIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitATANH_fun(ATANH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNOW_fun(NOW_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMEDIAN_fun(MEDIAN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPOWER_fun(POWER_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDEC2OCT_fun(DEC2OCT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPROPER_fun(PROPER_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTRUNC_fun(TRUNC_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitGCD_fun(GCD_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTANH_fun(TANH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitHEX2BIN_fun(HEX2BIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSINH_fun(SINH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSHA512_fun(SHA512_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMIN_fun(MIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitADDDAYS_fun(ADDDAYS_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitISNONTEXT_fun(ISNONTEXT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitABS_fun(ABS_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitROUNDDOWN_fun(ROUNDDOWN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitIF_fun(IF_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitJOIN_fun(JOIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFIND_fun(FIND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSUBSTITUTE_fun(SUBSTITUTE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREPT_fun(REPT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitASIN_fun(ASIN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitMulDiv_fun(MulDiv_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitREMOVESTART_fun(REMOVESTART_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitT_fun(T_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitWEEKDAY_fun(WEEKDAY_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBIN2OCT_fun(BIN2OCT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitBASE64TOTEXT_fun(BASE64TOTEXT_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTDIST_fun(TDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitDATEVALUE_fun(DATEVALUE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSTARTSWITH_fun(STARTSWITH_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitEVEN_fun(EVEN_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOGNORMDIST_fun(LOGNORMDIST_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISNULLOREMPTY_fun(ISNULLOREMPTY_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTRUE_fun(TRUE_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitFISHERINV_fun(FISHERINV_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitSHA1_fun(SHA1_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitTIME_fun(TIME_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitATAN2_fun(ATAN2_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitADDHOURS_fun(ADDHOURS_funContext context) {
        return visitChildren(context);
    }

    @Override
    public Object visitRAND_fun(RAND_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNum(NumContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitUnit(UnitContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitArrayJson(ArrayJsonContext ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitVLOOKUP_fun(VLOOKUP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitLOOKUP_fun(LOOKUP_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitNULL_fun(NULL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISNULL_fun(ISNULL_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitISNULLORERROR_fun(ISNULLORERROR_funContext context) {

        return visitChildren(context);
    }

    @Override
    public Object visitPercentage_fun(Percentage_funContext context) {

        return visitChildren(context);
    }
    @Override
    public Object visitCOVARIANCES_fun(COVARIANCES_funContext context) {
        return visitChildren(context);
    }
    @Override
    public Object visitCOVAR_fun(COVAR_funContext context) {
        return visitChildren(context);
    }
    @Override
    public Object visitVersion_fun(Version_funContext context) {
        return visitChildren(context);
    }

}
