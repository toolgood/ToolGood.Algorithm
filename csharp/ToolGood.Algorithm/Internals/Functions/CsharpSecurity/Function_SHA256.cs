using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_SHA256 : Function_1
	{
		public Function_SHA256(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "SHA256";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }
			try {
				var t = GetSha256String(Encoding.UTF8.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
		}

		private string GetSha256String(byte[] buffer)
		{
#if NETSTANDARD2_1
			SHA256 sha512 = SHA256.Create();
			byte[] retVal = sha512.ComputeHash(buffer); //计算指定Stream 对象的哈希�?
			sha512.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
#else
            var retVal = SHA256.HashData(buffer);
            return Convert.ToHexString(retVal);
#endif
		}
	}


}
