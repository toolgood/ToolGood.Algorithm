using System;
using System.Collections.Generic;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals
{
	sealed class MathSimplifiedFormulaVisitor : MathVisitor
	{
		private int inFunctionCount = 0;

		public override Operand VisitProg(mathParser.ProgContext context)
		{
			return base.VisitProg(context);
		}


		public override Operand VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			if (inFunctionCount > 0) {
				return base.VisitAddSub_fun(context);
			}

			List<Operand> args = new List<Operand>();
			foreach (var item in context.expr()) {
				Operand aa = Visit(item);
				if (aa.IsError) {
					return aa;
				}
				args.Add(aa);
			}

			Operand firstValue = args[0].ToText();
			Operand secondValue = args[1].ToText();
			String t = context.op.Text;

			return Operand.Create($"{firstValue.TextValue} {t} {secondValue.TextValue}");
		}


		public override Operand VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			if (inFunctionCount > 0) {
				return base.VisitMulDiv_fun(context);
			}
			List<Operand> args = new List<Operand>();
			foreach (var item in context.expr()) {
				Operand aa = Visit(item);
				if (aa.IsError) {
					return aa;
				}
				args.Add(aa);
			}

			Operand firstValue = args[0].ToText();
			Operand secondValue = args[1].ToText();
			String t = context.op.Text;

			return Operand.Create($"{firstValue.TextValue} {t} {secondValue.TextValue}");
		}


		public override Operand VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			if (inFunctionCount > 0) {
				return base.VisitBracket_fun(context);
			}
			Operand firstValue = Visit(context.expr()).ToText();
			if (firstValue.IsError) {
				return firstValue;
			}
			return Operand.Create("(" + firstValue.TextValue + ")");
		}


		public override Operand VisitABS_fun(mathParser.ABS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitABS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitACOSH_fun(mathParser.ACOSH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitACOSH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitACOS_fun(mathParser.ACOS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitACOS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitAND_fun(mathParser.AND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitAND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitASC_fun(mathParser.ASC_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitASC_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitASINH_fun(mathParser.ASINH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitASINH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitASIN_fun(mathParser.ASIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitASIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitATAN2_fun(mathParser.ATAN2_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitATAN2_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitATANH_fun(mathParser.ATANH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitATANH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitATAN_fun(mathParser.ATAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitATAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitAVEDEV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitAVERAGEIF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitAVERAGE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitAndOr_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitArray_fun(mathParser.Array_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitArray_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBASE64TOTEXT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBASE64URLTOTEXT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBETADIST_fun(mathParser.BETADIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBETADIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBETAINV_fun(mathParser.BETAINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBETAINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBIN2DEC_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBIN2HEX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBIN2OCT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitBINOMDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCEILING_fun(mathParser.CEILING_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCEILING_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCHAR_fun(mathParser.CHAR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCHAR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCLEAN_fun(mathParser.CLEAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCLEAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCODE_fun(mathParser.CODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCOMBIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCONCATENATE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCOSH_fun(mathParser.COSH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCOSH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCOS_fun(mathParser.COS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCOS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCOUNTIF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCOUNT_fun(mathParser.COUNT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCOUNT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitCRC32_fun(mathParser.CRC32_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitCRC32_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDATEDIF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDATEVALUE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDATE_fun(mathParser.DATE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDATE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDAYS360_fun(mathParser.DAYS360_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDAYS360_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDAY_fun(mathParser.DAY_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDAY_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDEC2BIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDEC2HEX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDEC2OCT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDEGREES_fun(mathParser.DEGREES_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDEGREES_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDEVSQ_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitDiyFunction_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEDATE_fun(mathParser.EDATE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEDATE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitENDSWITH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEOMONTH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEVEN_fun(mathParser.EVEN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEVEN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEXACT_fun(mathParser.EXACT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEXACT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEXPONDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitEXP_fun(mathParser.EXP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitEXP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitE_fun(mathParser.E_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFACTDOUBLE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFACT_fun(mathParser.FACT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFACT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFALSE_fun(mathParser.FALSE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFALSE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFDIST_fun(mathParser.FDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFIND_fun(mathParser.FIND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFIND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFINV_fun(mathParser.FINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFISHERINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFISHER_fun(mathParser.FISHER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFISHER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFIXED_fun(mathParser.FIXED_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFIXED_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitFLOOR_fun(mathParser.FLOOR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitFLOOR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGAMMADIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGAMMAINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGAMMALN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGCD_fun(mathParser.GCD_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGCD_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGEOMEAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGUID_fun(mathParser.GUID_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGUID_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitGetJsonValue_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHARMEAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHEX2BIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHEX2DEC_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHEX2OCT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHMACMD5_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHMACSHA1_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHMACSHA256_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHMACSHA512_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHOUR_fun(mathParser.HOUR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHOUR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHTMLDECODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHTMLENCODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitHYPGEOMDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitIFERROR_fun(mathParser.IFERROR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitIFERROR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitIF_fun(mathParser.IF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitIF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitINDEXOF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitINT_fun(mathParser.INT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitINT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISERROR_fun(mathParser.ISERROR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISERROR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISEVEN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISLOGICAL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNONTEXT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNULLOREMPTY_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNULLORERROR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNULLORWHITESPACE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNULL_fun(mathParser.ISNULL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNULL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISNUMBER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISODD_fun(mathParser.ISODD_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISODD_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISREGEX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitISTEXT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitJIS_fun(mathParser.JIS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitJIS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitJOIN_fun(mathParser.JOIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitJOIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitJSON_fun(mathParser.JSON_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitJSON_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitJudge_fun(mathParser.Judge_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitJudge_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLARGE_fun(mathParser.LARGE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLARGE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLASTINDEXOF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLCM_fun(mathParser.LCM_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLCM_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLEFT_fun(mathParser.LEFT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLEFT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLEN_fun(mathParser.LEN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLEN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLN_fun(mathParser.LN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOG10_fun(mathParser.LOG10_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOG10_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOGINV_fun(mathParser.LOGINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOGINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOGNORMDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOG_fun(mathParser.LOG_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOG_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOOKUP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitLOWER_fun(mathParser.LOWER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitLOWER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMAX_fun(mathParser.MAX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMAX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMD5_fun(mathParser.MD5_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMD5_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMEDIAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMID_fun(mathParser.MID_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMID_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMINUTE_fun(mathParser.MINUTE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMINUTE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMIN_fun(mathParser.MIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMODE_fun(mathParser.MODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMOD_fun(mathParser.MOD_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMOD_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMONTH_fun(mathParser.MONTH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMONTH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMROUND_fun(mathParser.MROUND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMROUND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitMULTINOMIAL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNEGBINOMDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNETWORKDAYS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNORMDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNORMINV_fun(mathParser.NORMINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNORMINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNORMSDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNORMSINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNOT_fun(mathParser.NOT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNOT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNOW_fun(mathParser.NOW_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNOW_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNULL_fun(mathParser.NULL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNULL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitNUM_fun(mathParser.NUM_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitNUM_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitOCT2BIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitOCT2DEC_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitOCT2HEX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitODD_fun(mathParser.ODD_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitODD_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitOR_fun(mathParser.OR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitOR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPARAMETER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPERCENTILE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPERCENTRANK_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPERMUT_fun(mathParser.PERMUT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPERMUT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPI_fun(mathParser.PI_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPI_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPOISSON_fun(mathParser.POISSON_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPOISSON_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPOWER_fun(mathParser.POWER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPOWER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPRODUCT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPROPER_fun(mathParser.PROPER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPROPER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitParameter2(mathParser.Parameter2Context context)
		{
			inFunctionCount++;
			Operand r = base.VisitParameter2(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitPercentage_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitQUARTILE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitQUOTIENT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitRADIANS_fun(mathParser.RADIANS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitRADIANS_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitRANDBETWEEN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitRAND_fun(mathParser.RAND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitRAND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREGEXREPALCE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREGEX_fun(mathParser.REGEX_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREGEX_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREMOVEEND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREMOVESTART_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREPLACE_fun(mathParser.REPLACE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREPLACE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitREPT_fun(mathParser.REPT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitREPT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitRIGHT_fun(mathParser.RIGHT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitRIGHT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitRMB_fun(mathParser.RMB_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitRMB_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitROUNDDOWN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitROUNDUP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitROUND_fun(mathParser.ROUND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitROUND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSEARCH_fun(mathParser.SEARCH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSEARCH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSECOND_fun(mathParser.SECOND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSECOND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSHA1_fun(mathParser.SHA1_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSHA1_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSHA256_fun(mathParser.SHA256_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSHA256_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSHA512_fun(mathParser.SHA512_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSHA512_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSIGN_fun(mathParser.SIGN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSIGN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSINH_fun(mathParser.SINH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSINH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSIN_fun(mathParser.SIN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSIN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSMALL_fun(mathParser.SMALL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSMALL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSPLIT_fun(mathParser.SPLIT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSPLIT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSQRTPI_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSQRT_fun(mathParser.SQRT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSQRT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSTARTSWITH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSTDEVP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSTDEV_fun(mathParser.STDEV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSTDEV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSTRING_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSUBSTITUTE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSUBSTRING_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSUMIF_fun(mathParser.SUMIF_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSUMIF_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSUMSQ_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitSUM_fun(mathParser.SUM_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitSUM_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTANH_fun(mathParser.TANH_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTANH_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTAN_fun(mathParser.TAN_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTAN_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTDIST_fun(mathParser.TDIST_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTDIST_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTEXTTOBASE64URL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTEXTTOBASE64_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTEXT_fun(mathParser.TEXT_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTEXT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTIMEVALUE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTIME_fun(mathParser.TIME_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTIME_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTINV_fun(mathParser.TINV_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTINV_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTODAY_fun(mathParser.TODAY_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTODAY_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTRIMEND_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTRIMSTART_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTRIM_fun(mathParser.TRIM_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTRIM_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTRUE_fun(mathParser.TRUE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTRUE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitTRUNC_fun(mathParser.TRUNC_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitTRUNC_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitT_fun(mathParser.T_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitT_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitUPPER_fun(mathParser.UPPER_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitUPPER_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitURLDECODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitURLENCODE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitVALUE_fun(mathParser.VALUE_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitVALUE_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitVARP_fun(mathParser.VARP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitVARP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitVAR_fun(mathParser.VAR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitVAR_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitVLOOKUP_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitWEEKDAY_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitWEEKNUM_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitWEIBULL_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitWORKDAY_fun(context);
			inFunctionCount--;
			return r;
		}


		public override Operand VisitYEAR_fun(mathParser.YEAR_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitYEAR_fun(context);
			inFunctionCount--;
			return r;
		}

		public override Operand VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDYEARS_fun(context);
			inFunctionCount--;
			return r;
		}
		public override Operand VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDMONTHS_fun(context);
			inFunctionCount--;
			return r;
		}
		public override Operand VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDDAYS_fun(context);
			inFunctionCount--;
			return r;
		}
		public override Operand VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDHOURS_fun(context);
			inFunctionCount--;
			return r;
		}
		public override Operand VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDMINUTES_fun(context);
			inFunctionCount--;
			return r;
		}
		public override Operand VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
		{
			inFunctionCount++;
			Operand r = base.VisitADDSECONDS_fun(context);
			inFunctionCount--;
			return r;
		}


	}
}
