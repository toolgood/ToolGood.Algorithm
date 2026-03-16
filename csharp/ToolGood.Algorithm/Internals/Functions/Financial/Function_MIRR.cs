using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_MIRR : Function_3
	{
		public Function_MIRR(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "MIRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var valuesArg = GetArray_1(engine, tempParameter);
			if (valuesArg.IsErrorOrNone) return valuesArg;
			var values = new List<decimal>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.NumberValue);
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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
