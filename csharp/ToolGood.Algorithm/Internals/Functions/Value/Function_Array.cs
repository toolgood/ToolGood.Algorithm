using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ARRAY : Function_N
	{
		public Function_ARRAY(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Array";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length);
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }
			return Operand.Create(args);
		}
		public override OperandType GetResultType()
		{
			return OperandType.ARRAY;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}

}
