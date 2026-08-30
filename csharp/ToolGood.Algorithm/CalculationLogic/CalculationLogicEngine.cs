using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.CalculationLogic
{
	/// <summary>
	/// 计算逻辑引擎
	/// </summary>
	public class CalculationLogicEngine
	{
		private readonly FunctionCache _functionCache;
		private readonly List<CalculationLogicInfo> _calculationLogicInfos = new List<CalculationLogicInfo>();
		private readonly bool _UseCalculationLogicInfo;

		/// <summary>
		/// 计算逻辑引擎
		/// </summary>
		/// <param name="functionCache"></param>
		/// <param name="useCalculationLogicInfo"></param>
		public CalculationLogicEngine(FunctionCache functionCache, bool useCalculationLogicInfo)
		{
			_functionCache = functionCache;
			_UseCalculationLogicInfo = useCalculationLogicInfo;
		}

		#region SetScene
		/// <summary>
		/// 设置场景名称
		/// </summary>
		/// <param name="scene"></param>
		public void SetSceneName(string scene)
		{
			if(_UseCalculationLogicInfo) {
				_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.Scene, Name = scene });
			}
		}
		#endregion

		#region InitValue
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, string value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, string value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, string value, string remark)
		{
			InitValue(key, value, 0, remark);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, string value, int layer, string remark)
		{
		}
		#endregion

		#region CheckCondition
		/// <summary>
		/// 检查条件
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public bool CheckCondition(string condition)
		{
			return CheckCondition(condition, 0, null);
		}
		/// <summary>
		/// 检查条件
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public bool CheckCondition(string condition, int layer)
		{
			return CheckCondition(condition, layer, null);
		}
		/// <summary>
		/// 检查条件
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="remark"></param>
		/// <returns></returns>
		public bool CheckCondition(string condition, string remark)
		{
			return CheckCondition(condition, 0, remark);
		}
		/// <summary>
		/// 检查条件
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="layer"></param>
		/// <param name="remark"></param>
		/// <returns></returns>
		public bool CheckCondition(string condition, int layer, string remark)
		{
			return false;
		}
		#endregion

		#region SetValue
		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="exp"></param>
		public void SetValue(string key, string exp)
		{
			SetValue(key, exp, 0, null);
		}
		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="exp"></param>
		/// <param name="layer"></param>
		public void SetValue(string key, string exp, int layer)
		{
			SetValue(key, exp, layer, null);
		}
		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="exp"></param>
		/// <param name="remark"></param>
		public void SetValue(string key, string exp, string remark)
		{
			SetValue(key, exp, 0, remark);
		}
		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="exp"></param>
		/// <param name="layer"></param>
		/// <param name="remark"></param>
		public void SetValue(string key, string exp, int layer, string remark)
		{

		}
		#endregion

		#region BlankLine
		/// <summary>
		/// 添加空行
		/// </summary>
		public void BlankLine()
		{
			if(_UseCalculationLogicInfo) {
				_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.BlankLine, });
			}
		}
		#endregion

		/// <summary>
		/// 转换为信息字符串
		/// </summary>
		/// <returns></returns>
		public String ToInfoString()
		{
			var sb = new StringBuilder();
			foreach(var item in _calculationLogicInfos) {
				sb.AppendLine(item.ToInfoString());
			}
			return sb.ToString();
		}


	}
}
