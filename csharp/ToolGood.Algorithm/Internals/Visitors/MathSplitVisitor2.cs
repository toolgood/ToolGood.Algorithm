using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal class MathSplitVisitor2 : AbstractParseTreeVisitor<CalculateTree>, ImathVisitor<CalculateTree>
	{
		public CalculateTree VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept(this);
		}

		public CalculateTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}
		public CalculateTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>()
			};
			var exprs = context.expr();
			var t = context.op.Text;
			if(CharUtil.Equals(t, '*')) {
				tree.Type = CalculateTreeType.Mul;
			} else if(CharUtil.Equals(t, '/')) {
				tree.Type = CalculateTreeType.Div;
			} else {
				tree.Type = CalculateTreeType.Mod;
			}
			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.ConditionString = context.GetText();
			return tree;
		}
		public CalculateTree VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			var tree = new CalculateTree {
				Nodes = new List<CalculateTree>()
			};
			var exprs = context.expr();
			var t = context.op.Text;
			if(CharUtil.Equals(t, '+')) {
				tree.Type = CalculateTreeType.Add;
			} else if(CharUtil.Equals(t, '-')) {
				tree.Type = CalculateTreeType.Sub;
			} else {
				tree.Type = CalculateTreeType.Connect;
			}
			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.ConditionString = context.GetText();
			return tree;
		}


		public CalculateTree Visit_fun(ParserRuleContext context)
		{
			var tree = new CalculateTree {
				Start = context.Start.StartIndex,
				End = context.Stop.StopIndex,
				ConditionString = context.GetText()
			};
			return tree;
		}
		 

		public CalculateTree VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			return Visit_fun(context);
		}
 
		public CalculateTree VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitArray_fun(mathParser.Array_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNum(mathParser.NumContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNUM_fun(mathParser.NUM_funContext context)
		{
			return Visit_fun(context);
		}
 

		public CalculateTree VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			return Visit_fun(context);
		}
 
		public CalculateTree VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			return Visit_fun(context);
		}

	 
		public CalculateTree VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitJudge_fun(mathParser.Judge_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitNOT_fun(mathParser.NOT_funContext context)
		{
			return Visit_fun(context);
		}

		public CalculateTree VisitIF_fun(mathParser.IF_funContext context)
		{
			return Visit_fun(context);
		}
	}
}
