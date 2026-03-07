using System;
using System.Text;
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
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;
            var p = args1.DoubleValue;
            var avg = args2.DoubleValue;
            var STDEV = args3.DoubleValue;
            if (p <= 0.0 || p >= 1.0) {
                return FunctionError();
            }
            if (STDEV <= 0.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.NormInv(p, avg, STDEV));
        }

    }

}
