using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_HMACSHA256 : Function_2
	{
		public Function_HMACSHA256(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA256", 1); if(args1.IsError) return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA256", 2); if(args2.IsError) return args2; }
			try {
				var t = GetHmacSha256String(Encoding.UTF8.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function '{0}' is error!{1}", "HmacSHA256", ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "HmacSHA256");
		}
		private string GetHmacSha256String(byte[] buffer, string secret)
		{
			byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
			using(var hmacsha256 = new HMACSHA256(keyByte)) {
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
