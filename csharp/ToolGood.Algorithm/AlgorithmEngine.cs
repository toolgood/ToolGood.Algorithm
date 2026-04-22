using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 算法引擎类
	/// </summary>
	public class AlgorithmEngine
	{
		internal int ExcelIndex = 1;

		/// <summary>
		/// 使用 本地时间，影响 时间戳转化
		/// </summary>
		public bool UseLocalTime { get; set; } = true;

		/// <summary>
		/// 长度单位
		/// </summary>
		public DistanceUnitType DistanceUnit { get; set; } = DistanceUnitType.M;

		/// <summary>
		/// 面积单位
		/// </summary>
		public AreaUnitType AreaUnit { get; set; } = AreaUnitType.M2;

		/// <summary>
		/// 体积单位
		/// </summary>
		public VolumeUnitType VolumeUnit { get; set; } = VolumeUnitType.M3;

		/// <summary>
		/// 重量单位
		/// </summary>
		public MassUnitType MassUnit { get; set; } = MassUnitType.KG;

		/// <summary>
		/// 最后一个错误
		/// </summary>
		public string LastError { get; internal set; }

		/// <summary>
		/// 使用EXCEL索引
		/// </summary>
		public bool UseExcelIndex { set { ExcelIndex = value ? 1 : 0; } }
		/// <summary>
		/// 使用缓存
		/// </summary>
		public bool UseParseCache { get; set; } = false;
		private readonly ConcurrentDictionary<string, FunctionBase> _parseCache = new ConcurrentDictionary<string, FunctionBase>();

		/// <summary>
		/// 自定义参数 请重写此方法
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual Operand GetParameter(string parameter)
		{
			return Operand.Error($"Parameter [{parameter}] is missing.");
		}

		/// <summary>
		/// 自定义函数 请重写此方法
		/// </summary>
		/// <param name="parameter"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ExecuteDiyFunction(string parameter, List<Operand> args)
		{
			return Operand.Error($"DiyFunction [{parameter}] is missing.");
		}

		#region Parse Evaluate

		/// <summary>
		/// 编译公式，默认
		/// </summary>
		/// <param name="exp">公式</param>
		/// <returns></returns>
		public virtual FunctionBase Parse(string exp)
		{
			LastError = null;
			if(string.IsNullOrWhiteSpace(exp)) {
				LastError = "Parameter exp invalid !";
				throw new Exception(LastError);
			}
			if(UseParseCache) {
				return _parseCache.GetOrAdd(exp.Trim(), _ => ParseInternal(exp));
			}
			return ParseInternal(exp);
		}
		private FunctionBase ParseInternal(string exp)
		{
			var stream = new AntlrCharStream(exp);
			var lexer = new mathLexer(stream, TextWriter.Null, TextWriter.Null);
			//var lexer = new mathLexer2(stream);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, TextWriter.Null, TextWriter.Null);

			var data = new AntlrErrorData();
			lexer.AddErrorData(data);
			parser.AddErrorData(data);

			//parser.Interpreter.PredictionMode = PredictionMode.SLL;
			var context = parser.prog();
			if(data.IsError) {
				LastError = data.ErrorMsg;
				throw new Exception(LastError);
			}
			var visitor = new MathFunctionVisitor();
			return visitor.Visit(context);
		}

		/// <summary>
		/// 编译公式，失败尽可能返回null,不抛错
		/// </summary>
		/// <param name="exp"></param>
		/// <returns></returns>
		public virtual FunctionBase ParseWithoutError(string exp)
		{
			LastError = null;
			if(string.IsNullOrWhiteSpace(exp)) {
				LastError = "Parameter exp invalid !";
				return null;
			}
			if(UseParseCache) {
				if(_parseCache.TryGetValue(exp.Trim(), out FunctionBase r)) {
					return r;
				}
				r = ParseInternalWithoutError(exp);
				if(r != null) {
					_parseCache.TryAdd(exp.Trim(), r);
				}
				return r;
			}
			return ParseInternalWithoutError(exp);
		}

		private FunctionBase ParseInternalWithoutError(string exp)
		{
			var stream = new AntlrCharStream(exp);
			var lexer = new mathLexer(stream, TextWriter.Null, TextWriter.Null);
			//var lexer = new mathLexer2(stream);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens, TextWriter.Null, TextWriter.Null);

			var data = new AntlrErrorData();
			lexer.AddErrorData(data);
			parser.AddErrorData(data);

			//parser.Interpreter.PredictionMode = PredictionMode.SLL;
			var context = parser.prog();
			if(data.IsError) {
				LastError = data.ErrorMsg;
				return null;
			}
			var visitor = new MathFunctionVisitor();
			return visitor.Visit(context);
		}

		/// <summary>
		/// 执行函数
		/// </summary>
		/// <returns></returns>
		public virtual Operand Evaluate(FunctionBase function)
		{
			return function.Evaluate(this);
		}

		#endregion Parse Evaluate

		#region TryEvaluate
		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual int TryEvaluate(string exp, int def)
		{
			return TryEvaluateCore(exp, def, o => o.IsNumber ? o : o.ToNumber("It can't be converted to number!"), o => o.IntValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual long TryEvaluate(string exp, long def)
		{
			return TryEvaluateCore(exp, def, o => o.IsNumber ? o : o.ToNumber("It can't be converted to number!"), o => o.LongValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual double TryEvaluate(string exp, double def)
		{
			return TryEvaluateCore(exp, def, o => o.IsNumber ? o : o.ToNumber("It can't be converted to number!"), o => o.DoubleValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual decimal TryEvaluate(string exp, decimal def)
		{
			return TryEvaluateCore(exp, def, o => o.IsNumber ? o : o.ToNumber("It can't be converted to number!"), o => o.NumberValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual string TryEvaluate(string exp, string def)
		{
			return TryEvaluateCore(exp, def, o => o.IsText ? o : o.ToText("It can't be converted to string!"), o => o.TextValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual bool TryEvaluate(string exp, bool def)
		{
			return TryEvaluateCore(exp, def, o => o.IsBoolean ? o : o.ToBoolean("It can't be converted to bool!"), o => o.BooleanValue);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual DateTime TryEvaluate(string exp, DateTime def)
		{
			return TryEvaluateCore(exp, def,
				o => o.IsDate ? o : o.ToMyDate("It can't be converted to DateTime!"),
				o => o.DateValue.ToDateTime(UseLocalTime ? DateTimeKind.Local : DateTimeKind.Utc));
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual TimeSpan TryEvaluate(string exp, TimeSpan def)
		{
			return TryEvaluateCore(exp, def,
				o => o.IsDate ? o : o.ToMyDate("It can't be converted to DateTime!"),
				o => o.DateValue.ToTimeSpan());
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值。
		/// 解决 def 为 null 二义性问题
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public virtual MyDate TryEvaluate_MyDate(string exp, MyDate def)
		{
			return TryEvaluateCore(exp, def,
				o => o.IsDate ? o : o.ToMyDate("It can't be converted to DateTime!"),
				o => o.DateValue);
		}

		private T TryEvaluateCore<T>(string exp, T def, Func<Operand, Operand> convert, Func<Operand, T> getValue)
		{
			try {
				var function = ParseWithoutError(exp);
				if(function == null) { return def; }
				var obj = function.Evaluate(this);
				obj = convert(obj);
				if(obj.IsError) {
					LastError = obj.ErrorMsg;
					return def;
				}
				return getValue(obj);
			} catch(Exception ex) {
				LastError = ex.ToString();
			}
			return def;
		}

		#endregion TryEvaluate
	}
}