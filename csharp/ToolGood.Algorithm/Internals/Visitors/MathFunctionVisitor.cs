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
				list[i] = exprs[i].Accept2(this);
			}
			return list;
		}
		public FunctionBase VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept2(this);
		}
		public FunctionBase VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			if(type == mathLexer2.OPMUL) {
				return new Function_Mul(funcs);
			} else if(type == mathLexer2.OPDIV) {
				return new Function_Div(funcs);
			}
			return new Function_Mod(funcs);
		}
		public FunctionBase VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			if(type == mathLexer2.OPADD) {
				return new Function_Add(funcs);
			} else if(type == mathLexer2.OPSUB) {
				return new Function_Sub(funcs);
			}
			return new Function_Connect(funcs);
		}
		public FunctionBase VisitJudge_fun(mathParser.Judge_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.op.Type;
			switch(type) {
				case mathLexer2.OPGT: return new Function_GT(funcs);
				case mathLexer2.OPGE: return new Function_GE(funcs);
				case mathLexer2.OPLT: return new Function_LT(funcs);
				case mathLexer2.OPLE: return new Function_LE(funcs);
				case mathLexer2.OPEQ: return new Function_EQ(funcs);
				case mathLexer2.OPNE:
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
			var args1 = context.expr().Accept2(this);
			return new Function_NOT(args1);
		}
		public FunctionBase VisitCONST2_fun(mathParser.CONST2_funContext context)
		{
			var type = context.f.Type;
			switch(type) {
				case mathLexer2.TRUE: return new Function_ValueBoolean(true);
				case mathLexer2.FALSE: return new Function_ValueBoolean(false);
				case mathLexer2.ALGORITHMVERSION: return new Function_Version();
				case mathLexer2.NULL:
				default: return new Function_NULL();
			}
		}

		public FunctionBase VisitCONST_fun(mathParser.CONST_funContext context)
		{
			var type = context.f.Type;
			switch(type) {
				case mathLexer2.E: return new Function_Number_E();
				case mathLexer2.PI: return new Function_Number_PI();
				case mathLexer2.RAND: return new Function_RAND();
				case mathLexer2.GUID: return new Function_GUID();
				case mathLexer2.NOW: return new Function_NOW();
				case mathLexer2.TODAY:
				default: return new Function_TODAY();
			}
		}

		public FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			var args1 = context.expr().Accept2(this);
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
			return context.expr().Accept2(this);
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
				var op = context.parameter2().Accept2(this);
				return new Function_GetJsonValue(funcs[0], op);
			}
			return new Function_GetJsonValue(funcs[0], funcs[1]);
		}
		public FunctionBase VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			var exprs = context.arrayJson();
			FunctionBase[] args = new FunctionBase[exprs.Length];
			for(int i = 0; i < exprs.Length; i++) {
				args[i] = exprs[i].Accept2(this);
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
			var f = context.expr().Accept2(this);
			return new Function_ArrayJsonItem(keyName, f);
		}

		#endregion getValue

		public FunctionBase VisitFunction_fun(mathParser.Function_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			var type = context.f.Type;
			switch(type) {
				case mathLexer2.ABS: return new Function_ABS(funcs);
				case mathLexer2.ACOS: return new Function_ACOS(funcs);
				case mathLexer2.ACOSH: return new Function_ACOSH(funcs);
				case mathLexer2.ACOT: return new Function_ACOT(funcs);
				case mathLexer2.ACOTH: return new Function_ACOTH(funcs);
				case mathLexer2.ADDDAYS: return new Function_ADDDAYS(funcs);
				case mathLexer2.ADDHOURS: return new Function_ADDHOURS(funcs);
				case mathLexer2.ADDMINUTES: return new Function_ADDMINUTES(funcs);
				case mathLexer2.ADDMONTHS: return new Function_ADDMONTHS(funcs);
				case mathLexer2.ADDSECONDS: return new Function_ADDSECONDS(funcs);
				case mathLexer2.ADDYEARS: return new Function_ADDYEARS(funcs);
				case mathLexer2.AND: return new Function_AND_N(funcs);
				case mathLexer2.ARABIC: return new Function_ARABIC(funcs);
				case mathLexer2.ARRAY: return new Function_ARRAY(funcs);
				case mathLexer2.ASC: return new Function_ASC(funcs);
				case mathLexer2.ASIN: return new Function_ASIN(funcs);
				case mathLexer2.ASINH: return new Function_ASINH(funcs);
				case mathLexer2.ATAN: return new Function_ATAN(funcs);
				case mathLexer2.ATAN2: return new Function_ATAN2(funcs);
				case mathLexer2.ATANH: return new Function_ATANH(funcs);
				case mathLexer2.AVEDEV: return new Function_AVEDEV(funcs);
				case mathLexer2.AVERAGE: return new Function_AVERAGE(funcs);
				case mathLexer2.AVERAGEIF: return new Function_AVERAGEIF(funcs);
				case mathLexer2.BASE64TOTEXT: return new Function_BASE64TOTEXT(funcs);
				case mathLexer2.BASE64URLTOTEXT: return new Function_BASE64URLTOTEXT(funcs);
				case mathLexer2.BESSELI: return new Function_BESSELI(funcs);
				case mathLexer2.BESSELJ: return new Function_BESSELJ(funcs);
				case mathLexer2.BESSELK: return new Function_BESSELK(funcs);
				case mathLexer2.BESSELY: return new Function_BESSELY(funcs);
				case mathLexer2.BETADIST: return new Function_BETADIST(funcs);
				case mathLexer2.BETAINV: return new Function_BETAINV(funcs);
				case mathLexer2.BIN2DEC: return new Function_BIN2DEC(funcs);
				case mathLexer2.BIN2HEX: return new Function_BIN2HEX(funcs);
				case mathLexer2.BIN2OCT: return new Function_BIN2OCT(funcs);
				case mathLexer2.BINOMDIST: return new Function_BINOMDIST(funcs);
				case mathLexer2.CEILING: return new Function_CEILING(funcs);
				case mathLexer2.CHAR: return new Function_CHAR(funcs);
				case mathLexer2.CLEAN: return new Function_CLEAN(funcs);
				case mathLexer2.CODE: return new Function_CODE(funcs);
				case mathLexer2.COMBIN: return new Function_COMBIN(funcs);
				case mathLexer2.CONCATENATE: return new Function_CONCATENATE(funcs);
				case mathLexer2.CORREL: return new Function_CORREL(funcs);
				case mathLexer2.COS: return new Function_COS(funcs);
				case mathLexer2.COSH: return new Function_COSH(funcs);
				case mathLexer2.COT: return new Function_COT(funcs);
				case mathLexer2.COTH: return new Function_COTH(funcs);
				case mathLexer2.COUNT: return new Function_COUNT(funcs);
				case mathLexer2.COUNTIF: return new Function_COUNTIF(funcs);
				case mathLexer2.COVAR: return new Function_COVAR(funcs);
				case mathLexer2.COVARIANCES: return new Function_COVARIANCES(funcs);
				case mathLexer2.CSC: return new Function_CSC(funcs);
				case mathLexer2.CSCH: return new Function_CSCH(funcs);
				case mathLexer2.DATE: return new Function_DATE(funcs);
				case mathLexer2.DATEDIF: return new Function_DATEDIF(funcs);
				case mathLexer2.DATEVALUE: return new Function_DATEVALUE(funcs);
				case mathLexer2.DAY: return new Function_DAY(funcs);
				case mathLexer2.DAYS: return new Function_DAYS(funcs);
				case mathLexer2.DAYS360: return new Function_DAYS360(funcs);
				case mathLexer2.DB: return new Function_DB(funcs);
				case mathLexer2.DDB: return new Function_DDB(funcs);
				case mathLexer2.DEC2BIN: return new Function_DEC2BIN(funcs);
				case mathLexer2.DEC2HEX: return new Function_DEC2HEX(funcs);
				case mathLexer2.DEC2OCT: return new Function_DEC2OCT(funcs);
				case mathLexer2.DEGREES: return new Function_DEGREES(funcs);
				case mathLexer2.DELTA: return new Function_DELTA(funcs);
				case mathLexer2.DEVSQ: return new Function_DEVSQ(funcs);
				case mathLexer2.EDATE: return new Function_EDATE(funcs);
				case mathLexer2.ENDSWITH: return new Function_ENDSWITH(funcs);
				case mathLexer2.EOMONTH: return new Function_EOMONTH(funcs);
				case mathLexer2.ERF: return new Function_ERF(funcs);
				case mathLexer2.ERFC: return new Function_ERFC(funcs);
				case mathLexer2.ERROR: return new Function_ERROR(funcs);
				case mathLexer2.EVEN: return new Function_EVEN(funcs);
				case mathLexer2.EXACT: return new Function_EXACT(funcs);
				case mathLexer2.EXP: return new Function_EXP(funcs);
				case mathLexer2.EXPONDIST: return new Function_EXPONDIST(funcs);
				case mathLexer2.FACT: return new Function_FACT(funcs);
				case mathLexer2.FACTDOUBLE: return new Function_FACTDOUBLE(funcs);
				case mathLexer2.FDIST: return new Function_FDIST(funcs);
				case mathLexer2.FIND: return new Function_FIND(funcs);
				case mathLexer2.FINV: return new Function_FINV(funcs);
				case mathLexer2.FISHER: return new Function_FISHER(funcs);
				case mathLexer2.FISHERINV: return new Function_FISHERINV(funcs);
				case mathLexer2.FIXED: return new Function_FIXED(funcs);
				case mathLexer2.FLOOR: return new Function_FLOOR(funcs);
				case mathLexer2.FORECAST: return new Function_FORECAST(funcs);
				case mathLexer2.FV: return new Function_FV(funcs);
				case mathLexer2.GAMMADIST: return new Function_GAMMADIST(funcs);
				case mathLexer2.GAMMAINV: return new Function_GAMMAINV(funcs);
				case mathLexer2.GAMMALN: return new Function_GAMMALN(funcs);
				case mathLexer2.GCD: return new Function_GCD(funcs);
				case mathLexer2.GEOMEAN: return new Function_GEOMEAN(funcs);
				case mathLexer2.GESTEP: return new Function_GESTEP(funcs);
				case mathLexer2.HARMEAN: return new Function_HARMEAN(funcs);
				case mathLexer2.HAS: return new Function_HAS(funcs);
				case mathLexer2.HASVALUE: return new Function_HASVALUE(funcs);
				case mathLexer2.HEX2BIN: return new Function_HEX2BIN(funcs);
				case mathLexer2.HEX2DEC: return new Function_HEX2DEC(funcs);
				case mathLexer2.HEX2OCT: return new Function_HEX2OCT(funcs);
				case mathLexer2.HMACMD5: return new Function_HMACMD5(funcs);
				case mathLexer2.HMACSHA1: return new Function_HMACSHA1(funcs);
				case mathLexer2.HMACSHA256: return new Function_HMACSHA256(funcs);
				case mathLexer2.HMACSHA512: return new Function_HMACSHA512(funcs);
				case mathLexer2.HOUR: return new Function_HOUR(funcs);
				case mathLexer2.HTMLDECODE: return new Function_HTMLDECODE(funcs);
				case mathLexer2.HTMLENCODE: return new Function_HTMLENCODE(funcs);
				case mathLexer2.HYPGEOMDIST: return new Function_HYPGEOMDIST(funcs);
				case mathLexer2.IF: return new Function_IF(funcs);
				case mathLexer2.IFERROR: return new Function_IFERROR(funcs);
				case mathLexer2.IFS: return new Function_IFS(funcs);
				case mathLexer2.INDEXOF: return new Function_INDEXOF(funcs);
				case mathLexer2.INT: return new Function_INT(funcs);
				case mathLexer2.INTERCEPT: return new Function_INTERCEPT(funcs);
				case mathLexer2.IPMT: return new Function_IPMT(funcs);
				case mathLexer2.IRR: return new Function_IRR(funcs);
				case mathLexer2.ISERROR: return new Function_ISERROR(funcs);
				case mathLexer2.ISEVEN: return new Function_ISEVEN(funcs);
				case mathLexer2.ISLOGICAL: return new Function_ISLOGICAL(funcs);
				case mathLexer2.ISNONTEXT: return new Function_ISNONTEXT(funcs);
				case mathLexer2.ISNULL: return new Function_ISNULL(funcs);
				case mathLexer2.ISNULLOREMPTY: return new Function_ISNULLOREMPTY(funcs);
				case mathLexer2.ISNULLORERROR: return new Function_ISNULLORERROR(funcs);
				case mathLexer2.ISNULLORWHITESPACE: return new Function_ISNULLORWHITESPACE(funcs);
				case mathLexer2.ISNUMBER: return new Function_ISNUMBER(funcs);
				case mathLexer2.ISODD: return new Function_ISODD(funcs);
				case mathLexer2.ISREGEX: return new Function_ISREGEX(funcs);
				case mathLexer2.ISTEXT: return new Function_ISTEXT(funcs);
				case mathLexer2.JIS: return new Function_JIS(funcs);
				case mathLexer2.JOIN: return new Function_JOIN(funcs);
				case mathLexer2.JSON: return new Function_JSON(funcs);
				case mathLexer2.LARGE: return new Function_LARGE(funcs);
				case mathLexer2.LASTINDEXOF: return new Function_LASTINDEXOF(funcs);
				case mathLexer2.LCM: return new Function_LCM(funcs);
				case mathLexer2.LEFT: return new Function_LEFT(funcs);
				case mathLexer2.LEN: return new Function_LEN(funcs);
				case mathLexer2.LN: return new Function_LN(funcs);
				case mathLexer2.LOG: return new Function_LOG(funcs);
				case mathLexer2.LOG10: return new Function_LOG10(funcs);
				case mathLexer2.LOGINV: return new Function_LOGINV(funcs);
				case mathLexer2.LOGNORMDIST: return new Function_LOGNORMDIST(funcs);
				case mathLexer2.LOOKCEILING: return new Function_LOOKCEILING(funcs);
				case mathLexer2.LOOKFLOOR: return new Function_LOOKFLOOR(funcs);
				case mathLexer2.LOWER: return new Function_LOWER(funcs);
				case mathLexer2.MAX: return new Function_MAX(funcs);
				case mathLexer2.MD5: return new Function_MD5(funcs);
				case mathLexer2.MEDIAN: return new Function_MEDIAN(funcs);
				case mathLexer2.MID: return new Function_MID(funcs);
				case mathLexer2.MIN: return new Function_MIN(funcs);
				case mathLexer2.MINUTE: return new Function_MINUTE(funcs);
				case mathLexer2.MIRR: return new Function_MIRR(funcs);
				case mathLexer2.MOD: return new Function_Mod(funcs);
				case mathLexer2.MODE: return new Function_MODE(funcs);
				case mathLexer2.MONTH: return new Function_MONTH(funcs);
				case mathLexer2.MROUND: return new Function_MROUND(funcs);
				case mathLexer2.MULTINOMIAL: return new Function_MULTINOMIAL(funcs);
				case mathLexer2.NEGBINOMDIST: return new Function_NEGBINOMDIST(funcs);
				case mathLexer2.NETWORKDAYS: return new Function_NETWORKDAYS(funcs);
				case mathLexer2.NORMDIST: return new Function_NORMDIST(funcs);
				case mathLexer2.NORMINV: return new Function_NORMINV(funcs);
				case mathLexer2.NORMSDIST: return new Function_NORMSDIST(funcs);
				case mathLexer2.NORMSINV: return new Function_NORMSINV(funcs);
				case mathLexer2.NOT: return new Function_NOT(funcs);
				case mathLexer2.NPER: return new Function_NPER(funcs);
				case mathLexer2.NPV: return new Function_NPV(funcs);
				case mathLexer2.OCT2BIN: return new Function_OCT2BIN(funcs);
				case mathLexer2.OCT2DEC: return new Function_OCT2DEC(funcs);
				case mathLexer2.OCT2HEX: return new Function_OCT2HEX(funcs);
				case mathLexer2.ODD: return new Function_ODD(funcs);
				case mathLexer2.OR: return new Function_OR_N(funcs);
				case mathLexer2.PARAM: return new Function_PARAM(funcs);
				case mathLexer2.PEARSON: return new Function_PEARSON(funcs);
				case mathLexer2.PERCENTILE: return new Function_PERCENTILE(funcs);
				case mathLexer2.PERCENTRANK: return new Function_PERCENTRANK(funcs);
				case mathLexer2.PERMUT: return new Function_PERMUT(funcs);
				case mathLexer2.PMT: return new Function_PMT(funcs);
				case mathLexer2.POISSON: return new Function_POISSON(funcs);
				case mathLexer2.POWER: return new Function_POWER(funcs);
				case mathLexer2.PPMT: return new Function_PPMT(funcs);
				case mathLexer2.PRODUCT: return new Function_PRODUCT(funcs);
				case mathLexer2.PROPER: return new Function_PROPER(funcs);
				case mathLexer2.PV: return new Function_PV(funcs);
				case mathLexer2.QUARTILE: return new Function_QUARTILE(funcs);
				case mathLexer2.QUOTIENT: return new Function_QUOTIENT(funcs);
				case mathLexer2.RADIANS: return new Function_RADIANS(funcs);
				case mathLexer2.RANDBETWEEN: return new Function_RANDBETWEEN(funcs);
				case mathLexer2.RANK: return new Function_RANK(funcs);
				case mathLexer2.RATE: return new Function_RATE(funcs);
				case mathLexer2.REGEX: return new Function_REGEX(funcs);
				case mathLexer2.REGEXREPLACE: return new Function_REGEXREPLACE(funcs);
				case mathLexer2.REMOVEEND: return new Function_REMOVEEND(funcs);
				case mathLexer2.REMOVESTART: return new Function_REMOVESTART(funcs);
				case mathLexer2.REPLACE: return new Function_REPLACE(funcs);
				case mathLexer2.REPT: return new Function_REPT(funcs);
				case mathLexer2.RIGHT: return new Function_RIGHT(funcs);
				case mathLexer2.RMB: return new Function_RMB(funcs);
				case mathLexer2.ROMAN: return new Function_ROMAN(funcs);
				case mathLexer2.ROUND: return new Function_ROUND(funcs);
				case mathLexer2.ROUNDDOWN: return new Function_ROUNDDOWN(funcs);
				case mathLexer2.ROUNDUP: return new Function_ROUNDUP(funcs);
				case mathLexer2.SEARCH: return new Function_SEARCH(funcs);
				case mathLexer2.SEC: return new Function_SEC(funcs);
				case mathLexer2.SECH: return new Function_SECH(funcs);
				case mathLexer2.SECOND: return new Function_SECOND(funcs);
				case mathLexer2.SERIESSUM: return new Function_SERIESSUM(funcs);
				case mathLexer2.SHA1: return new Function_SHA1(funcs);
				case mathLexer2.SHA256: return new Function_SHA256(funcs);
				case mathLexer2.SHA512: return new Function_SHA512(funcs);
				case mathLexer2.SIGN: return new Function_SIGN(funcs);
				case mathLexer2.SIN: return new Function_SIN(funcs);
				case mathLexer2.SINH: return new Function_SINH(funcs);
				case mathLexer2.SLN: return new Function_SLN(funcs);
				case mathLexer2.SLOPE: return new Function_SLOPE(funcs);
				case mathLexer2.SMALL: return new Function_SMALL(funcs);
				case mathLexer2.SPLIT: return new Function_SPLIT(funcs);
				case mathLexer2.SQRT: return new Function_SQRT(funcs);
				case mathLexer2.SQRTPI: return new Function_SQRTPI(funcs);
				case mathLexer2.STARTSWITH: return new Function_STARTSWITH(funcs);
				case mathLexer2.STDEV: return new Function_STDEV(funcs);
				case mathLexer2.STDEVP: return new Function_STDEVP(funcs);
				case mathLexer2.SUBSTITUTE: return new Function_SUBSTITUTE(funcs);
				case mathLexer2.SUBSTRING: return new Function_SUBSTRING(funcs);
				case mathLexer2.SUM: return new Function_SUM(funcs);
				case mathLexer2.SUMIF: return new Function_SUMIF(funcs);
				case mathLexer2.SUMPRODUCT: return new Function_SUMPRODUCT(funcs);
				case mathLexer2.SUMSQ: return new Function_SUMSQ(funcs);
				case mathLexer2.SUMX2MY2: return new Function_SUMX2MY2(funcs);
				case mathLexer2.SUMX2PY2: return new Function_SUMX2PY2(funcs);
				case mathLexer2.SUMXMY2: return new Function_SUMXMY2(funcs);
				case mathLexer2.SWITCH: return new Function_SWITCH(funcs);
				case mathLexer2.SYD: return new Function_SYD(funcs);
				case mathLexer2.T: return new Function_T(funcs);
				case mathLexer2.TAN: return new Function_TAN(funcs);
				case mathLexer2.TANH: return new Function_TANH(funcs);
				case mathLexer2.TDIST: return new Function_TDIST(funcs);
				case mathLexer2.TEXT: return new Function_TEXT(funcs);
				case mathLexer2.TEXTTOBASE64: return new Function_TEXTTOBASE64(funcs);
				case mathLexer2.TEXTTOBASE64URL: return new Function_TEXTTOBASE64URL(funcs);
				case mathLexer2.TIME: return new Function_TIME(funcs);
				case mathLexer2.TIMESTAMP: return new Function_TIMESTAMP(funcs);
				case mathLexer2.TIMEVALUE: return new Function_TIMEVALUE(funcs);
				case mathLexer2.TINV: return new Function_TINV(funcs);
				case mathLexer2.TRIM: return new Function_TRIM(funcs);
				case mathLexer2.TRIMEND: return new Function_TRIMEND(funcs);
				case mathLexer2.TRIMSTART: return new Function_TRIMSTART(funcs);
				case mathLexer2.TRUNC: return new Function_TRUNC(funcs);
				case mathLexer2.UNICHAR: return new Function_UNICHAR(funcs);
				case mathLexer2.UNICODE: return new Function_UNICODE(funcs);
				case mathLexer2.UPPER: return new Function_UPPER(funcs);
				case mathLexer2.URLDECODE: return new Function_URLDECODE(funcs);
				case mathLexer2.URLENCODE: return new Function_URLENCODE(funcs);
				case mathLexer2.VALUE: return new Function_VALUE(funcs);
				case mathLexer2.VAR: return new Function_VAR(funcs);
				case mathLexer2.VARP: return new Function_VARP(funcs);
				case mathLexer2.WEEKDAY: return new Function_WEEKDAY(funcs);
				case mathLexer2.WEEKNUM: return new Function_WEEKNUM(funcs);
				case mathLexer2.WEIBULL: return new Function_WEIBULL(funcs);
				case mathLexer2.WORKDAY: return new Function_WORKDAY(funcs);
				case mathLexer2.XIRR: return new Function_XIRR(funcs);
				case mathLexer2.XNPV: return new Function_XNPV(funcs);
				case mathLexer2.XOR: return new Function_XOR(funcs);
				case mathLexer2.YEAR: return new Function_YEAR(funcs);
				case mathLexer2.YEARFRAC: return new Function_YEARFRAC(funcs);
				case mathLexer2.PARAMETER:
				default:
					var funName = context.PARAMETER().GetText();
					return new Function_DiyFunction(funName, funcs);
			}
		}


	}
}