using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MEDIAN : Function_N
	{
		public Function_MEDIAN(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length < 1) {
				throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
			}
		}

		public override string Name => "Median";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }

			var list = new List<decimal>();
			if(FunctionUtil.FlattenToList(args, list) == false) { return FunctionError(); }
			if(list.Count == 0) { return FunctionError(); }

			int n = list.Count;
			if(n == 1) return Operand.Create(list[0]);

			int mid = n / 2;
			if(n % 2 == 0) {
				return Operand.Create((FunctionUtil.QuickSelect(list, mid - 1, false) + FunctionUtil.QuickSelect(list, mid, false)) / 2);
			}
			return Operand.Create(FunctionUtil.QuickSelect(list, mid, false));
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 1) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}
}
