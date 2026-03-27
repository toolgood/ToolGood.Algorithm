using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_HMACSHA512 : Function_2
	{
		public Function_HMACSHA512(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "HmacSHA512";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }
			var t = GetHmacSha512String(Encoding.UTF8.GetBytes(args1.TextValue), args2.TextValue);
			return Operand.Create(t);
		}

		private string GetHmacSha512String(byte[] buffer, string secret)
		{
			var keyByte = Encoding.UTF8.GetBytes(secret ?? string.Empty);
			using var hmacSha512 = new HMACSHA512(keyByte);
			var hashMessage = hmacSha512.ComputeHash(buffer);
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
