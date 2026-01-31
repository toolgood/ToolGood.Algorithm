using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_TEXTTOBASE64URL : Function_2
	{
		public Function_TEXTTOBASE64URL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TextToBase64url", 1); if(args1.IsError) return args1; }
			try {
				Encoding encoding;
				if(func2 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TextToBase64url", 2); if(args2.IsError) return args2; }
					encoding = Encoding.GetEncoding(args2.TextValue);
				}
				var bytes = encoding.GetBytes(args1.TextValue);
				var t = Base64.ToBase64ForUrlString(bytes);
				return Operand.Create(t);
			} catch(Exception) { }
			return Operand.Error("Function 'TextToBase64url' is error!");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "TextToBase64url");
		}
	}


}
