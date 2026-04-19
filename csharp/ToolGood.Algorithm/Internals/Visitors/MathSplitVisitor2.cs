using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class MathSplitVisitor2 : mathBaseVisitor<CalculateTree>
	{
		private bool hasBracket = false;

		public override CalculateTree VisitProg(mathParser.ProgContext context)
		{
			hasBracket = false;
			return context.expr().Accept2(this);
		}

		public override CalculateTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			hasBracket = true;
			return context.expr().Accept2(this);
		}
		public override CalculateTree VisitOr_fun(mathParser.Or_funContext context)
		{
			var tree = new CalculateTree { HasBracket = hasBracket, };
			hasBracket = false;
			var exprs = context.expr();
			var f1 = exprs[0].Accept2(this);
			var f2 = exprs[1].Accept2(this);
			tree.Nodes = new List<CalculateTree>(2) { f1, f2 };

			tree.Type = CalculateTreeType.Or;
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();
			return tree;
		}
		public override CalculateTree VisitAnd_fun(mathParser.And_funContext context)
		{
			var tree = new CalculateTree { HasBracket = hasBracket, };
			hasBracket = false;
			var exprs = context.expr();
			var f1 = exprs[0].Accept2(this);
			var f2 = exprs[1].Accept2(this);
			tree.Nodes = new List<CalculateTree>(2) { f1, f2 };

			tree.Type = CalculateTreeType.And;
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();

			return tree;
		}
		public override CalculateTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var tree = new CalculateTree { HasBracket = hasBracket, };
			hasBracket = false;
			var exprs = context.expr();
			var f1 = exprs[0].Accept2(this);
			var f2 = exprs[1].Accept2(this);
			tree.Nodes = new List<CalculateTree>(2) { f1, f2 };

			var t = context.op.Type;
			if(t == math.mathLexer2.OPMUL) {
				tree.Type = CalculateTreeType.Mul;
			} else if(t == math.mathLexer2.OPDIV) {
				tree.Type = CalculateTreeType.Div;
			} else {
				tree.Type = CalculateTreeType.Mod;
			}
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();
			return tree;
		}
		public override CalculateTree VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var tree = new CalculateTree { HasBracket = hasBracket, };
			hasBracket = false;
			var exprs = context.expr();
			var f1 = exprs[0].Accept2(this);
			var f2 = exprs[1].Accept2(this);
			tree.Nodes = new List<CalculateTree>(2) { f1, f2 };

			var t = context.op.Type;
			if(t == math.mathLexer2.OPADD) {
				tree.Type = CalculateTreeType.Add;
			} else if(t == math.mathLexer2.OPSUB) {
				tree.Type = CalculateTreeType.Sub;
			} else {
				tree.Type = CalculateTreeType.Connect;
			}
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();
			return tree;
		}
		public override CalculateTree VisitJudge_fun(mathParser.Judge_funContext context)
		{
			var tree = new CalculateTree { HasBracket = hasBracket, };
			hasBracket = false;
			var exprs = context.expr();
			var f1 = exprs[0].Accept2(this);
			var f2 = exprs[1].Accept2(this);
			tree.Nodes = new List<CalculateTree>(2) { f1, f2 };

			var t = context.op.Type;
			if(t == math.mathLexer2.OPGE) {
				tree.Type = CalculateTreeType.OpGe;
			} else if(t == math.mathLexer2.OPLE) {
				tree.Type = CalculateTreeType.OpLe;
			} else if(t == math.mathLexer2.OPGT) {
				tree.Type = CalculateTreeType.OpGt;
			} else if(t == math.mathLexer2.OPLT) {
				tree.Type = CalculateTreeType.OpLt;
			} else if(t == math.mathLexer2.OPEQ) {
				tree.Type = CalculateTreeType.OpEq;
			} else {
				tree.Type = CalculateTreeType.OpNe;
			}
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.Text = context.GetText();
			return tree;
		}
		public override CalculateTree VisitChildren(IRuleNode node)
		{
			var context = node as ParserRuleContext;
			var tree = new CalculateTree {
				Start = context.Start.StartIndex,
				End = context.Stop.StopIndex,
				Text = context.GetText()
			};
			return tree;
		}
	}
}
