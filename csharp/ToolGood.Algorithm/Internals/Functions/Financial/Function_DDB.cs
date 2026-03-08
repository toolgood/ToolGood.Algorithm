using System;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_DDB : Function_N
	{
		public Function_DDB(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "DDB";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 4) return ParameterError(1);

			var costArg = GetNumber(engine, tempParameter, 0);
			if (costArg.IsError) return costArg;
			var cost = costArg.NumberValue;

			var salvageArg = GetNumber(engine, tempParameter, 1);
			if (salvageArg.IsError) return salvageArg;
			var salvage = salvageArg.NumberValue;

			var lifeArg = GetNumber(engine, tempParameter, 2);
			if (lifeArg.IsError) return lifeArg;
			var life = lifeArg.NumberValue;

			var periodArg = GetNumber(engine, tempParameter, 3);
			if (periodArg.IsError) return periodArg;
			var period = periodArg.NumberValue;

			decimal factor = 2;
			if (funcs.Length > 4) {
				var factorArg = GetNumber(engine, tempParameter, 4);
				if (factorArg.IsError) return factorArg;
				factor = factorArg.NumberValue;
			}

			if (life == 0 || factor == 0) return Div0Error();

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
	}
}
