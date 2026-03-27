using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpSecurity
{
	internal sealed class Function_MD5 : Function_1
	{
		public Function_MD5(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "MD5";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var t = GetMd5String(Encoding.UTF8.GetBytes(args1.TextValue));
			return Operand.Create(t);
		}

		private string GetMd5String(byte[] buffer)
		{
#if NETSTANDARD2_1
			using var md5 = MD5.Create();
			var retVal = md5.ComputeHash(buffer);
			return BitConverter.ToString(retVal).Replace("-", string.Empty);
#else
			var retVal = MD5.HashData(buffer);
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
