using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UNICHAR : Function_1
	{
		public Function_UNICHAR(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "UniChar";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			// 数值超出 int 范围时按参数错误处理,避免 OverflowException 外泄
			if (TryGetInt(args1, out int code) == false) {
				return ParameterError(1);
			}
			// 已排除负数、超出 Unicode 上限、以及代理区(0xD800~0xDFFF)，
			// ConvertFromUtf32 在其余取值上不会抛异常，无需再包 try
			if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
				return ParameterError(1);
			}
			return Operand.Create(char.ConvertFromUtf32(code));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
