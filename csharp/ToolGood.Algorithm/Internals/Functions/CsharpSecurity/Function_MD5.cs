using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_MD5 : Function_2
	{
		public Function_MD5(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "MD5", 1); if(args1.IsError) return args1; }
			try {
				Encoding encoding;
				if(func2 == null) {
					encoding = Encoding.UTF8;
				} else {
					var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "MD5", 2); if(args2.IsError) return args2; }
					encoding = Encoding.GetEncoding(args2.TextValue);
				}
				var t = GetMd5String(encoding.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function 'MD5' is error!" + ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "MD5");
		}

		private string GetMd5String(byte[] buffer)
		{
#if WebAssembly
            return MD5.MDString(buffer);
#else
#if NETSTANDARD2_1
			System.Security.Cryptography.MD5 md5 = MD5.Create();
			byte[] retVal = md5.ComputeHash(buffer);
			md5.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
#else
            var retVal = MD5.HashData(buffer);
            return Convert.ToHexString(retVal);
#endif
#endif
		}
	}


}
