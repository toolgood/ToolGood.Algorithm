using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_TEXTTOBASE64 : Function_1
	{
		public Function_TEXTTOBASE64(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "TextToBase64", 1);
			if(args1.IsError) { return args1; }
			try {
				var bytes = Encoding.UTF8.GetBytes(args1.TextValue);
				var t = Base64.ToBase64String(bytes);
				return Operand.Create(t);
			} catch(Exception) { }
			return Operand.Error("Function '{0}' is error!", "TextToBase64");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "TextToBase64");
		}
	}


}
