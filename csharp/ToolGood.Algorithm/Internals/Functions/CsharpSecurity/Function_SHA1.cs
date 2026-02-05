using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_SHA1 : Function_1
	{
		public Function_SHA1(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "SHA1";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "SHA1", 1);
			if(args1.IsError) { return args1; }
			try {
				var t = GetSha1String(Encoding.UTF8.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function '{0}' is error!{1}", "SHA1", ex.Message);
			}
		}


		private string GetSha1String(byte[] buffer)
		{

#if NETSTANDARD2_1
			SHA1 sha512 = SHA1.Create();
			byte[] retVal = sha512.ComputeHash(buffer); //计算指定Stream 对象的哈希值
			sha512.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
#else
            var retVal = SHA1.HashData(buffer);
            return Convert.ToHexString(retVal);
#endif
		}
	}


}
