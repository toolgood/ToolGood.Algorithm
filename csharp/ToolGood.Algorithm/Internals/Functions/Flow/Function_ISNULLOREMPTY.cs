using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISNULLOREMPTY : Function_1
	{
		public Function_ISNULLOREMPTY(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "IsNullOrEmpty";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsNull) { return Operand.True; }
			var textArg = ConvertToText(args1, 1);
			if(textArg.IsErrorOrNone) { return textArg; }
			return Operand.Create(string.IsNullOrEmpty(textArg.TextValue));
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}

}
