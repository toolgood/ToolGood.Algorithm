using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_SHA1 : Function_1
	{
		public Function_SHA1(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "SHA1";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var t = GetSha1String(Encoding.UTF8.GetBytes(args1.TextValue));
				return Operand.Create(t);
			} catch(Exception) {
				return FunctionError();
			}
		}

		private string GetSha1String(byte[] buffer)
		{
#if NETSTANDARD2_1
			using var sha1 = SHA1.Create();
			var retVal = sha1.ComputeHash(buffer);
			return BitConverter.ToString(retVal).Replace("-", "");
#else
			var retVal = SHA1.HashData(buffer);
			return Convert.ToHexString(retVal);
#endif
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
