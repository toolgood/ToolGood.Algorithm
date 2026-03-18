using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_HMACSHA256 : Function_2
	{
		public Function_HMACSHA256(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "HmacSHA256";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }
			try {
				var t = GetHmacSha256String(Encoding.UTF8.GetBytes(args1.TextValue), args2.TextValue);
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
		}

		private string GetHmacSha256String(byte[] buffer, string secret)
		{
			var keyByte = Encoding.UTF8.GetBytes(secret ?? string.Empty);
			using var hmacSha256 = new HMACSHA256(keyByte);
			var hashMessage = hmacSha256.ComputeHash(buffer);
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
