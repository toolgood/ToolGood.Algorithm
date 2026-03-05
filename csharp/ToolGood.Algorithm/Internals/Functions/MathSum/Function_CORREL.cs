using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_CORREL : Function_N
	{
		public Function_CORREL(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "CORREL";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if(funcs.Length < 2) return ParameterError(1);

			var array1Arg = GetArray(engine, tempParameter, 0);
			if(array1Arg.IsError) return array1Arg;

			var array2Arg = GetArray(engine, tempParameter, 1);
			if(array2Arg.IsError) return array2Arg;

			var xValues = new List<double>();
			foreach(var item in array1Arg.ArrayValue) {
				if(item.IsNumber) xValues.Add(item.DoubleValue);
			}

			var yValues = new List<double>();
			foreach(var item in array2Arg.ArrayValue) {
				if(item.IsNumber) yValues.Add(item.DoubleValue);
			}

			if(xValues.Count != yValues.Count || xValues.Count < 2) return FunctionError();

			int n = xValues.Count;
			double sumX = 0, sumY = 0;

			for(int i = 0; i < n; i++) {
				sumX += xValues[i];
				sumY += yValues[i];
			}

			var meanX = sumX / n;
			var meanY = sumY / n;

			double numerator = 0, denomX = 0, denomY = 0;

			for(int i = 0; i < n; i++) {
				var dx = xValues[i] - meanX;
				var dy = yValues[i] - meanY;
				numerator += dx * dy;
				denomX += dx * dx;
				denomY += dy * dy;
			}

			if(denomX == 0 || denomY == 0) return Div0Error();

			return Operand.Create(numerator / Math.Sqrt((denomX * denomY)));
		}
	}
}
