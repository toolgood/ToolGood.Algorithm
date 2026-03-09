using System;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_NORMINV : Function_3
    {
		public Function_NORMINV(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "NormInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone) return args3;
            var p = args1.DoubleValue;
            var avg = args2.DoubleValue;
            var STDEV = args3.DoubleValue;
            if (p <= 0.0 || p >= 1.0) {
                return ParameterError(1);
            }
            if (STDEV <= 0.0) {
                return ParameterError(3);
            }
            return Operand.Create(ExcelFunctions.NormInv(p, avg, STDEV));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}
