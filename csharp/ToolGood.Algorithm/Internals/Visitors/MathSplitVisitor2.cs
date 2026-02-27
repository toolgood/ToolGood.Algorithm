using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class MathSplitVisitor2 : AbstractParseTreeVisitor<CalculateTree>, ImathVisitor<CalculateTree>
	{
		public CalculateTree VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept(this);
		}

		public CalculateTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}
		public CalculateTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>()
			};
			var exprs = context.expr();
			var t = context.op.Text;
			if(CharUtil.Equals(t, '*')) {
				tree.Type = CalculateTreeType.Mul;
			} else if(CharUtil.Equals(t, '/')) {
				tree.Type = CalculateTreeType.Div;
			} else {
				tree.Type = CalculateTreeType.Mod;
			}
			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.ConditionString = context.GetText();
			return tree;
		}
		public CalculateTree VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>()
			};
			var exprs = context.expr();
			var t = context.op.Text;
			if(CharUtil.Equals(t, '+')) {
				tree.Type = CalculateTreeType.Add;
			} else if(CharUtil.Equals(t, '-')) {
				tree.Type = CalculateTreeType.Sub;
			} else {
				tree.Type = CalculateTreeType.Connect;
			}
			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.ConditionString = context.GetText();
			return tree;
		}


		public CalculateTree Visit_fun(ParserRuleContext context)
		{
			var tree = new CalculateTree {
				Start = context.Start.StartIndex,
				End = context.Stop.StopIndex,
				ConditionString = context.GetText()
			};
			return tree;
		}

		public CalculateTree VisitABS_fun(mathParser.ABS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitACOSH_fun(mathParser.ACOSH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitACOS_fun(mathParser.ACOS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitAND_fun(mathParser.AND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitArray_fun(mathParser.Array_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitASC_fun(mathParser.ASC_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitASINH_fun(mathParser.ASINH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitASIN_fun(mathParser.ASIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitATAN2_fun(mathParser.ATAN2_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitATANH_fun(mathParser.ATANH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitATAN_fun(mathParser.ATAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBETADIST_fun(mathParser.BETADIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBETAINV_fun(mathParser.BETAINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCEILING_fun(mathParser.CEILING_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCHAR_fun(mathParser.CHAR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCLEAN_fun(mathParser.CLEAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCODE_fun(mathParser.CODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOSH_fun(mathParser.COSH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOS_fun(mathParser.COS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOUNT_fun(mathParser.COUNT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitCOVAR_fun(mathParser.COVAR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDATE_fun(mathParser.DATE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDAYS360_fun(mathParser.DAYS360_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDAY_fun(mathParser.DAY_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDEGREES_fun(mathParser.DEGREES_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEDATE_fun(mathParser.EDATE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitERROR_fun(mathParser.ERROR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEVEN_fun(mathParser.EVEN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEXACT_fun(mathParser.EXACT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitEXP_fun(mathParser.EXP_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitE_fun(mathParser.E_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFACT_fun(mathParser.FACT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFALSE_fun(mathParser.FALSE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFDIST_fun(mathParser.FDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFIND_fun(mathParser.FIND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFINV_fun(mathParser.FINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFISHER_fun(mathParser.FISHER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFIXED_fun(mathParser.FIXED_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitFLOOR_fun(mathParser.FLOOR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGCD_fun(mathParser.GCD_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGUID_fun(mathParser.GUID_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHASVALUE_fun(mathParser.HASVALUE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHAS_fun(mathParser.HAS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHOUR_fun(mathParser.HOUR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitIFERROR_fun(mathParser.IFERROR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitIF_fun(mathParser.IF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitINT_fun(mathParser.INT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISERROR_fun(mathParser.ISERROR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNULL_fun(mathParser.ISNULL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISODD_fun(mathParser.ISODD_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitJIS_fun(mathParser.JIS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitJOIN_fun(mathParser.JOIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitJSON_fun(mathParser.JSON_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitJudge_fun(mathParser.Judge_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLARGE_fun(mathParser.LARGE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLCM_fun(mathParser.LCM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLEFT_fun(mathParser.LEFT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLEN_fun(mathParser.LEN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLN_fun(mathParser.LN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOG10_fun(mathParser.LOG10_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOGINV_fun(mathParser.LOGINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOG_fun(mathParser.LOG_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOWER_fun(mathParser.LOWER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMAX_fun(mathParser.MAX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMD5_fun(mathParser.MD5_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMID_fun(mathParser.MID_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMINUTE_fun(mathParser.MINUTE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMIN_fun(mathParser.MIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMODE_fun(mathParser.MODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMOD_fun(mathParser.MOD_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMONTH_fun(mathParser.MONTH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMROUND_fun(mathParser.MROUND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNORMINV_fun(mathParser.NORMINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNOT_fun(mathParser.NOT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNOW_fun(mathParser.NOW_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNum(mathParser.NumContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNUM_fun(mathParser.NUM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitODD_fun(mathParser.ODD_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitOR_fun(mathParser.OR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitParameter2(mathParser.Parameter2Context context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPARAM_fun(mathParser.PARAM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPERMUT_fun(mathParser.PERMUT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPI_fun(mathParser.PI_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPOISSON_fun(mathParser.POISSON_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPOWER_fun(mathParser.POWER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
		{
			return Visit_fun(context);
		}
 
		public CalculateTree VisitPROPER_fun(mathParser.PROPER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitRADIANS_fun(mathParser.RADIANS_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitRAND_fun(mathParser.RAND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREGEX_fun(mathParser.REGEX_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREPLACE_fun(mathParser.REPLACE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitREPT_fun(mathParser.REPT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitRIGHT_fun(mathParser.RIGHT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitRMB_fun(mathParser.RMB_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitROUND_fun(mathParser.ROUND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSEARCH_fun(mathParser.SEARCH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSECOND_fun(mathParser.SECOND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSHA1_fun(mathParser.SHA1_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSHA256_fun(mathParser.SHA256_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSHA512_fun(mathParser.SHA512_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSIGN_fun(mathParser.SIGN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSINH_fun(mathParser.SINH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSIN_fun(mathParser.SIN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSMALL_fun(mathParser.SMALL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSPLIT_fun(mathParser.SPLIT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSQRT_fun(mathParser.SQRT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSTDEV_fun(mathParser.STDEV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSUMIF_fun(mathParser.SUMIF_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitSUM_fun(mathParser.SUM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTANH_fun(mathParser.TANH_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTAN_fun(mathParser.TAN_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTDIST_fun(mathParser.TDIST_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTEXT_fun(mathParser.TEXT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTIME_fun(mathParser.TIME_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTINV_fun(mathParser.TINV_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTODAY_fun(mathParser.TODAY_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTRIM_fun(mathParser.TRIM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTRUE_fun(mathParser.TRUE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitTRUNC_fun(mathParser.TRUNC_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitT_fun(mathParser.T_funContext context)
		{
			return Visit_fun(context);
		}


		public CalculateTree VisitUPPER_fun(mathParser.UPPER_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitVALUE_fun(mathParser.VALUE_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitVARP_fun(mathParser.VARP_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitVAR_fun(mathParser.VAR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitVersion_fun(mathParser.Version_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitYEAR_fun(mathParser.YEAR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOOKFLOOR_fun(mathParser.LOOKFLOOR_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitLOOKCEILING_fun(mathParser.LOOKCEILING_funContext context)
		{
			return Visit_fun(context);
		}
	}
}
