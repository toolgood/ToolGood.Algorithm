using System.Collections.Generic;
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
		#region ConditionTree

		/// <summary>
		/// 只有 And / Or 会拆分成节点，其余整体作为文本叶子；
		/// 括号本身不产生节点，而是标记被包裹节点/叶子的 <see cref="ConditionTree.HasBracket"/>。
		/// </summary>
		internal static ConditionTree ToConditionTree(string source, FastNode node)
		{
			ConditionTreeType type;
			switch(node.Kind) {
				case FastNodeKind.Bracket: {
					var inner = ToConditionTree(source, node.Children[0]);
					inner.HasBracket = true;
					return inner;
				}
				case FastNodeKind.And: type = ConditionTreeType.And; break;
				case FastNodeKind.Or: type = ConditionTreeType.Or; break;
				default:
					var leaf = new ConditionTree { Start = node.Start, End = node.End };
					leaf.SetSource(source);
					return leaf;
			}

			var tree = new ConditionTree { Start = node.Start, End = node.End, Type = type };
			tree.SetSource(source);
			tree.Nodes = new List<ConditionTree>(2) {
				ToConditionTree(source, node.Children[0]),
				ToConditionTree(source, node.Children[1]),
			};
			return tree;
		}

		#endregion

		#region CalculateTree

		internal static CalculateTree ToCalculateTree(string source, FastNode node)
		{
			switch(node.Kind) {
				case FastNodeKind.Bracket: {
					var inner = ToCalculateTree(source, node.Children[0]);
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
						ToCalculateTree(source, node.Children[0]),
						ToCalculateTree(source, node.Children[1]),
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
		/// </summary>
		internal static void CollectDiyNames(FastNode node, List<DiyNameKeyInfo> parameters, List<DiyNameKeyInfo> functions)
		{
			if(node.Kind == FastNodeKind.Parameter) {
				parameters.Add(new DiyNameKeyInfo { Name = node.Text, Start = node.Start, End = node.End });
			} else if(node.Kind == FastNodeKind.Call && node.Type == FastToken.PARAMETER) {
				functions.Add(new DiyNameKeyInfo { Name = node.Text, Start = node.NameStart, End = node.NameEnd });
			}
			var children = node.Children;
			for(int i = 0; i < children.Length; i++) {
				CollectDiyNames(children[i], parameters, functions);
			}
		}

		#endregion
	}
}
