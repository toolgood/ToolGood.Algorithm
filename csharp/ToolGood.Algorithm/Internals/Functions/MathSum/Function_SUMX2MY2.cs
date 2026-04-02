using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUMX2MY2 : Function_N
	{
		public Function_SUMX2MY2(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "SUMX2MY2";

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
				result += arrayX[i] * arrayX[i] - arrayY[i] * arrayY[i];
			}

			return Operand.Create(result);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
		}
	}
}
