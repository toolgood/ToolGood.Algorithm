﻿using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

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
		/// 文本
		/// </summary>
		public string Text { get; internal set; }

		/// <summary>
		/// 外面是否有括号
		/// </summary>
		public bool HasBracket {  get; internal set; }

		/// <summary>
		/// 出错信息
		/// </summary>
		public string ErrorMessage { get; internal set; }

		internal ConditionTree()
		{
		}
		 
	}
}