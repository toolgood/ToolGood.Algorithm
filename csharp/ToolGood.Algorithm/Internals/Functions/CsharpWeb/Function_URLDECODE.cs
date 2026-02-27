using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal sealed class Function_URLDECODE : Function_1
	{
		public Function_URLDECODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "UrlDecode";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.UrlDecode(s);
			return Operand.Create(r);
		}

	}


}
