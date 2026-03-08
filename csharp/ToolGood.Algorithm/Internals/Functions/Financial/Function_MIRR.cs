using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_MIRR : Function_N
	{
		public Function_MIRR(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "MIRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var valuesArg = GetArray(engine, tempParameter, 0);
			if (valuesArg.IsError) return valuesArg;
			var values = new List<decimal>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.NumberValue);
			}

			var financeRateArg = GetNumber(engine, tempParameter, 1);
			if (financeRateArg.IsError) return financeRateArg;
			var financeRate = financeRateArg.NumberValue;

			var reinvestRateArg = GetNumber(engine, tempParameter, 2);
			if (reinvestRateArg.IsError) return reinvestRateArg;
			var reinvestRate = reinvestRateArg.NumberValue;

			decimal npvNegative = 0;
			decimal npvPositive = 0;
			int n = values.Count;

			for (int i = 0; i < n; i++) {
				if (values[i] < 0) {
					npvNegative += values[i] / MathEx.Pow((1 + financeRate), i);
				} else {
					npvPositive += values[i] * MathEx.Pow((1 + reinvestRate), n - 1 - i);
				}
			}

			if (npvNegative == 0) return Div0Error();

			var mirr = MathEx.Pow((-npvPositive / npvNegative), 1.0m / (n - 1)) - 1;
			return Operand.Create(mirr);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
