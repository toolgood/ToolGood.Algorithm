using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_HTMLDECODE : Function_1
	{
		public Function_HTMLDECODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "HtmlDecode";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "HtmlDecode", 1);
			if(args1.IsError) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.HtmlDecode(s);
			return Operand.Create(r);
		}

	}


}
