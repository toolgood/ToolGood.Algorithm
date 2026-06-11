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
		private readonly ConcurrentDictionary<string, FunctionBase> functionCache = new ConcurrentDictionary<string, FunctionBase>();
		 
		/// <summary>
		/// 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">函数表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseWithCache(string funExp)
		{
			return functionCache.GetOrAdd(funExp, key => {
				var tree = AlgorithmEngineHelper.ParseCalculate(key);
				return CreateCalculate(tree);
			});
		}
		private FunctionBase CreateCalculate(CalculateTree tree)
		{
			if(functionCache.TryGetValue(tree.Text, out FunctionBase value)) { return value; }
			if(tree.Type == Algorithm.Enums.CalculateTreeType.String) { return AlgorithmEngineHelper.ParseFormula(tree.Text); }
			if(tree.Type == Algorithm.Enums.CalculateTreeType.Error) { throw new Exception(tree.ErrorMessage); }

			var leftFunc = CreateCalculate(tree.Nodes[0]);
			var rightFunc = CreateCalculate(tree.Nodes[1]);
			var fun = AlgorithmEngineHelper.CombineCalculate(leftFunc, (Algorithm.Enums.CombineCalculateType)(byte)tree.Type, rightFunc);
			functionCache[tree.Text] = fun;
			return fun;
		}

		/// <summary>
		/// 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
		/// </summary>
		/// <param name="funExp">条件表达式字符串</param>
		/// <returns>解析后的函数对象</returns>
		public FunctionBase ParseConditionWithCache(string funExp)
		{
			return functionCache.GetOrAdd(funExp, key => {
				var tree = AlgorithmEngineHelper.ParseCondition(key);
				return CreateCondition(tree);
			});
		}
		private FunctionBase CreateCondition(ConditionTree tree)
		{
			if(functionCache.TryGetValue(tree.Text, out FunctionBase value)) { return value; }
			if(tree.Type == Algorithm.Enums.ConditionTreeType.String) { return ParseWithCache(tree.Text); }

			var leftFunc = CreateCondition(tree.Nodes[0]);
			var rightFunc = CreateCondition(tree.Nodes[1]);
			if(tree.Type == Algorithm.Enums.ConditionTreeType.And) {
				var fun = AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
				functionCache[tree.Text] = fun;
				return fun;
			} else if(tree.Type == Algorithm.Enums.ConditionTreeType.Or) {
				var fun = AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
				functionCache[tree.Text] = fun;
				return fun;
			}
			throw new Exception(tree.ErrorMessage);
		}
	 
	}
}
