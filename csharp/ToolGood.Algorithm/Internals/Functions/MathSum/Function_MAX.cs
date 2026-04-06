using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_MAX : Function_N
	{
		public Function_MAX(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length < 1) {
				throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
			}
		}

		public override string Name => "Max";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }

			var list = new List<decimal>();
			var o = FunctionUtil.FlattenToList(args, list);
			if(o == false) { return FunctionError(); }
			if(list.Count == 0) { return FunctionError(); }

			decimal max = list[0];
			for(int i = 1; i < list.Count; i++) {
				if(list[i] > max) { max = list[i]; }
			}
			return Operand.Create(max);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			if(funcs.Length == 1) {
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			} else if(funcs.Length == 2) {
				var t1 = funcs[0].GetResultType();
				var t2 = funcs[1].GetResultType();
				if(t1 == OperandType.NONE && t2 == OperandType.NUMBER) {
					var p = noneEngine.Evaluate(funcs[1]).ToText();
					if(t2 != OperandType.ERROR && p.IsErrorOrNone == false) {
						funcs[0].GetParameterTypes(noneEngine, result, t2, Name, p.TextValue);
						funcs[1].GetParameterTypes(noneEngine, result, t2);
						return;
					}
				} else if(t1 == OperandType.NUMBER && t2 == OperandType.NONE) {
					var p = noneEngine.Evaluate(funcs[0]).ToText();
					if(t1 != OperandType.ERROR && p.IsErrorOrNone == false) {
						funcs[1].GetParameterTypes(noneEngine, result, t1, Name, p.TextValue);
						funcs[0].GetParameterTypes(noneEngine, result, t1);
						return;
					}
				}
				funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			} else {
				for(int i = 0; i < funcs.Length; i++) {
					funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
				}
			}
		}
	}

}
