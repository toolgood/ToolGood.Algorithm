using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	/// <summary>
	/// 财务函数共用的数值守卫
	/// </summary>
	/// <remarks>
	/// MathEx.Pow/Log/Exp 在负底非整指数、零底负指数、指数超出 int 范围或结果溢出时会抛异常,
	/// 而函数求值链路(Visitor -&gt; Evaluate)没有统一的异常拦截, 异常会直接冒泡给宿主。
	/// 此处把这类异常转换成"失败"返回值, 由调用方转成错误 Operand。
	/// </remarks>
	internal static class FinancialMath
	{
		/// <summary>
		/// 安全指数上限。超过该值时 MathEx.Exp 的循环次数(与指数绝对值成正比)过大,
		/// 或 PowerN 必然溢出, 均视为计算失败。
		/// </summary>
		private const decimal MaxSafeExponent = 1000000m;

		/// <summary>
		/// 以正数为底求幂, 失败返回 false
		/// </summary>
		/// <param name="baseValue">底数, 必须大于 0</param>
		/// <param name="exponent">指数</param>
		/// <param name="result">幂运算结果</param>
		/// <returns>计算成功返回 true</returns>
		public static bool TryPowPositiveBase(decimal baseValue, decimal exponent, out decimal result)
		{
			result = 0;
			if (baseValue <= 0) return false;
			if (exponent > MaxSafeExponent || exponent < -MaxSafeExponent) return false;
			try {
				result = MathEx.Pow(baseValue, exponent);
			} catch (OverflowException) {
				return false;
			}
			return true;
		}
	}
}
