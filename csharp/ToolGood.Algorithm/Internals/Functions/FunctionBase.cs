using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions
{
	/// <summary>
	/// Represents the base class for all function implementations that can be calculated by an algorithm engine.
	/// </summary>
	/// <remarks>This abstract class defines a contract for functions that can be evaluated within the context
	/// of an algorithm engine. Derived classes must implement the <see cref="Evaluate"/> method to provide specific
	/// function logic.</remarks>
	public abstract class FunctionBase
	{
		/// <summary>
		/// 进行计算
		/// </summary>
		/// <param name="work"></param>
		/// <param name="tempParameter">临时参数，未找到返回null</param>
		/// <returns></returns>
		public abstract Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter = null);

		#region TryEvaluate

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="converter"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		private T TryEvaluate<T>(AlgorithmEngine work, T def, Func<Operand, Operand> converter, Func<Operand, T> resultConverter, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				var converted = converter(obj);
				if(converted.IsError) {
					work.LastError = converted.ErrorMsg;
					return def;
				}
				return resultConverter(converted);
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public ushort TryEvaluate(AlgorithmEngine work, ushort def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (ushort)obj.IntValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public uint TryEvaluate(AlgorithmEngine work, uint def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (uint)obj.IntValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public ulong TryEvaluate(AlgorithmEngine work, ulong def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (ulong)obj.IntValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public short TryEvaluate(AlgorithmEngine work, short def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (short)obj.IntValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public int TryEvaluate(AlgorithmEngine work, int def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => obj.IntValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public long TryEvaluate(AlgorithmEngine work, long def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => obj.LongValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public float TryEvaluate(AlgorithmEngine work, float def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (float)obj.DoubleValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public double TryEvaluate(AlgorithmEngine work, double def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (double)obj.DoubleValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public decimal TryEvaluate(AlgorithmEngine work, decimal def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotNumber ? obj.ToNumber("It can't be converted to number!") : obj,
				obj => (decimal)obj.NumberValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public string TryEvaluate(AlgorithmEngine work, string def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotText ? obj.ToText("It can't be converted to string!") : obj,
				obj => obj.TextValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public bool TryEvaluate(AlgorithmEngine work, bool def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotBoolean ? obj.ToBoolean("It can't be converted to bool!") : obj,
				obj => obj.BooleanValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public DateTime TryEvaluate(AlgorithmEngine work, DateTime def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				var converted = obj.IsNotDate ? obj.ToMyDate("It can't be converted to DateTime!") : obj;
				if(converted.IsError) {
					work.LastError = converted.ErrorMsg;
					return def;
				}
				if(work.UseLocalTime) {
					return converted.DateValue.ToDateTime(DateTimeKind.Local);
				}
				return converted.DateValue.ToDateTime(DateTimeKind.Utc);
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public TimeSpan TryEvaluate(AlgorithmEngine work, TimeSpan def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotDate ? obj.ToMyDate("It can't be converted to DateTime!") : obj,
				obj => (TimeSpan)obj.DateValue, tempParameter);
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值。
		/// 解决 def 为 null 二义性问题
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public MyDate TryEvaluate_MyDate(AlgorithmEngine work, MyDate def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def, 
				obj => obj.IsNotDate ? obj.ToMyDate("It can't be converted to DateTime!") : obj,
				obj => obj.DateValue, tempParameter);
		}

		#endregion TryEvaluate
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			ToString(stringBuilder, false);
			return stringBuilder.ToString();
		}
		/// <summary>
		/// Appends a string representation of the current object to the specified StringBuilder, optionally including
		/// brackets.
		/// </summary>
		/// <param name="stringBuilder">The StringBuilder to which the string representation will be appended. Cannot be null.</param>
		/// <param name="addBrackets">true to enclose the string representation in brackets; otherwise, false.</param>
		public abstract void ToString(StringBuilder stringBuilder, bool addBrackets);
	}
}