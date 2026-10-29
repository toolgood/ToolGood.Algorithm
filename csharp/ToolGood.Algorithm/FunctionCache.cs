using System;
using System.Collections.Concurrent;
using System.Threading;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 函数缓存类，使用 ConcurrentDictionary 实现线程安全的函数缓存。
	/// <para>
	/// 计算表达式、条件表达式、自定义名称三份缓存用的是同一套流程：
	/// 「查缓存 → 未命中则解析 → 写入 → 按写入次数分摊容量检查」。
	/// 原先这段流程被复制了三遍（各带一个写计数字段），此处收敛为 <see cref="CacheSlot{TValue}"/>。
	/// </para>
	/// </summary>
	public sealed class FunctionCache : IFunctionCache
	{
		private readonly CacheSlot<FunctionBase> calculateCache = new CacheSlot<FunctionBase>();
		private readonly CacheSlot<FunctionBase> conditionCache = new CacheSlot<FunctionBase>();
		private readonly CacheSlot<DiyNameInfo> diyNameCache = new CacheSlot<DiyNameInfo>();

		/// <summary>
		/// 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="exp">表达式字符串</param>
		/// <returns>自定义名称信息</returns>
		public DiyNameInfo GetDiyNamesWithCache(string exp)
		{
			return diyNameCache.GetOrAdd(exp, AlgorithmEngineHelper.GetDiyNames);
		}

		/// <summary>
		/// 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">函数表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseWithCache(string funExp)
		{
			return calculateCache.GetOrAdd(funExp, exp => CreateCalculate(AlgorithmEngineHelper.ParseCalculate(exp)));
		}

		private FunctionBase CreateCalculate(CalculateTree tree)
		{
			if(tree.Type == CalculateTreeType.String) {
				// 仅叶子节点需要字符串内容；中间节点不再以 Text(子串)作为缓存 key，避免 Substring 产生 O(n²) 子串驻留
				return calculateCache.GetOrAdd(tree.Text, AlgorithmEngineHelper.ParseFormula);
			}
			if(tree.Type == CalculateTreeType.Error) { throw new Exception(tree.ErrorMessage); }

			var leftFunc = CreateCalculate(tree.Nodes[0]);
			var rightFunc = CreateCalculate(tree.Nodes[1]);
			return AlgorithmEngineHelper.CombineCalculate(leftFunc, (CombineCalculateType)(byte)tree.Type, rightFunc);
		}

		/// <summary>
		/// 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">条件表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseConditionWithCache(string funExp)
		{
			return conditionCache.GetOrAdd(funExp, exp => CreateCondition(AlgorithmEngineHelper.ParseCondition(exp)));
		}

		private FunctionBase CreateCondition(ConditionTree tree)
		{
			if(tree.Type == ConditionTreeType.String) {
				// 直接走计算树路径，避免对同一 key 重入 GetOrAdd（ConcurrentDictionary 文档禁止 valueFactory 递归调用）
				var calcTree = AlgorithmEngineHelper.ParseCalculate(tree.Text);
				return CreateCalculate(calcTree);
			}

			var leftFunc = CreateCondition(tree.Nodes[0]);
			var rightFunc = CreateCondition(tree.Nodes[1]);
			if(tree.Type == ConditionTreeType.And) {
				return AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
			} else if(tree.Type == ConditionTreeType.Or) {
				return AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
			}
			throw new Exception(tree.ErrorMessage);
		}

		/// <summary>
		/// 单个缓存槽：一个字典 + 一份写入计数 + 一套容量淘汰规则。
		/// </summary>
		private sealed class CacheSlot<TValue>
		{
			// 缓存条目软上限：动态拼接的公式串会让缓存无限增长（内存泄漏），超限淘汰一半。
			// 缓存仅用于免重复解析，被淘汰的条目下次命中时重新解析，不影响正确性
			private const int MaxCacheSize = 100_000;

			// 每隔多少次「新增」才检查一次容量。
			// ConcurrentDictionary.Count 需要遍历全部 stripe 加锁，10 万条时单次约 20 µs，
			// 命中路径上每次点击都被它拖慢（实测 153 ns → 21.6 µs），所以只能按写入次数分摊。
			private const int TrimCheckInterval = 1024;

			private readonly ConcurrentDictionary<string, TValue> _items = new ConcurrentDictionary<string, TValue>();
			private int _writes;

			/// <summary>
			/// 命中直接返回；未命中时调用 <paramref name="factory"/> 解析并写入。
			/// </summary>
			public TValue GetOrAdd(string key, Func<string, TValue> factory)
			{
				if(_items.TryGetValue(key, out var value)) { return value; }
				value = factory(key);
				if(_items.TryAdd(key, value)) { AfterEntryAdded(); }
				return value;
			}

			/// <summary>
			/// 仅在真正新增条目后调用（命中路径不调用），按写入次数分摊容量检查成本。
			/// </summary>
			private void AfterEntryAdded()
			{
				if(Interlocked.Increment(ref _writes) < TrimCheckInterval) { return; }
				// 只有抢到计数重置权的那个线程做淘汰，避免并发时重复遍历
				if(Interlocked.Exchange(ref _writes, 0) < TrimCheckInterval) { return; }
				Trim();
			}

			private void Trim()
			{
				// Count 只在这里取一次：下面按删除个数计数，不能每删一条再问一次 Count
				var count = _items.Count;
				if(count < MaxCacheSize) { return; }
				// 淘汰一半而不是全部清空：全清会让高频 key 与长尾 key 一起被踢掉，命中率塌缩
				var needRemove = count - MaxCacheSize / 2;
				foreach(var key in _items.Keys) {
					if(needRemove <= 0) { break; }
					if(_items.TryRemove(key, out _)) { needRemove--; }
				}
			}
		}
	}
}
