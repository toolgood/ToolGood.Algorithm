using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISODD : Function_1
    {
        public Function_ISODD(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsOdd";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (args1.IsNumber) {
                if (Math.Abs(args1.IntValue) % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
