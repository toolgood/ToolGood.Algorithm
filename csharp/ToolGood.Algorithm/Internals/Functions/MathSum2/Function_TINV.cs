using System;
using System.Text;
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
            var p = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            if (p < 0.0 || p > 1.0) {
                return ParameterError(1);
            }
            if (degreesFreedom <= 0) {
                return ParameterError(2);
            }
            return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
