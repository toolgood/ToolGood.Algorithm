using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UNICHAR : Function_1
	{
		public Function_UNICHAR(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "UniChar";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var code = args1.IntValue;
			if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
				return ParameterError(1);
			}
			try {
				return Operand.Create(char.ConvertFromUtf32(code));
			} catch {
				return ParameterError(1);
			}
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}
	}
}
