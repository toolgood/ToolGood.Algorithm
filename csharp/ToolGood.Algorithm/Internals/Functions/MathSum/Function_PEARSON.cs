using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_PEARSON : Function_N
	{
		public Function_PEARSON(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PEARSON";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var array1Arg = GetArray(engine, tempParameter, 0);
			if (array1Arg.IsError) return array1Arg;

			var array2Arg = GetArray(engine, tempParameter, 1);
			if (array2Arg.IsError) return array2Arg;

			var xValues = new List<decimal>();
			foreach (var item in array1Arg.ArrayValue) {
				if (item.IsNumber) xValues.Add(item.NumberValue);
			}

			var yValues = new List<decimal>();
			foreach (var item in array2Arg.ArrayValue) {
				if (item.IsNumber) yValues.Add(item.NumberValue);
			}

			if (xValues.Count != yValues.Count || xValues.Count < 2) return FunctionError();

			int n = xValues.Count;
			decimal sumX = 0, sumY = 0;

			for (int i = 0; i < n; i++) {
				sumX += xValues[i];
				sumY += yValues[i];
			}

			var meanX = sumX / n;
			var meanY = sumY / n;

			decimal numerator = 0, denomX = 0, denomY = 0;

			for (int i = 0; i < n; i++) {
				var dx = xValues[i] - meanX;
				var dy = yValues[i] - meanY;
				numerator += dx * dy;
				denomX += dx * dx;
				denomY += dy * dy;
			}

			if (denomX == 0 || denomY == 0) return Div0Error();

			return Operand.Create(numerator / MathEx.Sqrt((denomX * denomY)));
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
