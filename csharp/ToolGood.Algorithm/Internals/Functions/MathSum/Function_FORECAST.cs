using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_FORECAST : Function_N
	{
		public Function_FORECAST(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
		}

		public override string Name => "FORECAST";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
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

			int n = yValues.Count;

			decimal sumX = 0, sumY = 0;
			for (int i = 0; i < n; i++) {
				sumX += xValues[i];
				sumY += yValues[i];
			}

			var meanX = sumX / n;
			var meanY = sumY / n;

			// 采用中心化两遍法,避免 n*sumX2 - sumX*sumX 的病态消减与溢出
			decimal numerator = 0, denominator = 0;
			for (int i = 0; i < n; i++) {
				var dx = xValues[i] - meanX;
				numerator += dx * (yValues[i] - meanY);
				denominator += dx * dx;
			}

			if (denominator == 0) {
				return Div0Error();
			}
			var slope = numerator / denominator;
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
