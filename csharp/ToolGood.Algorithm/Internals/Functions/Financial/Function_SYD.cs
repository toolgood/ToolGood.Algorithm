using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_SYD : Function_4
	{
		public Function_SYD(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SYD";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (func1 == null || func2 == null || func3 == null || func4 == null) return ParameterError(1);

			var costArg = GetNumber_1(engine, tempParameter);
			if (costArg.IsErrorOrNone) return costArg;
			var cost = costArg.NumberValue;

			var salvageArg = GetNumber_2(engine, tempParameter);
			if (salvageArg.IsErrorOrNone) return salvageArg;
			var salvage = salvageArg.NumberValue;

			var lifeArg = GetNumber_3(engine, tempParameter);
			if (lifeArg.IsErrorOrNone) return lifeArg;
			var life = lifeArg.NumberValue;

			var periodArg = GetNumber_4(engine, tempParameter);
			if (periodArg.IsErrorOrNone) return periodArg;
			var period = periodArg.NumberValue;

			if (life == 0) return Div0Error();

			var syd = (cost - salvage) * (life - period + 1) * 2 / (life * (life + 1));
			return Operand.Create(syd);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
