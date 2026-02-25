using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Functions.Compare;
using ToolGood.Algorithm.Internals.Functions.Csharp;
using ToolGood.Algorithm.Internals.Functions.CsharpSecurity;
using ToolGood.Algorithm.Internals.Functions.CsharpWeb;
using ToolGood.Algorithm.Internals.Functions.DateTimes;
using ToolGood.Algorithm.Internals.Functions.Flow;
using ToolGood.Algorithm.Internals.Functions.MathBase;
using ToolGood.Algorithm.Internals.Functions.MathSum;
using ToolGood.Algorithm.Internals.Functions.MathTransformation;
using ToolGood.Algorithm.Internals.Functions.MathTrigonometric;
using ToolGood.Algorithm.Internals.Functions.Operator;
using ToolGood.Algorithm.Internals.Functions.String;
using ToolGood.Algorithm.Internals.Functions.Value;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal class MathFunctionVisitor : AbstractParseTreeVisitor<FunctionBase>, ImathVisitor<FunctionBase>
	{
		private FunctionBase[] VisitExprs(mathParser.ExprContext[] exprs)
		{
			FunctionBase[] list = new FunctionBase[exprs.Length];
			for(int i = 0; i < exprs.Length; i++) {
				list[i] = exprs[i].Accept(this);
			}
			return list;
		}
		internal static Dictionary<string, Func<FunctionBase[], FunctionBase>> funcDict = new Dictionary<string, Func<FunctionBase[], FunctionBase>>(StringComparer.OrdinalIgnoreCase) {
{ "IF", (args) => new Function_IF(args) },
{ "IFERROR", (args) => new Function_IFERROR(args) },
{ "ISNUMBER", (args) => new Function_ISNUMBER(args) },
{ "ISTEXT", (args) => new Function_ISTEXT(args) },
{ "ISERROR", (args) => new Function_ISERROR(args) },
{ "ISNONTEXT", (args) => new Function_ISNONTEXT(args) },
{ "ISLOGICAL", (args) => new Function_ISLOGICAL(args) },
{ "ISEVEN", (args) => new Function_ISEVEN(args) },
{ "ISODD", (args) => new Function_ISODD(args) },
{ "ISNULL", (args) => new Function_ISNULL(args) },
{ "ISNULLORERROR", (args) => new Function_ISNULLORERROR(args) },
{ "AND", (args) => new Function_AND(args) },
{ "OR", (args) => new Function_OR(args) },
{ "NOT", (args) => new Function_NOT(args) },
{ "DEC2BIN", (args) => new Function_DEC2BIN(args) },
{ "DEC2HEX", (args) => new Function_DEC2HEX(args) },
{ "DEC2OCT", (args) => new Function_DEC2OCT(args) },
{ "HEX2BIN", (args) => new Function_HEX2BIN(args) },
{ "HEX2DEC", (args) => new Function_HEX2DEC(args) },
{ "HEX2OCT", (args) => new Function_HEX2OCT(args) },
{ "OCT2BIN", (args) => new Function_OCT2BIN(args) },
{ "OCT2DEC", (args) => new Function_OCT2DEC(args) },
{ "OCT2HEX", (args) => new Function_OCT2HEX(args) },
{ "BIN2OCT", (args) => new Function_BIN2OCT(args) },
{ "BIN2DEC", (args) => new Function_BIN2DEC(args) },
{ "BIN2HEX", (args) => new Function_BIN2HEX(args) },
{ "ABS", (args) => new Function_ABS(args) },
{ "QUOTIENT", (args) => new Function_QUOTIENT(args) },
{ "MOD", (args) => new Function_Mod(args) },
{ "SIGN", (args) => new Function_SIGN(args) },
{ "SQRT", (args) => new Function_SQRT(args) },
{ "TRUNC", (args) => new Function_TRUNC(args) },
{ "INT", (args) => new Function_TRUNC(args) },
{ "GCD", (args) => new Function_GCD(args) },
{ "LCM", (args) => new Function_LCM(args) },
{ "COMBIN", (args) => new Function_COMBIN(args) },
{ "PERMUT", (args) => new Function_PERMUT(args) },
{ "DEGREES", (args) => new Function_DEGREES(args) },
{ "RADIANS", (args) => new Function_RADIANS(args) },
{ "COS", (args) => new Function_COS(args) },
{ "COSH", (args) => new Function_COSH(args) },
{ "SIN", (args) => new Function_SIN(args) },
{ "SINH", (args) => new Function_SINH(args) },
{ "TAN", (args) => new Function_TAN(args) },
{ "TANH", (args) => new Function_TANH(args) },
{ "ACOS", (args) => new Function_ACOS(args) },
{ "ACOSH", (args) => new Function_ACOSH(args) },
{ "ASIN", (args) => new Function_ASIN(args) },
{ "ASINH", (args) => new Function_ASINH(args) },
{ "ATAN", (args) => new Function_ATAN(args) },
{ "ATANH", (args) => new Function_ATANH(args) },
{ "ATAN2", (args) => new Function_ATAN2(args) },
{ "ROUND", (args) => new Function_ROUND(args) },
{ "ROUNDDOWN", (args) => new Function_ROUNDDOWN(args) },
{ "ROUNDUP", (args) => new Function_ROUNDUP(args) },
{ "CEILING", (args) => new Function_CEILING(args) },
{ "FLOOR", (args) => new Function_FLOOR(args) },
{ "EVEN", (args) => new Function_EVEN(args) },
{ "ODD", (args) => new Function_ODD(args) },
{ "MROUND", (args) => new Function_MROUND(args) },
{ "RANDBETWEEN", (args) => new Function_RANDBETWEEN(args) },
{ "FACT", (args) => new Function_FACT(args) },
{ "FACTDOUBLE", (args) => new Function_FACTDOUBLE(args) },
{ "POWER", (args) => new Function_POWER(args) },
{ "EXP", (args) => new Function_EXP(args) },
{ "LN", (args) => new Function_LN(args) },
{ "LOG", (args) => new Function_LOG(args) },
{ "LOG10", (args) => new Function_LOG10(args) },
{ "MULTINOMIAL", (args) => new Function_MULTINOMIAL(args) },
{ "PRODUCT", (args) => new Function_PRODUCT(args) },
{ "SQRTPI", (args) => new Function_SQRTPI(args) },
{ "SUMSQ", (args) => new Function_SUMSQ(args) },
{ "ASC", (args) => new Function_ASC(args) },
{ "JIS", (args) => new Function_JIS(args) },
{ "WIDECHAR", (args) => new Function_JIS(args) },
{ "CHAR", (args) => new Function_CHAR(args) },
{ "CLEAN", (args) => new Function_CLEAN(args) },
{ "CODE", (args) => new Function_CODE(args) },
{ "CONCATENATE", (args) => new Function_CONCATENATE(args) },
{ "EXACT", (args) => new Function_EXACT(args) },
{ "FIND", (args) => new Function_FIND(args) },
{ "FIXED", (args) => new Function_FIXED(args) },
{ "LEFT", (args) => new Function_LEFT(args) },
{ "LEN", (args) => new Function_LEN(args) },
{ "LOWER", (args) => new Function_LOWER(args) },
{ "TOLOWER", (args) => new Function_LOWER(args) },
{ "MID", (args) => new Function_MID(args) },
{ "PROPER", (args) => new Function_PROPER(args) },
{ "REPLACE", (args) => new Function_REPLACE(args) },
{ "REPT", (args) => new Function_REPT(args) },
{ "RIGHT", (args) => new Function_RIGHT(args) },
{ "RMB", (args) => new Function_RMB(args) },
{ "SEARCH", (args) => new Function_SEARCH(args) },
{ "SUBSTITUTE", (args) => new Function_SUBSTITUTE(args) },
{ "T", (args) => new Function_T(args) },
{ "TEXT", (args) => new Function_TEXT(args) },
{ "TRIM", (args) => new Function_TRIM(args) },
{ "UPPER", (args) => new Function_UPPER(args) },
{ "TOUPPER", (args) => new Function_UPPER(args) },
{ "VALUE", (args) => new Function_VALUE(args) },
{ "DATEVALUE", (args) => new Function_DATEVALUE(args) },
{ "TIMEVALUE", (args) => new Function_TIMEVALUE(args) },
{ "DATE", (args) => new Function_DATE(args) },
{ "TIME", (args) => new Function_TIME(args) },
{ "YEAR", (args) => new Function_YEAR(args) },
{ "MONTH", (args) => new Function_MONTH(args) },
{ "DAY", (args) => new Function_DAY(args) },
{ "HOUR", (args) => new Function_HOUR(args) },
{ "MINUTE", (args) => new Function_MINUTE(args) },
{ "SECOND", (args) => new Function_SECOND(args) },
{ "WEEKDAY", (args) => new Function_WEEKDAY(args) },
{ "DATEDIF", (args) => new Function_DATEDIF(args) },
{ "DAYS360", (args) => new Function_DAYS360(args) },
{ "EDATE", (args) => new Function_EDATE(args) },
{ "EOMONTH", (args) => new Function_EOMONTH(args) },
{ "NETWORKDAYS", (args) => new Function_NETWORKDAYS(args) },
{ "WORKDAY", (args) => new Function_WORKDAY(args) },
{ "WEEKNUM", (args) => new Function_WEEKNUM(args) },
{ "MAX", (args) => new Function_MAX(args) },
{ "MEDIAN", (args) => new Function_MEDIAN(args) },
{ "MIN", (args) => new Function_MIN(args) },
{ "QUARTILE", (args) => new Function_QUARTILE(args) },
{ "MODE", (args) => new Function_MODE(args) },
{ "LARGE", (args) => new Function_LARGE(args) },
{ "SMALL", (args) => new Function_SMALL(args) },
{ "PERCENTILE", (args) => new Function_PERCENTILE(args) },
{ "PERCENTILE.INC", (args) => new Function_PERCENTILE(args) },
{ "PERCENTRANK", (args) => new Function_PERCENTRANK(args) },
{ "PERCENTRANK.INC", (args) => new Function_PERCENTRANK(args) },
{ "AVERAGE", (args) => new Function_AVERAGE(args) },
{ "AVERAGEIF", (args) => new Function_AVERAGEIF(args) },
{ "GEOMEAN", (args) => new Function_GEOMEAN(args) },
{ "HARMEAN", (args) => new Function_HARMEAN(args) },
{ "COUNT", (args) => new Function_COUNT(args) },
{ "COUNTIF", (args) => new Function_COUNTIF(args) },
{ "SUM", (args) => new Function_SUM(args) },
{ "SUMIF", (args) => new Function_SUMIF(args) },
{ "AVEDEV", (args) => new Function_AVEDEV(args) },
{ "STDEV", (args) => new Function_STDEV(args) },
{ "STDEV.S", (args) => new Function_STDEV(args) },
{ "STDEVP", (args) => new Function_STDEVP(args) },
{ "STDEV.P", (args) => new Function_STDEVP(args) },
{ "COVAR", (args) => new Function_COVAR(args) },
{ "COVARIANCE.P", (args) => new Function_COVAR(args) },
{ "COVARIANCE.S", (args) => new Function_COVARIANCES(args) },
{ "DEVSQ", (args) => new Function_DEVSQ(args) },
{ "VAR", (args) => new Function_VAR(args) },
{ "VAR.S", (args) => new Function_VAR(args) },
{ "VARP", (args) => new Function_VARP(args) },
{ "VAR.P", (args) => new Function_VARP(args) },
{ "NORMDIST", (args) => new Function_NORMDIST(args) },
{ "NORM.DIST", (args) => new Function_NORMDIST(args) },
{ "NORMINV", (args) => new Function_NORMINV(args) },
{ "NORM.INV", (args) => new Function_NORMINV(args) },
{ "NORMSDIST", (args) => new Function_NORMSDIST(args) },
{ "NORM.S.DIST", (args) => new Function_NORMSDIST(args) },
{ "NORMSINV", (args) => new Function_NORMSINV(args) },
{ "NORM.S.INV", (args) => new Function_NORMSINV(args) },
{ "BETADIST", (args) => new Function_BETADIST(args) },
{ "BETA.DIST", (args) => new Function_BETADIST(args) },
{ "BETAINV", (args) => new Function_BETAINV(args) },
{ "BETA.INV", (args) => new Function_BETAINV(args) },
{ "BINOMDIST", (args) => new Function_BINOMDIST(args) },
{ "BINOM.DIST", (args) => new Function_BINOMDIST(args) },
{ "EXPONDIST", (args) => new Function_EXPONDIST(args) },
{ "EXPON.DIST", (args) => new Function_EXPONDIST(args) },
{ "FDIST", (args) => new Function_FDIST(args) },
{ "F.DIST", (args) => new Function_FDIST(args) },
{ "FINV", (args) => new Function_FINV(args) },
{ "F.INV", (args) => new Function_FINV(args) },
{ "FISHER", (args) => new Function_FISHER(args) },
{ "FISHERINV", (args) => new Function_FISHERINV(args) },
{ "GAMMADIST", (args) => new Function_GAMMADIST(args) },
{ "GAMMA.DIST", (args) => new Function_GAMMADIST(args) },
{ "GAMMAINV", (args) => new Function_GAMMAINV(args) },
{ "GAMMA.INV", (args) => new Function_GAMMAINV(args) },
{ "GAMMALN", (args) => new Function_GAMMALN(args) },
{ "GAMMALN.PRECISE", (args) => new Function_GAMMALN(args) },
{ "HYPGEOMDIST", (args) => new Function_HYPGEOMDIST(args) },
{ "HYPGEOM.DIST", (args) => new Function_HYPGEOMDIST(args) },
{ "LOGINV", (args) => new Function_LOGINV(args) },
{ "LOGNORM.INV", (args) => new Function_LOGINV(args) },
{ "LOGNORMDIST", (args) => new Function_LOGNORMDIST(args) },
{ "LOGNORM.DIST", (args) => new Function_LOGNORMDIST(args) },
{ "NEGBINOMDIST", (args) => new Function_NEGBINOMDIST(args) },
{ "NEGBINOM.DIST", (args) => new Function_NEGBINOMDIST(args) },
{ "POISSON", (args) => new Function_POISSON(args) },
{ "POISSON.DIST", (args) => new Function_POISSON(args) },
{ "TDIST", (args) => new Function_TDIST(args) },
{ "T.DIST", (args) => new Function_TDIST(args) },
{ "TINV", (args) => new Function_TINV(args) },
{ "T.INV", (args) => new Function_TINV(args) },
{ "WEIBULL", (args) => new Function_WEIBULL(args) },
{ "URLENCODE", (args) => new Function_URLENCODE(args) },
{ "URLDECODE", (args) => new Function_URLDECODE(args) },
{ "HTMLENCODE", (args) => new Function_HTMLENCODE(args) },
{ "HTMLDECODE", (args) => new Function_HTMLDECODE(args) },
{ "BASE64TOTEXT", (args) => new Function_BASE64TOTEXT(args) },
{ "BASE64URLTOTEXT", (args) => new Function_BASE64URLTOTEXT(args) },
{ "TEXTTOBASE64", (args) => new Function_TEXTTOBASE64(args) },
{ "TEXTTOBASE64URL", (args) => new Function_TEXTTOBASE64URL(args) },
{ "REGEX", (args) => new Function_REGEX(args) },
{ "REGEXREPALCE", (args) => new Function_REGEXREPALCE(args) },
{ "ISREGEX", (args) => new Function_ISREGEX(args) },
{ "ISMATCH", (args) => new Function_ISREGEX(args) },
{ "MD5", (args) => new Function_MD5(args) },
{ "SHA1", (args) => new Function_SHA1(args) },
{ "SHA256", (args) => new Function_SHA256(args) },
{ "SHA512", (args) => new Function_SHA512(args) },
{ "HMACMD5", (args) => new Function_HMACMD5(args) },
{ "HMACSHA1", (args) => new Function_HMACSHA1(args) },
{ "HMACSHA256", (args) => new Function_HMACSHA256(args) },
{ "HMACSHA512", (args) => new Function_HMACSHA512(args) },
{ "TRIMSTART", (args) => new Function_TRIMSTART(args) },
{ "LTRIM", (args) => new Function_TRIMSTART(args) },
{ "TRIMEND", (args) => new Function_TRIMEND(args) },
{ "RTRIM", (args) => new Function_TRIMEND(args) },
{ "INDEXOF", (args) => new Function_INDEXOF(args) },
{ "LASTINDEXOF", (args) => new Function_LASTINDEXOF(args) },
{ "SPLIT", (args) => new Function_SPLIT(args) },
{ "JOIN", (args) => new Function_JOIN(args) },
{ "SUBSTRING", (args) => new Function_SUBSTRING(args) },
{ "STARTSWITH", (args) => new Function_STARTSWITH(args) },
{ "ENDSWITH", (args) => new Function_ENDSWITH(args) },
{ "ISNULLOREMPTY", (args) => new Function_ISNULLOREMPTY(args) },
{ "ISNULLORWHITESPACE", (args) => new Function_ISNULLORWHITESPACE(args) },
{ "REMOVESTART", (args) => new Function_REMOVESTART(args) },
{ "REMOVEEND", (args) => new Function_REMOVEEND(args) },
{ "JSON", (args) => new Function_JSON(args) },
{ "LOOKCEILING", (args) => new Function_LOOKCEILING(args) },
{ "LOOKFLOOR", (args) => new Function_LOOKFLOOR(args) },
{ "ARRAY", (args) => new Function_Array(args) },
{ "ADDYEARS", (args) => new Function_ADDYEARS(args) },
{ "ADDMONTHS", (args) => new Function_ADDMONTHS(args) },
{ "ADDDAYS", (args) => new Function_ADDDAYS(args) },
{ "ADDHOURS", (args) => new Function_ADDHOURS(args) },
{ "ADDMINUTES", (args) => new Function_ADDMINUTES(args) },
{ "ADDSECONDS", (args) => new Function_ADDSECONDS(args) },
{ "TIMESTAMP", (args) => new Function_TIMESTAMP(args) },
{ "HAS", (args) => new Function_HAS(args) },
{ "HASKEY", (args) => new Function_HAS(args) },
{ "CONTAINS", (args) => new Function_HAS(args) },
{ "CONTAINSKEY", (args) => new Function_HAS(args) },
{ "HASVALUE", (args) => new Function_HASVALUE(args) },
{ "CONTAINSVALUE", (args) => new Function_HASVALUE(args) },
{ "PARAM", (args) => new Function_PARAM(args) },
{ "PARAMETER", (args) => new Function_PARAM(args) },
{ "GETPARAMETER", (args) => new Function_PARAM(args) },
{ "ERROR", (args) => new Function_ERROR(args) },
		};


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
		public FunctionBase VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			var t = CharUtil.StandardString(context.f.Text);
			if(t == "E") {
				return new Function_Value(Operand.Create(Math.E), "E");
			} else if(t == "PI") {
				return new Function_Value(Operand.Create(Math.PI), "PI");
			} else if(t == "TRUE" || t == "YES" || t == "是" || t == "有") {
				return new Function_Value(Operand.True, t);
			} else if(t == "FALSE" || t == "NO" || t == "不是" || t == "否" || t == "没有" || t == "无") {
				return new Function_Value(Operand.False, t);
			} else if(t == "ALGORITHMVERSION" || t == "ENGINEVERSION") {
				return new Function_Value(Operand.Version, t);

			} else if(t == "RAND") {
				return new Function_RAND();
			} else if(t == "GUID") {
				return new Function_GUID();
			} else if(t == "NOW") {
				return new Function_NOW();
			} else if(t == "TODAY") {
				return new Function_TODAY();
			}


			var funcs = VisitExprs(context.expr());
			if(funcDict.TryGetValue(t, out Func<FunctionBase[], FunctionBase> func)) {
				return func(funcs);
			}
			return new Function_DiyFunction(t, funcs);
		}
		public FunctionBase VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			var t = CharUtil.StandardString(node.GetText());
			if(t == "E") {
				return new Function_Value(Operand.Create(Math.E), "E");
			} else if(t == "PI") {
				return new Function_Value(Operand.Create(Math.PI), "PI");
			} else if(t == "TRUE" || t == "YES" || t == "是" || t == "有") {
				return new Function_Value(Operand.True, t);
			} else if(t == "FALSE" || t == "NO" || t == "不是" || t == "否" || t == "没有" || t == "无") {
				return new Function_Value(Operand.False, t);
			} else if(t == "ALGORITHMVERSION" || t == "ENGINEVERSION") {
				return new Function_Value(Operand.Version, t);
			}
			return new Function_PARAMETER(node.GetText());
		}


		public FunctionBase VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_Percentage(args1);
		}

		public FunctionBase VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			var opd = context.GetText();
			var sb = new StringBuilder();
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
					else sb.Append(opd[index++]);
				} else {
					sb.Append(c);
				}
			}
			return new Function_Value(Operand.Create(sb.ToString()));
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

		public FunctionBase VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			if(context.p != null) {
				string keyName = context.p.Text.Trim(new char[] { '"', '\'', ' ', '\t', '\r', '\n', '\f' });
				var op = new Function_Value(Operand.Create(keyName), keyName);
				return new Function_GetJsonValue(funcs[0], op);
			}
			if(context.PARAMETER() != null) {
				var op = new Function_PARAMETER(context.PARAMETER().GetText());
				return new Function_GetJsonValue(funcs[0], op);
			}
			return new Function_GetJsonValue(funcs[0], funcs[1]);
		}

		public FunctionBase VisitArray_fun(mathParser.Array_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_Array(funcs);
		}

		public FunctionBase VisitNOT_fun(mathParser.NOT_funContext context)
		{
			var args1 = context.expr().Accept(this);
			return new Function_NOT(args1);
		}

		public FunctionBase VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}

		public FunctionBase VisitNUM_fun(mathParser.NUM_funContext context)
		{
			var d = decimal.Parse(context.num().GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
			if(context.unit == null) { return new Function_Value(Operand.Create(d), context.num().GetText()); }
			var unit = context.unit.Text;
			return new Function_NUM(d, unit);
		}

		public FunctionBase VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return new Function_Value(Operand.CreateNull(), "NULL");
		}

		public FunctionBase VisitNum(mathParser.NumContext context)
		{
			var d = decimal.Parse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
			return new Function_Value(Operand.Create(d), context.GetText());
		}


		public FunctionBase VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			string keyName = context.key.Text.Trim(new char[] { '"', '\'', ' ', '\t', '\r', '\n', '\f' });
			var f = context.expr().Accept(this);
			return new Function_ArrayJsonItem(keyName, f);
		}

		public FunctionBase VisitIF_fun(mathParser.IF_funContext context)
		{
			var funcs = VisitExprs(context.expr());
			return new Function_IF(funcs);
		}
	}
}
