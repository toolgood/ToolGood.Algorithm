using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_SHA256 : Function_1
	{
		public Function_SHA256(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "SHA256";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var t = GetSha256String(Encoding.UTF8.GetBytes(args1.TextValue));
			return Operand.Create(t);
		}

		private string GetSha256String(byte[] buffer)
		{
#if NETSTANDARD2_1
			using var sha256 = SHA256.Create();
			var retVal = sha256.ComputeHash(buffer);
			return BitConverter.ToString(retVal).Replace("-", string.Empty);
#else
			var retVal = SHA256.HashData(buffer);
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
