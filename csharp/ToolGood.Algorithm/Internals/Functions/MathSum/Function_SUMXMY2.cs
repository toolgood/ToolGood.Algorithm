using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUMXMY2 : Function_N
	{
		public Function_SUMXMY2(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SUMXMY2";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var arrayXArg = GetArray(engine, tempParameter, 0);
			if (arrayXArg.IsErrorOrNone) return arrayXArg;
			var arrayX = new List<decimal>();
			foreach (var item in arrayXArg.ArrayValue) {
				if (item.IsNumber) {
					arrayX.Add(item.NumberValue);
				}
			}

			var arrayYArg = GetArray(engine, tempParameter, 1);
			if (arrayYArg.IsErrorOrNone) return arrayYArg;
			var arrayY = new List<decimal>();
			foreach (var item in arrayYArg.ArrayValue) {
				if (item.IsNumber) {
					arrayY.Add(item.NumberValue);
				}
			}

			int minLength = arrayX.Count < arrayY.Count ? arrayX.Count : arrayY.Count;

			decimal result = 0;
			for (int i = 0; i < minLength; i++) {
				var diff = arrayX[i] - arrayY[i];
				result += diff * diff;
			}

			return Operand.Create(result);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
