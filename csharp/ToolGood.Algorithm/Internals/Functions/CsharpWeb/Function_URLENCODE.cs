using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_URLENCODE : Function_1
	{
		public Function_URLENCODE(FunctionBase[] func1) : base(func1)
		{
		}

		public override string Name => "UrlEncode";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.UrlEncode(s);
			return Operand.Create(r);
		}

	}


}
