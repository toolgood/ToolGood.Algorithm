package toolgood.algorithm.internals.visitors;

import java.util.List;

import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.operator.Function_Add;
import toolgood.algorithm.internals.functions.operator.Function_Div;
import toolgood.algorithm.internals.functions.operator.Function_Mod;
import toolgood.algorithm.internals.functions.operator.Function_Mul;
import toolgood.algorithm.internals.functions.operator.Function_Sub;
import toolgood.algorithm.internals.functions.compare.Function_EQ;
import toolgood.algorithm.internals.functions.compare.Function_GE;
import toolgood.algorithm.internals.functions.compare.Function_GT;
import toolgood.algorithm.internals.functions.compare.Function_LE;
import toolgood.algorithm.internals.functions.compare.Function_LT;
import toolgood.algorithm.internals.functions.compare.Function_NE;
import toolgood.algorithm.internals.functions.csharp.Function_ENDSWITH;
import toolgood.algorithm.internals.functions.csharp.Function_GUID;
import toolgood.algorithm.internals.functions.csharp.Function_HAS;
import toolgood.algorithm.internals.functions.csharp.Function_HASVALUE;
import toolgood.algorithm.internals.functions.csharp.Function_INDEXOF;
import toolgood.algorithm.internals.functions.csharp.Function_ISREGEX;
import toolgood.algorithm.internals.functions.csharp.Function_JOIN;
import toolgood.algorithm.internals.functions.csharp.Function_LASTINDEXOF;
import toolgood.algorithm.internals.functions.csharp.Function_LOOKCEILING;
import toolgood.algorithm.internals.functions.csharp.Function_LOOKFLOOR;
import toolgood.algorithm.internals.functions.csharp.Function_REGEX;
import toolgood.algorithm.internals.functions.csharp.Function_REGEXREPALCE;
import toolgood.algorithm.internals.functions.csharp.Function_REMOVEEND;
import toolgood.algorithm.internals.functions.csharp.Function_REMOVESTART;
import toolgood.algorithm.internals.functions.csharp.Function_SPLIT;
import toolgood.algorithm.internals.functions.csharp.Function_STARTSWITH;
import toolgood.algorithm.internals.functions.csharp.Function_SUBSTRING;
import toolgood.algorithm.internals.functions.csharp.Function_TRIMEND;
import toolgood.algorithm.internals.functions.csharp.Function_TRIMSTART;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_HMACMD5;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_HMACSHA1;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_HMACSHA256;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_HMACSHA512;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_MD5;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_SHA1;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_SHA256;
import toolgood.algorithm.internals.functions.csharpsecurity.Function_SHA512;
import toolgood.algorithm.internals.functions.csharpweb.Function_BASE64TOTEXT;
import toolgood.algorithm.internals.functions.csharpweb.Function_BASE64URLTOTEXT;
import toolgood.algorithm.internals.functions.csharpweb.Function_HTMLDECODE;
import toolgood.algorithm.internals.functions.csharpweb.Function_HTMLENCODE;
import toolgood.algorithm.internals.functions.csharpweb.Function_TEXTTOBASE64;
import toolgood.algorithm.internals.functions.csharpweb.Function_TEXTTOBASE64URL;
import toolgood.algorithm.internals.functions.csharpweb.Function_URLDECODE;
import toolgood.algorithm.internals.functions.csharpweb.Function_URLENCODE;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDDAYS;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDHOURS;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDMINUTES;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDMONTHS;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDSECONDS;
import toolgood.algorithm.internals.functions.datetimes.Function_ADDYEARS;
import toolgood.algorithm.internals.functions.datetimes.Function_DATE;
import toolgood.algorithm.internals.functions.datetimes.Function_DATEDIF;
import toolgood.algorithm.internals.functions.datetimes.Function_DATEVALUE;
import toolgood.algorithm.internals.functions.datetimes.Function_DAY;
import toolgood.algorithm.internals.functions.datetimes.Function_DAYS360;
import toolgood.algorithm.internals.functions.datetimes.Function_EDATE;
import toolgood.algorithm.internals.functions.datetimes.Function_EOMONTH;
import toolgood.algorithm.internals.functions.datetimes.Function_HOUR;
import toolgood.algorithm.internals.functions.datetimes.Function_MINUTE;
import toolgood.algorithm.internals.functions.datetimes.Function_MONTH;
import toolgood.algorithm.internals.functions.datetimes.Function_NETWORKDAYS;
import toolgood.algorithm.internals.functions.datetimes.Function_NOW;
import toolgood.algorithm.internals.functions.datetimes.Function_SECOND;
import toolgood.algorithm.internals.functions.datetimes.Function_TIME;
import toolgood.algorithm.internals.functions.datetimes.Function_TIMESTAMP;
import toolgood.algorithm.internals.functions.datetimes.Function_TIMEVALUE;
import toolgood.algorithm.internals.functions.datetimes.Function_TODAY;
import toolgood.algorithm.internals.functions.datetimes.Function_WEEKDAY;
import toolgood.algorithm.internals.functions.datetimes.Function_WEEKNUM;
import toolgood.algorithm.internals.functions.datetimes.Function_WORKDAY;
import toolgood.algorithm.internals.functions.datetimes.Function_YEAR;
import toolgood.algorithm.internals.functions.operator.Function_AND;
import toolgood.algorithm.internals.functions.operator.Function_AND_N;
import toolgood.algorithm.internals.functions.flow.Function_IF;
import toolgood.algorithm.internals.functions.flow.Function_IFERROR;
import toolgood.algorithm.internals.functions.flow.Function_ISEVEN;
import toolgood.algorithm.internals.functions.flow.Function_ISLOGICAL;
import toolgood.algorithm.internals.functions.flow.Function_ISNONTEXT;
import toolgood.algorithm.internals.functions.flow.Function_ISNULL;
import toolgood.algorithm.internals.functions.flow.Function_ISNULLOREMPTY;
import toolgood.algorithm.internals.functions.flow.Function_ISNULLORERROR;
import toolgood.algorithm.internals.functions.flow.Function_ISNULLORWHITESPACE;
import toolgood.algorithm.internals.functions.flow.Function_ISODD;
import toolgood.algorithm.internals.functions.flow.Function_IsText;
import toolgood.algorithm.internals.functions.flow.Function_NOT;
import toolgood.algorithm.internals.functions.operator.Function_OR;
import toolgood.algorithm.internals.functions.operator.Function_OR_N;
import toolgood.algorithm.internals.functions.mathbase.*;
import toolgood.algorithm.internals.functions.mathsum.*;
import toolgood.algorithm.internals.functions.mathtransformation.*;
import toolgood.algorithm.internals.functions.mathtrigonometric.*;
import toolgood.algorithm.internals.functions.operator.Function_Connect;
import toolgood.algorithm.internals.functions.string.*;
import toolgood.algorithm.internals.functions.value.Function_Array;
import toolgood.algorithm.internals.functions.value.Function_ArrayJson;
import toolgood.algorithm.internals.functions.value.Function_DiyFunction;
import toolgood.algorithm.internals.functions.value.Function_ERROR;
import toolgood.algorithm.internals.functions.value.Function_GetJsonValue;
import toolgood.algorithm.internals.functions.value.Function_JSON;
import toolgood.algorithm.internals.functions.value.Function_NUM;
import toolgood.algorithm.internals.functions.value.Function_PARAM;
import toolgood.algorithm.internals.functions.value.Function_PARAMETER;
import toolgood.algorithm.internals.functions.value.Function_Value;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ExprContext;
import toolgood.algorithm.math.mathVisitor;

public class MathFunctionVisitor extends AbstractParseTreeVisitor<FunctionBase> implements mathVisitor<FunctionBase> {
//#region base

		public FunctionBase visitProg(mathParser.ProgContext context)
		{
			return context.expr().accept(this);
		}

		public FunctionBase visitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			String t = context.op.getText();
			if (CharUtil.Equals(t, '*')) {
				return new Function_Mul(args1, args2);
			} else if (CharUtil.Equals(t, '/')) {
				return new Function_Div(args1, args2);
			}
			return new Function_Mod(args1, args2);
		}

		public FunctionBase visitAddSub_fun(mathParser.AddSub_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			String t = context.op.getText();
			if (CharUtil.Equals(t, '&')) {
				return new Function_Connect(args1, args2);
			} else if (CharUtil.Equals(t, '+')) {
				return new Function_Add(args1, args2);
			}
			return new Function_Sub(args1, args2);
		}

		public FunctionBase visitJudge_fun(mathParser.Judge_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);

			String type = context.op.getText();
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

		public FunctionBase visitAndOr_fun(mathParser.AndOr_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			String t = context.op.getText();
			if (CharUtil.Equals(t, "&&", "AND")) {
				return new Function_AND(args1, args2);
			}
			return new Function_OR(args1, args2);
		}

		//#endregion base

		//#region flow

		public FunctionBase visitIF_fun(mathParser.IF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 3) {
				FunctionBase args3 = exprs.get(2).accept(this);
				return new Function_IF(args1, args2, args3);
			}
			return new Function_IF(args1, args2, null);
		}

		public FunctionBase visitIFERROR_fun(mathParser.IFERROR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_IFERROR(args1, args2, args3);
		}

		public FunctionBase visitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
		{
			FunctionBase args1 = this.visit(context.expr());
			return new Function_ISNUMBER(args1);
		}

		public FunctionBase visitIsText_fun(mathParser.IsText_funContext context)
		{
			FunctionBase args1 = this.visit(context.expr());
			return new Function_IsText(args1);
		}

		public FunctionBase visitISERROR_fun(mathParser.ISERROR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 2) {
				FunctionBase args2 = exprs.get(1).accept(this);
				return new Function_ISERROR(args1, args2);
			}
			return new Function_ISERROR(args1, null);
		}

		public FunctionBase visitISNULL_fun(mathParser.ISNULL_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 2) {
				FunctionBase args2 = exprs.get(1).accept(this);
				return new Function_ISNULL(args1, args2);
			}
			return new Function_ISNULL(args1, null);
		}

		public FunctionBase visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 2) {
				FunctionBase args2 = exprs.get(1).accept(this);
				return new Function_ISNULLORERROR(args1, args2);
			}
			return new Function_ISNULLORERROR(args1, null);
		}

		public FunctionBase visitISEVEN_fun(mathParser.ISEVEN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISEVEN(args1);
		}

		public FunctionBase visitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISLOGICAL(args1);
		}

		public FunctionBase visitISODD_fun(mathParser.ISODD_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISODD(args1);
		}

		public FunctionBase visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISNONTEXT(args1);
		}

		public FunctionBase visitAND_fun(mathParser.AND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_AND_N(args);
		}

		public FunctionBase visitOR_fun(mathParser.OR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_OR_N(args);
		}

		public FunctionBase visitNOT_fun(mathParser.NOT_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_NOT(args1);
		}

		public FunctionBase visitTRUE_fun(mathParser.TRUE_funContext context)
		{
			return new Function_Value(Operand.True);
		}

		public FunctionBase visitFALSE_fun(mathParser.FALSE_funContext context)
		{
			return new Function_Value(Operand.False);
		}

		//#endregion flow

		//#region math

		//#region base

		public FunctionBase visitE_fun(mathParser.E_funContext context)
		{
			return new Function_Value(Operand.Create(Math.E), "E");
		}

		public FunctionBase visitPI_fun(mathParser.PI_funContext context)
		{
			return new Function_Value(Operand.Create(Math.PI), "PI");
		}

		public FunctionBase visitABS_fun(mathParser.ABS_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ABS(args1);
		}

		public FunctionBase visitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_QUOTIENT(args1, args2);
		}

		public FunctionBase visitMOD_fun(mathParser.MOD_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_Mod(args1, args2);
		}

		public FunctionBase visitSIGN_fun(mathParser.SIGN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SIGN(args1);
		}

		public FunctionBase visitSQRT_fun(mathParser.SQRT_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SQRT(args1);
		}

		public FunctionBase visitTRUNC_fun(mathParser.TRUNC_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TRUNC(args1);
		}

		public FunctionBase visitINT_fun(mathParser.INT_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TRUNC(args1);
		}

		public FunctionBase visitGCD_fun(mathParser.GCD_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_GCD(args);
		}

		public FunctionBase visitLCM_fun(mathParser.LCM_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_LCM(args);
		}

		public FunctionBase visitCOMBIN_fun(mathParser.COMBIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_COMBIN(args1, args2);
		}

		public FunctionBase visitPERMUT_fun(mathParser.PERMUT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_PERMUT(args1, args2);
		}

		public FunctionBase visitPercentage_fun(mathParser.Percentage_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_Percentage(args1);
		}

		//#endregion base

		//#region trigonometric functions

		public FunctionBase visitDEGREES_fun(mathParser.DEGREES_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_DEGREES(args1);
		}

		public FunctionBase visitRADIANS_fun(mathParser.RADIANS_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_RADIANS(args1);
		}

		public FunctionBase visitCOS_fun(mathParser.COS_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_COS(args1);
		}

		public FunctionBase visitCOSH_fun(mathParser.COSH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_COSH(args1);
		}

		public FunctionBase visitSIN_fun(mathParser.SIN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SIN(args1);
		}

		public FunctionBase visitSINH_fun(mathParser.SINH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SINH(args1);
		}

		public FunctionBase visitTAN_fun(mathParser.TAN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TAN(args1);
		}

		public FunctionBase visitTANH_fun(mathParser.TANH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TANH(args1);
		}

		public FunctionBase visitACOS_fun(mathParser.ACOS_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ACOS(args1);
		}

		public FunctionBase visitACOSH_fun(mathParser.ACOSH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ACOSH(args1);
		}

		public FunctionBase visitASIN_fun(mathParser.ASIN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ASIN(args1);
		}

		public FunctionBase visitASINH_fun(mathParser.ASINH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ASINH(args1);
		}

		public FunctionBase visitATAN_fun(mathParser.ATAN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ATAN(args1);
		}

		public FunctionBase visitATANH_fun(mathParser.ATANH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ATANH(args1);
		}

		public FunctionBase visitATAN2_fun(mathParser.ATAN2_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ATAN2(args1, args2);
		}

		public FunctionBase visitFIXED_fun(mathParser.FIXED_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_FIXED(args1, null, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_FIXED(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_FIXED(args1, args2, args3);
		}

		//#endregion trigonometric functions

		//#region transformation

		public FunctionBase visitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_BIN2OCT(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_BIN2OCT(args1, args2);
		}

		public FunctionBase visitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_BIN2DEC(args1);
		}

		public FunctionBase visitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_BIN2HEX(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_BIN2HEX(args1, args2);
		}

		public FunctionBase visitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_OCT2BIN(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_OCT2BIN(args1, args2);
		}

		public FunctionBase visitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_OCT2DEC(args1);
		}

		public FunctionBase visitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_OCT2HEX(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_OCT2HEX(args1, args2);
		}

		public FunctionBase visitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_DEC2BIN(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_DEC2BIN(args1, args2);
		}

		public FunctionBase visitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_DEC2OCT(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_DEC2OCT(args1, args2);
		}

		public FunctionBase visitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_DEC2HEX(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_DEC2HEX(args1, args2);
		}

		public FunctionBase visitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_HEX2BIN(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_HEX2BIN(args1, args2);
		}

		public FunctionBase visitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_HEX2OCT(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_HEX2OCT(args1, args2);
		}

		public FunctionBase visitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_HEX2DEC(args1);
		}

		//#endregion transformation

		//#region rounding

		public FunctionBase visitROUND_fun(mathParser.ROUND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) {
				return new Function_ROUND(args1, null);
			}
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ROUND(args1, args2);
		}

		public FunctionBase visitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ROUNDDOWN(args1, args2);
		}

		public FunctionBase visitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ROUNDUP(args1, args2);
		}

		public FunctionBase visitCEILING_fun(mathParser.CEILING_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1)
				return new Function_CEILING(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_CEILING(args1, args2);
		}

		public FunctionBase visitFLOOR_fun(mathParser.FLOOR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1)
				return new Function_FLOOR(args1, null);

			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_FLOOR(args1, args2);
		}

		public FunctionBase visitEVEN_fun(mathParser.EVEN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_EVEN(args1);
		}

		public FunctionBase visitODD_fun(mathParser.ODD_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ODD(args1);
		}

		public FunctionBase visitMROUND_fun(mathParser.MROUND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_MROUND(args1, args2);
		}

		//#endregion rounding

		//#region RAND

		public FunctionBase visitRAND_fun(mathParser.RAND_funContext context)
		{
			return new Function_RAND();
		}

		public FunctionBase visitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_RANDBETWEEN(args1, args2);
		}

		//#endregion RAND

		//#region power logarithm factorial

		public FunctionBase visitCOVARIANCES_fun(mathParser.COVARIANCES_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_COVARIANCES(args1, args2);
		}

		public FunctionBase visitCOVAR_fun(mathParser.COVAR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_COVAR(args1, args2);
		}

		public FunctionBase visitFACT_fun(mathParser.FACT_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_FACT(args1);
		}

		public FunctionBase visitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_FACTDOUBLE(args1);
		}

		public FunctionBase visitPOWER_fun(mathParser.POWER_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_POWER(args1, args2);
		}

		public FunctionBase visitEXP_fun(mathParser.EXP_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_EXP(args1);
		}

		public FunctionBase visitLN_fun(mathParser.LN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_LN(args1);
		}

		public FunctionBase visitLOG_fun(mathParser.LOG_funContext context)
		{
			List<ExprContext> exprs = context.expr();

			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() > 1) {
				FunctionBase args2 = exprs.get(1).accept(this);
				return new Function_LOG(args1, args2);
			}
			return new Function_LOG(args1, null);
		}

		public FunctionBase visitLOG10_fun(mathParser.LOG10_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_LOG(args1, null);
		}

		public FunctionBase visitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_MULTINOMIAL(args);
		}

		public FunctionBase visitPRODUCT_fun(mathParser.PRODUCT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_PRODUCT(args);
		}

		public FunctionBase visitSQRTPI_fun(mathParser.SQRTPI_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SQRTPI(args1);
		}

		public FunctionBase visitSUMSQ_fun(mathParser.SUMSQ_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_SUMSQ(args);
		}

		//#endregion

		//#endregion math

		//#region string

		public FunctionBase visitASC_fun(mathParser.ASC_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ASC(args1);
		}

		public FunctionBase visitJIS_fun(mathParser.JIS_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_JIS(args1);
		}

		public FunctionBase visitCHAR_fun(mathParser.CHAR_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_CHAR(args1);
		}

		public FunctionBase visitCLEAN_fun(mathParser.CLEAN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_CLEAN(args1);
		}

		public FunctionBase visitCODE_fun(mathParser.CODE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_CODE(args1);
		}

		public FunctionBase visitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_CONCATENATE(args);
		}

		public FunctionBase visitEXACT_fun(mathParser.EXACT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_EXACT(args1, args2);
		}

		public FunctionBase visitFIND_fun(mathParser.FIND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);

			if (exprs.size() == 2) {
				return new Function_FIND(args1, args2, null);
			}
			FunctionBase count = exprs.get(2).accept(this);
			return new Function_FIND(args1, args2, count);
		}

		public FunctionBase visitLEFT_fun(mathParser.LEFT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) {
				return new Function_LEFT(args1, null);
			}
			return new Function_LEFT(args1, exprs.get(1).accept(this));
		}

		public FunctionBase visitLEN_fun(mathParser.LEN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_LEN(args1);
		}

		public FunctionBase visitLOWER_fun(mathParser.LOWER_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_LOWER(args1);
		}

		public FunctionBase visitMID_fun(mathParser.MID_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_MID(args1, args2, args3);
		}

		public FunctionBase visitPROPER_fun(mathParser.PROPER_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_PROPER(args1);
		}

		public FunctionBase visitREPLACE_fun(mathParser.REPLACE_funContext context)
		{
			List<ExprContext> exprs = context.expr();

			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 3) {
				FunctionBase args22 = exprs.get(1).accept(this);
				FunctionBase args32 = exprs.get(2).accept(this);
				return new Function_REPLACE(args1, args22, args32, null);
			}
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_REPLACE(args1, args2, args3, args4);
		}

		public FunctionBase visitREPT_fun(mathParser.REPT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_REPT(args1, args2);
		}

		public FunctionBase visitRIGHT_fun(mathParser.RIGHT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);

			if (exprs.size() == 1) {
				return new Function_RIGHT(args1);
			}
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_RIGHT(args1, args2);
		}

		public FunctionBase visitRMB_fun(mathParser.RMB_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_RMB(args1);
		}

		public FunctionBase visitSEARCH_fun(mathParser.SEARCH_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);

			if (exprs.size() == 2) {
				return new Function_SEARCH(args1, args2, null);
			}
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_SEARCH(args1, args2, args3);
		}

		public FunctionBase visitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			if (exprs.size() == 3) {
				return new Function_SUBSTITUTE(args1, args2, args3, null);
			}
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_SUBSTITUTE(args1, args2, args3, args4);
		}

		public FunctionBase visitT_fun(mathParser.T_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_T(args1);
		}

		public FunctionBase visitTEXT_fun(mathParser.TEXT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TEXT(args1, args2);
		}

		public FunctionBase visitTRIM_fun(mathParser.TRIM_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TRIM(args1);
		}

		public FunctionBase visitUPPER_fun(mathParser.UPPER_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_UPPER(args1);
		}

		public FunctionBase visitVALUE_fun(mathParser.VALUE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_VALUE(args1);
		}

		//#endregion string

		//#region MyDate time

		public FunctionBase visitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_DATEVALUE(args);
		}

		public FunctionBase visitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_TIMESTAMP(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TIMESTAMP(args1, args2);
		}

		public FunctionBase visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_TIMEVALUE(args1);
		}

		public FunctionBase visitDATE_fun(mathParser.DATE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_DATE(args);
		}

		public FunctionBase visitTIME_fun(mathParser.TIME_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_TIME(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_TIME(args1, args2, args3);
		}

		public FunctionBase visitNOW_fun(mathParser.NOW_funContext context)
		{
			return new Function_NOW();
		}

		public FunctionBase visitTODAY_fun(mathParser.TODAY_funContext context)
		{
			return new Function_TODAY();
		}

		public FunctionBase visitYEAR_fun(mathParser.YEAR_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_YEAR(args1);
		}

		public FunctionBase visitMONTH_fun(mathParser.MONTH_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_MONTH(args1);
		}

		public FunctionBase visitDAY_fun(mathParser.DAY_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_DAY(args1);
		}

		public FunctionBase visitHOUR_fun(mathParser.HOUR_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_HOUR(args1);
		}

		public FunctionBase visitMINUTE_fun(mathParser.MINUTE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_MINUTE(args1);
		}

		public FunctionBase visitSECOND_fun(mathParser.SECOND_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_SECOND(args1);
		}

		public FunctionBase visitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_WEEKDAY(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_WEEKDAY(args1, args2);
		}

		public FunctionBase visitDATEDIF_fun(mathParser.DATEDIF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_DATEDIF(args1, args2, args3);
		}

		public FunctionBase visitDAYS360_fun(mathParser.DAYS360_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 3) {
				FunctionBase args3 = exprs.get(2).accept(this);
				return new Function_DAYS360(args1, args2, args3);
			}
			return new Function_DAYS360(args1, args2, null);
		}

		public FunctionBase visitEDATE_fun(mathParser.EDATE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_EDATE(args1, args2);
		}

		public FunctionBase visitEOMONTH_fun(mathParser.EOMONTH_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_EOMONTH(args1, args2);
		}

		public FunctionBase visitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_NETWORKDAYS(args);
		}

		public FunctionBase visitWORKDAY_fun(mathParser.WORKDAY_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_WORKDAY(args);
		}

		public FunctionBase visitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_WEEKNUM(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_WEEKNUM(args1, args2);
		}

		public FunctionBase visitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDMONTHS(args1, args2);
		}

		public FunctionBase visitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDYEARS(args1, args2);
		}

		public FunctionBase visitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDSECONDS(args1, args2);
		}

		public FunctionBase visitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDMINUTES(args1, args2);
		}

		public FunctionBase visitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDDAYS(args1, args2);
		}

		public FunctionBase visitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ADDHOURS(args1, args2);
		}

		//#endregion MyDate time

		//#region sum

		public FunctionBase visitMAX_fun(mathParser.MAX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_MAX(args);
		}

		public FunctionBase visitMEDIAN_fun(mathParser.MEDIAN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_MEDIAN(args);
		}

		public FunctionBase visitMIN_fun(mathParser.MIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_MIN(args);
		}

		public FunctionBase visitQUARTILE_fun(mathParser.QUARTILE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_QUARTILE(args1, args2);
		}

		public FunctionBase visitMODE_fun(mathParser.MODE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_MODE(args);
		}

		public FunctionBase visitLARGE_fun(mathParser.LARGE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_LARGE(args1, args2);
		}

		public FunctionBase visitSMALL_fun(mathParser.SMALL_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_SMALL(args1, args2);
		}

		public FunctionBase visitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_PERCENTILE(args1, args2);
		}

		public FunctionBase visitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_PERCENTRANK(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_PERCENTRANK(args1, args2, args3);
		}

		public FunctionBase visitAVERAGE_fun(mathParser.AVERAGE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_AVERAGE(args);
		}

		public FunctionBase visitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_AVERAGEIF(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_AVERAGEIF(args1, args2, args3);
		}

		public FunctionBase visitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_GEOMEAN(args);
		}

		public FunctionBase visitHARMEAN_fun(mathParser.HARMEAN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_HARMEAN(args);
		}

		public FunctionBase visitCOUNT_fun(mathParser.COUNT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_COUNT(args);
		}

		public FunctionBase visitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_COUNTIF(args1, args2);
		}

		public FunctionBase visitSUM_fun(mathParser.SUM_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_SUM(args);
		}

		public FunctionBase visitSUMIF_fun(mathParser.SUMIF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_SUMIF(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_SUMIF(args1, args2, args3);
		}

		public FunctionBase visitAVEDEV_fun(mathParser.AVEDEV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_AVEDEV(args);
		}

		public FunctionBase visitSTDEV_fun(mathParser.STDEV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_STDEV(args);
		}

		public FunctionBase visitSTDEVP_fun(mathParser.STDEVP_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_STDEVP(args);
		}

		public FunctionBase visitDEVSQ_fun(mathParser.DEVSQ_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_DEVSQ(args);
		}

		public FunctionBase visitVAR_fun(mathParser.VAR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_VAR(args);
		}

		public FunctionBase visitVARP_fun(mathParser.VARP_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_VARP(args);
		}

		public FunctionBase visitNORMDIST_fun(mathParser.NORMDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_NORMDIST(args1, args2, args3, args4);
		}

		public FunctionBase visitNORMINV_fun(mathParser.NORMINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_NORMINV(args1, args2, args3);
		}

		public FunctionBase visitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_NORMSDIST(args1);
		}

		public FunctionBase visitNORMSINV_fun(mathParser.NORMSINV_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_NORMSINV(args1);
		}

		public FunctionBase visitBETADIST_fun(mathParser.BETADIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_BETADIST(args1, args2, args3);
		}

		public FunctionBase visitBETAINV_fun(mathParser.BETAINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_BETAINV(args1, args2, args3);
		}

		public FunctionBase visitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_BINOMDIST(args1, args2, args3, args4);
		}

		public FunctionBase visitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_EXPONDIST(args1, args2, args3);
		}

		public FunctionBase visitFDIST_fun(mathParser.FDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_FDIST(args1, args2, args3);
		}

		public FunctionBase visitFINV_fun(mathParser.FINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_FINV(args1, args2, args3);
		}

		public FunctionBase visitFISHER_fun(mathParser.FISHER_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_FISHER(args1);
		}

		public FunctionBase visitFISHERINV_fun(mathParser.FISHERINV_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_FISHERINV(args1);
		}

		public FunctionBase visitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_GAMMADIST(args1, args2, args3, args4);
		}

		public FunctionBase visitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_GAMMAINV(args1, args2, args3);
		}

		public FunctionBase visitGAMMALN_fun(mathParser.GAMMALN_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_GAMMALN(args1);
		}

		public FunctionBase visitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_HYPGEOMDIST(args1, args2, args3, args4);
		}

		public FunctionBase visitLOGINV_fun(mathParser.LOGINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_LOGINV(args1, args2, args3);
		}

		public FunctionBase visitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_LOGNORMDIST(args1, args2, args3);
		}

		public FunctionBase visitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_NEGBINOMDIST(args1, args2, args3);
		}

		public FunctionBase visitPOISSON_fun(mathParser.POISSON_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_POISSON(args1, args2, args3);
		}

		public FunctionBase visitTDIST_fun(mathParser.TDIST_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_TDIST(args1, args2, args3);
		}

		public FunctionBase visitTINV_fun(mathParser.TINV_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TINV(args1, args2);
		}

		public FunctionBase visitWEIBULL_fun(mathParser.WEIBULL_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_WEIBULL(args1, args2, args3, args4);
		}

		//#endregion sum

		//#region csharp

		public FunctionBase visitURLENCODE_fun(mathParser.URLENCODE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_URLENCODE(args1);
		}

		public FunctionBase visitURLDECODE_fun(mathParser.URLDECODE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_URLDECODE(args1);
		}

		public FunctionBase visitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_HTMLENCODE(args1);
		}

		public FunctionBase visitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_HTMLDECODE(args1);
		}

		public FunctionBase visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_BASE64TOTEXT(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_BASE64TOTEXT(args1, args2);
		}

		public FunctionBase visitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_BASE64URLTOTEXT(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_BASE64URLTOTEXT(args1, args2);
		}

		public FunctionBase visitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_TEXTTOBASE64(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TEXTTOBASE64(args1, args2);
		}

		public FunctionBase visitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_TEXTTOBASE64URL(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TEXTTOBASE64URL(args1, args2);
		}

		public FunctionBase visitREGEX_fun(mathParser.REGEX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_REGEX(args1, args2);
		}

		public FunctionBase visitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_REGEXREPALCE(args1, args2, args3);
		}

		public FunctionBase visitISREGEX_fun(mathParser.ISREGEX_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_ISREGEX(args1, args2);
		}

		public FunctionBase visitGUID_fun(mathParser.GUID_funContext context)
		{
			return new Function_GUID();
		}

		public FunctionBase visitMD5_fun(mathParser.MD5_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_MD5(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_MD5(args1, args2);
		}

		public FunctionBase visitSHA1_fun(mathParser.SHA1_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_SHA1(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_SHA1(args1, args2);
		}

		public FunctionBase visitSHA256_fun(mathParser.SHA256_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_SHA256(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_SHA256(args1, args2);
		}

		public FunctionBase visitSHA512_fun(mathParser.SHA512_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_SHA512(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_SHA512(args1, args2);
		}

		public FunctionBase visitHMACMD5_fun(mathParser.HMACMD5_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_HMACMD5(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_HMACMD5(args1, args2, args3);
		}

		public FunctionBase visitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_HMACSHA1(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_HMACSHA1(args1, args2, args3);
		}

		public FunctionBase visitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_HMACSHA256(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_HMACSHA256(args1, args2, args3);
		}

		public FunctionBase visitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_HMACSHA512(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_HMACSHA512(args1, args2, args3);
		}

		public FunctionBase visitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_TRIMSTART(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TRIMSTART(args1, args2);
		}

		public FunctionBase visitTRIMEND_fun(mathParser.TRIMEND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) return new Function_TRIMEND(args1, null);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_TRIMEND(args1, args2);
		}

		public FunctionBase visitINDEXOF_fun(mathParser.INDEXOF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_INDEXOF(args1, args2, null, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			if (exprs.size() == 3) return new Function_INDEXOF(args1, args2, args3, null);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_INDEXOF(args1, args2, args3, args4);
		}

		public FunctionBase visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_LASTINDEXOF(args1, args2, null, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			if (exprs.size() == 3) return new Function_LASTINDEXOF(args1, args2, args3, null);
			FunctionBase args4 = exprs.get(3).accept(this);
			return new Function_LASTINDEXOF(args1, args2, args3, args4);
		}

		public FunctionBase visitSPLIT_fun(mathParser.SPLIT_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_SPLIT(args1, args2);
		}

		public FunctionBase visitJOIN_fun(mathParser.JOIN_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_JOIN(args);
		}

		public FunctionBase visitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_SUBSTRING(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_SUBSTRING(args1, args2, args3);
		}

		public FunctionBase visitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_STARTSWITH(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_STARTSWITH(args1, args2, args3);
		}

		public FunctionBase visitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_ENDSWITH(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_ENDSWITH(args1, args2, args3);
		}

		public FunctionBase visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISNULLOREMPTY(args1);
		}

		public FunctionBase visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_ISNULLORWHITESPACE(args1);
		}

		public FunctionBase visitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_REMOVESTART(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_REMOVESTART(args1, args2, args3);
		}

		public FunctionBase visitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			if (exprs.size() == 2) return new Function_REMOVEEND(args1, args2, null);
			FunctionBase args3 = exprs.get(2).accept(this);
			return new Function_REMOVEEND(args1, args2, args3);
		}

		public FunctionBase visitJSON_fun(mathParser.JSON_funContext context)
		{
			FunctionBase args1 = context.expr().accept(this);
			return new Function_JSON(args1);
		}

		//#endregion csharp

		//#region LOOKFLOOR LOOKCEILING
		public FunctionBase visitLOOKFLOOR_fun(mathParser.LOOKFLOOR_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_LOOKFLOOR(args1, args2);
		}

		public FunctionBase visitLOOKCEILING_fun(mathParser.LOOKCEILING_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase args1 = exprs.get(0).accept(this);
			FunctionBase args2 = exprs.get(1).accept(this);
			return new Function_LOOKCEILING(args1, args2);
		} 
		//#endregion

		//#region getValue

		public FunctionBase visitArray_fun(mathParser.Array_funContext context)
		{
			List<ExprContext> exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_Array(args);
		}

		public FunctionBase visitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().accept(this);
		}

		public FunctionBase visitNUM_fun(mathParser.NUM_funContext context)
		{
			double d = Double.parseDouble(context.num().getText());
			if (context.unit() == null) { return new Function_Value(Operand.Create(d), context.num().getText()); }
			String unit = context.unit().getText();
			return new Function_NUM(d, unit);
		}

		public FunctionBase visitNum(mathParser.NumContext context)
		{
			double d = Double.parseDouble(context.getText());
			return new Function_Value(Operand.Create(d), context.getText());
		}

		public FunctionBase visitUnit(mathParser.UnitContext context)
		{
			return new Function_Value(Operand.Create(context.getText()));
		}

		public FunctionBase visitSTRING_fun(mathParser.STRING_funContext context)
		{
			String opd = context.getText();
			StringBuilder sb = new StringBuilder();
			int index = 1;
			while (index < opd.length() - 1) {
				char c = opd.charAt(index++);
				if (c == '\\') {
					char c2 = opd.charAt(index++);
					if (c2 == 'n') sb.append('\n');
					else if (c2 == 'r') sb.append('\r');
					else if (c2 == 't') sb.append('\t');
					else if (c2 == '0') sb.append('\0');
					else if (c2 == 'v') sb.append('\v');
					else if (c2 == 'a') sb.append('\a');
					else if (c2 == 'b') sb.append('\b');
					else if (c2 == 'f') sb.append('\f');
					else sb.append(opd.charAt(index++));
				} else {
					sb.append(c);
				}
			}
			return new Function_Value(Operand.Create(sb.toString()));
		}

		public FunctionBase visitNULL_fun(mathParser.NULL_funContext context)
		{
			return new Function_Value(Operand.CreateNull(), "NULL");
		}

		public FunctionBase visitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			return new Function_PARAMETER(node.GetText());
		}

		public FunctionBase visitParameter2(mathParser.Parameter2Context context)
		{
			return new Function_Value(Operand.Create(context.children[0].GetText()));
		}

		public FunctionBase visitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			var exprs = context.expr();
			var args1 = exprs.get(0).accept(this);
			if (context.PARAMETER()!=null) {
				var op = new Function_PARAMETER(context.PARAMETER().GetText());
				return new Function_GetJsonValue(args1, op);
			}
			if (context.parameter2() != null) {
				var op = context.parameter2().accept(this);
				return new Function_GetJsonValue(args1, op);
			}
			var op2 = exprs.get(1).accept(this);
			return new Function_GetJsonValue(args1, op2);
		}

		public FunctionBase visitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			var funName = context.PARAMETER().GetText();
			var exprs = context.expr();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_DiyFunction(funName, args);
		}

		public FunctionBase visitPARAM_fun(mathParser.PARAM_funContext context)
		{
			var exprs = context.expr();
			var args1 = exprs.get(0).accept(this);
			if (exprs.size() == 1) {
				return new Function_PARAM(args1, null);
			}
			var args2 = exprs.get(1).accept(this);
			return new Function_PARAM(args1, args2);
		}

		public FunctionBase visitHAS_fun(mathParser.HAS_funContext context)
		{
			var exprs = context.expr();
			var args1 = exprs.get(0).accept(this);
			var args2 = exprs.get(1).accept(this);
			return new Function_HAS(args1, args2);
		}

		public FunctionBase visitHASVALUE_fun(mathParser.HASVALUE_funContext context)
		{
			var exprs = context.expr();
			var args1 = exprs.get(0).accept(this);
			var args2 = exprs.get(1).accept(this);
			return new Function_HASVALUE(args1, args2);
		}

		public FunctionBase visitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			var exprs = context.arrayJson();
			FunctionBase[] args = new FunctionBase[exprs.size()];
			for (int i = 0; i < exprs.size(); i++) {
				args[i] = exprs.get(i).accept(this);
			}
			return new Function_ArrayJson(args);
		}

		public FunctionBase visitArrayJson(mathParser.ArrayJsonContext context)
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
				var sb = new StringBuilder(opd.Length - 2);
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
			var f = context.expr().accept(this);
			return new Function_ArrayJsonItem(keyName, f);
		}

		public FunctionBase visitERROR_fun(mathParser.ERROR_funContext context)
		{
			if (context.expr() == null) { return new Function_Value(Operand.Error("")); }
			var args1 = context.expr().accept(this);
			return new Function_ERROR(args1);
		}

		public FunctionBase visitVersion_fun(mathParser.Version_funContext context)
		{
			return new Function_Value(Operand.Version);
		}



		//#endregion getValue
	}
