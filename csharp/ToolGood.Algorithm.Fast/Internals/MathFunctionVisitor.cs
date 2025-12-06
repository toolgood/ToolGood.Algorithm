using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Fast.Internals
{
    public class MathFunctionVisitor : AbstractParseTreeVisitor<FunctionBase>, ImathVisitor<FunctionBase>
    {

        #region base
        public virtual FunctionBase VisitProg(mathParser.ProgContext context)
        {
            return context.expr().Accept(this);
        }
        public virtual FunctionBase VisitMulDiv_fun(mathParser.MulDiv_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var t = context.op.Text;
            if (CharUtil.Equals(t, '*')) {
                return new Function_Mul(args1, args2);
            } else if (CharUtil.Equals(t, '/')) {
                return new Function_Div(args1, args2);
            }
            return new Function_Mod(args1, args2);
        }

        public virtual FunctionBase VisitAddSub_fun(mathParser.AddSub_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var t = context.op.Text;
            if (CharUtil.Equals(t, '&')) {
                return new Function_Connect(args1, args2);
            } else if (CharUtil.Equals(t, '+')) {
                return new Function_Add(args1, args2);
            }
            return new Function_Sub(args1, args2);
        }

        public virtual FunctionBase VisitJudge_fun(mathParser.Judge_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);

            string type = context.op.Text;
            if (CharUtil.Equals(type, "=", "==", "===")) {
                return new Function_EQ(args1, args2);
            } else if (CharUtil.Equals(type, "<")) {
                return new Function_LT(args1, args2);
            } else if (CharUtil.Equals(type, "<=")) {
                return new Function_LE(args1, args2);
            } else if (CharUtil.Equals(type, ">")) {
                return new Function_GT(args1, args2);
            } else if (CharUtil.Equals(type, ">=")) {
                return new Function_GE(args1, args2);
            }
            return new Function_NE(args1, args2);
        }

        public virtual FunctionBase VisitAndOr_fun(mathParser.AndOr_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var t = context.op.Text;
            if (CharUtil.Equals(t, "&&", "AND")) {
                return new Function_AND(args1, args2);
            }
            return new Function_OR(args1, args2);
        }

        #endregion base

        #region flow

        public virtual FunctionBase VisitIF_fun(mathParser.IF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_IF(args1, args2, args3);
        }

        public virtual FunctionBase VisitIFERROR_fun(mathParser.IFERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_IFERROR(args1, args2, args3);
        }

        public virtual FunctionBase VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            var args1 = this.Visit(context.expr());
            return new Function_ISNUMBER(args1);
        }

        public virtual FunctionBase VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            var args1 = this.Visit(context.expr());
            return new Function_ISTEXT(args1);
        }

        public virtual FunctionBase VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISERROR(args1, args2);
            }
            return new Function_ISERROR(args1, null);
        }

        public virtual FunctionBase VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISNULL(args1, args2);
            }
            return new Function_ISNULL(args1, null);
        }

        public virtual FunctionBase VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISNULLORERROR(args1, args2);
            }
            return new Function_ISNULLORERROR(args1, null);
        }

        public virtual FunctionBase VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISEVEN(args1);
        }

        public virtual FunctionBase VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISLOGICAL(args1);
        }

        public virtual FunctionBase VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISODD(args1);
        }

        public virtual FunctionBase VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNONTEXT(args1);
        }

        public virtual FunctionBase VisitAND_fun(mathParser.AND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AND_N(args);
        }

        public virtual FunctionBase VisitOR_fun(mathParser.OR_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OR_N(args);
        }

        public virtual FunctionBase VisitNOT_fun(mathParser.NOT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NOT(args1);
        }

        public virtual FunctionBase VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return new Function_Value(Operand.True);

        }

        public virtual FunctionBase VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return new Function_Value(Operand.False);
        }

        #endregion flow

        #region math

        #region base

        public virtual FunctionBase VisitE_fun(mathParser.E_funContext context)
        {
            return new Function_Value(Operand.Create(Math.E));
        }

        public virtual FunctionBase VisitPI_fun(mathParser.PI_funContext context)
        {
            return new Function_Value(Operand.Create(Math.PI));
        }

        public virtual FunctionBase VisitABS_fun(mathParser.ABS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ABS(args1);
        }

        public virtual FunctionBase VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_QUOTIENT(args1, args2);
        }

        public virtual FunctionBase VisitMOD_fun(mathParser.MOD_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_Mod(args1, args2);
        }

        public virtual FunctionBase VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SIGN(args1);
        }

        public virtual FunctionBase VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SQRT(args1);
        }

        public virtual FunctionBase VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRUNC(args1);
        }

        public virtual FunctionBase VisitINT_fun(mathParser.INT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRUNC(args1);
        }

        public virtual FunctionBase VisitGCD_fun(mathParser.GCD_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_GCD(args);
        }

        public virtual FunctionBase VisitLCM_fun(mathParser.LCM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_LCM(args);
        }

        public virtual FunctionBase VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COMBIN(args1, args2);
        }

        public virtual FunctionBase VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_PERMUT(args1, args2);
        }

        public virtual FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_Percentage(args1);
        }
        #endregion base

        #region trigonometric functions

        public virtual FunctionBase VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_DEGREES(args1);
        }

        public virtual FunctionBase VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_RADIANS(args1);
        }

        public virtual FunctionBase VisitCOS_fun(mathParser.COS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_COS(args1);
        }

        public virtual FunctionBase VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_COSH(args1);
        }

        public virtual FunctionBase VisitSIN_fun(mathParser.SIN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SIN(args1);
        }

        public virtual FunctionBase VisitSINH_fun(mathParser.SINH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SINH(args1);
        }

        public virtual FunctionBase VisitTAN_fun(mathParser.TAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TAN(args1);
        }

        public virtual FunctionBase VisitTANH_fun(mathParser.TANH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TANH(args1);
        }

        public virtual FunctionBase VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ACOS(args1);
        }

        public virtual FunctionBase VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ACOSH(args1);
        }

        public virtual FunctionBase VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASIN(args1);
        }

        public virtual FunctionBase VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASINH(args1);
        }

        public virtual FunctionBase VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ATAN(args1);
        }

        public virtual FunctionBase VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ATANH(args1);
        }

        public virtual FunctionBase VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ATAN2(args1, args2);
        }

        public virtual FunctionBase VisitFIXED_fun(mathParser.FIXED_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_FIXED(args);
        }

        #endregion trigonometric functions

        #region transformation

        public virtual FunctionBase VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BIN2OCT(args);
        }

        public virtual FunctionBase VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_BIN2DEC(args1);
        }

        public virtual FunctionBase VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BIN2HEX(args);
        }

        public virtual FunctionBase VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OCT2BIN(args);
        }

        public virtual FunctionBase VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_OCT2DEC(args1);
        }

        public virtual FunctionBase VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OCT2HEX(args);
        }

        public virtual FunctionBase VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2BIN(args);
        }

        public virtual FunctionBase VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2OCT(args);
        }

        public virtual FunctionBase VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2HEX(args);
        }

        public virtual FunctionBase VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HEX2BIN(args);
        }

        public virtual FunctionBase VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HEX2OCT(args);
        }

        public virtual FunctionBase VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HEX2DEC(args1);
        }

        #endregion transformation

        #region rounding

        public virtual FunctionBase VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1) {
                return new Function_ROUND(args1, null);
            }
            var args2 = exprs[1].Accept(this);
            return new Function_ROUND(args1, args2);
        }

        public virtual FunctionBase VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ROUNDDOWN(args1, args2);
        }

        public virtual FunctionBase VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ROUNDUP(args1, args2);
        }

        public virtual FunctionBase VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1)
                return new Function_CEILING(args1, null);
            var args2 = exprs[1].Accept(this);
            return new Function_CEILING(args1, args2);
        }

        public virtual FunctionBase VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1)
                return new Function_FLOOR(args1, null);

            var args2 = exprs[1].Accept(this);
            return new Function_FLOOR(args1, args2);
        }

        public virtual FunctionBase VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_EVEN(args1);
        }

        public virtual FunctionBase VisitODD_fun(mathParser.ODD_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ODD(args1);
        }

        public virtual FunctionBase VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_MROUND(args1, args2);
        }

        #endregion rounding

        #region RAND

        public virtual FunctionBase VisitRAND_fun(mathParser.RAND_funContext context)
        {
            return new Function_RAND();

        }

        public virtual FunctionBase VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_RANDBETWEEN(args1, args2);
        }

        #endregion RAND

        #region power logarithm factorial

        public FunctionBase VisitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COVARIANCES(args1, args2);
        }

        public FunctionBase VisitCOVAR_fun(mathParser.COVAR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COVAR(args1, args2);
        }

        public virtual FunctionBase VisitFACT_fun(mathParser.FACT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FACT(args1);
        }

        public virtual FunctionBase VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FACTDOUBLE(args1);
        }

        public virtual FunctionBase VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_POWER(args1, args2);
        }

        public virtual FunctionBase VisitEXP_fun(mathParser.EXP_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_EXP(args1);
        }

        public virtual FunctionBase VisitLN_fun(mathParser.LN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LN(args1);
        }

        public virtual FunctionBase VisitLOG_fun(mathParser.LOG_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this);
            if (exprs.Length > 1) {
                var args2 = exprs[1].Accept(this);
                return new Function_LOG(args1, args2);
            }
            return new Function_LOG(args1, null);
        }

        public virtual FunctionBase VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LOG(args1, null);
        }

        public virtual FunctionBase VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MULTINOMIAL(args);
        }

        public virtual FunctionBase VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_PRODUCT(args);
        }

        public virtual FunctionBase VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SQRTPI(args1);
        }

        public virtual FunctionBase VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUMSQ(args);
        }
        #endregion power logarithm factorial

        #endregion math

        #region string

        public virtual FunctionBase VisitASC_fun(mathParser.ASC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASC(args1);
        }

        public virtual FunctionBase VisitJIS_fun(mathParser.JIS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_JIS(args1);
        }

        public virtual FunctionBase VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CHAR(args1);
        }

        public virtual FunctionBase VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CLEAN(args1);
        }

        public virtual FunctionBase VisitCODE_fun(mathParser.CODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CODE(args1);
        }

        public virtual FunctionBase VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_CONCATENATE(args);
        }

        public virtual FunctionBase VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EXACT(args1, args2);
        }

        public virtual FunctionBase VisitFIND_fun(mathParser.FIND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);

            if (exprs.Length == 2) {
                return new Function_FIND(args1, args2, null);
            }
            var count = exprs[2].Accept(this);
            return new Function_FIND(args1, args2, count);
        }

        public virtual FunctionBase VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1) {
                return new Function_LEFT(args1, null);
            }
            return new Function_LEFT(args1, exprs[1].Accept(this));
        }

        public virtual FunctionBase VisitLEN_fun(mathParser.LEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LEN(args1);
        }

        public virtual FunctionBase VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LOWER(args1);
        }

        public virtual FunctionBase VisitMID_fun(mathParser.MID_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_MID(args1, args2, args3);
        }

        public virtual FunctionBase VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_PROPER(args1);
        }

        public virtual FunctionBase VisitREPLACE_fun(mathParser.REPLACE_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 3) {
                var args22 = exprs[1].Accept(this);
                var args32 = exprs[2].Accept(this);
                return new Function_REPLACE(args1, args22, args32, null);
            }
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_REPLACE(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitREPT_fun(mathParser.REPT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_REPT(args1, args2);
        }

        public virtual FunctionBase VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (exprs.Length == 1) {
                return new Function_RIGHT(args1);
            }
            var args2 = exprs[1].Accept(this);
            return new Function_RIGHT(args1, args2);
        }

        public virtual FunctionBase VisitRMB_fun(mathParser.RMB_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_RMB(args1);
        }

        public virtual FunctionBase VisitSEARCH_fun(mathParser.SEARCH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);

            if (exprs.Length == 2) {
                return new Function_SEARCH(args1, args2, null);
            }
            var args3 = exprs[2].Accept(this);
            return new Function_SEARCH(args1, args2, args3);
        }

        public virtual FunctionBase VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            if (exprs.Length == 3) {
                return new Function_SUBSTITUTE(args1, args2, args3, null);
            }
            var args4 = exprs[3].Accept(this);
            return new Function_SUBSTITUTE(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitT_fun(mathParser.T_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_T(args1);
        }

        public virtual FunctionBase VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_TEXT(args1, args2);
        }

        public virtual FunctionBase VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRIM(args1);
        }

        public virtual FunctionBase VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_UPPER(args1);
        }

        public virtual FunctionBase VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_VALUE(args1);
        }
        #endregion string

        #region MyDate time

        public virtual FunctionBase VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DATEVALUE(args);
        }

        public FunctionBase VisitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TIMESTAMP(args);
        }

        public virtual FunctionBase VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TIMEVALUE(args1);
        }

        public virtual FunctionBase VisitDATE_fun(mathParser.DATE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DATE(args);
        }

        public virtual FunctionBase VisitTIME_fun(mathParser.TIME_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TIME(args);
        }

        public virtual FunctionBase VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return new Function_NOW();
        }

        public virtual FunctionBase VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return new Function_TODAY();
        }

        public virtual FunctionBase VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_YEAR(args1);
        }

        public virtual FunctionBase VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_MONTH(args1);
        }

        public virtual FunctionBase VisitDAY_fun(mathParser.DAY_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_DAY(args1);
        }

        public virtual FunctionBase VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HOUR(args1);
        }

        public virtual FunctionBase VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_MINUTE(args1);
        }

        public virtual FunctionBase VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SECOND(args1);
        }

        public virtual FunctionBase VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WEEKDAY(args);
        }

        public virtual FunctionBase VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_DATEDIF(args1, args2, args3);
        }

        public virtual FunctionBase VisitDAYS360_fun(mathParser.DAYS360_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this);
                return new Function_DAYS360(args1, args2, args3);
            }
            return new Function_DAYS360(args1, args2, null);
        }

        public virtual FunctionBase VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EDATE(args1, args2);

        }

        public virtual FunctionBase VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EOMONTH(args1, args2);
        }

        public virtual FunctionBase VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_NETWORKDAYS(args);
        }

        public virtual FunctionBase VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WORKDAY(args);
        }

        public virtual FunctionBase VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WEEKNUM(args);
        }

        public virtual FunctionBase VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDMONTHS(args1, args2);
        }

        public virtual FunctionBase VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDYEARS(args1, args2);
        }

        public virtual FunctionBase VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDSECONDS(args1, args2);
        }

        public virtual FunctionBase VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDMINUTES(args1, args2);
        }

        public virtual FunctionBase VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDDAYS(args1, args2);
        }

        public virtual FunctionBase VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDHOURS(args1, args2);
        }

        #endregion MyDate time

        #region sum

        public virtual FunctionBase VisitMAX_fun(mathParser.MAX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MAX(args);
        }

        public virtual FunctionBase VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MEDIAN(args);
        }

        public virtual FunctionBase VisitMIN_fun(mathParser.MIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MIN(args);
        }

        public virtual FunctionBase VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_QUARTILE(args1, args2);
        }

        public virtual FunctionBase VisitMODE_fun(mathParser.MODE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MODE(args);
        }

        public virtual FunctionBase VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_LARGE(args1, args2);
        }

        public virtual FunctionBase VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_SMALL(args1, args2);
        }

        public virtual FunctionBase VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_PERCENTILE(args1, args2);
        }

        public virtual FunctionBase VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_PERCENTRANK(args);
        }

        public virtual FunctionBase VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVERAGE(args);

        }

        public virtual FunctionBase VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVERAGEIF(args);
        }

        public virtual FunctionBase VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_GEOMEAN(args);
        }

        public virtual FunctionBase VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HARMEAN(args);
        }

        public virtual FunctionBase VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_COUNT(args);
        }

        public virtual FunctionBase VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COUNTIF(args1, args2);
        }

        public virtual FunctionBase VisitSUM_fun(mathParser.SUM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUM(args);
        }

        public virtual FunctionBase VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUMIF(args);
        }

        public virtual FunctionBase VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVEDEV(args);
        }

        public virtual FunctionBase VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STDEV(args);
        }

        public virtual FunctionBase VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STDEVP(args);
        }

        public virtual FunctionBase VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEVSQ(args);
        }

        public virtual FunctionBase VisitVAR_fun(mathParser.VAR_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VAR(args);
        }

        public virtual FunctionBase VisitVARP_fun(mathParser.VARP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VARP(args);
        }

        public virtual FunctionBase VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_NORMDIST(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_NORMINV(args1, args2, args3);


        }

        public virtual FunctionBase VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NORMSDIST(args1);
        }

        public virtual FunctionBase VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NORMSINV(args1);
        }

        public virtual FunctionBase VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_BETADIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_BETAINV(args1, args2, args3);
        }

        public virtual FunctionBase VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_BINOMDIST(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_EXPONDIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_FDIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitFINV_fun(mathParser.FINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_FINV(args1, args2, args3);
        }

        public virtual FunctionBase VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FISHER(args1);
        }

        public virtual FunctionBase VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FISHERINV(args1);
        }

        public virtual FunctionBase VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_GAMMADIST(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_GAMMAINV(args1, args2, args3);
        }

        public virtual FunctionBase VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_GAMMALN(args1);
        }

        public virtual FunctionBase VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_HYPGEOMDIST(args1, args2, args3, args4);
        }

        public virtual FunctionBase VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_LOGINV(args1, args2, args3);
        }

        public virtual FunctionBase VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_LOGNORMDIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_NEGBINOMDIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_POISSON(args1, args2, args3);
        }

        public virtual FunctionBase VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_TDIST(args1, args2, args3);
        }

        public virtual FunctionBase VisitTINV_fun(mathParser.TINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_TINV(args1, args2);
        }

        public virtual FunctionBase VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_WEIBULL(args1, args2, args3, args4);
        }
        #endregion sum

        #region csharp

        public virtual FunctionBase VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_URLENCODE(args1);
        }

        public virtual FunctionBase VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_URLDECODE(args1);
        }

        public virtual FunctionBase VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HTMLENCODE(args1);
        }

        public virtual FunctionBase VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HTMLDECODE(args1);
        }

        public virtual FunctionBase VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BASE64TOTEXT(args);
        }

        public virtual FunctionBase VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BASE64URLTOTEXT(args);
        }

        public virtual FunctionBase VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TEXTTOBASE64(args);
        }

        public virtual FunctionBase VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TEXTTOBASE64URL(args);
        }

        public virtual FunctionBase VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_REGEX(args1, args2);
        }

        public virtual FunctionBase VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_REGEXREPALCE(args1, args2, args3);
        }

        public virtual FunctionBase VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ISREGEX(args1, args2);
        }

        public virtual FunctionBase VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return new Function_GUID();
        }

        public virtual FunctionBase VisitMD5_fun(mathParser.MD5_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MD5(args);
        }

        public virtual FunctionBase VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA1(args);
        }

        public virtual FunctionBase VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA256(args);
        }

        public virtual FunctionBase VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA512(args);
        }

        public virtual FunctionBase VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_CRC32(args);
        }

        public virtual FunctionBase VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACMD5(args);
        }

        public virtual FunctionBase VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA1(args);
        }

        public virtual FunctionBase VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA256(args);
        }

        public virtual FunctionBase VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA512(args);
        }

        public virtual FunctionBase VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TRIMSTART(args);
        }

        public virtual FunctionBase VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TRIMEND(args);
        }

        public virtual FunctionBase VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_INDEXOF(args);
        }

        public virtual FunctionBase VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_LASTINDEXOF(args);
        }

        public virtual FunctionBase VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_SPLIT(args1, args2);
        }

        public virtual FunctionBase VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_JOIN(args);
        }

        public virtual FunctionBase VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUBSTRING(args);
        }

        public virtual FunctionBase VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STARTSWITH(args);
        }

        public virtual FunctionBase VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_ENDSWITH(args);
        }

        public virtual FunctionBase VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNULLOREMPTY(args1);
        }

        public virtual FunctionBase VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNULLORWHITESPACE(args1);
        }

        public virtual FunctionBase VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_REMOVESTART(args);
        }

        public virtual FunctionBase VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_REMOVEEND(args);
        }

        public virtual FunctionBase VisitJSON_fun(mathParser.JSON_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_JSON(args1);
        }

        #endregion csharp

        #region Lookup

        public virtual FunctionBase VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VLOOKUP(args);
        }

        public virtual FunctionBase VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_LOOKUP(args);
        }

        #endregion Lookup

        #region getValue

        public virtual FunctionBase VisitArray_fun(mathParser.Array_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_Array(args);
        }

        public virtual FunctionBase VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return context.expr().Accept(this);
        }

        public virtual FunctionBase VisitNUM_fun(mathParser.NUM_funContext context)
        {
            var d = decimal.Parse(context.num().GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            if (context.unit() == null) { return new Function_Value(Operand.Create(d)); }
            var unit = context.unit().GetText();
            return new Function_NUM(d, unit);
        }

        public FunctionBase VisitNum(mathParser.NumContext context)
        {
            var d = decimal.Parse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            return new Function_Value(Operand.Create(d));
        }

        public FunctionBase VisitUnit(mathParser.UnitContext context)
        {
            return new Function_Value(Operand.Create(context.GetText()));
        }

        public virtual FunctionBase VisitSTRING_fun(mathParser.STRING_funContext context)
        {
            var opd = context.GetText();
            StringBuilder sb = new StringBuilder();
            int index = 1;
            while (index < opd.Length - 1) {
                var c = opd[index++];
                if (c == '\\') {
                    var c2 = opd[index++];
                    if (c2 == 'n') sb.Append('\n');
                    else if (c2 == 'r') sb.Append('\r');
                    else if (c2 == 't') sb.Append('\t');
                    else if (c2 == '0') sb.Append('\0');
                    else if (c2 == 'v') sb.Append('\v');
                    else if (c2 == 'a') sb.Append('\a');
                    else if (c2 == 'b') sb.Append('\b');
                    else if (c2 == 'f') sb.Append('\f');
                    else sb.Append(opd[index++]);
                } else {
                    sb.Append(c);
                }
            }
            return new Function_Value(Operand.Create(sb.ToString()));
        }

        public virtual FunctionBase VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return new Function_Value(Operand.CreateNull());
        }

        public virtual FunctionBase VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
        {
            ITerminalNode node = context.PARAMETER();
            if (node != null) {
                return new Function_PARAMETER(node.GetText());
            }
            node = context.PARAMETER2();
            if (node != null) {
                string str = node.GetText();
                if (str.StartsWith('@')) {
                    return new Function_PARAMETER(str.AsSpan(1).ToString());
                }
                return new Function_PARAMETER(str.AsSpan(1, str.Length - 2).ToString());
            }
            //防止 多重引用
            if (context.expr() != null) {
                var p = context.expr().Accept(this);
                return new Function_PARAMETER(p);
            }
            return new Function_Value(Operand.Error( "Function PARAMETER first parameter is error!"));
        }

        public virtual FunctionBase VisitParameter2(mathParser.Parameter2Context context)
        {
            return new  Function_Value(Operand.Create(context.children[0].GetText()));
        }

        public virtual FunctionBase VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (args1.IsError) { return args1; }

            var obj = args1;
            Operand op;
            if (context.parameter2() != null) {
                op = context.parameter2().Accept(this);
            } else {
                op = exprs[1].Accept(this);
                if (op.IsError) {
                    op = Operand.Create(exprs[1].GetText());
                }
            }

            if (obj.Type == OperandType.ARRARY) {
                op = op.ToNumber("ARRARY index is error!");
                if (op.IsError) { return op; }
                var index = op.IntValue - excelIndex;
                if (index < obj.ArrayValue.Count)
                    return obj.ArrayValue[index];
                return Operand.Error($"ARRARY index {index} greater than maximum length!");
            }
            if (obj.Type == OperandType.ARRARYJSON) {
                if (op.Type == OperandType.NUMBER) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                } else if (op.Type == OperandType.TEXT) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                }
                return Operand.Error("Parameter name is missing!");
            }

            if (obj.Type == OperandType.JSON) {
                var json = obj.JsonValue;
                if (json.IsArray) {
                    op = op.ToNumber("JSON parameter index is error!");
                    if (op.IsError) { return op; }
                    var index = op.IntValue - excelIndex;
                    if (index < json.Count) {
                        var v = json[index];
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                    return Operand.Error($"JSON index {index} greater than maximum length!");
                } else {
                    op = op.ToText("JSON parameter name is error!");
                    if (op.IsError) { return op; }
                    var v = json[op.TextValue];
                    if (v != null) {
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                }
            }
            return Operand.Error(" Operator is error!");
        }

        public virtual FunctionBase VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            if (DiyFunction != null) {
                var funName = context.PARAMETER().GetText();
                var args = new List<Operand>();
                foreach (var item in context.expr()) { var aa = item.Accept(this); args.Add(aa); }
                return DiyFunction(mainContext, funName, args);
            }
            return Operand.Error("DiyFunction is error!");
        }

        public FunctionBase VisitPARAM_fun(mathParser.PARAM_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
            var result = GetParameter(mainContext, args1.TextValue);
            if (result.IsError) {
                if (exprs.Length == 2) {
                    return exprs[1].Accept(this);
                }
            }
            return result;
        }

        public FunctionBase VisitHAS_fun(mathParser.HAS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this).ToText("Function HAS parameter 2 is error!"); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.ARRARYJSON) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsKey(args2));
            } else if (args1.Type == OperandType.JSON) {
                var json = args1.JsonValue;
                if (json.IsArray) {
                    for (int i = 0; i < json.Count; i++) {
                        var v = json[i];
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                } else {
                    var v = json[args2.TextValue];
                    if (v != null) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            } else if (args1.Type == OperandType.ARRARY) {
                var ar = ((OperandArray)args1);
                foreach (var item in ar.ArrayValue) {
                    var t = item.ToText();
                    if (t.IsError) { continue; }
                    if (t.TextValue == args2.TextValue) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            }
            return Operand.Error("Function HAS parameter 1 is error!");
        }

        public FunctionBase VisitHASVALUE_fun([Antlr4.Runtime.Misc.NotNull] mathParser.HASVALUE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this).ToText("Function HASVALUE parameter 2 is error!"); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.ARRARYJSON) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsValue(args2));
            } else if (args1.Type == OperandType.JSON) {
                var json = args1.JsonValue;
                if (json.IsArray) {
                    for (int i = 0; i < json.Count; i++) {
                        var v = json[i];
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                } else {
                    foreach (var item in json.inst_object) {
                        var v = item.Value;
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                }
                return Operand.False;
            } else if (args1.Type == OperandType.ARRARY) {
                var ar = ((OperandArray)args1);
                foreach (var item in ar.ArrayValue) {
                    var t = item.ToText();
                    if (t.IsError) { continue; }
                    if (t.TextValue == args2.TextValue) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            }
            return Operand.Error("Function HASVALUE parameter 1 is error!");
        }

        public FunctionBase VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
        {
            OperandKeyValueList result = new OperandKeyValueList(null);
            var js = context.arrayJson();
            for (int i = 0; i < js.Length; i++) {
                var item = js[i];
                var aa = item.Accept(this); if (aa.IsError) { return aa; }
                result.AddValue((KeyValue)((OperandKeyValue)aa).Value);
            }
            return result;
        }

        public FunctionBase VisitArrayJson(mathParser.ArrayJsonContext context)
        {
            KeyValue keyValue = new KeyValue();
            if (context.NUM() != null) {
                if (int.TryParse(context.NUM().GetText(), out int key)) {
                    keyValue.Key = key.ToString();
                } else {
                    return Operand.Error("Json key '" + context.NUM().GetText() + "' is error!");
                }
            }
            if (context.STRING() != null) {
                var opd = context.STRING().GetText();
                StringBuilder sb = new StringBuilder(opd.Length - 2);
                int index = 1;
                while (index < opd.Length - 1) {
                    var c = opd[index++];
                    if (c == '\\') {
                        var c2 = opd[index++];
                        if (c2 == 'n') sb.Append('\n');
                        else if (c2 == 'r') sb.Append('\r');
                        else if (c2 == 't') sb.Append('\t');
                        else if (c2 == '0') sb.Append('\0');
                        else if (c2 == 'v') sb.Append('\v');
                        else if (c2 == 'a') sb.Append('\a');
                        else if (c2 == 'b') sb.Append('\b');
                        else if (c2 == 'f') sb.Append('\f');
                        else sb.Append(opd[index++]);
                    } else {
                        sb.Append(c);
                    }
                }
                keyValue.Key = sb.ToString();
            }
            if (context.parameter2() != null) {
                keyValue.Key = context.parameter2().GetText();
            }
            keyValue.Value = context.expr().Accept(this);
            return new OperandKeyValue(keyValue);
        }

        public FunctionBase VisitERROR_fun(mathParser.ERROR_funContext context)
        {
            if (context.expr() == null) { return new Function_Value(Operand.Error("")); }
            var args1 = context.expr().Accept(this);
            return  new Function_ERROR(args1);
        }

        public FunctionBase VisitVersion_fun(mathParser.Version_funContext context)
        {
            return new Function_Value(Operand.Version);
        }

        #endregion getValue

    }
}
