using Antlr4.Runtime.Tree;
using System;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class DiyNameVisitor : AbstractParseTreeVisitor<Object>, ImathVisitor<Object>
	{
		internal DiyNameInfo diy = new DiyNameInfo();

		public object VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			var t = CharUtil.StandardString(node.GetText());
			if(t == "E") {
				return VisitChildren(context);
			} else if(t == "PI") {
				return VisitChildren(context);
			} else if(t == "TRUE" || t == "YES" || t == "是" || t == "有") {
				return VisitChildren(context);
			} else if(t == "FALSE" || t == "NO" || t == "不是" || t == "否" || t == "没有" || t == "无") {
				return VisitChildren(context);
			} else if(t == "ALGORITHMVERSION" || t == "ENGINEVERSION") {
				return VisitChildren(context);
			}
			diy.Parameters.Add(new DiyNameKeyInfo() { Name = node.GetText(), Start = node.Symbol.StartIndex, End = node.Symbol.StopIndex });
			return VisitChildren(context);
		}
		public object VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			var t = CharUtil.StandardString(context.f.Text);
			if(t == "E") {
				return VisitChildren(context);
			} else if(t == "PI") {
				return VisitChildren(context);
			} else if(t == "TRUE" || t == "YES" || t == "是" || t == "有") {
				return VisitChildren(context);
			} else if(t == "FALSE" || t == "NO" || t == "不是" || t == "否" || t == "没有" || t == "无") {
				return VisitChildren(context);
			} else if(t == "ALGORITHMVERSION" || t == "ENGINEVERSION") {
				return VisitChildren(context);
			} else if(t == "RAND") {
				return VisitChildren(context);
			} else if(t == "GUID") {
				return VisitChildren(context);
			} else if(t == "NOW") {
				return VisitChildren(context);
			} else if(t == "TODAY") {
				return VisitChildren(context);
			}
			if(MathFunctionVisitor.funcDict.ContainsKey(t)) {
				return VisitChildren(context);
			}
			var node = context.f;
			diy.Functions.Add(new DiyNameKeyInfo() { Name = node.Text, Start = node.StartIndex, End = node.StopIndex });
			return VisitChildren(context);
		}

		public object VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			if(node != null) {
				diy.Parameters.Add(new DiyNameKeyInfo() { Name = node.GetText(), Start = node.Symbol.StartIndex, End = node.Symbol.StopIndex });
			}
			return VisitChildren(context);
		}

		public object VisitAddSub_fun(mathParser.AddSub_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitAndOr_fun(mathParser.AndOr_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitArray_fun(mathParser.Array_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitNum(mathParser.NumContext context)
		{
			return VisitChildren(context);
		}

		public object VisitArrayJson(mathParser.ArrayJsonContext context)
		{
			return VisitChildren(context);
		}

		public object VisitProg(mathParser.ProgContext context)
		{
			return VisitChildren(context);
		}

		public object VisitJudge_fun(mathParser.Judge_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitPercentage_fun(mathParser.Percentage_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitSTRING_fun(mathParser.STRING_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitMulDiv_fun(mathParser.MulDiv_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitNOT_fun(mathParser.NOT_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitBracket_fun(mathParser.Bracket_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitNUM_fun(mathParser.NUM_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitNULL_fun(mathParser.NULL_funContext context)
		{
			return VisitChildren(context);
		}

		public object VisitIF_fun(mathParser.IF_funContext context)
		{
			return VisitChildren(context);
		}
	}
}