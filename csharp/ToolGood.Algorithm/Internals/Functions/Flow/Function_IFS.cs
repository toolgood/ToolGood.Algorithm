using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_IFS : Function_N
	{
		public Function_IFS(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 4) {
				throw new ArgumentException($"Function '{Name}' requires at least 4 parameters.");
			}
			if (funcs.Length % 2 != 0) {
				throw new ArgumentException($"Function '{Name}' requires an even number of parameters.");
			}
		}

		public override string Name => "Ifs";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			for(int i = 0; i < funcs.Length - 1; i += 2) {
				var condition = GetBoolean(engine, tempParameter, i);
				if(condition.IsErrorOrNone) { return condition; }
				if(condition.BooleanValue) {
					return funcs[i + 1].Evaluate(engine, tempParameter);
				}
			}
			return FunctionError();
		}
		public override OperandType GetResultType()
		{
			for(int i = 0; i < funcs.Length - 1; i += 2) {
				var t = funcs[i + 1].GetResultType();
				if(t != OperandType.NONE) {
					return t;
				}
			}
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length - 1; i += 2) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
				funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}
}
