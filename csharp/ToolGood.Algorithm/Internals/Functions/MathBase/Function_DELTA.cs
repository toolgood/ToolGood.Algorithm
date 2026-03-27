using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_DELTA : Function_2
	{
		public Function_DELTA(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Delta";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var num1 = args1.NumberValue;

			decimal num2 = 0;
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
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
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
