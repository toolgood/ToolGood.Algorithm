using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_TINV : Function_2
    {
		public Function_TINV(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "TInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;
            var p = args1.NumberValue;
            if (p <= 0m || p >= 1m) {
                return ParameterError(1);
            }
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0) {
                return ParameterError(2);
            }
            return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
