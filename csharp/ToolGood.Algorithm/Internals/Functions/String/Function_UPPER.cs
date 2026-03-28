using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UPPER : Function_1
	{
		public Function_UPPER(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Upper";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var text = args1.TextValue;
			for (int i = 0; i < text.Length; i++) {
				if (char.IsLower(text[i])) {
					return Operand.Create(text.ToUpperInvariant());
				}
			}
			return args1;
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
