using Antlr4.Runtime.Tree;
using System;
using ToolGood.Algorithm.math;

namespace ToolGood.Algorithm.Internals.Visitors
{
	internal sealed class DiyNameVisitor : mathBaseVisitor<Object>
	{
		internal DiyNameInfo diy = new DiyNameInfo();

		public override object VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			if(node != null) {
				diy.Parameters.Add(new DiyNameKeyInfo() { Name = node.GetText(), Start = node.Symbol.StartIndex, End = node.Symbol.StopIndex });
			}
			return VisitChildren(context);
		}
		public override object VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			if(node != null) {
				diy.Parameters.Add(new DiyNameKeyInfo() { Name = node.GetText(), Start = node.Symbol.StartIndex, End = node.Symbol.StopIndex });
			}
			return VisitChildren(context);
		}
		public override object VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
		{
			ITerminalNode node = context.PARAMETER();
			diy.Functions.Add(new DiyNameKeyInfo() { Name = node.GetText(), Start = node.Symbol.StartIndex, End = node.Symbol.StopIndex });
			return VisitChildren(context);
		}
		 
	}
}