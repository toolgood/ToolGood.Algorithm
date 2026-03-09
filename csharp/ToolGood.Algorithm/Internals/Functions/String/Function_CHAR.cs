using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_CHAR : Function_1
	{
		public Function_CHAR(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Char";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var code = args1.IntValue;
			if (code < 0 || code > 65535) {
				return ParameterError(1);
			}
			char c = (char)code;
			return Operand.Create(new string(c, 1));
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
