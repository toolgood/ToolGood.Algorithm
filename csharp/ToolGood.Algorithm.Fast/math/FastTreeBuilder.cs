using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 语法树到 <see cref="ConditionTree"/> / <see cref="CalculateTree"/> 的转换，
	/// 以及自定义名称收集。等价于原来的 MathSplitVisitor / MathSplitVisitor2 / DiyNameVisitor。
	/// </summary>
	internal static class FastTreeBuilder
	{
		/// <summary>条件树/计算树允许的最大嵌套层数（逻辑上限）</summary>
		private const int MaxTreeDepth = 2000;

		/// <summary>每隔多少层做一次剩余栈探测</summary>
		private const int StackProbeInterval = 16;

		#region ConditionTree

		/// <summary>
		/// 只有 And / Or 会拆分成节点，其余整体作为文本叶子；
		/// 括号本身不产生节点，而是标记被包裹节点/叶子的 <see cref="ConditionTree.HasBracket"/>。
		/// </summary>
		internal static ConditionTree ToConditionTree(string source, FastNode[] nodes, int root)
		{
			return BuildCondition(source, nodes, nodes[root], 0);
		}

		private static ConditionTree BuildCondition(string source, FastNode[] nodes, FastNode node, int depth)
		{
			if(++depth > MaxTreeDepth) {
				throw new FormatException($"Condition is nested too deeply (more than {MaxTreeDepth} levels).");
			}
			if((depth & (StackProbeInterval - 1)) == 0 && !RuntimeHelpers.TryEnsureSufficientExecutionStack()) {
				throw new FormatException($"Condition is nested too deeply for the current thread stack (stopped at level {depth}).");
			}
			ConditionTreeType type;
			switch(node.Kind) {
				case FastNodeKind.Bracket: {
					var inner = BuildCondition(source, nodes, nodes[node.GetChild(0)], depth);
					inner.HasBracket = true;
					return inner;
				}
				case FastNodeKind.And: type = ConditionTreeType.And; break;
				case FastNodeKind.Or: type = ConditionTreeType.Or; break;
				default: {
					var leaf = new ConditionTree { Start = node.Start, End = node.End };
					leaf.SetSource(source);
					return leaf;
				}
			}

			var tree = new ConditionTree { Start = node.Start, End = node.End, Type = type };
			tree.SetSource(source);
			tree.Nodes = new List<ConditionTree>(2) {
				BuildCondition(source, nodes, nodes[node.GetChild(0)], depth),
				BuildCondition(source, nodes, nodes[node.GetChild(1)], depth),
			};
			return tree;
		}

		#endregion

		#region CalculateTree

		internal static CalculateTree ToCalculateTree(string source, FastNode[] nodes, int root)
		{
			return BuildCalculate(source, nodes, nodes[root], 0);
		}

		private static CalculateTree BuildCalculate(string source, FastNode[] nodes, FastNode node, int depth)
		{
			if(++depth > MaxTreeDepth) {
				throw new FormatException($"Expression is nested too deeply (more than {MaxTreeDepth} levels).");
			}
			if((depth & (StackProbeInterval - 1)) == 0 && !RuntimeHelpers.TryEnsureSufficientExecutionStack()) {
				throw new FormatException($"Expression is nested too deeply for the current thread stack (stopped at level {depth}).");
			}
			switch(node.Kind) {
				case FastNodeKind.Bracket: {
					var inner = BuildCalculate(source, nodes, nodes[node.GetChild(0)], depth);
					inner.HasBracket = true;
					return inner;
				}
				case FastNodeKind.Mul:
				case FastNodeKind.Div:
				case FastNodeKind.Mod:
				case FastNodeKind.Add:
				case FastNodeKind.Sub:
				case FastNodeKind.Concat:
				case FastNodeKind.GreaterThan:
				case FastNodeKind.GreaterOrEqual:
				case FastNodeKind.LessThan:
				case FastNodeKind.LessOrEqual:
				case FastNodeKind.Equal:
				case FastNodeKind.NotEqual:
				case FastNodeKind.And:
				case FastNodeKind.Or: {
					var tree = new CalculateTree {
						Start = node.Start,
						End = node.End,
						Type = ToCalculateTreeType(node.Kind),
					};
					tree.SetSource(source);
					tree.Nodes = new List<CalculateTree>(2) {
						BuildCalculate(source, nodes, nodes[node.GetChild(0)], depth),
						BuildCalculate(source, nodes, nodes[node.GetChild(1)], depth),
					};
					return tree;
				}
				default: {
					var leaf = new CalculateTree { Start = node.Start, End = node.End };
					leaf.SetSource(source);
					return leaf;
				}
			}
		}

		private static CalculateTreeType ToCalculateTreeType(FastNodeKind kind)
		{
			switch(kind) {
				case FastNodeKind.Mul: return CalculateTreeType.Mul;
				case FastNodeKind.Div: return CalculateTreeType.Div;
				case FastNodeKind.Mod: return CalculateTreeType.Mod;
				case FastNodeKind.Add: return CalculateTreeType.Add;
				case FastNodeKind.Sub: return CalculateTreeType.Sub;
				case FastNodeKind.Concat: return CalculateTreeType.Connect;
				case FastNodeKind.GreaterThan: return CalculateTreeType.OpGt;
				case FastNodeKind.GreaterOrEqual: return CalculateTreeType.OpGe;
				case FastNodeKind.LessThan: return CalculateTreeType.OpLt;
				case FastNodeKind.LessOrEqual: return CalculateTreeType.OpLe;
				case FastNodeKind.Equal: return CalculateTreeType.OpEq;
				case FastNodeKind.NotEqual: return CalculateTreeType.OpNe;
				case FastNodeKind.And: return CalculateTreeType.And;
				default: return CalculateTreeType.Or;
			}
		}

		#endregion

		#region DiyName

		/// <summary>
		/// 收集自定义参数与自定义函数。
		/// 规则与 DiyNameVisitor 一致：PARAMETER 记为参数，自定义函数名记为函数，
		/// <c>expr.关键字</c> 形式的关键字不计入。
		/// 采用显式栈遍历，长链公式不会把栈打爆。
		/// </summary>
		internal static void CollectDiyNames(FastNode[] nodes, int root, List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions)
		{
			var stack = ArrayPool<int>.Shared.Rent(16);
			try {
				var top = 0;
				stack[top++] = root;
				while(top > 0) {
					var node = nodes[stack[--top]];
					if(node.Kind == FastNodeKind.Parameter) {
						parameters.Add(new DiyNameKeyInfo { Name = node.Text, Start = node.Start, End = node.End });
					} else if(node.Kind == FastNodeKind.Call && node.Type == FastToken.PARAMETER) {
						functions.Add(new DiyNameKeyInfo { Name = node.Text, Start = node.NameStart, End = node.NameStart + node.Text.Length - 1 });
					}
					// 逆序压栈，保证子节点按原文从左到右的顺序被访问
					for(int i = node.ChildCount - 1; i >= 0; i--) {
						if(top == stack.Length) {
							var bigger = ArrayPool<int>.Shared.Rent(stack.Length * 2);
							Array.Copy(stack, bigger, top);
							ArrayPool<int>.Shared.Return(stack);
							stack = bigger;
						}
						stack[top++] = node.GetChild(i);
					}
				}
			} finally {
				ArrayPool<int>.Shared.Return(stack);
			}
		}

		#endregion
	}
}
