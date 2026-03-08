using System;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_DB : Function_N
	{
		public Function_DB(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "DB";

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

			int month = 12;
			if (funcs.Length > 4) {
				var monthArg = GetNumber(engine, tempParameter, 4);
				if (monthArg.IsError) return monthArg;
				month = monthArg.IntValue;
			}

			if (life == 0 || cost == 0) return Div0Error();

			decimal rate = 1 - MathEx.Pow((salvage / cost), 1.0m / life);
			rate = Math.Round(rate, 3);

			decimal depreciation = 0;
			if (period == 1) {
				depreciation = cost * rate * month / 12;
			} else if ((int)period == (int)life) {
				var remainingCost = cost;
				for (int i = 1; i < life; i++) {
					remainingCost -= depreciation;
					if (i == 1) {
						depreciation = cost * rate * month / 12;
					} else if (i < life) {
						depreciation = remainingCost * rate;
					}
				}
				remainingCost -= depreciation;
				depreciation = remainingCost * rate * (12 - month) / 12;
			} else {
				var remainingCost = cost;
				for (int i = 1; i <= period; i++) {
					if (i == 1) {
						depreciation = cost * rate * month / 12;
					} else {
						remainingCost -= depreciation;
						depreciation = remainingCost * rate;
					}
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
