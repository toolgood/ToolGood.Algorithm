using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals
{
    internal sealed class DiyNameVisitor : AbstractParseTreeVisitor<Object>, ImathVisitor<Object>
    {
        internal DiyNameInfo diy = new DiyNameInfo();

        public object VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
        {
            ITerminalNode node = context.PARAMETER();
            if (node != null) {
                diy.Parameters.Add(new ParameterInfo(node.GetText(), node.Symbol.StartIndex, node.Symbol.StopIndex));
            }
            node = context.PARAMETER2();
            if (node != null) {
                string str = node.GetText();
                if (str.StartsWith('@')) {
                    diy.Parameters.Add(new ParameterInfo(str.AsSpan(1).ToString(), node.Symbol.StartIndex, node.Symbol.StopIndex));
                } else if ((str.StartsWith("【") && str.EndsWith("】"))
                    || (str.StartsWith("[") && str.EndsWith("]"))
                    || (str.StartsWith("#") && str.EndsWith("#"))) {
                    diy.Parameters.Add(new ParameterInfo(str.AsSpan(1, str.Length - 2).ToString(), node.Symbol.StartIndex, node.Symbol.StopIndex));
                } else {
                    diy.Parameters.Add(new ParameterInfo(str, node.Symbol.StartIndex, node.Symbol.StopIndex));
                }
            }

            return VisitChildren(context);
        }

        public object VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            diy.Functions.Add(context.PARAMETER().GetText());
            return VisitChildren(context);
        }

        public object VisitABS_fun(mathParser.ABS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAddSub_fun(mathParser.AddSub_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAndOr_fun(mathParser.AndOr_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAND_fun(mathParser.AND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitArray_fun(mathParser.Array_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitASC_fun(mathParser.ASC_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCODE_fun(mathParser.CODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOS_fun(mathParser.COS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDATE_fun(mathParser.DATE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDAYS360_fun(mathParser.DAYS360_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDAY_fun(mathParser.DAY_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitEXP_fun(mathParser.EXP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitE_fun(mathParser.E_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFACT_fun(mathParser.FACT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFIND_fun(mathParser.FIND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFINV_fun(mathParser.FINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFIXED_fun(mathParser.FIXED_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGCD_fun(mathParser.GCD_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitIFERROR_fun(mathParser.IFERROR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitIF_fun(mathParser.IF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitINT_fun(mathParser.INT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitJIS_fun(mathParser.JIS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitJSON_fun(mathParser.JSON_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitJudge_fun(mathParser.Judge_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLCM_fun(mathParser.LCM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLEN_fun(mathParser.LEN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLN_fun(mathParser.LN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOG_fun(mathParser.LOG_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMAX_fun(mathParser.MAX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMD5_fun(mathParser.MD5_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMID_fun(mathParser.MID_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMIN_fun(mathParser.MIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMODE_fun(mathParser.MODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMOD_fun(mathParser.MOD_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMulDiv_fun(mathParser.MulDiv_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNOT_fun(mathParser.NOT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNUM_fun(mathParser.NUM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitODD_fun(mathParser.ODD_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitOR_fun(mathParser.OR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitParameter2(mathParser.Parameter2Context context)
        {
            return VisitChildren(context);
        }

        public object VisitPercentage_fun(mathParser.Percentage_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPI_fun(mathParser.PI_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitProg(mathParser.ProgContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitRAND_fun(mathParser.RAND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREPLACE_fun(mathParser.REPLACE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitREPT_fun(mathParser.REPT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitRMB_fun(mathParser.RMB_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSEARCH_fun(mathParser.SEARCH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSINH_fun(mathParser.SINH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSIN_fun(mathParser.SIN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSTRING_fun(mathParser.STRING_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitSUM_fun(mathParser.SUM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTANH_fun(mathParser.TANH_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTAN_fun(mathParser.TAN_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTIME_fun(mathParser.TIME_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTINV_fun(mathParser.TINV_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitT_fun(mathParser.T_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitVARP_fun(mathParser.VARP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitVAR_fun(mathParser.VAR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitPARAM_fun(mathParser.PARAM_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHAS_fun(mathParser.HAS_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitNum(mathParser.NumContext context)
        {
            return VisitChildren(context);
        }

        public object VisitUnit(mathParser.UnitContext context)
        {
            return VisitChildren(context);
        }

        public object VisitArrayJson(mathParser.ArrayJsonContext context)
        {
            return VisitChildren(context);
        }

        public object VisitERROR_fun(mathParser.ERROR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitHASVALUE_fun(mathParser.HASVALUE_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitCOVAR_fun(mathParser.COVAR_funContext context)
        {
            return VisitChildren(context);
        }

        public object VisitVersion_fun([NotNull] mathParser.Version_funContext context)
        {
            return VisitChildren(context);
        }
    }
}