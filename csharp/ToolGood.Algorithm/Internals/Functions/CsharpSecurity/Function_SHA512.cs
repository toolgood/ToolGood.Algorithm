using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_SHA512 : Function_2
	{
		public Function_SHA512(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA512", 1); if(args1.IsError) return args1; }
			try {
				Encoding encoding;
				if(func2 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA512", 2); if(args2.IsError) return args2; }
					encoding = Encoding.GetEncoding(args2.TextValue);
				}
				var t = GetSha512String(encoding.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function 'SHA512' is error!" + ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "SHA512");
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
