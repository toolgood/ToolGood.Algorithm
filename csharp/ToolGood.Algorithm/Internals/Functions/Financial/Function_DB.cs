using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_DB : Function_5
	{
		public Function_DB(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "DB";

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

			int month = 12;
			if (func5 != null) {
				var monthArg = GetNumber_5(engine, tempParameter);
				if (monthArg.IsErrorOrNone) return monthArg;
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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
