using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal class Function_BASE64TOTEXT : Function_1
	{
		public Function_BASE64TOTEXT(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Base64ToText";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			try {
				var t = Encoding.UTF8.GetString(Convert.FromBase64String(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
		}

	}


}
