using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_MIRR : Function_3
	{
		public Function_MIRR(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
		}

		public override string Name => "MIRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var valuesArg = GetArray_1(engine, tempParameter);
			if (valuesArg.IsErrorOrNone) return valuesArg;
			var values = new List<decimal>();
			foreach(var v in valuesArg.ArrayValue) {
				if(v.IsNumber) {
					values.Add(v.NumberValue);
				} else {
					var v2 = v.ToNumber($"Function '{Name}' parameter 1 is error!");
					if(v2.IsErrorOrNone) return v2;
					values.Add(v2.NumberValue);
				}
			}

			var financeRateArg = GetNumber_2(engine, tempParameter);
			if (financeRateArg.IsErrorOrNone) return financeRateArg;
			var financeRate = financeRateArg.NumberValue;

			var reinvestRateArg = GetNumber_3(engine, tempParameter);
			if (reinvestRateArg.IsErrorOrNone) return reinvestRateArg;
			var reinvestRate = reinvestRateArg.NumberValue;

			decimal npvNegative = 0;
			decimal npvPositive = 0;
			int n = values.Count;

			if (n == 0) {
				return ParameterError(1);
			}
			if (n == 1) {
				return Div0Error();
			}

			for (int i = 0; i < n; i++) {
				if (values[i] < 0) {
					// financeRate <= -1 时底数非正, MathEx.Pow 会除零或抛异常
					if (!FinancialMath.TryPowPositiveBase((1 + financeRate), i, out var discountFactor)) return NumError();
					npvNegative += values[i] / discountFactor;
				} else {
					// reinvestRate <= -1 时底数非正, 同样需要守卫
					if (!FinancialMath.TryPowPositiveBase((1 + reinvestRate), n - 1 - i, out var compoundFactor)) return NumError();
					npvPositive += values[i] * compoundFactor;
				}
			}

			if (npvNegative == 0 || npvPositive == 0) return Div0Error();

			if (!FinancialMath.TryPowPositiveBase((-npvPositive / npvNegative), 1.0m / (n - 1), out var ratio)) return NumError();
			var mirr = ratio - 1;
			return Operand.Create(mirr);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
