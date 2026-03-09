using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_DELTA : Function_N
	{
		public Function_DELTA(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Delta";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if(funcs.Length < 1) {
				return ParameterError(1);
			}

			var args1 = GetNumber(engine, tempParameter, 0);
			if(args1.IsErrorOrNone) { return args1; }
			var num1 = args1.NumberValue;

			decimal num2 = 0;
			if(funcs.Length >= 2) {
				var args2 = GetNumber(engine, tempParameter, 1);
				if(args2.IsErrorOrNone) { return args2; }
				num2 = args2.NumberValue;
			}

			return Operand.Create(num1 == num2 ? 1 : 0);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}
}
