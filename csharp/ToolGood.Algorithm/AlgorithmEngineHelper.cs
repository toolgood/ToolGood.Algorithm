using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Functions.Operator;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.UnitConversion;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 算法引擎助手
	/// </summary>
	public static class AlgorithmEngineHelper
	{
		private static readonly HashSet<string> _lexerSet = new HashSet<string>(StringComparer.Ordinal) {
			"NULL", "IF", "IFERROR", "ISNUMBER", "ISTEXT", "ISERROR", "ISNONTEXT", "ISLOGICAL",
			"ISEVEN", "ISODD", "ISNULL", "ISNULLORERROR", "AND", "OR", "NOT", "TRUE", "FALSE",
			"E", "PI", "DEC2BIN", "DEC2HEX", "DEC2OCT", "HEX2BIN", "HEX2DEC", "HEX2OCT",
			"OCT2BIN", "OCT2DEC", "OCT2HEX", "BIN2OCT", "BIN2DEC", "BIN2HEX",
			"ABS", "QUOTIENT", "MOD", "SIGN", "SQRT", "TRUNC", "INT", "GCD", "LCM",
			"COMBIN", "PERMUT", "DEGREES", "RADIANS", "COS", "COSH", "SIN", "SINH",
			"TAN", "TANH", "ACOS", "ACOSH", "ASIN", "ASINH", "ATAN", "ATANH", "ATAN2",
			"ROUND", "ROUNDDOWN", "ROUNDUP", "CEILING", "FLOOR", "EVEN", "ODD", "MROUND",
			"RAND", "RANDBETWEEN", "FACT", "FACTDOUBLE", "POWER", "EXP", "LN", "LOG", "LOG10",
			"MULTINOMIAL", "PRODUCT", "SQRTPI", "SUMSQ", "ASC", "JIS", "WIDECHAR",
			"CHAR", "CLEAN", "CODE", "CONCATENATE", "EXACT", "FIND", "FIXED", "LEFT", "LEN",
			"LOWER", "TOLOWER", "MID", "PROPER", "REPLACE", "REPT", "RIGHT", "RMB",
			"SEARCH", "SUBSTITUTE", "T", "TEXT", "TRIM", "UPPER", "TOUPPER", "VALUE",
			"DATEVALUE", "TIMEVALUE", "DATE", "TIME", "NOW", "TODAY", "YEAR", "MONTH", "DAY",
			"HOUR", "MINUTE", "SECOND", "WEEKDAY", "DATEDIF", "DAYS360", "EDATE", "EOMONTH",
			"NETWORKDAYS", "WORKDAY", "WEEKNUM", "MAX", "MEDIAN", "MIN", "QUARTILE", "MODE",
			"LARGE", "SMALL", "PERCENTILE", "PERCENTILE.INC", "PERCENTRANK", "PERCENTRANK.INC",
			"AVERAGE", "AVERAGEIF", "GEOMEAN", "HARMEAN", "COUNT", "COUNTIF", "SUM", "SUMIF",
			"AVEDEV", "STDEV", "STDEV.S", "STDEVP", "STDEV.P", "COVAR", "COVARIANCE.P",
			"COVARIANCE.S'", "DEVSQ", "VAR", "VAR.S", "VARP", "VAR.P",
			"NORMDIST", "NORM.DIST", "NORMINV", "NORM.INV", "NORMSDIST", "NORM.S.DIST",
			"NORMSINV", "NORM.S.INV", "BETADIST", "BETA.DIST", "BETAINV", "BETA.INV",
			"BINOMDIST", "BINOM.DIST", "EXPONDIST", "EXPON.DIST", "FDIST", "F.DIST",
			"FINV", "F.INV", "FISHER", "FISHERINV", "GAMMADIST", "GAMMA.DIST",
			"GAMMAINV", "GAMMA.INV", "GAMMALN", "GAMMALN.PRECISE", "HYPGEOMDIST", "HYPGEOM.DIST",
			"LOGINV", "LOGNORM.INV", "LOGNORMDIST", "LOGNORM.DIST", "NEGBINOMDIST", "NEGBINOM.DIST",
			"POISSON", "POISSON.DIST", "TDIST", "T.DIST", "TINV", "T.INV", "WEIBULL",
			"URLENCODE", "URLDECODE", "HTMLENCODE", "HTMLDECODE",
			"BASE64TOTEXT", "BASE64URLTOTEXT", "TEXTTOBASE64", "TEXTTOBASE64URL",
			"REGEX", "REGEXREPALCE", "ISREGEX", "ISMATCH", "GUID",
			"MD5", "SHA1", "SHA256", "SHA512", "HMACMD5", "HMACSHA1", "HMACSHA256", "HMACSHA512",
			"TRIMSTART", "LTRIM", "TRIMEND", "RTRIM", "INDEXOF", "LASTINDEXOF",
			"SPLIT", "JOIN", "SUBSTRING", "STARTSWITH", "ENDSWITH",
			"ISNULLOREMPTY", "ISNULLORWHITESPACE", "REMOVESTART", "REMOVEEND",
			"JSON", "ARRAY", "LOOKFLOOR", "LOOKCEILING",
			"ADDYEARS", "ADDMONTHS", "ADDDAYS", "ADDHOURS", "ADDMINUTES", "ADDSECONDS", "TIMESTAMP",
			"HAS", "HASKEY", "CONTAINS", "CONTAINSKEY", "HASVALUE", "CONTAINSVALUE",
			"PARAM", "PARAMETER", "GETPARAMETER", "ERROR"
		};
	
		private static readonly Regex unitRegex = new Regex(@"[\s \(\)（）\[\]<>]", RegexOptions.Compiled);

		/// <summary>
		/// 是否与内置关键字相同
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public static bool IsKeywords(string parameter)
		{
			return _lexerSet.Contains(CharUtil.StandardString(parameter));
		}

		/// <summary>
		/// 获取 DIY 名称
		/// </summary>
		/// <param name="exp"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static DiyNameInfo GetDiyNames(string exp)
		{
			if(string.IsNullOrWhiteSpace(exp)) {
				throw new Exception("Parameter exp invalid !");
			}
			AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
			var stream = new AntlrCharStream(new AntlrInputStream(exp));
			var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

			var context = parser.prog();
			if(antlrErrorTextWriter.IsError) {
				throw new Exception(antlrErrorTextWriter.ErrorMsg);
			}
			var visitor = new DiyNameVisitor();
			visitor.Visit(context);
			return visitor.diy;
		}

		/// <summary>
		/// 单位转换
		/// </summary>
		/// <param name="src"></param>
		/// <param name="oldSrcUnit"></param>
		/// <param name="oldTarUnit"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static decimal UnitConversion(decimal src, string oldSrcUnit, string oldTarUnit, string name = null)
		{
			if(string.IsNullOrWhiteSpace(oldSrcUnit) || string.IsNullOrWhiteSpace(oldTarUnit)) { return src; }
			oldSrcUnit = unitRegex.Replace(oldSrcUnit, "");
			if(oldSrcUnit == oldTarUnit) { return src; }

			if(DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
				var c = new DistanceConverter(oldSrcUnit, oldTarUnit);
				return c.LeftToRight(src);
			}
			if(MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
				var c = new MassConverter(oldSrcUnit, oldTarUnit);
				return c.LeftToRight(src);
			}
			if(AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
				var c = new AreaConverter(oldSrcUnit, oldTarUnit);
				return c.LeftToRight(src);
			}
			if(VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
				var c = new VolumeConverter(oldSrcUnit, oldTarUnit);
				return c.LeftToRight(src);
			}
			if(string.IsNullOrEmpty(name)) {
				throw new Exception($"The input item has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
			}
			throw new Exception($"The input item [{name}] has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
		}

		/// <summary>
		/// 编译公式
		/// </summary>
		/// <param name="exp">公式</param>
		/// <returns></returns>
		public static FunctionBase ParseFormula(string exp)
		{
			if(string.IsNullOrWhiteSpace(exp)) {
				throw new Exception("Parameter exp invalid !");
			}
			AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
			var stream = new AntlrCharStream(new AntlrInputStream(exp));
			var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

			var context = parser.prog();
			if(antlrErrorTextWriter.IsError) {
				throw new Exception(antlrErrorTextWriter.ErrorMsg);
			}
			var visitor = new MathFunctionVisitor();
			return visitor.Visit(context);
		}

		/// <summary>
		/// 检查公式是否正确
		/// </summary>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static bool CheckFormula(string exp)
		{
			if(string.IsNullOrWhiteSpace(exp)) { return false; }
			AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
			var stream = new AntlrCharStream(new AntlrInputStream(exp));
			var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

			var context = parser.prog();
			if(antlrErrorTextWriter.IsError) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 解析条件
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public static ConditionTree ParseCondition(string condition)
		{
			var tree = new ConditionTree();
			if(string.IsNullOrWhiteSpace(condition)) {
				tree.Type = ConditionTreeType.Error;
				tree.ErrorMessage = "condition is null";
				return tree;
			}
			try {
				AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
				var stream = new AntlrCharStream(new AntlrInputStream(condition));
				var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
				var tokens = new CommonTokenStream(lexer);
				var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

				var context = parser.prog();
				if(antlrErrorTextWriter.IsError) {
					tree.Type = ConditionTreeType.Error;
					tree.ErrorMessage = antlrErrorTextWriter.ErrorMsg;
					return tree;
				}
				var visitor = new MathSplitVisitor();
				return visitor.Visit(context);
			} catch(Exception ex) {
				tree.Type = ConditionTreeType.Error;
				tree.ErrorMessage = ex.Message;
			}
			return tree;
		}
		/// <summary>
		/// Creates a logical AND function that combines two specified functions.
		/// </summary>
		/// <param name="left">The left operand of the AND operation, representing the first function to be combined.</param>
		/// <param name="right">The right operand of the AND operation, representing the second function to be combined.</param>
		/// <returns>A new <see cref="FunctionBase"/> instance that represents the logical AND of the specified functions.</returns>
		public static FunctionBase Condition_And(FunctionBase left, FunctionBase right)
		{
			return new Function_AND(left, right);
		}
		/// <summary>
		/// Creates a logical OR function that combines two specified functions.
		/// </summary>
		/// <param name="left">The left operand of the OR operation, representing the first function to be combined.</param>
		/// <param name="right">The right operand of the OR operation, representing the second function to be combined.</param>
		/// <returns>A new <see cref="FunctionBase"/> instance that represents the logical OR of the specified functions.</returns>
		public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right)
		{
			return new Function_OR(left, right);
		}

		/// <summary>
		/// 解析计算表达式
		/// </summary>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static CalculateTree ParseCalculate(string exp)
		{
			var tree = new CalculateTree();
			if(string.IsNullOrWhiteSpace(exp)) {
				tree.Type = CalculateTreeType.Error;
				tree.ErrorMessage = "exp is null";
				return tree;
			}
			try {
				AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
				var stream = new AntlrCharStream(new AntlrInputStream(exp));
				var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
				var tokens = new CommonTokenStream(lexer);
				var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

				var context = parser.prog();
				if(antlrErrorTextWriter.IsError) {
					tree.Type = CalculateTreeType.Error;
					tree.ErrorMessage = antlrErrorTextWriter.ErrorMsg;
					return tree;
				}
				var visitor = new MathSplitVisitor2();
				return visitor.Visit(context);
			} catch(Exception ex) {
				tree.Type = CalculateTreeType.Error;
				tree.ErrorMessage = ex.Message;
			}
			return tree;
		}


		/// <summary>
		/// Creates a function that represents the sum of two specified functions.
		/// </summary>
		/// <param name="left">The first function to be added.</param>
		/// <param name="right">The second function to be added.</param>
		/// <returns>A function that computes the sum of the values returned by the specified functions.</returns>
		public static FunctionBase Calculate_Add(FunctionBase left, FunctionBase right)
		{
			return new Function_Add(left, right);
		}
		/// <summary>
		/// Creates a function that represents the subtraction of two functions.
		/// </summary>
		/// <param name="left">The function to use as the minuend in the subtraction operation. Cannot be null.</param>
		/// <param name="right">The function to use as the subtrahend in the subtraction operation. Cannot be null.</param>
		/// <returns>A function that, when evaluated, returns the result of subtracting the value of the right function from the value
		/// of the left function.</returns>
		public static FunctionBase Calculate_Subtract(FunctionBase left, FunctionBase right)
		{
			return new Function_Sub(left, right);
		}
		/// <summary>
		/// Creates a function that represents the multiplication of two functions.
		/// </summary>
		/// <param name="left">The left operand function to be multiplied.</param>
		/// <param name="right">The right operand function to be multiplied.</param>
		/// <returns>A function representing the product of the specified left and right functions.</returns>
		public static FunctionBase Calculate_Multiply(FunctionBase left, FunctionBase right)
		{
			return new Function_Mul(left, right);
		}
		/// <summary>
		/// Creates a function that represents the division of two functions.
		/// </summary>
		/// <param name="left">The numerator function to be divided.</param>
		/// <param name="right">The denominator function by which to divide.</param>
		/// <returns>A function representing the result of dividing the left function by the right function.</returns>
		public static FunctionBase Calculate_Divide(FunctionBase left, FunctionBase right)
		{
			return new Function_Div(left, right);
		}
		/// <summary>
		/// Creates a function that computes the remainder after dividing the result of the left function by the result of the
		/// right function.
		/// </summary>
		/// <param name="left">The function representing the dividend in the modulo operation. Cannot be null.</param>
		/// <param name="right">The function representing the divisor in the modulo operation. Cannot be null.</param>
		/// <returns>A function that, when evaluated, returns the result of the left function modulo the right function.</returns>
		public static FunctionBase Calculate_Mod(FunctionBase left, FunctionBase right)
		{
			return new Function_Mod(left, right);
		}
		/// <summary>
		/// Creates a new function that represents the connection of two functions.
		/// </summary>
		/// <param name="left">The first function to be connected. Cannot be null.</param>
		/// <param name="right">The second function to be connected. Cannot be null.</param>
		/// <returns>A FunctionBase instance representing the connection of the specified left and right functions.</returns>
		public static FunctionBase Calculate_Connect(FunctionBase left, FunctionBase right)
		{
			return new Function_Connect(left, right);
		}

	}
}