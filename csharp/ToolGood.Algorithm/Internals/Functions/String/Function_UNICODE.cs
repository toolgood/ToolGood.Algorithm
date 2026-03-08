using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UNICODE : Function_1
	{
		public Function_UNICODE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Unicode";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			if (string.IsNullOrEmpty(args1.TextValue)) {
				return ParameterError(1);
			}
			return Operand.Create(char.ConvertToUtf32(args1.TextValue, 0));
		}
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}
}
