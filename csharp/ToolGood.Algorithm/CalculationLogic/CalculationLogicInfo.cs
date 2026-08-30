using System;

namespace ToolGood.Algorithm.CalculationLogic
{
	/// <summary>
	/// 计算逻辑信息
	/// </summary>
	public class CalculationLogicInfo
	{
		/// <summary>
		/// 类型
		/// </summary>
		public CalculationLogicType LogicType { get; set; }
		/// <summary>
		/// 层级
		/// </summary>
		public int Layer { get; set; }
		/// <summary>
		/// 逻辑名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 公式
		/// </summary>
		public string Exp { get; set; }
		/// <summary>
		/// 公式2
		/// </summary>
		public string Exp2 { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public Operand Value { get; set; }

		/// <summary>
		/// 转换为信息字符串
		/// </summary>
		/// <returns></returns>
		public string ToInfoString()
		{
			var remark = string.IsNullOrEmpty(Remark) ? "" : $" //{Remark}";
			var layerStr = new string(' ', Layer * 3);
			switch(LogicType) {
				case CalculationLogicType.BlankLine: return String.Empty;
				case CalculationLogicType.Scene: return $"===== {Name} =====";
				case CalculationLogicType.InitValue: return $"{Name}={Exp};";
				case CalculationLogicType.Condition:
					if(Value.IsError) {
						return $"[条件][错误] {layerStr}if {Exp}: //{Exp2} = {Value.ErrorMsg}{remark}";
					}
					if(Value.BooleanValue) {
						return $"[成功] {layerStr}if {Exp}: //{Exp2}{remark}";
					}
					return $"[失败] {layerStr}if {Exp}: //{Exp2}{remark}";
				case CalculationLogicType.SetFormula:
					if(Value.IsError) {
						return $"[赋值][错误] {layerStr}{Name} = {Exp} = {Exp2} = {Value.ErrorMsg}{remark}";
					}
					return $"[赋值] {layerStr}{Name} = {Exp} = {Exp2} = {Value.ToText().TextValue}{remark}";
				case CalculationLogicType.SetValue:
					return $"[赋值] {layerStr}{Name} = {Exp}{remark}";
				case CalculationLogicType.Error: return $"[错误] {layerStr}{Exp}";
				default: break;
			}
			return $"[错误]: {layerStr}{Name}{Exp}{remark}";
		}
	}
}
