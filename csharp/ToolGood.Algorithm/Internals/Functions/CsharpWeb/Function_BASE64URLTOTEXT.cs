using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_BASE64URLTOTEXT : Function_1
	{
		public Function_BASE64URLTOTEXT(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "Base64urlToText", 1);
			if(args1.IsError) { return args1; }
			try {
				var t = Encoding.UTF8.GetString(Base64.FromBase64ForUrlString(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) { }
			return Operand.Error("Function '{0}' is error!", "Base64urlToText");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Base64urlToText");
		}
	}


}
