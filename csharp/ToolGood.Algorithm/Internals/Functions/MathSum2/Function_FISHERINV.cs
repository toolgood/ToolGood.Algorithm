using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_FISHERINV : Function_1
    {
        public Function_FISHERINV(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "FisherInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var x = args1.NumberValue;
            var n = (MathEx.Exp((2 * x)) - 1) / (MathEx.Exp((2 * x)) + 1);
            return Operand.Create(n);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
