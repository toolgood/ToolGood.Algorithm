using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 函数缓存接口，定义对计算表达式、条件表达式以及自定义名称解析结果的缓存行为。
	/// </summary>
	public interface IFunctionCache
	{
		/// <summary>
		/// 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="exp">表达式字符串</param>
		/// <returns>自定义名称信息</returns>
		DiyNameInfo GetDiyNamesWithCache(string exp);

		/// <summary>
		/// 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">函数表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		FunctionBase ParseWithCache(string funExp);

		/// <summary>
		/// 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">条件表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		FunctionBase ParseConditionWithCache(string funExp);
	}
}
