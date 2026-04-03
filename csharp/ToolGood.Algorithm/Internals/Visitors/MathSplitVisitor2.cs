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
			return context.expr().Accept(this);
		}

		public override CalculateTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			hasBracket = true;
			return context.expr().Accept(this);
		}
		public override CalculateTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var exprs = context.expr();
			var f1= exprs[0].Accept(this);
			var f2 = exprs[1].Accept(this);
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>(2) { f1,f2 },
				HasBracket = hasBracket,
			};
			hasBracket = false;
			var t = context.op.Type;
			if(t == math.mathLexer.OPMUL) {
				tree.Type = CalculateTreeType.Mul;
			} else if(t == math.mathLexer.OPDIV) {
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
			var exprs = context.expr();
			var f1 = exprs[0].Accept(this);
			var f2 = exprs[1].Accept(this);
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>(2) { f1, f2 },
				HasBracket = hasBracket,
			};
			hasBracket = false;
			var t = context.op.Type;
			if(t == math.mathLexer.OPADD) {
				tree.Type = CalculateTreeType.Add;
			} else if(t == math.mathLexer.OPSUB) {
				tree.Type = CalculateTreeType.Sub;
			} else {
				tree.Type = CalculateTreeType.Connect;
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
