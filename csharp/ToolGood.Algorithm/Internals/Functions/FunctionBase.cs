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
		/// 名称
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// 进行计算
		/// </summary>
		/// <param name="work"></param>
		/// <param name="tempParameter">临时参数，未找到返回null</param>
		/// <returns></returns>
		public abstract Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter = null);

		#region ToString
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
		#endregion

		#region ConvertToText
		/// <summary>
		/// 转换参数为文本
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		protected Operand ConvertToText(Operand arg, int paramIndex)
		{
			return arg.ToText("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}

		/// <summary>
		/// 转换参数为布尔值
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		protected Operand ConvertToBoolean(Operand arg, int paramIndex)
		{
			return arg.ToBoolean("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}

		/// <summary>
		/// 转换参数为数字
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		protected Operand ConvertToNumber(Operand arg, int paramIndex)
		{
			return arg.ToNumber("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}

		/// <summary>
		/// 转换参数为数组
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		protected Operand ConvertToArray(Operand arg, int paramIndex)
		{
			return arg.ToArray("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}

		/// <summary>
		/// 转换参数为日期
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		protected Operand ConvertToDate(Operand arg, int paramIndex)
		{
			return arg.ToMyDate("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}

		#endregion

		#region ParameterError FunctionError ParameterNull
		/// <summary>
		/// Creates an error operand indicating that a specific function parameter is invalid.
		/// </summary>
		/// <param name="paramIndex">The zero-based index of the parameter that caused the error.</param>
		/// <returns>An operand representing an error for the specified parameter.</returns>
		protected Operand ParameterError(int paramIndex)
		{
			return Operand.Error("Function '{0}' parameter {1} is error!", Name, paramIndex);
		}
		/// <summary>
		/// Creates an error operand indicating that a function parameter is invalid.
		/// </summary>
		/// <returns>An <see cref="Operand"/> representing an error state for the function due to an invalid parameter.</returns>
		protected Operand FunctionError()
		{
			return Operand.Error("Function '{0}' parameter is error!", Name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		protected Operand CompareError()
		{
			return Operand.Error("Function '{0}' compare is error.", Name);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected Operand Div0Error()
		{
			return Operand.Error("Function '{0}' Div 0 error!", Name);
		}
		#endregion

		#region TryEvaluate

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="converter"></param>
		/// <param name="resultConverter"></param>
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
				work.LastError = ex.Message;
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
		public int TryEvaluate(AlgorithmEngine work, int def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return TryEvaluate(work, def,
				obj => obj.IsNumber ? obj : obj.ToNumber("It can't be converted to number!"),
				obj => obj.IntValue, tempParameter);
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
				obj => obj.IsNumber ? obj : obj.ToNumber("It can't be converted to number!"),
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
				obj => obj.IsText ? obj : obj.ToText("It can't be converted to string!"),
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
				obj => obj.IsBoolean ? obj : obj.ToBoolean("It can't be converted to bool!"),
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
			return TryEvaluate(work, def,
				obj => obj.IsDate ? obj : obj.ToMyDate("It can't be converted to DateTime!"),
				obj => {
					if(work.UseLocalTime) {
						return obj.DateValue.ToDateTime(DateTimeKind.Local);
					}
					return obj.DateValue.ToDateTime(DateTimeKind.Utc);
				} , tempParameter);
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
				obj => obj.IsDate ? obj : obj.ToMyDate("It can't be converted to DateTime!"),
				obj => (TimeSpan)obj.DateValue, tempParameter);
		}
 
		#endregion
	}
}