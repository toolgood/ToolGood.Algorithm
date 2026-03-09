using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal sealed class Function_BASE64URLTOTEXT : Function_1
	{
		public Function_BASE64URLTOTEXT(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Base64UrlToText";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var base64Url = args1.TextValue.Replace('-', '+').Replace('_', '/');
				var padding = 4 - (base64Url.Length % 4);
				if (padding < 4) {
					base64Url += new string('=', padding);
				}
				var t = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url));
				return Operand.Create(t);
			} catch(Exception) {
				return ParameterError(1);
			}
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

	}


}
