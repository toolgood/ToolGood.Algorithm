using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_HTMLENCODE : Function_1
	{
		public Function_HTMLENCODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "HtmlEncode";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "HtmlEncode", 1);
			if(args1.IsError) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.HtmlEncode(s);
			return Operand.Create(r);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "HtmlEncode");
		}
	}


}
