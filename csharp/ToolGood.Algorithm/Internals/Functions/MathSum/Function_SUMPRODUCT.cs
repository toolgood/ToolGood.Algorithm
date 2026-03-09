using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUMPRODUCT : Function_N
	{
		public Function_SUMPRODUCT(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SUMPRODUCT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var arrays = new List<List<decimal>>();
			for (int i = 0; i < funcs.Length; i++) {
				var arg = GetArray(engine, tempParameter, i);
				if (arg.IsErrorOrNone) return arg;
				var list = new List<decimal>();
				foreach (var item in arg.ArrayValue) {
					if (item.IsNumber) {
						list.Add(item.NumberValue);
					}
				}
				arrays.Add(list);
			}

			int minLength = arrays[0].Count;
			for (int i = 1; i < arrays.Count; i++) {
				if (arrays[i].Count < minLength) {
					minLength = arrays[i].Count;
				}
			}

			decimal result = 0;
			for (int i = 0; i < minLength; i++) {
				decimal product = 1;
				for (int j = 0; j < arrays.Count; j++) {
					product *= arrays[j][i];
				}
				result += product;
			}

			return Operand.Create(result);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
