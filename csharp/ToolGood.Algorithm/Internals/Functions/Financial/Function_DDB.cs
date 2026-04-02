using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_DDB : Function_5
	{
		public Function_DDB(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 4 || funcs.Length > 5) {
				throw new ArgumentException($"Function '{Name}' requires 4 to 5 parameters.");
			}
		}

		public override string Name => "DDB";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{

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

			decimal factor = 2;
			if (func5 != null) {
				var factorArg = GetNumber_5(engine, tempParameter);
				if (factorArg.IsErrorOrNone) return factorArg;
				factor = factorArg.NumberValue;
			}

			if (life == 0 || factor == 0) return Div0Error();
			if (period < 1 || period > life) {
				return ParameterError(4);
			}
			if (life < 1) {
				return ParameterError(3);
			}

			decimal depreciation = 0;
			decimal remainingCost = cost;

			for (int i = 1; i <= period; i++) {
				var ddb = remainingCost * factor / life;
				var maxDepreciation = remainingCost - salvage;
				if (ddb > maxDepreciation) {
					ddb = maxDepreciation;
				}
				if (i == period) {
					depreciation = ddb;
				}
				remainingCost -= ddb;
				if (remainingCost <= salvage) {
					if (i == period) {
						depreciation = remainingCost + ddb - salvage;
					}
					break;
				}
			}

			return Operand.Create(depreciation);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
