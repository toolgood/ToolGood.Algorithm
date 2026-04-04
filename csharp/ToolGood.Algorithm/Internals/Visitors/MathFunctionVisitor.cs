using Antlr4.Runtime.Tree;
using System;
using System.Data.Common;
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
		public FunctionBase VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept(this);
		}
		public FunctionBase VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			if(type == mathLexer.OPMUL) {
				return new Function_Mul(funcs);
			} else if(type == mathLexer.OPDIV) {
				return new Function_Div(funcs);
			}
			return new Function_Mod(funcs);
		}
		public FunctionBase VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			if(type == mathLexer.OPADD) {
				return new Function_Add(funcs);
			} else if(type == mathLexer.OPSUB) {
				return new Function_Sub(funcs);
			}
			return new Function_Connect(funcs);
		}
		public FunctionBase VisitJudge_fun(mathParser.Judge_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			switch(type) {
				case mathLexer.OPGT: return new Function_GT(funcs);
				case mathLexer.OPGE: return new Function_GE(funcs);
				case mathLexer.OPLT: return new Function_LT(funcs);
				case mathLexer.OPLE: return new Function_LE(funcs);
				case mathLexer.OPEQ: return new Function_EQ(funcs);
				case mathLexer.OPNE:
				default: return new Function_NE(funcs);
			}
		}
		public FunctionBase VisitOr_fun(mathParser.Or_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_OR(funcs);
		}

		public FunctionBase VisitAnd_fun(mathParser.And_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_AND(funcs);
		}

		public FunctionBase VisitIF_fun(mathParser.IF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IF(funcs);
		}

		public FunctionBase VisitNOT_fun(mathParser.NOT_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_NOT(args1);
		}
		public FunctionBase VisitCONST2_fun(mathParser.CONST2_funContext context)
		{
			var type = context.f.Type;
			switch(type) {
				case mathLexer.TRUE: return new Function_ValueBoolean(true);
				case mathLexer.FALSE: return new Function_ValueBoolean(false);
				case mathLexer.ALGORITHMVERSION: return new Function_Version();
				case mathLexer.NULL:
				default: return new Function_NULL();
			}
		}

		public FunctionBase VisitCONST_fun(mathParser.CONST_funContext context)
		{
			var type = context.f.Type;
			switch(type) {
				case mathLexer.E: return new Function_Number_E();
				case mathLexer.PI: return new Function_Number_PI();
				case mathLexer.RAND: return new Function_RAND();
				case mathLexer.GUID: return new Function_GUID();
				case mathLexer.NOW: return new Function_NOW();
				case mathLexer.TODAY:
				default: return new Function_TODAY();
			}
		}

		public FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_Percentage(args1);
		}

		#region getValue
		public FunctionBase VisitArray_fun(mathParser.Array_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_ARRAY(funcs);
		}
		public FunctionBase VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}
		public FunctionBase VisitNUM_fun(mathParser.NUM_funContext context)
		{
			var text = context.GetText();
			if(decimal.TryParse(text.AsSpan(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
				return new Function_Number(Operand.Create(d));
			}
			string txt;
			string unit;
			var len = text.Length;
			if(len > 3 && text[len - 3] >= 'A' ) {
				txt=text.Substring(0, len - 3);
				unit=text.Substring(len - 3);
			} else if(len > 2 && text[len - 2] >= 'A') {
				txt = text.Substring(0, len - 2);
				unit = text.Substring(len - 2);
			} else {
				txt = text.Substring(0, len - 1);
				unit = text.Substring(len - 1);
			}
			var d2 = decimal.Parse(txt, NumberStyles.Any, CultureInfo.InvariantCulture);
			return new Function_Number2(d2, unit);
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

		#endregion getValue

		public FunctionBase VisitFunction_fun(mathParser.Function_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.f.Type;
			switch(type) {
				case mathLexer.ERROR: return new Function_ERROR(funcs);
				case mathLexer.YEAR: return new Function_YEAR(funcs);
				case mathLexer.MONTH: return new Function_MONTH(funcs);
				case mathLexer.DAY: return new Function_DAY(funcs);
				case mathLexer.HOUR: return new Function_HOUR(funcs);
				case mathLexer.MINUTE: return new Function_MINUTE(funcs);
				case mathLexer.SECOND: return new Function_SECOND(funcs);
				case mathLexer.ISNUMBER: return new Function_ISNUMBER(funcs);
				case mathLexer.ISTEXT: return new Function_ISTEXT(funcs);
				case mathLexer.ISNONTEXT: return new Function_ISNONTEXT(funcs);
				case mathLexer.ISLOGICAL: return new Function_ISLOGICAL(funcs);
				case mathLexer.ISEVEN: return new Function_ISEVEN(funcs);
				case mathLexer.ISODD: return new Function_ISODD(funcs);
				case mathLexer.NOT: return new Function_NOT(funcs);
				case mathLexer.ABS: return new Function_ABS(funcs);
				case mathLexer.SIGN: return new Function_SIGN(funcs);
				case mathLexer.SQRT: return new Function_SQRT(funcs);
				case mathLexer.INT: return new Function_INT(funcs);
				case mathLexer.DEGREES: return new Function_DEGREES(funcs);
				case mathLexer.RADIANS: return new Function_RADIANS(funcs);
				case mathLexer.COS: return new Function_COS(funcs);
				case mathLexer.COSH: return new Function_COSH(funcs);
				case mathLexer.SIN: return new Function_SIN(funcs);
				case mathLexer.SINH: return new Function_SINH(funcs);
				case mathLexer.TAN: return new Function_TAN(funcs);
				case mathLexer.TANH: return new Function_TANH(funcs);
				case mathLexer.COT: return new Function_COT(funcs);
				case mathLexer.COTH: return new Function_COTH(funcs);
				case mathLexer.CSC: return new Function_CSC(funcs);
				case mathLexer.CSCH: return new Function_CSCH(funcs);
				case mathLexer.SEC: return new Function_SEC(funcs);
				case mathLexer.SECH: return new Function_SECH(funcs);
				case mathLexer.ACOS: return new Function_ACOS(funcs);
				case mathLexer.ACOSH: return new Function_ACOSH(funcs);
				case mathLexer.ASIN: return new Function_ASIN(funcs);
				case mathLexer.ASINH: return new Function_ASINH(funcs);
				case mathLexer.ATAN: return new Function_ATAN(funcs);
				case mathLexer.ATANH: return new Function_ATANH(funcs);
				case mathLexer.ACOT: return new Function_ACOT(funcs);
				case mathLexer.ACOTH: return new Function_ACOTH(funcs);
				case mathLexer.EVEN: return new Function_EVEN(funcs);
				case mathLexer.ODD: return new Function_ODD(funcs);
				case mathLexer.FACT: return new Function_FACT(funcs);
				case mathLexer.FACTDOUBLE: return new Function_FACTDOUBLE(funcs);
				case mathLexer.EXP: return new Function_EXP(funcs);
				case mathLexer.LN: return new Function_LN(funcs);
				case mathLexer.LOG10: return new Function_LOG10(funcs);
				case mathLexer.SQRTPI: return new Function_SQRTPI(funcs);
				case mathLexer.ERF: return new Function_ERF(funcs);
				case mathLexer.ERFC: return new Function_ERFC(funcs);
				case mathLexer.ARABIC: return new Function_ARABIC(funcs);
				case mathLexer.ASC: return new Function_ASC(funcs);
				case mathLexer.JIS: return new Function_JIS(funcs);
				case mathLexer.CHAR: return new Function_CHAR(funcs);
				case mathLexer.CLEAN: return new Function_CLEAN(funcs);
				case mathLexer.CODE: return new Function_CODE(funcs);
				case mathLexer.UNICHAR: return new Function_UNICHAR(funcs);
				case mathLexer.UNICODE: return new Function_UNICODE(funcs);
				case mathLexer.LEN: return new Function_LEN(funcs);
				case mathLexer.LOWER: return new Function_LOWER(funcs);
				case mathLexer.PROPER: return new Function_PROPER(funcs);
				case mathLexer.TRIM: return new Function_TRIM(funcs);
				case mathLexer.UPPER: return new Function_UPPER(funcs);
				case mathLexer.VALUE: return new Function_VALUE(funcs);
				case mathLexer.TIMEVALUE: return new Function_TIMEVALUE(funcs);
				case mathLexer.NORMSDIST: return new Function_NORMSDIST(funcs);
				case mathLexer.NORMSINV: return new Function_NORMSINV(funcs);
				case mathLexer.FISHER: return new Function_FISHER(funcs);
				case mathLexer.FISHERINV: return new Function_FISHERINV(funcs);
				case mathLexer.GAMMALN: return new Function_GAMMALN(funcs);
				case mathLexer.URLENCODE: return new Function_URLENCODE(funcs);
				case mathLexer.URLDECODE: return new Function_URLDECODE(funcs);
				case mathLexer.HTMLENCODE: return new Function_HTMLENCODE(funcs);
				case mathLexer.HTMLDECODE: return new Function_HTMLDECODE(funcs);
				case mathLexer.BASE64TOTEXT: return new Function_BASE64TOTEXT(funcs);
				case mathLexer.BASE64URLTOTEXT: return new Function_BASE64URLTOTEXT(funcs);
				case mathLexer.TEXTTOBASE64: return new Function_TEXTTOBASE64(funcs);
				case mathLexer.TEXTTOBASE64URL: return new Function_TEXTTOBASE64URL(funcs);
				case mathLexer.ISNULLOREMPTY: return new Function_ISNULLOREMPTY(funcs);
				case mathLexer.ISNULLORWHITESPACE: return new Function_ISNULLORWHITESPACE(funcs);
				case mathLexer.JSON: return new Function_JSON(funcs);
				case mathLexer.T: return new Function_T(funcs);
				case mathLexer.RMB: return new Function_RMB(funcs);
				case mathLexer.MD5: return new Function_MD5(funcs);
				case mathLexer.SHA1: return new Function_SHA1(funcs);
				case mathLexer.SHA256: return new Function_SHA256(funcs);
				case mathLexer.SHA512: return new Function_SHA512(funcs);
				case mathLexer.DEC2BIN: return new Function_DEC2BIN(funcs);
				case mathLexer.DEC2HEX: return new Function_DEC2HEX(funcs);
				case mathLexer.DEC2OCT: return new Function_DEC2OCT(funcs);
				case mathLexer.HEX2BIN: return new Function_HEX2BIN(funcs);
				case mathLexer.HEX2DEC: return new Function_HEX2DEC(funcs);
				case mathLexer.HEX2OCT: return new Function_HEX2OCT(funcs);
				case mathLexer.OCT2BIN: return new Function_OCT2BIN(funcs);
				case mathLexer.OCT2DEC: return new Function_OCT2DEC(funcs);
				case mathLexer.OCT2HEX: return new Function_OCT2HEX(funcs);
				case mathLexer.BIN2OCT: return new Function_BIN2OCT(funcs);
				case mathLexer.BIN2DEC: return new Function_BIN2DEC(funcs);
				case mathLexer.BIN2HEX: return new Function_BIN2HEX(funcs);
				case mathLexer.ISERROR: return new Function_ISERROR(funcs);
				case mathLexer.ISNULL: return new Function_ISNULL(funcs);
				case mathLexer.ISNULLORERROR: return new Function_ISNULLORERROR(funcs);
				case mathLexer.TRUNC: return new Function_TRUNC(funcs);
				case mathLexer.ROUND: return new Function_ROUND(funcs);
				case mathLexer.CEILING: return new Function_CEILING(funcs);
				case mathLexer.FLOOR: return new Function_FLOOR(funcs);
				case mathLexer.LOG: return new Function_LOG(funcs);
				case mathLexer.DELTA: return new Function_DELTA(funcs);
				case mathLexer.GESTEP: return new Function_GESTEP(funcs);
				case mathLexer.ROMAN: return new Function_ROMAN(funcs);
				case mathLexer.LEFT: return new Function_LEFT(funcs);
				case mathLexer.RIGHT: return new Function_RIGHT(funcs);
				case mathLexer.SEARCH: return new Function_SEARCH(funcs);
				case mathLexer.WEEKDAY: return new Function_WEEKDAY(funcs);
				case mathLexer.WEEKNUM: return new Function_WEEKNUM(funcs);
				case mathLexer.IRR: return new Function_IRR(funcs);
				case mathLexer.TRIMSTART: return new Function_TRIMSTART(funcs);
				case mathLexer.TRIMEND: return new Function_TRIMEND(funcs);
				case mathLexer.TIMESTAMP: return new Function_TIMESTAMP(funcs);
				case mathLexer.PARAM: return new Function_PARAM(funcs);
				case mathLexer.DATEVALUE: return new Function_DATEVALUE(funcs);
				case mathLexer.DAYS360: return new Function_DAYS360(funcs);
				case mathLexer.NETWORKDAYS: return new Function_NETWORKDAYS(funcs);
				case mathLexer.WORKDAY: return new Function_WORKDAY(funcs);
				case mathLexer.AVERAGEIF: return new Function_AVERAGEIF(funcs);
				case mathLexer.SUMIF: return new Function_SUMIF(funcs);
				case mathLexer.XIRR: return new Function_XIRR(funcs);
				case mathLexer.IF: return new Function_IF(funcs);
				case mathLexer.IFERROR: return new Function_IFERROR(funcs);
				case mathLexer.TIME: return new Function_TIME(funcs);
				case mathLexer.YEARFRAC: return new Function_YEARFRAC(funcs);
				case mathLexer.SUBSTRING: return new Function_SUBSTRING(funcs);
				case mathLexer.STARTSWITH: return new Function_STARTSWITH(funcs);
				case mathLexer.ENDSWITH: return new Function_ENDSWITH(funcs);
				case mathLexer.FIND: return new Function_FIND(funcs);
				case mathLexer.RANK: return new Function_RANK(funcs);
				case mathLexer.SUBSTITUTE: return new Function_SUBSTITUTE(funcs);
				case mathLexer.REPLACE: return new Function_REPLACE(funcs);
				case mathLexer.DB: return new Function_DB(funcs);
				case mathLexer.DDB: return new Function_DDB(funcs);
				case mathLexer.FIXED: return new Function_FIXED(funcs);
				case mathLexer.REMOVESTART: return new Function_REMOVESTART(funcs);
				case mathLexer.REMOVEEND: return new Function_REMOVEEND(funcs);
				case mathLexer.ARRAY: return new Function_ARRAY(funcs);
				case mathLexer.AND: return new Function_AND_N(funcs);
				case mathLexer.OR: return new Function_OR_N(funcs);
				case mathLexer.XOR: return new Function_XOR(funcs);
				case mathLexer.GCD: return new Function_GCD(funcs);
				case mathLexer.LCM: return new Function_LCM(funcs);
				case mathLexer.MULTINOMIAL: return new Function_MULTINOMIAL(funcs);
				case mathLexer.PRODUCT: return new Function_PRODUCT(funcs);
				case mathLexer.SUMSQ: return new Function_SUMSQ(funcs);
				case mathLexer.SUMPRODUCT: return new Function_SUMPRODUCT(funcs);
				case mathLexer.CONCATENATE: return new Function_CONCATENATE(funcs);
				case mathLexer.MAX: return new Function_MAX(funcs);
				case mathLexer.MEDIAN: return new Function_MEDIAN(funcs);
				case mathLexer.MIN: return new Function_MIN(funcs);
				case mathLexer.MODE: return new Function_MODE(funcs);
				case mathLexer.AVERAGE: return new Function_AVERAGE(funcs);
				case mathLexer.GEOMEAN: return new Function_GEOMEAN(funcs);
				case mathLexer.HARMEAN: return new Function_HARMEAN(funcs);
				case mathLexer.COUNT: return new Function_COUNT(funcs);
				case mathLexer.SUM: return new Function_SUM(funcs);
				case mathLexer.AVEDEV: return new Function_AVEDEV(funcs);
				case mathLexer.STDEV: return new Function_STDEV(funcs);
				case mathLexer.STDEVP: return new Function_STDEVP(funcs);
				case mathLexer.DEVSQ: return new Function_DEVSQ(funcs);
				case mathLexer.VAR: return new Function_VAR(funcs);
				case mathLexer.VARP: return new Function_VARP(funcs);
				case mathLexer.NPV: return new Function_NPV(funcs);
				case mathLexer.QUOTIENT: return new Function_QUOTIENT(funcs);
				case mathLexer.MOD: return new Function_Mod(funcs);
				case mathLexer.COMBIN: return new Function_COMBIN(funcs);
				case mathLexer.PERMUT: return new Function_PERMUT(funcs);
				case mathLexer.ATAN2: return new Function_ATAN2(funcs);
				case mathLexer.ROUNDDOWN: return new Function_ROUNDDOWN(funcs);
				case mathLexer.ROUNDUP: return new Function_ROUNDUP(funcs);
				case mathLexer.MROUND: return new Function_MROUND(funcs);
				case mathLexer.RANDBETWEEN: return new Function_RANDBETWEEN(funcs);
				case mathLexer.POWER: return new Function_POWER(funcs);
				case mathLexer.BESSELI: return new Function_BESSELI(funcs);
				case mathLexer.BESSELJ: return new Function_BESSELJ(funcs);
				case mathLexer.BESSELK: return new Function_BESSELK(funcs);
				case mathLexer.BESSELY: return new Function_BESSELY(funcs);
				case mathLexer.SUMX2MY2: return new Function_SUMX2MY2(funcs);
				case mathLexer.SUMX2PY2: return new Function_SUMX2PY2(funcs);
				case mathLexer.SUMXMY2: return new Function_SUMXMY2(funcs);
				case mathLexer.EXACT: return new Function_EXACT(funcs);
				case mathLexer.REPT: return new Function_REPT(funcs);
				case mathLexer.TEXT: return new Function_TEXT(funcs);
				case mathLexer.DAYS: return new Function_DAYS(funcs);
				case mathLexer.EDATE: return new Function_EDATE(funcs);
				case mathLexer.EOMONTH: return new Function_EOMONTH(funcs);
				case mathLexer.QUARTILE: return new Function_QUARTILE(funcs);
				case mathLexer.LARGE: return new Function_LARGE(funcs);
				case mathLexer.SMALL: return new Function_SMALL(funcs);
				case mathLexer.PERCENTILE: return new Function_PERCENTILE(funcs);
				case mathLexer.PERCENTRANK: return new Function_PERCENTRANK(funcs);
				case mathLexer.COVAR: return new Function_COVAR(funcs);
				case mathLexer.COVARIANCES: return new Function_COVARIANCES(funcs);
				case mathLexer.TINV: return new Function_TINV(funcs);
				case mathLexer.REGEX: return new Function_REGEX(funcs);
				case mathLexer.ISREGEX: return new Function_ISREGEX(funcs);
				case mathLexer.HMACMD5: return new Function_HMACMD5(funcs);
				case mathLexer.HMACSHA1: return new Function_HMACSHA1(funcs);
				case mathLexer.HMACSHA256: return new Function_HMACSHA256(funcs);
				case mathLexer.HMACSHA512: return new Function_HMACSHA512(funcs);
				case mathLexer.SPLIT: return new Function_SPLIT(funcs);
				case mathLexer.LOOKCEILING: return new Function_LOOKCEILING(funcs);
				case mathLexer.LOOKFLOOR: return new Function_LOOKFLOOR(funcs);
				case mathLexer.ADDYEARS: return new Function_ADDYEARS(funcs);
				case mathLexer.ADDMONTHS: return new Function_ADDMONTHS(funcs);
				case mathLexer.ADDDAYS: return new Function_ADDDAYS(funcs);
				case mathLexer.ADDHOURS: return new Function_ADDHOURS(funcs);
				case mathLexer.ADDMINUTES: return new Function_ADDMINUTES(funcs);
				case mathLexer.ADDSECONDS: return new Function_ADDSECONDS(funcs);
				case mathLexer.HAS: return new Function_HAS(funcs);
				case mathLexer.HASVALUE: return new Function_HASVALUE(funcs);
				case mathLexer.INTERCEPT: return new Function_INTERCEPT(funcs);
				case mathLexer.SLOPE: return new Function_SLOPE(funcs);
				case mathLexer.CORREL: return new Function_CORREL(funcs);
				case mathLexer.PEARSON: return new Function_PEARSON(funcs);
				case mathLexer.COUNTIF: return new Function_COUNTIF(funcs);
				case mathLexer.FORECAST: return new Function_FORECAST(funcs);
				case mathLexer.NORMINV: return new Function_NORMINV(funcs);
				case mathLexer.BETADIST: return new Function_BETADIST(funcs);
				case mathLexer.BETAINV: return new Function_BETAINV(funcs);
				case mathLexer.EXPONDIST: return new Function_EXPONDIST(funcs);
				case mathLexer.FDIST: return new Function_FDIST(funcs);
				case mathLexer.FINV: return new Function_FINV(funcs);
				case mathLexer.GAMMAINV: return new Function_GAMMAINV(funcs);
				case mathLexer.LOGINV: return new Function_LOGINV(funcs);
				case mathLexer.XNPV: return new Function_XNPV(funcs);
				case mathLexer.MIRR: return new Function_MIRR(funcs);
				case mathLexer.SLN: return new Function_SLN(funcs);
				case mathLexer.MID: return new Function_MID(funcs);
				case mathLexer.DATEDIF: return new Function_DATEDIF(funcs);
				case mathLexer.REGEXREPLACE: return new Function_REGEXREPLACE(funcs);
				case mathLexer.LOGNORMDIST: return new Function_LOGNORMDIST(funcs);
				case mathLexer.NEGBINOMDIST: return new Function_NEGBINOMDIST(funcs);
				case mathLexer.POISSON: return new Function_POISSON(funcs);
				case mathLexer.TDIST: return new Function_TDIST(funcs);
				case mathLexer.NORMDIST: return new Function_NORMDIST(funcs);
				case mathLexer.BINOMDIST: return new Function_BINOMDIST(funcs);
				case mathLexer.GAMMADIST: return new Function_GAMMADIST(funcs);
				case mathLexer.HYPGEOMDIST: return new Function_HYPGEOMDIST(funcs);
				case mathLexer.WEIBULL: return new Function_WEIBULL(funcs);
				case mathLexer.SYD: return new Function_SYD(funcs);
				case mathLexer.SERIESSUM: return new Function_SERIESSUM(funcs);
				case mathLexer.PMT: return new Function_PMT(funcs);
				case mathLexer.PV: return new Function_PV(funcs);
				case mathLexer.FV: return new Function_FV(funcs);
				case mathLexer.NPER: return new Function_NPER(funcs);
				case mathLexer.RATE: return new Function_RATE(funcs);
				case mathLexer.DATE: return new Function_DATE(funcs);
				case mathLexer.PPMT: return new Function_PPMT(funcs);
				case mathLexer.IPMT: return new Function_IPMT(funcs);
				case mathLexer.INDEXOF: return new Function_INDEXOF(funcs);
				case mathLexer.LASTINDEXOF: return new Function_LASTINDEXOF(funcs);
				case mathLexer.JOIN: return new Function_JOIN(funcs);
				case mathLexer.IFS: return new Function_IFS(funcs);
				case mathLexer.SWITCH: return new Function_SWITCH(funcs);
				case mathLexer.PARAMETER:
					var funName = context.PARAMETER().GetText();
					return new Function_DiyFunction(funName, funcs);
				default: break;
			}

			throw new NotImplementedException();
		}


	}
}