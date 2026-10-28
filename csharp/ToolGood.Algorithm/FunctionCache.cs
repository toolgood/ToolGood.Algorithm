using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 函数缓存类，使用ConcurrentDictionary实现线程安全的函数缓存。
	/// </summary>
	public sealed class FunctionCache : IFunctionCache
	{
		private readonly ConcurrentDictionary<string, FunctionBase> calculateCache = new ConcurrentDictionary<string, FunctionBase>();
		private readonly ConcurrentDictionary<string, FunctionBase> conditionCache = new ConcurrentDictionary<string, FunctionBase>();
		private readonly ConcurrentDictionary<string, DiyNameInfo> diyNameCache = new ConcurrentDictionary<string, DiyNameInfo>();

		// 缓存条目软上限：动态拼接的公式串会让缓存无限增长（内存泄漏），超限淘汰一半。
		// 缓存仅用于免重复解析，被淘汰的条目下次命中时重新解析，不影响正确性
		private const int MaxCacheSize = 100_000;

		// 每隔多少次「新增」才检查一次容量。
		// ConcurrentDictionary.Count 需要遍历全部 stripe 加锁，10 万条时单次约 20 µs，
		// 命中路径上每次点击都被它拖慢（实测 153 ns → 21.6 µs），所以只能按写入次数分摊。
		private const int TrimCheckInterval = 1024;

		private int _calculateWrites;
		private int _conditionWrites;
		private int _diyNameWrites;

		/// <summary>
		/// 仅在真正新增条目后调用（命中路径不调用），按写入次数分摊容量检查成本。
		/// </summary>
		private static void AfterEntryAdded<TValue>(ConcurrentDictionary<string, TValue> cache, ref int writes)
		{
			if(Interlocked.Increment(ref writes) < TrimCheckInterval) { return; }
			// 只有抢到计数重置权的那个线程做淘汰，避免并发时重复遍历
			if(Interlocked.Exchange(ref writes, 0) < TrimCheckInterval) { return; }
			TrimCache(cache);
		}

		private static void TrimCache<TValue>(ConcurrentDictionary<string, TValue> cache)
		{
			// Count 只在这里取一次：下面按删除个数计数，不能每删一条再问一次 Count
			var count = cache.Count;
			if(count < MaxCacheSize) { return; }
			// 淘汰一半而不是全部清空：全清会让高频 key 与长尾 key 一起被踢掉，命中率塌缩
			var needRemove = count - MaxCacheSize / 2;
			foreach(var key in cache.Keys) {
				if(needRemove <= 0) { break; }
				if(cache.TryRemove(key, out _)) { needRemove--; }
			}
		}

		/// <summary>
		/// 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="exp">表达式字符串</param>
		/// <returns>自定义名称信息</returns>
		public DiyNameInfo GetDiyNamesWithCache(string exp)
		{
			if(diyNameCache.TryGetValue(exp, out var value)) { return value; }
			value = AlgorithmEngineHelper.GetDiyNames(exp);
			if(diyNameCache.TryAdd(exp, value)) { AfterEntryAdded(diyNameCache, ref _diyNameWrites); }
			return value;
		}

		/// <summary>
		/// 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">函数表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseWithCache(string funExp)
		{
			if(calculateCache.TryGetValue(funExp, out var value)) { return value; }
			value = CreateCalculate(AlgorithmEngineHelper.ParseCalculate(funExp));
			if(calculateCache.TryAdd(funExp, value)) { AfterEntryAdded(calculateCache, ref _calculateWrites); }
			return value;
		}
		private FunctionBase CreateCalculate(CalculateTree tree)
		{
			if(tree.Type == Algorithm.Enums.CalculateTreeType.String) {
				// 仅叶子节点需要字符串内容；中间节点不再以 Text(子串)作为缓存 key，避免 Substring 产生 O(n²) 子串驻留
				var text = tree.Text;
				if(calculateCache.TryGetValue(text, out FunctionBase value)) { return value; }
				var leafFun = AlgorithmEngineHelper.ParseFormula(text);
				if(calculateCache.TryAdd(text, leafFun)) { AfterEntryAdded(calculateCache, ref _calculateWrites); }
				return leafFun;
			}
			if(tree.Type == Algorithm.Enums.CalculateTreeType.Error) { throw new Exception(tree.ErrorMessage); }

			var leftFunc = CreateCalculate(tree.Nodes[0]);
			var rightFunc = CreateCalculate(tree.Nodes[1]);
			return AlgorithmEngineHelper.CombineCalculate(leftFunc, (Algorithm.Enums.CombineCalculateType)(byte)tree.Type, rightFunc);
		}

		/// <summary>
		/// 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">条件表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseConditionWithCache(string funExp)
		{
			if(conditionCache.TryGetValue(funExp, out var value)) { return value; }
			value = CreateCondition(AlgorithmEngineHelper.ParseCondition(funExp));
			if(conditionCache.TryAdd(funExp, value)) { AfterEntryAdded(conditionCache, ref _conditionWrites); }
			return value;
		}
		private FunctionBase CreateCondition(ConditionTree tree)
		{
			if(tree.Type == Algorithm.Enums.ConditionTreeType.String) {
				// 直接走计算树路径(CreateCalculate 内部用 TryGetValue+索引器赋值),
				// 避免对同一 key 重入 GetOrAdd(ConcurrentDictionary 文档禁止 valueFactory 递归调用)
				var calcTree = AlgorithmEngineHelper.ParseCalculate(tree.Text);
				return CreateCalculate(calcTree);
			}

			var leftFunc = CreateCondition(tree.Nodes[0]);
			var rightFunc = CreateCondition(tree.Nodes[1]);
			if(tree.Type == Algorithm.Enums.ConditionTreeType.And) {
				return AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
			} else if(tree.Type == Algorithm.Enums.ConditionTreeType.Or) {
				return AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
			}
			throw new Exception(tree.ErrorMessage);
		}

	}
}
