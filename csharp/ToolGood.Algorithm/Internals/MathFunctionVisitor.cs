using Antlr4.Runtime.Tree;
using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals
{
    internal class MathFunctionVisitor : AbstractParseTreeVisitor<FunctionBase>, ImathVisitor<FunctionBase>
    {
        #region base

        public FunctionBase VisitProg(mathParser.ProgContext context)
        {
            return context.expr().Accept(this);
        }

        public FunctionBase VisitMulDiv_fun(mathParser.MulDiv_funContext context)
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

        public FunctionBase VisitAddSub_fun(mathParser.AddSub_funContext context)
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

        public FunctionBase VisitJudge_fun(mathParser.Judge_funContext context)
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

        public FunctionBase VisitAndOr_fun(mathParser.AndOr_funContext context)
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

        public FunctionBase VisitIF_fun(mathParser.IF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this);
                return new Function_IF(args1, args2, args3);
            }
            return new Function_IF(args1, args2, null);
        }

        public FunctionBase VisitIFERROR_fun(mathParser.IFERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_IFERROR(args1, args2, args3);
        }

        public FunctionBase VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            var args1 = this.Visit(context.expr());
            return new Function_ISNUMBER(args1);
        }

        public FunctionBase VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            var args1 = this.Visit(context.expr());
            return new Function_ISTEXT(args1);
        }

        public FunctionBase VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISERROR(args1, args2);
            }
            return new Function_ISERROR(args1, null);
        }

        public FunctionBase VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISNULL(args1, args2);
            }
            return new Function_ISNULL(args1, null);
        }

        public FunctionBase VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this);
                return new Function_ISNULLORERROR(args1, args2);
            }
            return new Function_ISNULLORERROR(args1, null);
        }

        public FunctionBase VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISEVEN(args1);
        }

        public FunctionBase VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISLOGICAL(args1);
        }

        public FunctionBase VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISODD(args1);
        }

        public FunctionBase VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNONTEXT(args1);
        }

        public FunctionBase VisitAND_fun(mathParser.AND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AND_N(args);
        }

        public FunctionBase VisitOR_fun(mathParser.OR_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OR_N(args);
        }

        public FunctionBase VisitNOT_fun(mathParser.NOT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NOT(args1);
        }

        public FunctionBase VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return new Function_Value(Operand.True);
        }

        public FunctionBase VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return new Function_Value(Operand.False);
        }

        #endregion flow

        #region math

        #region base

        public FunctionBase VisitE_fun(mathParser.E_funContext context)
        {
            return new Function_Value(Operand.Create(Math.E),"E");
        }

        public FunctionBase VisitPI_fun(mathParser.PI_funContext context)
        {
            return new Function_Value(Operand.Create(Math.PI),"PI");
        }

        public FunctionBase VisitABS_fun(mathParser.ABS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ABS(args1);
        }

        public FunctionBase VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_QUOTIENT(args1, args2);
        }

        public FunctionBase VisitMOD_fun(mathParser.MOD_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_Mod(args1, args2);
        }

        public FunctionBase VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SIGN(args1);
        }

        public FunctionBase VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SQRT(args1);
        }

        public FunctionBase VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRUNC(args1);
        }

        public FunctionBase VisitINT_fun(mathParser.INT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRUNC(args1);
        }

        public FunctionBase VisitGCD_fun(mathParser.GCD_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_GCD(args);
        }

        public FunctionBase VisitLCM_fun(mathParser.LCM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_LCM(args);
        }

        public FunctionBase VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COMBIN(args1, args2);
        }

        public FunctionBase VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_PERMUT(args1, args2);
        }

        public FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_Percentage(args1);
        }

        #endregion base

        #region trigonometric functions

        public FunctionBase VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_DEGREES(args1);
        }

        public FunctionBase VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_RADIANS(args1);
        }

        public FunctionBase VisitCOS_fun(mathParser.COS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_COS(args1);
        }

        public FunctionBase VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_COSH(args1);
        }

        public FunctionBase VisitSIN_fun(mathParser.SIN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SIN(args1);
        }

        public FunctionBase VisitSINH_fun(mathParser.SINH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SINH(args1);
        }

        public FunctionBase VisitTAN_fun(mathParser.TAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TAN(args1);
        }

        public FunctionBase VisitTANH_fun(mathParser.TANH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TANH(args1);
        }

        public FunctionBase VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ACOS(args1);
        }

        public FunctionBase VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ACOSH(args1);
        }

        public FunctionBase VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASIN(args1);
        }

        public FunctionBase VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASINH(args1);
        }

        public FunctionBase VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ATAN(args1);
        }

        public FunctionBase VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ATANH(args1);
        }

        public FunctionBase VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ATAN2(args1, args2);
        }

        public FunctionBase VisitFIXED_fun(mathParser.FIXED_funContext context)
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

        public FunctionBase VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BIN2OCT(args);
        }

        public FunctionBase VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_BIN2DEC(args1);
        }

        public FunctionBase VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BIN2HEX(args);
        }

        public FunctionBase VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OCT2BIN(args);
        }

        public FunctionBase VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_OCT2DEC(args1);
        }

        public FunctionBase VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_OCT2HEX(args);
        }

        public FunctionBase VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2BIN(args);
        }

        public FunctionBase VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2OCT(args);
        }

        public FunctionBase VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEC2HEX(args);
        }

        public FunctionBase VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HEX2BIN(args);
        }

        public FunctionBase VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HEX2OCT(args);
        }

        public FunctionBase VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HEX2DEC(args1);
        }

        #endregion transformation

        #region rounding

        public FunctionBase VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1) {
                return new Function_ROUND(args1, null);
            }
            var args2 = exprs[1].Accept(this);
            return new Function_ROUND(args1, args2);
        }

        public FunctionBase VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ROUNDDOWN(args1, args2);
        }

        public FunctionBase VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ROUNDUP(args1, args2);
        }

        public FunctionBase VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1)
                return new Function_CEILING(args1, null);
            var args2 = exprs[1].Accept(this);
            return new Function_CEILING(args1, args2);
        }

        public FunctionBase VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1)
                return new Function_FLOOR(args1, null);

            var args2 = exprs[1].Accept(this);
            return new Function_FLOOR(args1, args2);
        }

        public FunctionBase VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_EVEN(args1);
        }

        public FunctionBase VisitODD_fun(mathParser.ODD_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ODD(args1);
        }

        public FunctionBase VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_MROUND(args1, args2);
        }

        #endregion rounding

        #region RAND

        public FunctionBase VisitRAND_fun(mathParser.RAND_funContext context)
        {
            return new Function_RAND();
        }

        public FunctionBase VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
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

        public FunctionBase VisitFACT_fun(mathParser.FACT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FACT(args1);
        }

        public FunctionBase VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FACTDOUBLE(args1);
        }

        public FunctionBase VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_POWER(args1, args2);
        }

        public FunctionBase VisitEXP_fun(mathParser.EXP_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_EXP(args1);
        }

        public FunctionBase VisitLN_fun(mathParser.LN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LN(args1);
        }

        public FunctionBase VisitLOG_fun(mathParser.LOG_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this);
            if (exprs.Length > 1) {
                var args2 = exprs[1].Accept(this);
                return new Function_LOG(args1, args2);
            }
            return new Function_LOG(args1, null);
        }

        public FunctionBase VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LOG(args1, null);
        }

        public FunctionBase VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MULTINOMIAL(args);
        }

        public FunctionBase VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_PRODUCT(args);
        }

        public FunctionBase VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SQRTPI(args1);
        }

        public FunctionBase VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
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

        public FunctionBase VisitASC_fun(mathParser.ASC_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ASC(args1);
        }

        public FunctionBase VisitJIS_fun(mathParser.JIS_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_JIS(args1);
        }

        public FunctionBase VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CHAR(args1);
        }

        public FunctionBase VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CLEAN(args1);
        }

        public FunctionBase VisitCODE_fun(mathParser.CODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_CODE(args1);
        }

        public FunctionBase VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_CONCATENATE(args);
        }

        public FunctionBase VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EXACT(args1, args2);
        }

        public FunctionBase VisitFIND_fun(mathParser.FIND_funContext context)
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

        public FunctionBase VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1) {
                return new Function_LEFT(args1, null);
            }
            return new Function_LEFT(args1, exprs[1].Accept(this));
        }

        public FunctionBase VisitLEN_fun(mathParser.LEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LEN(args1);
        }

        public FunctionBase VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_LOWER(args1);
        }

        public FunctionBase VisitMID_fun(mathParser.MID_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_MID(args1, args2, args3);
        }

        public FunctionBase VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_PROPER(args1);
        }

        public FunctionBase VisitREPLACE_fun(mathParser.REPLACE_funContext context)
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

        public FunctionBase VisitREPT_fun(mathParser.REPT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_REPT(args1, args2);
        }

        public FunctionBase VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (exprs.Length == 1) {
                return new Function_RIGHT(args1);
            }
            var args2 = exprs[1].Accept(this);
            return new Function_RIGHT(args1, args2);
        }

        public FunctionBase VisitRMB_fun(mathParser.RMB_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_RMB(args1);
        }

        public FunctionBase VisitSEARCH_fun(mathParser.SEARCH_funContext context)
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

        public FunctionBase VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
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

        public FunctionBase VisitT_fun(mathParser.T_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_T(args1);
        }

        public FunctionBase VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_TEXT(args1, args2);
        }

        public FunctionBase VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TRIM(args1);
        }

        public FunctionBase VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_UPPER(args1);
        }

        public FunctionBase VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_VALUE(args1);
        }

        #endregion string

        #region MyDate time

        public FunctionBase VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
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

        public FunctionBase VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_TIMEVALUE(args1);
        }

        public FunctionBase VisitDATE_fun(mathParser.DATE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DATE(args);
        }

        public FunctionBase VisitTIME_fun(mathParser.TIME_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TIME(args);
        }

        public FunctionBase VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return new Function_NOW();
        }

        public FunctionBase VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return new Function_TODAY();
        }

        public FunctionBase VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_YEAR(args1);
        }

        public FunctionBase VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_MONTH(args1);
        }

        public FunctionBase VisitDAY_fun(mathParser.DAY_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_DAY(args1);
        }

        public FunctionBase VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HOUR(args1);
        }

        public FunctionBase VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_MINUTE(args1);
        }

        public FunctionBase VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_SECOND(args1);
        }

        public FunctionBase VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WEEKDAY(args);
        }

        public FunctionBase VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_DATEDIF(args1, args2, args3);
        }

        public FunctionBase VisitDAYS360_fun(mathParser.DAYS360_funContext context)
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

        public FunctionBase VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EDATE(args1, args2);
        }

        public FunctionBase VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_EOMONTH(args1, args2);
        }

        public FunctionBase VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_NETWORKDAYS(args);
        }

        public FunctionBase VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WORKDAY(args);
        }

        public FunctionBase VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_WEEKNUM(args);
        }

        public FunctionBase VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDMONTHS(args1, args2);
        }

        public FunctionBase VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDYEARS(args1, args2);
        }

        public FunctionBase VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDSECONDS(args1, args2);
        }

        public FunctionBase VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDMINUTES(args1, args2);
        }

        public FunctionBase VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDDAYS(args1, args2);
        }

        public FunctionBase VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ADDHOURS(args1, args2);
        }

        #endregion MyDate time

        #region sum

        public FunctionBase VisitMAX_fun(mathParser.MAX_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MAX(args);
        }

        public FunctionBase VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MEDIAN(args);
        }

        public FunctionBase VisitMIN_fun(mathParser.MIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MIN(args);
        }

        public FunctionBase VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_QUARTILE(args1, args2);
        }

        public FunctionBase VisitMODE_fun(mathParser.MODE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MODE(args);
        }

        public FunctionBase VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_LARGE(args1, args2);
        }

        public FunctionBase VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_SMALL(args1, args2);
        }

        public FunctionBase VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_PERCENTILE(args1, args2);
        }

        public FunctionBase VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_PERCENTRANK(args);
        }

        public FunctionBase VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVERAGE(args);
        }

        public FunctionBase VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVERAGEIF(args);
        }

        public FunctionBase VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_GEOMEAN(args);
        }

        public FunctionBase VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HARMEAN(args);
        }

        public FunctionBase VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_COUNT(args);
        }

        public FunctionBase VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_COUNTIF(args1, args2);
        }

        public FunctionBase VisitSUM_fun(mathParser.SUM_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUM(args);
        }

        public FunctionBase VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUMIF(args);
        }

        public FunctionBase VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_AVEDEV(args);
        }

        public FunctionBase VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STDEV(args);
        }

        public FunctionBase VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STDEVP(args);
        }

        public FunctionBase VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DEVSQ(args);
        }

        public FunctionBase VisitVAR_fun(mathParser.VAR_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VAR(args);
        }

        public FunctionBase VisitVARP_fun(mathParser.VARP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VARP(args);
        }

        public FunctionBase VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_NORMDIST(args1, args2, args3, args4);
        }

        public FunctionBase VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_NORMINV(args1, args2, args3);
        }

        public FunctionBase VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NORMSDIST(args1);
        }

        public FunctionBase VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_NORMSINV(args1);
        }

        public FunctionBase VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_BETADIST(args1, args2, args3);
        }

        public FunctionBase VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_BETAINV(args1, args2, args3);
        }

        public FunctionBase VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_BINOMDIST(args1, args2, args3, args4);
        }

        public FunctionBase VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_EXPONDIST(args1, args2, args3);
        }

        public FunctionBase VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_FDIST(args1, args2, args3);
        }

        public FunctionBase VisitFINV_fun(mathParser.FINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_FINV(args1, args2, args3);
        }

        public FunctionBase VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FISHER(args1);
        }

        public FunctionBase VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_FISHERINV(args1);
        }

        public FunctionBase VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_GAMMADIST(args1, args2, args3, args4);
        }

        public FunctionBase VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_GAMMAINV(args1, args2, args3);
        }

        public FunctionBase VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_GAMMALN(args1);
        }

        public FunctionBase VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            var args4 = exprs[3].Accept(this);
            return new Function_HYPGEOMDIST(args1, args2, args3, args4);
        }

        public FunctionBase VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_LOGINV(args1, args2, args3);
        }

        public FunctionBase VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_LOGNORMDIST(args1, args2, args3);
        }

        public FunctionBase VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_NEGBINOMDIST(args1, args2, args3);
        }

        public FunctionBase VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_POISSON(args1, args2, args3);
        }

        public FunctionBase VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_TDIST(args1, args2, args3);
        }

        public FunctionBase VisitTINV_fun(mathParser.TINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_TINV(args1, args2);
        }

        public FunctionBase VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
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

        public FunctionBase VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_URLENCODE(args1);
        }

        public FunctionBase VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_URLDECODE(args1);
        }

        public FunctionBase VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HTMLENCODE(args1);
        }

        public FunctionBase VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_HTMLDECODE(args1);
        }

        public FunctionBase VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BASE64TOTEXT(args);
        }

        public FunctionBase VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_BASE64URLTOTEXT(args);
        }

        public FunctionBase VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TEXTTOBASE64(args);
        }

        public FunctionBase VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TEXTTOBASE64URL(args);
        }

        public FunctionBase VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_REGEX(args1, args2);
        }

        public FunctionBase VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            var args3 = exprs[2].Accept(this);
            return new Function_REGEXREPALCE(args1, args2, args3);
        }

        public FunctionBase VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_ISREGEX(args1, args2);
        }

        public FunctionBase VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return new Function_GUID();
        }

        public FunctionBase VisitMD5_fun(mathParser.MD5_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_MD5(args);
        }

        public FunctionBase VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA1(args);
        }

        public FunctionBase VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA256(args);
        }

        public FunctionBase VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SHA512(args);
        }

        public FunctionBase VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_CRC32(args);
        }

        public FunctionBase VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACMD5(args);
        }

        public FunctionBase VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA1(args);
        }

        public FunctionBase VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA256(args);
        }

        public FunctionBase VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_HMACSHA512(args);
        }

        public FunctionBase VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TRIMSTART(args);
        }

        public FunctionBase VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_TRIMEND(args);
        }

        public FunctionBase VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_INDEXOF(args);
        }

        public FunctionBase VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_LASTINDEXOF(args);
        }

        public FunctionBase VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_SPLIT(args1, args2);
        }

        public FunctionBase VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_JOIN(args);
        }

        public FunctionBase VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_SUBSTRING(args);
        }

        public FunctionBase VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_STARTSWITH(args);
        }

        public FunctionBase VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_ENDSWITH(args);
        }

        public FunctionBase VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNULLOREMPTY(args1);
        }

        public FunctionBase VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_ISNULLORWHITESPACE(args1);
        }

        public FunctionBase VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_REMOVESTART(args);
        }

        public FunctionBase VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_REMOVEEND(args);
        }

        public FunctionBase VisitJSON_fun(mathParser.JSON_funContext context)
        {
            var args1 = context.expr().Accept(this);
            return new Function_JSON(args1);
        }

        #endregion csharp

        #region Lookup

        public FunctionBase VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_VLOOKUP(args);
        }

        #endregion Lookup

        #region getValue

        public FunctionBase VisitArray_fun(mathParser.Array_funContext context)
        {
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_Array(args);
        }

        public FunctionBase VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return context.expr().Accept(this);
        }

        public FunctionBase VisitNUM_fun(mathParser.NUM_funContext context)
        {
            var d = decimal.Parse(context.num().GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            if (context.unit() == null) { return new Function_Value(Operand.Create(d)); }
            var unit = context.unit().GetText();
            return new Function_NUM(d, unit);
        }

        public FunctionBase VisitNum(mathParser.NumContext context)
        {
            var d = decimal.Parse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            return new Function_Value(Operand.Create(d), context.GetText());
        }

        public FunctionBase VisitUnit(mathParser.UnitContext context)
        {
            return new Function_Value(Operand.Create(context.GetText()));
        }

        public FunctionBase VisitSTRING_fun(mathParser.STRING_funContext context)
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

        public FunctionBase VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return new Function_Value(Operand.CreateNull(),"NULL");
        }

        public FunctionBase VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
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
            return new Function_Value(Operand.Error("Function PARAMETER first parameter is error!"));
        }

        public FunctionBase VisitParameter2(mathParser.Parameter2Context context)
        {
            return new Function_Value(Operand.Create(context.children[0].GetText()));
        }

        public FunctionBase VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (context.parameter2() != null) {
                var op = context.parameter2().Accept(this);
                return new Function_GetJsonValue(args1, op);
            }
            var op2 = exprs[1].Accept(this);
            return new Function_GetJsonValue(args1, op2);
        }

        public FunctionBase VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            var funName = context.PARAMETER().GetText();
            var exprs = context.expr();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_DiyFunction(funName, args);
        }

        public FunctionBase VisitPARAM_fun(mathParser.PARAM_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (exprs.Length == 1) {
                return new Function_PARAM(args1, null);
            }
            var args2 = exprs[1].Accept(this);
            return new Function_PARAM(args1, args2);
        }

        public FunctionBase VisitHAS_fun(mathParser.HAS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_HAS(args1, args2);
        }

        public FunctionBase VisitHASVALUE_fun(mathParser.HASVALUE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            var args2 = exprs[1].Accept(this);
            return new Function_HASVALUE(args1, args2);
        }

        public FunctionBase VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
        {
            var exprs = context.arrayJson();
            FunctionBase[] args = new FunctionBase[exprs.Length];
            for (int i = 0; i < exprs.Length; i++) {
                args[i] = exprs[i].Accept(this);
            }
            return new Function_ArrayJson(args);
        }

        public FunctionBase VisitArrayJson(mathParser.ArrayJsonContext context)
        {
            string keyName = null;
            //KeyValue keyValue = new KeyValue();
            if (context.NUM() != null) {
                if (int.TryParse(context.NUM().GetText(), out int key)) {
                    keyName = key.ToString();
                } else {
                    return new Function_Value(Operand.Error("Json key '" + context.NUM().GetText() + "' is error!"));
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
                keyName = sb.ToString();
            }
            if (context.parameter2() != null) {
                keyName = context.parameter2().GetText();
            }
            var f = context.expr().Accept(this);
            return new Function_ArrayJsonItem(keyName, f);
        }

        public FunctionBase VisitERROR_fun(mathParser.ERROR_funContext context)
        {
            if (context.expr() == null) { return new Function_Value(Operand.Error("")); }
            var args1 = context.expr().Accept(this);
            return new Function_ERROR(args1);
        }

        public FunctionBase VisitVersion_fun(mathParser.Version_funContext context)
        {
            return new Function_Value(Operand.Version);
        }

        #endregion getValue
    }
}