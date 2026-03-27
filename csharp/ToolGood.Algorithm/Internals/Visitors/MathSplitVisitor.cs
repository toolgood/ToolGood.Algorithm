using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
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

		public override ConditionTree VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			var tree = new ConditionTree {
				Nodes = new List<ConditionTree>(2),
				HasBracket = hasBracket,
			};
			hasBracket = false;
			var t = context.op.Type;
			if(t == mathLexer.OPAND) {
				tree.Type = ConditionTreeType.And;
			} else {
				tree.Type = ConditionTreeType.Or;
			}
			var exprs = context.expr();

			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
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