using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal sealed class Function_OR_N : Function_N
	{
		public Function_OR_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "OrN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			bool b = false;
			for(int i = 0; i < funcs.Length; i++) {
				var a = GetBoolean(engine, tempParameter, i);
				if(a.IsErrorOrNone) { return a; }
				if(a.BooleanValue) b = true;
			}
			return b ? Operand.True : Operand.False;
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			}
		}

	}
}