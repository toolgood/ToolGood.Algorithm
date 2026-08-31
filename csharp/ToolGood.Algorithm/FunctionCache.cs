using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 函数缓存类，使用ConcurrentDictionary实现线程安全的函数缓存。
	/// </summary>
	public sealed class FunctionCache
	{
		private readonly ConcurrentDictionary<string, FunctionBase> calculateCache = new ConcurrentDictionary<string, FunctionBase>();
		private readonly ConcurrentDictionary<string, FunctionBase> conditionCache = new ConcurrentDictionary<string, FunctionBase>();
		private readonly ConcurrentDictionary<string, DiyNameInfo> diyNameCache = new ConcurrentDictionary<string, DiyNameInfo>();

		/// <summary>
		/// 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="exp">表达式字符串</param>
		/// <returns>自定义名称信息</returns>
		public DiyNameInfo GetDiyNamesWithCache(string exp)
		{
			return diyNameCache.GetOrAdd(exp, key => AlgorithmEngineHelper.GetDiyNames(key));
		}

		/// <summary>
		/// 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">函数表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseWithCache(string funExp)
		{
			return calculateCache.GetOrAdd(funExp, key => {
				var tree = AlgorithmEngineHelper.ParseCalculate(key);
				return CreateCalculate(tree);
			});
		}
		private FunctionBase CreateCalculate(CalculateTree tree)
		{
			if(calculateCache.TryGetValue(tree.Text, out FunctionBase value)) { return value; }
			if(tree.Type == Algorithm.Enums.CalculateTreeType.String) {
				var leafFun = AlgorithmEngineHelper.ParseFormula(tree.Text);
				calculateCache[tree.Text] = leafFun;
				return leafFun;
			}
			if(tree.Type == Algorithm.Enums.CalculateTreeType.Error) { throw new Exception(tree.ErrorMessage); }

			var leftFunc = CreateCalculate(tree.Nodes[0]);
			var rightFunc = CreateCalculate(tree.Nodes[1]);
			var fun = AlgorithmEngineHelper.CombineCalculate(leftFunc, (Algorithm.Enums.CombineCalculateType)(byte)tree.Type, rightFunc);
			calculateCache[tree.Text] = fun;
			return fun;
		}

		/// <summary>
		/// 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">条件表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseConditionWithCache(string funExp)
		{
			return conditionCache.GetOrAdd(funExp, key => {
				var tree = AlgorithmEngineHelper.ParseCondition(key);
				return CreateCondition(tree);
			});
		}
		private FunctionBase CreateCondition(ConditionTree tree)
		{
			if(conditionCache.TryGetValue(tree.Text, out FunctionBase value)) { return value; }
			if(tree.Type == Algorithm.Enums.ConditionTreeType.String) {
				// 直接走计算树路径(CreateCalculate 内部用 TryGetValue+索引器赋值),
				// 避免对同一 key 重入 GetOrAdd(ConcurrentDictionary 文档禁止 valueFactory 递归调用)
				var calcTree = AlgorithmEngineHelper.ParseCalculate(tree.Text);
				return CreateCalculate(calcTree);
			}

			var leftFunc = CreateCondition(tree.Nodes[0]);
			var rightFunc = CreateCondition(tree.Nodes[1]);
			if(tree.Type == Algorithm.Enums.ConditionTreeType.And) {
				var fun = AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
				conditionCache[tree.Text] = fun;
				return fun;
			} else if(tree.Type == Algorithm.Enums.ConditionTreeType.Or) {
				var fun = AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
				conditionCache[tree.Text] = fun;
				return fun;
			}
			throw new Exception(tree.ErrorMessage);
		}
	 
	}
}
