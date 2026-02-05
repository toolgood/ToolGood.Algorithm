using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_TEXTTOBASE64URL : Function_1
	{
		public Function_TEXTTOBASE64URL(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "TextToBase64Url";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "TextToBase64url", 1);
			if(args1.IsError) { return args1; }
			try {
				var bytes = Encoding.UTF8.GetBytes(args1.TextValue);
				var t = Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
				return Operand.Create(t);
			} catch(Exception) { }
			return Operand.Error("Function '{0}' is error!", "TextToBase64url");
		}

	}


}
