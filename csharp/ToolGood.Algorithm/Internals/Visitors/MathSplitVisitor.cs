using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class MathSplitVisitor : AbstractParseTreeVisitor<ConditionTree>, ImathVisitor<ConditionTree>
	{
		public ConditionTree VisitProg(mathParser.ProgContext context)
		{
			return context.expr().Accept(this);
		}

		public ConditionTree VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			var tree = new ConditionTree {
				Nodes = new List<ConditionTree>()
			};
			var t = context.op.Text;
			if(CharUtil.Equals(t, "&&", "and")) {
				tree.Type = ConditionTreeType.And;
			} else {
				tree.Type = ConditionTreeType.Or;
			}
			var exprs = context.expr();

			tree.Nodes.Add(exprs[0].Accept(this));
			tree.Nodes.Add(exprs[1].Accept(this));
			tree.Start = context.Start.StartIndex;
			tree.End = context.Stop.StopIndex;
			tree.ConditionString = context.GetText();
			return tree;
		}

		public ConditionTree VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return context.expr().Accept(this);
		}

		public ConditionTree Visit_fun(ParserRuleContext context)
		{
			var tree = new ConditionTree {
				Start = context.Start.StartIndex,
				End = context.Stop.StopIndex,
				ConditionString = context.GetText()
			};
			return tree;
		}
		public ConditionTree VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitArray_fun(mathParser.Array_funContext context)
		{
			return Visit_fun(context);
		}
		 
		public ConditionTree VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitNum(mathParser.NumContext context)
		{
			return Visit_fun(context);
		}


		public ConditionTree VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitJudge_fun(mathParser.Judge_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitNOT_fun(mathParser.NOT_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitNUM_fun(mathParser.NUM_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return Visit_fun(context);
		}

		public ConditionTree VisitIF_fun(mathParser.IF_funContext context)
		{
			return Visit_fun(context);
		}
	}
}