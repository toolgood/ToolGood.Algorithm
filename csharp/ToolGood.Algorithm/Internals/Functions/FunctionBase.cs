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
	}
}