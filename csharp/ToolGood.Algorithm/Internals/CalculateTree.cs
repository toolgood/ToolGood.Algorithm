using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals
{
	/// <summary>
	/// 计算树
	/// </summary>
	public sealed class CalculateTree
	{
		/// <summary>
		/// 子节点
		/// </summary>
		public List<CalculateTree> Nodes { get; internal set; }
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
		public CalculateTreeType Type { get; internal set; }

		/// <summary>
		/// 条件
		/// </summary>
		public string ConditionString { get; internal set; }

		/// <summary>
		/// 出错信息
		/// </summary>
		public string ErrorMessage { get; internal set; }

		internal CalculateTree()
		{
		}

		/// <summary>
		/// 解析
		/// </summary>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static CalculateTree Parse(string exp)
		{
			var tree = new CalculateTree();
			if(string.IsNullOrWhiteSpace(exp)) {
				tree.Type = CalculateTreeType.Error;
				tree.ErrorMessage = "exp is null";
				return tree;
			}
			try {
				AntlrErrorTextWriter antlrErrorTextWriter = new AntlrErrorTextWriter();
				var stream = new AntlrCharStream(new AntlrInputStream(exp));
				var lexer = new mathLexer(stream, Console.Out, antlrErrorTextWriter);
				var tokens = new CommonTokenStream(lexer);
				var parser = new mathParser(tokens, Console.Out, antlrErrorTextWriter);

				var context = parser.prog();
				if(antlrErrorTextWriter.IsError) {
					tree.Type = CalculateTreeType.Error;
					tree.ErrorMessage = antlrErrorTextWriter.ErrorMsg;
					return tree;
				}
				var visitor = new MathSplitVisitor2();
				return visitor.Visit(context);
			} catch(Exception ex) {
				tree.Type = CalculateTreeType.Error;
				tree.ErrorMessage = ex.Message;
			}
			return tree;
		}
	}
}
