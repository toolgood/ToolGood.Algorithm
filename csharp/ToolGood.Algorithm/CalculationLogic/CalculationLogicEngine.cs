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
		private readonly List<CalculationLogicInfo> _initValueInfos = new List<CalculationLogicInfo>();
		private readonly List<CalculationLogicInfo> _calculationLogicInfos = new List<CalculationLogicInfo>();
		private readonly bool _useCalculationLogicInfo;
		private readonly AlgorithmEngineEx _engine;

		/// <summary>
		/// 计算逻辑引擎
		/// </summary>
		/// <param name="functionCache"></param>
		/// <param name="useCalculationLogicInfo"></param>
		public CalculationLogicEngine(FunctionCache functionCache, bool useCalculationLogicInfo)
		{
			_functionCache = functionCache;
			_useCalculationLogicInfo = useCalculationLogicInfo;
			_engine = new AlgorithmEngineEx();
		}

		#region SetScene
		/// <summary>
		/// 设置场景名称
		/// </summary>
		/// <param name="scene"></param>
		public void SetSceneName(string scene)
		{
			if(_useCalculationLogicInfo) {
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
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value, Layer = layer, Remark = remark });
			}
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, decimal value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, decimal value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, decimal value, string remark)
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
		public void InitValue(string key, decimal value, int layer, string remark)
		{
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value.ToString(), Layer = layer, Remark = remark });
			}
		}

		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, double value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, double value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, double value, string remark)
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
		public void InitValue(string key, double value, int layer, string remark)
		{
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value.ToString(), Layer = layer, Remark = remark });
			}
		}

		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, float value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, float value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, float value, string remark)
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
		public void InitValue(string key, float value, int layer, string remark)
		{
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value.ToString(), Layer = layer, Remark = remark });
			}
		}

		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, bool value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, bool value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, bool value, string remark)
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
		public void InitValue(string key, bool value, int layer, string remark)
		{
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value.ToString(), Layer = layer, Remark = remark });
			}
		}

		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void InitValue(string key, int value)
		{
			InitValue(key, value, 0, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="layer"></param>
		public void InitValue(string key, int value, int layer)
		{
			InitValue(key, value, layer, null);
		}
		/// <summary>
		/// 初始化值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="remark"></param>
		public void InitValue(string key, int value, string remark)
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
		public void InitValue(string key, int value, int layer, string remark)
		{
			_engine.AddParameter(key, value);
			if(_useCalculationLogicInfo) {
				_initValueInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.InitValue, Name = key, Exp = value.ToString(), Layer = layer, Remark = remark });
			}
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
			var func = _functionCache.ParseWithCache(condition);
			var operand = func.Evaluate(_engine);
			operand = operand.ToBoolean("The condition must be a boolean value!");
			if(operand.IsError) {
				if(_useCalculationLogicInfo) {
					var expStr = ExpAnalysis(condition);
					_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.Condition, Exp = condition, Exp2 = expStr, Value = operand, Layer = layer, Remark = remark });
				}
				throw new FormatException(operand.ErrorMsg);
			}
			if(_useCalculationLogicInfo) {
				var expStr = ExpAnalysis(condition);
				_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.Condition, Exp = condition, Exp2 = expStr, Value = operand, Layer = layer, Remark = remark });
			}
			return operand.BooleanValue;
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
			var func = _functionCache.ParseWithCache(exp);
			var operand = func.Evaluate(_engine);
			_engine.AddParameter(key, operand);

			if(_useCalculationLogicInfo) {
				var expStr = ExpAnalysis(exp);
				_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.SetValue, Name = key, Exp = exp, Exp2 = expStr, Value = operand, Layer = layer, Remark = remark });
			}
		}
		#endregion

		private string ExpAnalysis(string exp)
		{


			return exp.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
		}

		#region BlankLine
		/// <summary>
		/// 添加空行
		/// </summary>
		public void BlankLine()
		{
			if(_useCalculationLogicInfo) {
				_calculationLogicInfos.Add(new CalculationLogicInfo() { LogicType = CalculationLogicType.BlankLine, });
			}
		}
		#endregion

		#region ToInfoString
		/// <summary>
		/// 转换为信息字符串
		/// </summary>
		/// <returns></returns>
		public String ToInfoString()
		{
			if(_useCalculationLogicInfo) {
				var sb = new StringBuilder();
				if(_initValueInfos.Count > 0) {
					sb.Append("[初始] ");
					foreach(var item in _initValueInfos) {
						sb.Append(item.ToInfoString());
					}
					sb.AppendLine();
				}
				foreach(var item in _calculationLogicInfos) {
					sb.AppendLine(item.ToInfoString());
				}
				return sb.ToString();
			}
			return String.Empty;
		}

		#endregion

	}
}
