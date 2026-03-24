using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_XOR : Function_N
	{
		public Function_XOR(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Xor";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			int trueCount = 0;
			for(int i = 0; i < funcs.Length; i++) {
				var a = GetBoolean(engine, tempParameter, i);
				if(a.IsErrorOrNone) { return a; }
				if(a.BooleanValue) trueCount++;
			}
			return (trueCount % 2 == 1) ? Operand.True : Operand.False;
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
