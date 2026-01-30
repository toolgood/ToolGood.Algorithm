using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 条件树
	/// </summary>
	public sealed class ConditionTree
	{
		/// <summary>
		/// 子节点
		/// </summary>
		public List<ConditionTree> Nodes { get; internal set; }

		/// <summary>
		/// 开始位置
		/// </summary>
		public int Start { get; internal set; }

		/// <summary>
		/// 结束位置
		/// </summary>
		public int End { get; internal set; }

		/// <summary>
		/// 类型
		/// </summary>
		public ConditionTreeType Type { get; internal set; }

		/// <summary>
		/// 条件
		/// </summary>
		public string ConditionString { get; internal set; }

		/// <summary>
		/// 出错信息
		/// </summary>
		public string ErrorMessage { get; internal set; }

		internal ConditionTree()
		{
		}
		 
	}
}