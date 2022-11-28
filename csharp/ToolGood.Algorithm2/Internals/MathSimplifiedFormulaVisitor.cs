using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static mathParser;

namespace ToolGood.Algorithm.Internals
{
   sealed class MathSimplifiedFormulaVisitor : MathVisitor
    {
        private int inFunctionCount = 0;

        public override Operand VisitProg(ProgContext context)
        {
            return base.VisitProg(context);
        }


        public override Operand VisitAddSub_fun(AddSub_funContext context)
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

            Operand firstValue = args[0].ToText("");
            Operand secondValue = args[1].ToText("");
            String t = context.op.Text;

            return Operand.Create($"{firstValue.TextValue} {t} {secondValue.TextValue}");
        }


        public override Operand VisitMulDiv_fun(MulDiv_funContext context)
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

            Operand firstValue = args[0].ToText("");
            Operand secondValue = args[1].ToText("");
            String t = context.op.Text;

            return Operand.Create($"{firstValue.TextValue} {t} {secondValue.TextValue}");
        }


        public override Operand VisitBracket_fun(Bracket_funContext context)
        {
            if (inFunctionCount > 0) {
                return base.VisitBracket_fun(context);
            }
            Operand firstValue = Visit(context.expr()).ToText("");
            if (firstValue.IsError) {
                return firstValue;
            }
            return Operand.Create("(" + firstValue.TextValue + ")");
        }


        public override Operand VisitABS_fun(ABS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitABS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitACOSH_fun(ACOSH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitACOSH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitACOS_fun(ACOS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitACOS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitAND_fun(AND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitAND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitASC_fun(ASC_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitASC_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitASINH_fun(ASINH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitASINH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitASIN_fun(ASIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitASIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitATAN2_fun(ATAN2_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitATAN2_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitATANH_fun(ATANH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitATANH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitATAN_fun(ATAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitATAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitAVEDEV_fun(AVEDEV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitAVEDEV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitAVERAGEIF_fun(AVERAGEIF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitAVERAGEIF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitAVERAGE_fun(AVERAGE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitAVERAGE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitAndOr_fun(AndOr_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitAndOr_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitArray_fun(Array_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitArray_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBASE64TOTEXT_fun(BASE64TOTEXT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBASE64TOTEXT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBASE64URLTOTEXT_fun(BASE64URLTOTEXT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBASE64URLTOTEXT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBETADIST_fun(BETADIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBETADIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBETAINV_fun(BETAINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBETAINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBIN2DEC_fun(BIN2DEC_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBIN2DEC_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBIN2HEX_fun(BIN2HEX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBIN2HEX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBIN2OCT_fun(BIN2OCT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBIN2OCT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitBINOMDIST_fun(BINOMDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitBINOMDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCEILING_fun(CEILING_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCEILING_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCHAR_fun(CHAR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCHAR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCLEAN_fun(CLEAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCLEAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCODE_fun(CODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCOMBIN_fun(COMBIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCOMBIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCONCATENATE_fun(CONCATENATE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCONCATENATE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCOSH_fun(COSH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCOSH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCOS_fun(COS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCOS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCOUNTIF_fun(COUNTIF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCOUNTIF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCOUNT_fun(COUNT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCOUNT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitCRC32_fun(CRC32_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitCRC32_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDATEDIF_fun(DATEDIF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDATEDIF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDATEVALUE_fun(DATEVALUE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDATEVALUE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDATE_fun(DATE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDATE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDAYS360_fun(DAYS360_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDAYS360_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDAY_fun(DAY_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDAY_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDEC2BIN_fun(DEC2BIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDEC2BIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDEC2HEX_fun(DEC2HEX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDEC2HEX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDEC2OCT_fun(DEC2OCT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDEC2OCT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDEGREES_fun(DEGREES_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDEGREES_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDEVSQ_fun(DEVSQ_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDEVSQ_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitDiyFunction_fun(DiyFunction_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitDiyFunction_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEDATE_fun(EDATE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEDATE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitENDSWITH_fun(ENDSWITH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitENDSWITH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEOMONTH_fun(EOMONTH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEOMONTH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEVEN_fun(EVEN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEVEN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEXACT_fun(EXACT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEXACT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEXPONDIST_fun(EXPONDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEXPONDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitEXP_fun(EXP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitEXP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitE_fun(E_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFACTDOUBLE_fun(FACTDOUBLE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFACTDOUBLE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFACT_fun(FACT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFACT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFALSE_fun(FALSE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFALSE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFDIST_fun(FDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFIND_fun(FIND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFIND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFINV_fun(FINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFISHERINV_fun(FISHERINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFISHERINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFISHER_fun(FISHER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFISHER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFIXED_fun(FIXED_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFIXED_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitFLOOR_fun(FLOOR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitFLOOR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGAMMADIST_fun(GAMMADIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGAMMADIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGAMMAINV_fun(GAMMAINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGAMMAINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGAMMALN_fun(GAMMALN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGAMMALN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGCD_fun(GCD_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGCD_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGEOMEAN_fun(GEOMEAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGEOMEAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGUID_fun(GUID_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGUID_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitGetJsonValue_fun(GetJsonValue_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitGetJsonValue_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHARMEAN_fun(HARMEAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHARMEAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHEX2BIN_fun(HEX2BIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHEX2BIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHEX2DEC_fun(HEX2DEC_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHEX2DEC_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHEX2OCT_fun(HEX2OCT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHEX2OCT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHMACMD5_fun(HMACMD5_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHMACMD5_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHMACSHA1_fun(HMACSHA1_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHMACSHA1_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHMACSHA256_fun(HMACSHA256_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHMACSHA256_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHMACSHA512_fun(HMACSHA512_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHMACSHA512_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHOUR_fun(HOUR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHOUR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHTMLDECODE_fun(HTMLDECODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHTMLDECODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHTMLENCODE_fun(HTMLENCODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHTMLENCODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitHYPGEOMDIST_fun(HYPGEOMDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitHYPGEOMDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitIFERROR_fun(IFERROR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitIFERROR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitIF_fun(IF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitIF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitINDEXOF_fun(INDEXOF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitINDEXOF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitINT_fun(INT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitINT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISERROR_fun(ISERROR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISERROR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISEVEN_fun(ISEVEN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISEVEN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISLOGICAL_fun(ISLOGICAL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISLOGICAL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNONTEXT_fun(ISNONTEXT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNONTEXT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNULLOREMPTY_fun(ISNULLOREMPTY_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNULLOREMPTY_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNULLORERROR_fun(ISNULLORERROR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNULLORERROR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNULLORWHITESPACE_fun(ISNULLORWHITESPACE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNULLORWHITESPACE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNULL_fun(ISNULL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNULL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISNUMBER_fun(ISNUMBER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISNUMBER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISODD_fun(ISODD_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISODD_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISREGEX_fun(ISREGEX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISREGEX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitISTEXT_fun(ISTEXT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitISTEXT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitJIS_fun(JIS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitJIS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitJOIN_fun(JOIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitJOIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitJSON_fun(JSON_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitJSON_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitJudge_fun(Judge_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitJudge_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLARGE_fun(LARGE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLARGE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLASTINDEXOF_fun(LASTINDEXOF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLASTINDEXOF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLCM_fun(LCM_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLCM_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLEFT_fun(LEFT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLEFT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLEN_fun(LEN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLEN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLN_fun(LN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOG10_fun(LOG10_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOG10_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOGINV_fun(LOGINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOGINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOGNORMDIST_fun(LOGNORMDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOGNORMDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOG_fun(LOG_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOG_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOOKUP_fun(LOOKUP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOOKUP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitLOWER_fun(LOWER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitLOWER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMAX_fun(MAX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMAX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMD5_fun(MD5_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMD5_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMEDIAN_fun(MEDIAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMEDIAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMID_fun(MID_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMID_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMINUTE_fun(MINUTE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMINUTE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMIN_fun(MIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMODE_fun(MODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMOD_fun(MOD_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMOD_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMONTH_fun(MONTH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMONTH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMROUND_fun(MROUND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMROUND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitMULTINOMIAL_fun(MULTINOMIAL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitMULTINOMIAL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNEGBINOMDIST_fun(NEGBINOMDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNEGBINOMDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNETWORKDAYS_fun(NETWORKDAYS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNETWORKDAYS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNORMDIST_fun(NORMDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNORMDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNORMINV_fun(NORMINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNORMINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNORMSDIST_fun(NORMSDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNORMSDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNORMSINV_fun(NORMSINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNORMSINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNOT_fun(NOT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNOT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNOW_fun(NOW_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNOW_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNULL_fun(NULL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNULL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitNUM_fun(NUM_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitNUM_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitOCT2BIN_fun(OCT2BIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitOCT2BIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitOCT2DEC_fun(OCT2DEC_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitOCT2DEC_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitOCT2HEX_fun(OCT2HEX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitOCT2HEX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitODD_fun(ODD_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitODD_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitOR_fun(OR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitOR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPARAMETER_fun(PARAMETER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPARAMETER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPERCENTILE_fun(PERCENTILE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPERCENTILE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPERCENTRANK_fun(PERCENTRANK_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPERCENTRANK_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPERMUT_fun(PERMUT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPERMUT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPI_fun(PI_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPI_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPOISSON_fun(POISSON_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPOISSON_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPOWER_fun(POWER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPOWER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPRODUCT_fun(PRODUCT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPRODUCT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPROPER_fun(PROPER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPROPER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitParameter2(Parameter2Context context)
        {
            inFunctionCount++;
            Operand r = base.VisitParameter2(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitPercentage_fun(Percentage_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitPercentage_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitQUARTILE_fun(QUARTILE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitQUARTILE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitQUOTIENT_fun(QUOTIENT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitQUOTIENT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitRADIANS_fun(RADIANS_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitRADIANS_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitRANDBETWEEN_fun(RANDBETWEEN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitRANDBETWEEN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitRAND_fun(RAND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitRAND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREGEXREPALCE_fun(REGEXREPALCE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREGEXREPALCE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREGEX_fun(REGEX_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREGEX_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREMOVEEND_fun(REMOVEEND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREMOVEEND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREMOVESTART_fun(REMOVESTART_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREMOVESTART_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREPLACE_fun(REPLACE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREPLACE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitREPT_fun(REPT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitREPT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitRIGHT_fun(RIGHT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitRIGHT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitRMB_fun(RMB_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitRMB_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitROUNDDOWN_fun(ROUNDDOWN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitROUNDDOWN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitROUNDUP_fun(ROUNDUP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitROUNDUP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitROUND_fun(ROUND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitROUND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSEARCH_fun(SEARCH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSEARCH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSECOND_fun(SECOND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSECOND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSHA1_fun(SHA1_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSHA1_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSHA256_fun(SHA256_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSHA256_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSHA512_fun(SHA512_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSHA512_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSIGN_fun(SIGN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSIGN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSINH_fun(SINH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSINH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSIN_fun(SIN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSIN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSMALL_fun(SMALL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSMALL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSPLIT_fun(SPLIT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSPLIT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSQRTPI_fun(SQRTPI_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSQRTPI_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSQRT_fun(SQRT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSQRT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSTARTSWITH_fun(STARTSWITH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSTARTSWITH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSTDEVP_fun(STDEVP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSTDEVP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSTDEV_fun(STDEV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSTDEV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSTRING_fun(STRING_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSTRING_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSUBSTITUTE_fun(SUBSTITUTE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSUBSTITUTE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSUBSTRING_fun(SUBSTRING_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSUBSTRING_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSUMIF_fun(SUMIF_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSUMIF_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSUMSQ_fun(SUMSQ_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSUMSQ_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitSUM_fun(SUM_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitSUM_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTANH_fun(TANH_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTANH_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTAN_fun(TAN_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTAN_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTDIST_fun(TDIST_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTDIST_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTEXTTOBASE64URL_fun(TEXTTOBASE64URL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTEXTTOBASE64URL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTEXTTOBASE64_fun(TEXTTOBASE64_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTEXTTOBASE64_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTEXT_fun(TEXT_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTEXT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTIMEVALUE_fun(TIMEVALUE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTIMEVALUE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTIME_fun(TIME_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTIME_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTINV_fun(TINV_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTINV_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTODAY_fun(TODAY_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTODAY_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTRIMEND_fun(TRIMEND_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTRIMEND_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTRIMSTART_fun(TRIMSTART_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTRIMSTART_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTRIM_fun(TRIM_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTRIM_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTRUE_fun(TRUE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTRUE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitTRUNC_fun(TRUNC_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitTRUNC_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitT_fun(T_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitT_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitUPPER_fun(UPPER_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitUPPER_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitURLDECODE_fun(URLDECODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitURLDECODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitURLENCODE_fun(URLENCODE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitURLENCODE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitVALUE_fun(VALUE_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitVALUE_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitVARP_fun(VARP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitVARP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitVAR_fun(VAR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitVAR_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitVLOOKUP_fun(VLOOKUP_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitVLOOKUP_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitWEEKDAY_fun(WEEKDAY_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitWEEKDAY_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitWEEKNUM_fun(WEEKNUM_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitWEEKNUM_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitWEIBULL_fun(WEIBULL_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitWEIBULL_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitWORKDAY_fun(WORKDAY_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitWORKDAY_fun(context);
            inFunctionCount--;
            return r;
        }


        public override Operand VisitYEAR_fun(YEAR_funContext context)
        {
            inFunctionCount++;
            Operand r = base.VisitYEAR_fun(context);
            inFunctionCount--;
            return r;
        }


    }
}
