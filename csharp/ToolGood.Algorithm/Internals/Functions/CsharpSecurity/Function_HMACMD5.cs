using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_HMACMD5 : Function_3
	{
		public Function_HMACMD5(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 1); if(args1.IsError) return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 2); if(args2.IsError) return args2; }
			try {
				Encoding encoding;
				if(func3 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args3 = func3.Evaluate(work, tempParameter); if(args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 3); if(args3.IsError) return args3; }
					encoding = Encoding.GetEncoding(args3.TextValue);
				}
				var t = Hash.GetHmacMd5String(encoding.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function 'HmacMD5' is error!" + ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "HmacMD5");
		}
	}


}
