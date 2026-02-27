using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
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
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;
            var p = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.TInv((double)p, degreesFreedom));
        }

    }

}
