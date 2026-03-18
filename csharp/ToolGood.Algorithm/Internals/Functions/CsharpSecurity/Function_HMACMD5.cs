using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_HMACMD5 : Function_2
	{
		public Function_HMACMD5(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "HmacMD5";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }
			var t = GetHmacMd5String(Encoding.UTF8.GetBytes(args1.TextValue), args2.TextValue);
			return Operand.Create(t);
		}

		private string GetHmacMd5String(byte[] buffer, string secret)
		{
			var keyByte = Encoding.UTF8.GetBytes(secret ?? string.Empty);
			using var hmacMd5 = new HMACMD5(keyByte);
			var hashMessage = hmacMd5.ComputeHash(buffer);
#if NETSTANDARD2_1
			return BitConverter.ToString(hashMessage).Replace("-", string.Empty);
#else
			return Convert.ToHexString(hashMessage);
#endif
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
