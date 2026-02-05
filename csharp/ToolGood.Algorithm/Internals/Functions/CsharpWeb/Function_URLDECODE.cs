using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_URLDECODE : Function_1
	{
		public Function_URLDECODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "UrlDecode";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "UrlDecode", 1);
			if(args1.IsError) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.UrlDecode(s);
			return Operand.Create(r);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "UrlDecode");
		}
	}


}
