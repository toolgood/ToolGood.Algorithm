using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_SLN : Function_N
	{
		public Function_SLN(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SLN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var costArg = GetNumber(engine, tempParameter, 0);
			if (costArg.IsErrorOrNone) return costArg;
			var cost = costArg.NumberValue;

			var salvageArg = GetNumber(engine, tempParameter, 1);
			if (salvageArg.IsErrorOrNone) return salvageArg;
			var salvage = salvageArg.NumberValue;

			var lifeArg = GetNumber(engine, tempParameter, 2);
			if (lifeArg.IsErrorOrNone) return lifeArg;
			var life = lifeArg.NumberValue;

			if (life == 0) return Div0Error();

			return Operand.Create((cost - salvage) / life);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}
}
