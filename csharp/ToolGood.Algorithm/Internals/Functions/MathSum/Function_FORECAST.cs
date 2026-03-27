using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_FORECAST : Function_N
	{
		public Function_FORECAST(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "FORECAST";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var xArg = GetNumber(engine, tempParameter, 0);
			if (xArg.IsErrorOrNone) return xArg;
			var x = xArg.NumberValue;

			var yArrayArg = GetArray(engine, tempParameter, 1);
			if (yArrayArg.IsErrorOrNone) return yArrayArg;

			var xArrayArg = GetArray(engine, tempParameter, 2);
			if (xArrayArg.IsErrorOrNone) return xArrayArg;

			var yValues = new List<decimal>();
			foreach (var item in yArrayArg.ArrayValue) {
				if (item.IsNumber) yValues.Add(item.NumberValue);
			}

			var xValues = new List<decimal>();
			foreach (var item in xArrayArg.ArrayValue) {
				if (item.IsNumber) xValues.Add(item.NumberValue);
			}

			if (yValues.Count != xValues.Count || yValues.Count < 2) return FunctionError();

			decimal sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
			int n = yValues.Count;

			for (int i = 0; i < n; i++) {
				sumX += xValues[i];
				sumY += yValues[i];
				sumXY += xValues[i] * yValues[i];
				sumX2 += xValues[i] * xValues[i];
			}

			var meanX = sumX / n;
			var meanY = sumY / n;

			var denominator = n * sumX2 - sumX * sumX;
			if (denominator == 0) {
				return Div0Error();
			}
			var slope = (n * sumXY - sumX * sumY) / denominator;
			var intercept = meanY - slope * meanX;

			return Operand.Create(intercept + slope * x);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			funcs[2].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
		}
	}
}
