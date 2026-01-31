using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_BASE64TOTEXT : Function_2
	{
		public Function_BASE64TOTEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 1); if(args1.IsError) return args1; }
			try {
				Encoding encoding;
				if(func2 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 2); if(args2.IsError) return args2; }
					encoding = Encoding.GetEncoding(args2.TextValue);
				}
				var t = encoding.GetString(Base64.FromBase64String(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) { }
			return Operand.Error("Function '{0}' is error!", "Base64ToText");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Base64ToText");
		}
	}


}
