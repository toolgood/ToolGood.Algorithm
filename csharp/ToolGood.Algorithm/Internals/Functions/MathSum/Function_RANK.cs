using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_RANK : Function_N
	{
		public Function_RANK(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "RANK";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var numArg = GetNumber(engine, tempParameter, 0);
			if (numArg.IsErrorOrNone) return numArg;
			var num = numArg.NumberValue;

			var arrayArg = GetArray(engine, tempParameter, 1);
			if (arrayArg.IsErrorOrNone) return arrayArg;

			int order = 0;
			if (funcs.Length > 2) {
				var orderArg = GetNumber(engine, tempParameter, 2);
				if (orderArg.IsErrorOrNone) return orderArg;
				order = orderArg.IntValue;
			}

			var values = new List<decimal>();
			foreach (var item in arrayArg.ArrayValue) {
				if (item.IsNumber) {
					values.Add(item.NumberValue);
				}
			}

			if (values.Count == 0) {
				return ParameterError(2);
			}

			values.Sort();
			int rank;
			if (order == 0) {
				values.Reverse();
				rank = values.IndexOf(num) + 1;
			} else {
				rank = values.IndexOf(num) + 1;
			}

			if (rank == 0) {
				return ParameterError(1);
			}

			return Operand.Create(rank);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
