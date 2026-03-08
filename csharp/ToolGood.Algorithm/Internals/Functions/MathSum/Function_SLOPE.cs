using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SLOPE : Function_N
	{
		public Function_SLOPE(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SLOPE";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var yArrayArg = GetArray(engine, tempParameter, 0);
			if (yArrayArg.IsError) return yArrayArg;

			var xArrayArg = GetArray(engine, tempParameter, 1);
			if (xArrayArg.IsError) return xArrayArg;

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

			var denominator = n * sumX2 - sumX * sumX;
			if (denominator == 0) {
				return Div0Error();
			}
			var slope = (n * sumXY - sumX * sumY) / denominator;
			return Operand.Create(slope);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
