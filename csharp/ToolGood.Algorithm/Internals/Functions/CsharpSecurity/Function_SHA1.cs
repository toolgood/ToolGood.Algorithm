using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_SHA1 : Function_2
	{
		public Function_SHA1(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA1", 1); if(args1.IsError) return args1; }
			try {
				Encoding encoding;
				if(func2 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA1", 2); if(args2.IsError) return args2; }
					encoding = Encoding.GetEncoding(args2.TextValue);
				}
				var t = GetSha1String(encoding.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function '{0}' is error!{1}", "SHA1", ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "SHA1");
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
