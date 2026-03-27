using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Functions.Compare;
using ToolGood.Algorithm.Internals.Functions.Csharp;
using ToolGood.Algorithm.Internals.Functions.CsharpSecurity;
using ToolGood.Algorithm.Internals.Functions.CsharpWeb;
using ToolGood.Algorithm.Internals.Functions.DateTimes;
using ToolGood.Algorithm.Internals.Functions.Financial;
using ToolGood.Algorithm.Internals.Functions.Flow;
using ToolGood.Algorithm.Internals.Functions.MathBase;
using ToolGood.Algorithm.Internals.Functions.MathSum;
using ToolGood.Algorithm.Internals.Functions.MathSum2;
using ToolGood.Algorithm.Internals.Functions.MathTransformation;
using ToolGood.Algorithm.Internals.Functions.MathTrigonometric;
using ToolGood.Algorithm.Internals.Functions.Operator;
using ToolGood.Algorithm.Internals.Functions.String;
using ToolGood.Algorithm.Internals.Functions.Value;
using ToolGood.Algorithm.math;
namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class MathFunctionVisitor : AbstractParseTreeVisitor<FunctionBase>, ImathVisitor<FunctionBase>
	{
		private FunctionBase[] VisitExprs(mathParser.ExprContext[] exprs)
		{
			FunctionBase[] list = new FunctionBase[exprs.Length];
			for(int i = 0; i < exprs.Length; i++) {
				list[i] = exprs[i].Accept(this);
			}
			return list;
		}
		#region base
		public FunctionBase VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept(this);
		}
		public FunctionBase VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var t = context.op.Text;
			if(CharUtil.Equals(t, '*')) {
				return new Function_Mul(funcs);
			} else if(CharUtil.Equals(t, '/')) {
				return new Function_Div(funcs);
			}
			return new Function_Mod(funcs);
		}
		public FunctionBase VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var t = context.op.Text;
			if(CharUtil.Equals(t, '&')) {
				return new Function_Connect(funcs);
			} else if(CharUtil.Equals(t, '+')) {
				return new Function_Add(funcs);
			}
			return new Function_Sub(funcs);
		}
		public FunctionBase VisitJudge_fun(mathParser.Judge_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			string type = context.op.Text;
			if(CharUtil.Equals(type, "=", "==", "===")) {
				return new Function_EQ(funcs);
			} else if(CharUtil.Equals(type, "<")) {
				return new Function_LT(funcs);
			} else if(CharUtil.Equals(type, "<=")) {
				return new Function_LE(funcs);
			} else if(CharUtil.Equals(type, ">")) {
				return new Function_GT(funcs);
			} else if(CharUtil.Equals(type, ">=")) {
				return new Function_GE(funcs);
			}
			return new Function_NE(funcs);
		}
		public FunctionBase VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var t = context.op.Text;
			if(CharUtil.Equals(t, "&&")) {
				return new Function_AND(funcs);
			}
			return new Function_OR(funcs);
		}
		#endregion base
		#region flow
		public FunctionBase VisitIF_fun(mathParser.IF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IF(funcs);
		}
		public FunctionBase VisitIFS_fun(mathParser.IFS_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IFS(funcs);
		}
		public FunctionBase VisitSWITCH_fun(mathParser.SWITCH_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SWITCH(funcs);
		}
		public FunctionBase VisitIFERROR_fun(mathParser.IFERROR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IFERROR(funcs);
		}
		public FunctionBase VisitIS_fun(mathParser.IS_funContext context)
		{
			var txt = context.f.Text;
			var args1 = this.Visit(context.expr());
			if(txt.Equals("ISNUMBER", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISNUMBER(args1);
			} else if(txt.Equals("ISTEXT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISTEXT(args1);
			} else if(txt.Equals("ISLOGICAL", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISLOGICAL(args1);
			} else if(txt.Equals("ISNONTEXT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISNONTEXT(args1);
			} else if(txt.Equals("ISEVEN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISEVEN(args1);
			}
			//} else if(txt.Equals("ISODD", StringComparison.OrdinalIgnoreCase)) {
			return new Function_ISODD(args1);
			//}
		}

		public FunctionBase VisitISNULL_check_fun(mathParser.ISNULL_check_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(txt.Equals("ISERROR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISERROR(funcs);
			} else if(txt.Equals("ISNULL", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISNULL(funcs);
			}
			//if(txt.Equals("ISNULLORERROR", StringComparison.OrdinalIgnoreCase)) {
			return new Function_ISNULLORERROR(funcs);
		}
		public FunctionBase VisitLOGIC_fun(mathParser.LOGIC_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());

			if(txt.Equals("AND", StringComparison.OrdinalIgnoreCase)) {
				return new Function_AND_N(funcs);
			} else if(txt.Equals("OR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_OR_N(funcs);
			}
			return new Function_XOR(funcs);
		}

		public FunctionBase VisitNOT_fun(mathParser.NOT_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_NOT(args1);
		}
		public FunctionBase VisitTRUE_fun(mathParser.TRUE_funContext context)
		{
			return new Function_ValueBoolean(true);
		}
		public FunctionBase VisitFALSE_fun(mathParser.FALSE_funContext context)
		{
			return new Function_ValueBoolean(false);
		}
		#endregion flow
		#region math
		#region base
		public FunctionBase VisitE_fun(mathParser.E_funContext context)
		{
			return new Function_ValueNumber(Operand.Create(MathEx.E), "E");
		}
		public FunctionBase VisitPI_fun(mathParser.PI_funContext context)
		{
			return new Function_ValueNumber(Operand.Create(MathEx.PI), "PI");
		}
		public FunctionBase VisitABS_fun(mathParser.ABS_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_ABS(args1);
		}
		public FunctionBase VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_QUOTIENT(funcs);
		}
		public FunctionBase VisitMOD_fun(mathParser.MOD_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_Mod(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_TRUNC(funcs);
		}
		public FunctionBase VisitINT_fun(mathParser.INT_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_INT(args1);
		}
		public FunctionBase VisitGCD_fun(mathParser.GCD_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_GCD(funcs);
		}
		public FunctionBase VisitLCM_fun(mathParser.LCM_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_LCM(funcs);
		}
		public FunctionBase VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_COMBIN(funcs);
		}
		public FunctionBase VisitPERMUT_fun(mathParser.PERMUT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PERMUT(funcs);
		}
		public FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_Percentage(args1);
		}
		#endregion base
		#region trigonometric functions
		public FunctionBase VisitTRIG_fun(mathParser.TRIG_funContext context)
		{
			var txt = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(txt.Equals("DEGREES", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DEGREES(args1);
			} else if(txt.Equals("RADIANS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_RADIANS(args1);
			} else if(txt.Equals("COS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_COS(args1);
			} else if(txt.Equals("COSH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_COSH(args1);
			} else if(txt.Equals("SIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SIN(args1);
			} else if(txt.Equals("SINH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SINH(args1);
			} else if(txt.Equals("TAN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_TAN(args1);
			} else if(txt.Equals("TANH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_TANH(args1);
			} else if(txt.Equals("COT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_COT(args1);
			} else if(txt.Equals("COTH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_COTH(args1);
			} else if(txt.Equals("CSC", StringComparison.OrdinalIgnoreCase)) {
				return new Function_CSC(args1);
			} else if(txt.Equals("CSCH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_CSCH(args1);
			} else if(txt.Equals("SEC", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SEC(args1);
			} else if(txt.Equals("SECH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SECH(args1);
			} else if(txt.Equals("ACOS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ACOS(args1);
			} else if(txt.Equals("ACOSH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ACOSH(args1);
			} else if(txt.Equals("ASIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ASIN(args1);
			} else if(txt.Equals("ASINH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ASINH(args1);
			} else if(txt.Equals("ATAN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ATAN(args1);
			} else if(txt.Equals("ATANH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ATANH(args1);
			} else if(txt.Equals("ACOT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ACOT(args1);
			}
			//if(txt.Equals("ACOTH", StringComparison.OrdinalIgnoreCase)) {
			return new Function_ACOTH(args1);
			//} 
		}
		 
		public FunctionBase VisitATAN2_fun(mathParser.ATAN2_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ATAN2(funcs);
		}
		public FunctionBase VisitFIXED_fun(mathParser.FIXED_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FIXED(funcs);
		}
		#endregion trigonometric functions
		#region transformation
		public FunctionBase VisitConvert_fun(mathParser.Convert_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(txt.Equals("BIN2OCT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_BIN2OCT(funcs);
			} else if(txt.Equals("BIN2DEC", StringComparison.OrdinalIgnoreCase)) {
				return new Function_BIN2DEC(funcs);
			} else if(txt.Equals("BIN2HEX", StringComparison.OrdinalIgnoreCase)) {
				return new Function_BIN2HEX(funcs);
			} else if(txt.Equals("OCT2BIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_OCT2BIN(funcs);
			} else if(txt.Equals("OCT2DEC", StringComparison.OrdinalIgnoreCase)) {
				return new Function_OCT2DEC(funcs);
			} else if(txt.Equals("OCT2HEX", StringComparison.OrdinalIgnoreCase)) {
				return new Function_OCT2HEX(funcs);
			} else if(txt.Equals("DEC2BIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DEC2BIN(funcs);
			} else if(txt.Equals("DEC2OCT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DEC2OCT(funcs);
			} else if(txt.Equals("DEC2HEX", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DEC2HEX(funcs);
			} else if(txt.Equals("HEX2BIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HEX2BIN(funcs);
			} else if(txt.Equals("HEX2OCT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HEX2OCT(funcs);
			}
			return new Function_HEX2DEC(funcs);
		}
		#endregion transformation
		#region rounding
		public FunctionBase VisitROUND_fun(mathParser.ROUND_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ROUND(funcs);
		}
		public FunctionBase VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ROUNDDOWN(funcs);
		}
		public FunctionBase VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ROUNDUP(funcs);
		}
		public FunctionBase VisitCEILING_fun(mathParser.CEILING_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_CEILING(funcs);
		}
		public FunctionBase VisitFLOOR_fun(mathParser.FLOOR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FLOOR(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_MROUND(funcs);
		}
		#endregion rounding
		#region RAND
		public FunctionBase VisitRAND_fun(mathParser.RAND_funContext context)
		{
			return new Function_RAND();
		}
		public FunctionBase VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_RANDBETWEEN(funcs);
		}
		#endregion RAND
		#region power logarithm factorial
		public FunctionBase VisitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_COVARIANCES(funcs);
		}
		public FunctionBase VisitCOVAR_fun(mathParser.COVAR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_COVAR(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_POWER(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_LOG(funcs);
		}
		public FunctionBase VisitLOG10_fun(mathParser.LOG10_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_LOG10(args1);
		}
		public FunctionBase VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_MULTINOMIAL(funcs);
		}
		public FunctionBase VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PRODUCT(funcs);
		}
		public FunctionBase VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_SQRTPI(args1);
		}
		public FunctionBase VisitERF_fun(mathParser.ERF_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_ERF(args1);
		}
		public FunctionBase VisitERFC_fun(mathParser.ERFC_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_ERFC(args1);
		}
		public FunctionBase VisitBESSELI_fun(mathParser.BESSELI_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BESSELI(funcs);
		}
		public FunctionBase VisitBESSELJ_fun(mathParser.BESSELJ_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BESSELJ(funcs);
		}
		public FunctionBase VisitBESSELK_fun(mathParser.BESSELK_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BESSELK(funcs);
		}
		public FunctionBase VisitBESSELY_fun(mathParser.BESSELY_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BESSELY(funcs);
		}
		public FunctionBase VisitDELTA_fun(mathParser.DELTA_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DELTA(funcs);
		}
		public FunctionBase VisitGESTEP_fun(mathParser.GESTEP_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_GESTEP(funcs);
		}
		public FunctionBase VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMSQ(funcs);
		}
		public FunctionBase VisitSUMPRODUCT_fun(mathParser.SUMPRODUCT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMPRODUCT(funcs);
		}
		public FunctionBase VisitSUMX2MY2_fun(mathParser.SUMX2MY2_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMX2MY2(funcs);
		}
		public FunctionBase VisitSUMX2PY2_fun(mathParser.SUMX2PY2_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMX2PY2(funcs);
		}
		public FunctionBase VisitSUMXMY2_fun(mathParser.SUMXMY2_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMXMY2(funcs);
		}
		public FunctionBase VisitARABIC_fun(mathParser.ARABIC_funContext context)
		{
			var func = Visit(context.expr());
			return new Function_ARABIC(func);
		}
		public FunctionBase VisitROMAN_fun(mathParser.ROMAN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ROMAN(funcs);
		}
		public FunctionBase VisitSERIESSUM_fun(mathParser.SERIESSUM_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SERIESSUM(funcs);
		}
		public FunctionBase VisitRANK_fun(mathParser.RANK_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_RANK(funcs);
		}
		public FunctionBase VisitFORECAST_fun(mathParser.FORECAST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FORECAST(funcs);
		}
		public FunctionBase VisitINTERCEPT_fun(mathParser.INTERCEPT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_INTERCEPT(funcs);
		}
		public FunctionBase VisitSLOPE_fun(mathParser.SLOPE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SLOPE(funcs);
		}
		public FunctionBase VisitCORREL_fun(mathParser.CORREL_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_CORREL(funcs);
		}
		public FunctionBase VisitPEARSON_fun(mathParser.PEARSON_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PEARSON(funcs);
		}
		public FunctionBase VisitYEARFRAC_fun(mathParser.YEARFRAC_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_YEARFRAC(funcs);
		}
		#endregion
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
		public FunctionBase VisitUNICODE_fun(mathParser.UNICODE_funContext context)
		{
			var txt = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(txt.Equals("UNICODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_UNICODE(args1);
			}
			return new Function_UNICHAR(args1);
		}
		public FunctionBase VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_CONCATENATE(funcs);
		}
		public FunctionBase VisitEXACT_fun(mathParser.EXACT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_EXACT(funcs);
		}
		public FunctionBase VisitFIND_fun(mathParser.FIND_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FIND(funcs);
		}
		public FunctionBase VisitLR_fun(mathParser.LR_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(txt.Equals("LEFT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_LEFT(funcs);
			}
			//if(txt.Equals("RIGHT", StringComparison.OrdinalIgnoreCase)) {
			return new Function_RIGHT(funcs);
			//}
		}

		public FunctionBase VisitLEN_fun(mathParser.LEN_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_LEN(args1);
		}
		public FunctionBase VisitCASE_fun(mathParser.CASE_funContext context)
		{
			var txt = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(txt.Equals("LOWER", StringComparison.OrdinalIgnoreCase) || txt.Equals("TOLOWER", StringComparison.OrdinalIgnoreCase)) {
				return new Function_LOWER(args1);
			}

			return new Function_UPPER(args1);
		}

		public FunctionBase VisitMID_fun(mathParser.MID_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_MID(funcs);
		}
		public FunctionBase VisitPROPER_fun(mathParser.PROPER_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_PROPER(args1);
		}
		public FunctionBase VisitREPLACE_fun(mathParser.REPLACE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_REPLACE(funcs);
		}
		public FunctionBase VisitREPT_fun(mathParser.REPT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_REPT(funcs);
		}

		public FunctionBase VisitRMB_fun(mathParser.RMB_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_RMB(args1);
		}
		public FunctionBase VisitSEARCH_fun(mathParser.SEARCH_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SEARCH(funcs);
		}
		public FunctionBase VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUBSTITUTE(funcs);
		}
		public FunctionBase VisitT_fun(mathParser.T_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_T(args1);
		}
		public FunctionBase VisitTEXT_fun(mathParser.TEXT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_TEXT(funcs);
		}
		public FunctionBase VisitTRIM_fun(mathParser.TRIM_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_TRIM(args1);
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
			var funcs = VisitExprs(context.expr());
			return new Function_DATEVALUE(funcs);
		}
		public FunctionBase VisitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_TIMESTAMP(funcs);
		}
		public FunctionBase VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_TIMEVALUE(args1);
		}
		public FunctionBase VisitDATE_fun(mathParser.DATE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DATE(funcs);
		}
		public FunctionBase VisitTIME_fun(mathParser.TIME_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_TIME(funcs);
		}
		public FunctionBase VisitNOW_fun(mathParser.NOW_funContext context)
		{
			return new Function_NOW();
		}
		public FunctionBase VisitTODAY_fun(mathParser.TODAY_funContext context)
		{
			return new Function_TODAY();
		}
		public FunctionBase VisitDATE_TIME_fun(mathParser.DATE_TIME_funContext context)
		{
			var txt = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(txt.Equals("YEAR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_YEAR(args1);
			} else if(txt.Equals("MONTH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MONTH(args1);
			} else if(txt.Equals("DAY", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DAY(args1);
			} else if(txt.Equals("HOUR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HOUR(args1);
			} else if(txt.Equals("MINUTE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MINUTE(args1);
			} else if(txt.Equals("SECOND", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SECOND(args1);
			}
			throw new NotSupportedException($"不支持的函数 {txt}");
		}

		public FunctionBase VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_WEEKDAY(funcs);
		}
		public FunctionBase VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DATEDIF(funcs);
		}
		public FunctionBase VisitDAYS_fun(mathParser.DAYS_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DAYS(funcs);
		}
		public FunctionBase VisitDAYS360_fun(mathParser.DAYS360_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DAYS360(funcs);
		}
		public FunctionBase VisitEDATE_fun(mathParser.EDATE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_EDATE(funcs);
		}
		public FunctionBase VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_EOMONTH(funcs);
		}
		public FunctionBase VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NETWORKDAYS(funcs);
		}
		public FunctionBase VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_WORKDAY(funcs);
		}
		public FunctionBase VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_WEEKNUM(funcs);
		}
		public FunctionBase VisitADD_DateTime_fun(mathParser.ADD_DateTime_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(txt.Equals("ADDYEARS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ADDYEARS(funcs);
			} else if(txt.Equals("ADDMONTHS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ADDMONTHS(funcs);
			} else if(txt.Equals("ADDDAYS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ADDDAYS(funcs);
			} else if(txt.Equals("ADDHOURS", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ADDHOURS(funcs);

			} else if(txt.Equals("ADDMINUTES", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ADDMINUTES(funcs);
			}
			//if(txt.Equals("ADDSECONDS", StringComparison.OrdinalIgnoreCase)) {
			return new Function_ADDSECONDS(funcs);
			//}
		}

		#endregion MyDate time
		#region sum

		public FunctionBase VisitSTAT_fun(mathParser.STAT_funContext context)
		{
			var txt = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(txt.Equals("MAX", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MAX(funcs);
			} else if(txt.Equals("MEDIAN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MEDIAN(funcs);
			} else if(txt.Equals("MIN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MIN(funcs);
			} else if(txt.Equals("MODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MODE(funcs);
			} else if(txt.Equals("AVERAGE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_AVERAGE(funcs);
			} else if(txt.Equals("GEOMEAN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_GEOMEAN(funcs);
			} else if(txt.Equals("HARMEAN", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HARMEAN(funcs);
			} else if(txt.Equals("COUNT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_COUNT(funcs);
			} else if(txt.Equals("SUM", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SUM(funcs);
			} else if(txt.Equals("AVEDEV", StringComparison.OrdinalIgnoreCase)) {
				return new Function_AVEDEV(funcs);
			} else if(txt.Equals("STDEV", StringComparison.OrdinalIgnoreCase)) {
				return new Function_STDEV(funcs);
			} else if(txt.Equals("STDEVP", StringComparison.OrdinalIgnoreCase)) {
				return new Function_STDEVP(funcs);
			} else if(txt.Equals("DEVSQ", StringComparison.OrdinalIgnoreCase)) {
				return new Function_DEVSQ(funcs);
			} else if(txt.Equals("VAR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_VAR(funcs);
			}
			//if (txt.Equals("VARP", StringComparison.OrdinalIgnoreCase)) {
			return new Function_VARP(funcs);
			//}
		}
		public FunctionBase VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_QUARTILE(funcs);
		}

		public FunctionBase VisitLARGE_fun(mathParser.LARGE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_LARGE(funcs);
		}
		public FunctionBase VisitSMALL_fun(mathParser.SMALL_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SMALL(funcs);
		}
		public FunctionBase VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PERCENTILE(funcs);
		}
		public FunctionBase VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PERCENTRANK(funcs);
		}

		public FunctionBase VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_AVERAGEIF(funcs);
		}

		public FunctionBase VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_COUNTIF(funcs);
		}

		public FunctionBase VisitSUMIF_fun(mathParser.SUMIF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUMIF(funcs);
		}

		public FunctionBase VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NORMDIST(funcs);
		}
		public FunctionBase VisitNORMINV_fun(mathParser.NORMINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NORMINV(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_BETADIST(funcs);
		}
		public FunctionBase VisitBETAINV_fun(mathParser.BETAINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BETAINV(funcs);
		}
		public FunctionBase VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_BINOMDIST(funcs);
		}
		public FunctionBase VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_EXPONDIST(funcs);
		}
		public FunctionBase VisitFDIST_fun(mathParser.FDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FDIST(funcs);
		}
		public FunctionBase VisitFINV_fun(mathParser.FINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FINV(funcs);
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
			var funcs = VisitExprs(context.expr());
			return new Function_GAMMADIST(funcs);
		}
		public FunctionBase VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_GAMMAINV(funcs);
		}
		public FunctionBase VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_GAMMALN(args1);
		}
		public FunctionBase VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_HYPGEOMDIST(funcs);
		}
		public FunctionBase VisitLOGINV_fun(mathParser.LOGINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_LOGINV(funcs);
		}
		public FunctionBase VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_LOGNORMDIST(funcs);
		}
		public FunctionBase VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NEGBINOMDIST(funcs);
		}
		public FunctionBase VisitPOISSON_fun(mathParser.POISSON_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_POISSON(funcs);
		}
		public FunctionBase VisitTDIST_fun(mathParser.TDIST_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_TDIST(funcs);
		}
		public FunctionBase VisitTINV_fun(mathParser.TINV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_TINV(funcs);
		}
		public FunctionBase VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_WEIBULL(funcs);
		}
		#endregion sum
		#region financial
		public FunctionBase VisitPMT_fun(mathParser.PMT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PMT(funcs);
		}
		public FunctionBase VisitPPMT_fun(mathParser.PPMT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PPMT(funcs);
		}
		public FunctionBase VisitIPMT_fun(mathParser.IPMT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IPMT(funcs);
		}
		public FunctionBase VisitPV_fun(mathParser.PV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_PV(funcs);
		}
		public FunctionBase VisitFV_fun(mathParser.FV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_FV(funcs);
		}
		public FunctionBase VisitNPER_fun(mathParser.NPER_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NPER(funcs);
		}
		public FunctionBase VisitRATE_fun(mathParser.RATE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_RATE(funcs);
		}
		public FunctionBase VisitNPV_fun(mathParser.NPV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_NPV(funcs);
		}
		public FunctionBase VisitXNPV_fun(mathParser.XNPV_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_XNPV(funcs);
		}
		public FunctionBase VisitIRR_fun(mathParser.IRR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IRR(funcs);
		}
		public FunctionBase VisitMIRR_fun(mathParser.MIRR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_MIRR(funcs);
		}
		public FunctionBase VisitXIRR_fun(mathParser.XIRR_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_XIRR(funcs);
		}
		public FunctionBase VisitSLN_fun(mathParser.SLN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SLN(funcs);
		}
		public FunctionBase VisitDB_fun(mathParser.DB_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DB(funcs);
		}
		public FunctionBase VisitDDB_fun(mathParser.DDB_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_DDB(funcs);
		}
		public FunctionBase VisitSYD_fun(mathParser.SYD_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SYD(funcs);
		}
		#endregion financial
		#region csharp
		public FunctionBase VisitENCODE_fun(mathParser.ENCODE_funContext context)
		{
			var text = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(text.Equals("URLENCODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_URLENCODE(args1);
			} else if(text.Equals("URLDECODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_URLDECODE(args1);
			} else if(text.Equals("HTMLENCODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HTMLENCODE(args1);
			} else if(text.Equals("HTMLDECODE", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HTMLDECODE(args1);
			} else if(text.Equals("BASE64TOTEXT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_BASE64TOTEXT(args1);
			} else if(text.Equals("BASE64URLTOTEXT", StringComparison.OrdinalIgnoreCase)) {
				return new Function_BASE64URLTOTEXT(args1);
			} else if(text.Equals("TEXTTOBASE64", StringComparison.OrdinalIgnoreCase)) {
				return new Function_TEXTTOBASE64(args1);
			}
			//else if(text.Equals("TEXTTOBASE64URL", StringComparison.OrdinalIgnoreCase)) {
			return new Function_TEXTTOBASE64URL(args1);
			//}
		}
		public FunctionBase VisitREGEX_fun(mathParser.REGEX_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_REGEX(funcs);
		}
		public FunctionBase VisitREGEXREPLACE_fun(mathParser.REGEXREPLACE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_REGEXREPLACE(funcs);
		}
		public FunctionBase VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ISREGEX(funcs);
		}
		public FunctionBase VisitGUID_fun(mathParser.GUID_funContext context)
		{
			return new Function_GUID();
		}
		public FunctionBase VisitHMAC_fun(mathParser.HMAC_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("HMACMD5", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HMACMD5(funcs);
			} else if(text.Equals("HMACSHA1", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HMACSHA1(funcs);
			} else if(text.Equals("HMACSHA256", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HMACSHA256(funcs);
			} else if(text.Equals("HMACSHA512", StringComparison.OrdinalIgnoreCase)) {
				return new Function_HMACSHA512(funcs);
			}
			throw new NotSupportedException($"不支持的函数 {text}");
		}
		public FunctionBase VisitHASH_fun(mathParser.HASH_funContext context)
		{
			var text = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(text.Equals("MD5", StringComparison.OrdinalIgnoreCase)) {
				return new Function_MD5(args1);
			} else if(text.Equals("SHA1", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SHA1(args1);
			} else if(text.Equals("SHA256", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SHA256(args1);
			} else if(text.Equals("SHA512", StringComparison.OrdinalIgnoreCase)) {
				return new Function_SHA512(args1);
			}
			throw new NotSupportedException($"不支持的函数 {text}");
		}
		public FunctionBase VisitTRIM_SE_fun(mathParser.TRIM_SE_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("TRIMSTART", StringComparison.OrdinalIgnoreCase) || text.Equals("LTRIM", StringComparison.OrdinalIgnoreCase)) {
				return new Function_TRIMSTART(funcs);
			}
			return new Function_TRIMEND(funcs);
		}
		public FunctionBase VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("INDEXOF", StringComparison.OrdinalIgnoreCase)) {
				return new Function_INDEXOF(funcs);
			}
			return new Function_LASTINDEXOF(funcs);

		}
		public FunctionBase VisitSPLIT_fun(mathParser.SPLIT_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SPLIT(funcs);
		}
		public FunctionBase VisitJOIN_fun(mathParser.JOIN_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_JOIN(funcs);
		}
		public FunctionBase VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_SUBSTRING(funcs);
		}
		public FunctionBase VisitSTRINGSuffix_fun(mathParser.STRINGSuffix_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("STARTSWITH", StringComparison.OrdinalIgnoreCase)) {
				return new Function_STARTSWITH(funcs);
			}
			return new Function_ENDSWITH(funcs);

		}
		public FunctionBase VisitISNULLOR_fun(mathParser.ISNULLOR_funContext context)
		{
			var text = context.f.Text;
			var args1 = context.expr().Accept(this);
			if(text.Equals("ISNULLOREMPTY", StringComparison.OrdinalIgnoreCase)) {
				return new Function_ISNULLOREMPTY(args1);
			}
			return new Function_ISNULLORWHITESPACE(args1);
		}
		public FunctionBase VisitREMOVE_fun(mathParser.REMOVE_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("REMOVESTART", StringComparison.OrdinalIgnoreCase)) {
				return new Function_REMOVESTART(funcs);
			}
			return new Function_REMOVEEND(funcs);
		}
		public FunctionBase VisitJSON_fun(mathParser.JSON_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_JSON(args1);
		}
		#endregion csharp
		#region LOOKFLOOR LOOKCEILING

		public FunctionBase VisitLOOK_fun(mathParser.LOOK_funContext context)
		{
			var text = context.f.Text;
			var funcs = VisitExprs(context.expr());
			if(text.Equals("LOOKFLOOR", StringComparison.OrdinalIgnoreCase)) {
				return new Function_LOOKFLOOR(funcs);
			}
			return new Function_LOOKCEILING(funcs);
		}
		#endregion
		#region getValue
		public FunctionBase VisitArray_fun(mathParser.Array_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_Array(funcs);
		}
		public FunctionBase VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}
		public FunctionBase VisitNUM_fun(mathParser.NUM_funContext context)
		{
			var text = context.num().GetText();
			var d = decimal.Parse(text.AsSpan(), NumberStyles.Any, CultureInfo.InvariantCulture);
			if(context.unit == null) { return new Function_ValueNumber(Operand.Create(d), text); }
			var unit = context.unit.Text;
			return new Function_Number(d, unit);
		}
		public FunctionBase VisitNum(mathParser.NumContext context)
		{
			var text = context.GetText();
			var d = decimal.Parse(text.AsSpan(), NumberStyles.Any, CultureInfo.InvariantCulture);
			return new Function_ValueNumber(Operand.Create(d), text);
		}

		public FunctionBase VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			var opd = context.GetText();
			var sb = new StringBuilder(opd.Length);
			int index = 1;
			while(index < opd.Length - 1) {
				var c = opd[index++];
				if(c == '\\') {
					var c2 = opd[index++];
					if(c2 == 'n') sb.Append('\n');
					else if(c2 == 'r') sb.Append('\r');
					else if(c2 == 't') sb.Append('\t');
					else if(c2 == '0') sb.Append('\0');
					else if(c2 == 'v') sb.Append('\v');
					else if(c2 == 'a') sb.Append('\a');
					else if(c2 == 'b') sb.Append('\b');
					else if(c2 == 'f') sb.Append('\f');
					else sb.Append(c2);
				} else {
					sb.Append(c);
				}
			}
			return new Function_ValueText(Operand.Create(sb.ToString()));
		}
		public FunctionBase VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return new Function_NULL();
		}
		public FunctionBase VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			return new Function_Parameter(node.GetText());
		}
		public FunctionBase VisitParameter2(mathParser.Parameter2Context context)
		{
			return new Function_ValueText(Operand.Create(context.children[0].GetText()));
		}
		public FunctionBase VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			if(context.PARAMETER() != null) {
				var op = new Function_Parameter(context.PARAMETER().GetText());
				return new Function_GetJsonValue(funcs[0], op);
			}
			if(context.parameter2() != null) {
				var op = context.parameter2().Accept(this);
				return new Function_GetJsonValue(funcs[0], op);
			}
			return new Function_GetJsonValue(funcs[0], funcs[1]);
		}
		public FunctionBase VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			var funName = context.PARAMETER().GetText();
			var funcs = VisitExprs(context.expr());
			return new Function_DiyFunction(funName, funcs);
		}
		public FunctionBase VisitPARAM_fun(mathParser.PARAM_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_Param(funcs);
		}
		public FunctionBase VisitHAS_fun(mathParser.HAS_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_HAS(funcs);
		}
		public FunctionBase VisitHASVALUE_fun(mathParser.HASVALUE_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_HASVALUE(funcs);
		}
		public FunctionBase VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			var exprs = context.arrayJson();
			FunctionBase[] args = new FunctionBase[exprs.Length];
			for(int i = 0; i < exprs.Length; i++) {
				args[i] = exprs[i].Accept(this);
			}
			return new Function_ArrayJson(args);
		}
		public FunctionBase VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			string keyName = null;
			if(context.key != null) {
				keyName = context.key.Text.Trim(new char[] { '"', '\'', ' ', '\t', '\r', '\n', '\f' });
			} else if(context.parameter2() != null) {
				keyName = context.parameter2().GetText();
			}
			var f = context.expr().Accept(this);
			return new Function_ArrayJsonItem(keyName, f);
		}
		public FunctionBase VisitERROR_fun(mathParser.ERROR_funContext context)
		{
			if(context.expr() == null) { return new Function_ERROR(null); }
			var args1 = context.expr().Accept(this);
			return new Function_ERROR(args1);
		}
		public FunctionBase VisitVersion_fun(mathParser.Version_funContext context)
		{
			return new Function_ValueText(Operand.Version, "ALGORITHMVERSION");
		}

















		#endregion getValue
	}
}