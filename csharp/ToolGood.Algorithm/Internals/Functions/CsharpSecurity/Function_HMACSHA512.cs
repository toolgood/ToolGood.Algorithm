using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_HMACSHA512 : Function_3
	{
		public Function_HMACSHA512(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 1); if(args1.IsError) return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 2); if(args2.IsError) return args2; }
			try {
				Encoding encoding;
				if(func3 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args3 = func3.Evaluate(work, tempParameter); if(args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 3); if(args3.IsError) return args3; }
					encoding = Encoding.GetEncoding(args3.TextValue);
				}
				var t = Hash.GetHmacSha512String(encoding.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function 'HmacSHA512' is error!" + ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "HmacSHA512");
		}
	}


}
