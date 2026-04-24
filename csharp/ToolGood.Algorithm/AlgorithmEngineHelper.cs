using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using System;
using System.IO;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Functions.Compare;
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
		private static readonly Regex unitRegex = new Regex(@"[\s\(\)（）\[\]<>]", RegexOptions.Compiled);

		internal static mathParser.ProgContext CreateParserContext(string exp, AntlrErrorData data)
		{
			var stream = new AntlrCharStream(exp);
			var lexer = new mathLexer(stream, TextWriter.Null, TextWriter.Null);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, TextWriter.Null, TextWriter.Null);

			lexer.AddErrorData(data);
			parser.AddErrorData(data);
			//lexer.RemoveErrorListeners();
			//lexer.AddErrorListener(data);
			//parser.RemoveErrorListeners();
			//parser.AddErrorListener(data);

			//parser.Interpreter.PredictionMode = PredictionMode.SLL;
			return parser.prog();
		}

		/// <summary>
		/// 是不是参数
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public static bool IsParameter(string parameter)
		{
			if(string.IsNullOrWhiteSpace(parameter)) { return false; }
			try {
				var diy = GetDiyNames(parameter);
				if(diy.Functions.Count > 0) { return false; }
				if(diy.Parameters.Count == 1) {
					var p = diy.Parameters[0];
					return p.Name == parameter;
				}
			} catch(Exception) { }
			return false;
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
				throw new ArgumentException("Parameter exp invalid !", nameof(exp));
			}
			var errorWriter = new AntlrErrorData();
			var context = CreateParserContext(exp, errorWriter);
			if(errorWriter.IsError) {
				throw new ArgumentException(errorWriter.ErrorMsg, nameof(exp));
			}
			var visitor = new DiyNameVisitor();
			visitor.Visit(context);
			return new DiyNameInfo(visitor.Parameters, visitor.Functions);
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
			if(oldSrcUnit == oldTarUnit) { return src; }

			var result = TryConvert(src, oldSrcUnit, oldTarUnit);
			if(result.HasValue) { return result.Value; }

			oldSrcUnit = unitRegex.Replace(oldSrcUnit, string.Empty);
			result = TryConvert(src, oldSrcUnit, oldTarUnit);
			if(result.HasValue) { return result.Value; }

			if(string.IsNullOrEmpty(name)) {
				throw new InvalidOperationException($"The input item has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
			}
			throw new InvalidOperationException($"The input item [{name}] has different units and cannot be converted from [{oldSrcUnit}] to [{oldTarUnit}]");
		}

		private static decimal? TryConvert(decimal src, string srcUnit, string tarUnit)
		{
			decimal d1;
			if(DistanceConverter.TryConvert(srcUnit, tarUnit, src, out d1)) {
				return d1;
			}
			if(MassConverter.TryConvert(srcUnit, tarUnit, src, out d1)) {
				return d1;
			}
			if(AreaConverter.TryConvert(srcUnit, tarUnit, src, out d1)) {
				return d1;
			}
			if(VolumeConverter.TryConvert(srcUnit, tarUnit, src, out d1)) {
				return d1;
			}
			return null;
		}

		/// <summary>
		/// 编译公式
		/// </summary>
		/// <param name="exp">公式</param>
		/// <returns></returns>
		public static FunctionBase ParseFormula(string exp)
		{
			if(string.IsNullOrWhiteSpace(exp)) {
				throw new ArgumentException("Parameter exp invalid !", nameof(exp));
			}
			var errorWriter = new AntlrErrorData();
			var context = CreateParserContext(exp, errorWriter);
			if(errorWriter.IsError) {
				throw new ArgumentException(errorWriter.ErrorMsg, nameof(exp));
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
			var errorWriter = new AntlrErrorData();
			CreateParserContext(exp, errorWriter);
			return !errorWriter.IsError;
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
				var errorWriter = new AntlrErrorData();
				var context = CreateParserContext(condition, errorWriter);
				if(errorWriter.IsError) {
					tree.Type = ConditionTreeType.Error;
					tree.ErrorMessage = errorWriter.ErrorMsg;
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
				var errorWriter = new AntlrErrorData();
				var context = CreateParserContext(exp, errorWriter);
				if(errorWriter.IsError) {
					tree.Type = CalculateTreeType.Error;
					tree.ErrorMessage = errorWriter.ErrorMsg;
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
		/// 拼接计算表达式
		/// </summary>
		/// <param name="left">左操作数</param>
		/// <param name="type">操作类型</param>
		/// <param name="right">右操作数</param>
		/// <returns>返回拼接后的计算表达式</returns>
		public static FunctionBase CombineCalculate(FunctionBase left, CombineCalculateType type, FunctionBase right)
		{
			switch(type) {
				case CombineCalculateType.Add: return new Function_Add(left, right);
				case CombineCalculateType.Sub: return new Function_Sub(left, right);
				case CombineCalculateType.Mul: return new Function_Mul(left, right);
				case CombineCalculateType.Div: return new Function_Div(left, right);
				case CombineCalculateType.Mod: return new Function_Mod(left, right);
				case CombineCalculateType.Connect: return new Function_Connect(left, right);
				case CombineCalculateType.And: return new Function_AND(left, right);
				case CombineCalculateType.Or: return new Function_OR(left, right);
				case CombineCalculateType.OpGt: return new Function_GT(left, right);
				case CombineCalculateType.OpLt: return new Function_LT(left, right);
				case CombineCalculateType.OpGe: return new Function_GE(left, right);
				case CombineCalculateType.OpLe: return new Function_LE(left, right);
				case CombineCalculateType.OpEq: return new Function_EQ(left, right);
				case CombineCalculateType.OpNe:
				default: return new Function_NE(left, right);
			}
		}


	}
}