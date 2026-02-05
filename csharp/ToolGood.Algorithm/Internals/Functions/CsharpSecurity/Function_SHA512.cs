using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_SHA512 : Function_1
	{
		public Function_SHA512(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "SHA512";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }
			try {
				var t = GetSha512String(Encoding.UTF8.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
		}


		private string GetSha512String(byte[] buffer)
		{
#if NETSTANDARD2_1
			SHA512 sha512 = SHA512.Create();
			byte[] retVal = sha512.ComputeHash(buffer); //计算指定Stream 对象的哈希值
			sha512.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
#else
            var retVal = SHA512.HashData(buffer);
            return Convert.ToHexString(retVal);
#endif
		}
	}


}
