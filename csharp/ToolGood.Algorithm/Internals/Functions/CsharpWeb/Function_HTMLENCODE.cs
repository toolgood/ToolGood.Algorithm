using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal sealed class Function_HTMLENCODE : Function_1
	{
		public Function_HTMLENCODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "HtmlEncode";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var s = args1.TextValue;
			var r = System.Web.HttpUtility.HtmlEncode(s);
			return Operand.Create(r);
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}
	}


}
