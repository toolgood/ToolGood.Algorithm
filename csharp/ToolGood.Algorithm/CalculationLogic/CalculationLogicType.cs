namespace ToolGood.Algorithm.CalculationLogic
{
	/// <summary>
	/// 计算逻辑类型
	/// </summary>
	public enum CalculationLogicType
	{
		/// <summary>
		/// 场景
		/// </summary>
		Scene,
		/// <summary>
		/// 值
		/// </summary>
		InitValue,
		/// <summary>
		/// 条件
		/// </summary>
		Condition,
		/// <summary>
		/// 用公式赋值
		/// </summary>
		SetFormula,
		/// <summary>
		/// 赋值
		/// </summary>
		SetValue,
		/// <summary>
		/// 错误
		/// </summary>
		Error,
		/// <summary>
		/// 空行
		/// </summary>
		BlankLine,
	}
}
