using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_HMACSHA256 : Function_2
	{
		public Function_HMACSHA256(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "HmacSHA256";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }
			try {
				var t = GetHmacSha256String(Encoding.UTF8.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
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
