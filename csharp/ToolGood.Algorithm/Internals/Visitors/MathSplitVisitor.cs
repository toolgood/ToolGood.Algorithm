using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class MathSplitVisitor : mathBaseVisitor<ConditionTree>
	{
		private bool hasBracket = false;

		public override ConditionTree VisitProg(mathParser.ProgContext context)
		{
			hasBracket = false;
			return context.expr().Accept(this);
		}
		public override ConditionTree VisitOr_fun(mathParser.Or_funContext context)
		{
			var exprs = context.expr();
			var f1 = exprs[0].Accept(this);
			var f2 = exprs[1].Accept(this);
			var tree = new ConditionTree {
				Nodes = new List<ConditionTree>(2) { f1, f2 },
				HasBracket = hasBracket,
			};
			hasBracket = false;
			tree.Type = ConditionTreeType.Or;
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();

			return tree;
		}

		public override ConditionTree VisitAnd_fun(mathParser.And_funContext context)
		{
			var exprs = context.expr();
			var f1 = exprs[0].Accept(this);
			var f2 = exprs[1].Accept(this);
			var tree = new ConditionTree {
				Nodes = new List<ConditionTree>(2) { f1, f2 },
				HasBracket = hasBracket,
			};
			hasBracket = false;
			tree.Type = ConditionTreeType.And;
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();

			return tree;
		}

		public override ConditionTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			hasBracket = true;
			return context.expr().Accept(this);
		}

		public override ConditionTree VisitChildren(IRuleNode node)
		{
			var context = node as ParserRuleContext;
			var tree = new ConditionTree {
				Start = context.Start.StartIndex,
				End = context.Stop.StopIndex,
				Text = context.GetText()
			};
			return tree;
		}

	}
}