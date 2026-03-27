using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_LOG : Function_2
    {
		public Function_LOG(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Log";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var z = args1.NumberValue;
			if (z <= 0) {
				return ParameterError(1);
			}

			if (func2 == null)
				return Operand.Create(MathEx.Log10(z));

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			var baseValue = args2.NumberValue;
			if (baseValue <= 0 || baseValue == 1) {
				return ParameterError(2);
			}
			return Operand.Create(MathEx.Log(z, baseValue));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
