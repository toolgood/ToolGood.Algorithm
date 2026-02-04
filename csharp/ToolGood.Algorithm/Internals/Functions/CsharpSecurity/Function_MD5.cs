using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal class Function_MD5 : Function_1
	{
		public Function_MD5(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "MD5", 1);
			if(args1.IsError) { return args1; }
			try {
				var t = GetMd5String(Encoding.UTF8.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception ex) {
				return Operand.Error("Function '{0}' is error!{1}", "MD5", ex.Message);
			}
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "MD5");
		}

		private string GetMd5String(byte[] buffer)
		{
#if NETSTANDARD2_1
			System.Security.Cryptography.MD5 md5 = MD5.Create();
			byte[] retVal = md5.ComputeHash(buffer);
			md5.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
#else
            var retVal = MD5.HashData(buffer);
            return Convert.ToHexString(retVal);
#endif
		}
	}


}
