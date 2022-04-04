using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    class MathSplitVisitor : AbstractParseTreeVisitor<ConditionTree>, ImathVisitor<ConditionTree>
    {
        public ConditionTree VisitProg(mathParser.ProgContext context)
        {
            return Visit(context.expr());
        }
        public ConditionTree VisitAndOr_fun(mathParser.AndOr_funContext context)
        {
            ConditionTree tree = new ConditionTree();
            tree.Nodes = new List<ConditionTree>();
            var t = context.op.Text;
            if (CharUtil.Equals(t, "&&", "and")) {
                tree.Type = ConditionTreeType.And;
            } else {
                tree.Type = ConditionTreeType.Or;
            }
            tree.Nodes.Add(this.Visit(context.expr(0)));
            tree.Nodes.Add(this.Visit(context.expr(1)));
            return tree;
        }
        public ConditionTree VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return Visit(context.expr());
        }
        public ConditionTree Visit_fun(ParserRuleContext context)
        {
            ConditionTree tree = new ConditionTree();
            tree.Start = context.Start.StartIndex;
            tree.End = context.Stop.StopIndex;
            tree.ConditionString = context.GetText();
            return tree;
        }


        public ConditionTree VisitIF_fun(mathParser.IF_funContext context)
        {
            return Visit_fun(context);
        }


        public ConditionTree VisitAND_fun(mathParser.AND_funContext context)
        {
            return Visit_fun(context);
        }
        public ConditionTree VisitOR_fun(mathParser.OR_funContext context)
        {
            return Visit_fun(context);
        }
        public ConditionTree VisitNOT_fun(mathParser.NOT_funContext context)
        {
            return Visit_fun(context);
        }
        public ConditionTree VisitABS_fun(mathParser.ABS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitAddSub_fun(mathParser.AddSub_funContext context)
        {
            return Visit_fun(context);
        }
        public ConditionTree VisitArray_fun(mathParser.Array_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitASC_fun(mathParser.ASC_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            return Visit_fun(context);
        }



        public ConditionTree VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCODE_fun(mathParser.CODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCOS_fun(mathParser.COS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDATE_fun(mathParser.DATE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDAYS360_fun(mathParser.DAYS360_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDAY_fun(mathParser.DAY_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitExpr2_fun(mathParser.Expr2_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitEXP_fun(mathParser.EXP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitE_fun(mathParser.E_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFACT_fun(mathParser.FACT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFIND_fun(mathParser.FIND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFINV_fun(mathParser.FINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFIXED_fun(mathParser.FIXED_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGCD_fun(mathParser.GCD_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitIFERROR_fun(mathParser.IFERROR_funContext context)
        {
            return Visit_fun(context);
        }



        public ConditionTree VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitINT_fun(mathParser.INT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitJIS_fun(mathParser.JIS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitJSON_fun(mathParser.JSON_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitJudge_fun(mathParser.Judge_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLCM_fun(mathParser.LCM_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLEN_fun(mathParser.LEN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLN_fun(mathParser.LN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOG_fun(mathParser.LOG_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMAX_fun(mathParser.MAX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMD5_fun(mathParser.MD5_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMID_fun(mathParser.MID_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMIN_fun(mathParser.MIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMODE_fun(mathParser.MODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMOD_fun(mathParser.MOD_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            return Visit_fun(context);
        }



        public ConditionTree VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitNUM_fun(mathParser.NUM_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitODD_fun(mathParser.ODD_funContext context)
        {
            return Visit_fun(context);
        }



        public ConditionTree VisitParameter2(mathParser.Parameter2Context context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPercentage_fun(mathParser.Percentage_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPI_fun(mathParser.PI_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitRAND_fun(mathParser.RAND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREPLACE_fun(mathParser.REPLACE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitREPT_fun(mathParser.REPT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitRMB_fun(mathParser.RMB_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSEARCH_fun(mathParser.SEARCH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSINH_fun(mathParser.SINH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSIN_fun(mathParser.SIN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSTRING_fun(mathParser.STRING_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitSUM_fun(mathParser.SUM_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTANH_fun(mathParser.TANH_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTAN_fun(mathParser.TAN_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTIME_fun(mathParser.TIME_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTINV_fun(mathParser.TINV_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitT_fun(mathParser.T_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitVARP_fun(mathParser.VARP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitVAR_fun(mathParser.VAR_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            return Visit_fun(context);
        }

        public ConditionTree VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            return Visit_fun(context);
        }


    }
}
