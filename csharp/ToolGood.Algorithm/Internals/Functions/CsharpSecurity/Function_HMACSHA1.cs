using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_HMACSHA1 : Function_3
	{
		public Function_HMACSHA1(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 1); if(args1.IsError) return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 2); if(args2.IsError) return args2; }
			try {
				Encoding encoding;
				if(func3 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args3 = func3.Evaluate(work, tempParameter); if(args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 3); if(args3.IsError) return args3; }
					encoding = Encoding.GetEncoding(args3.TextValue);
				}
				var t = GetHmacSha1String(encoding.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function '{0}' is error!{1}", "HmacSHA1", ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "HmacSHA1");
		}
		private string GetHmacSha1String(byte[] buffer, string secret)
		{
			byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
			using(var hmacsha256 = new HMACSHA1(keyByte)) {
				byte[] hashmessage = hmacsha256.ComputeHash(buffer);
#if NETSTANDARD2_1
				return BitConverter.ToString(hashmessage).Replace("-", "");
#else
                return Convert.ToHexString(hashmessage);
#endif
			}
		}
	}


}
