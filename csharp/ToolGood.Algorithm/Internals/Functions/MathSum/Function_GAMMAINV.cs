using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_GAMMAINV : Function_3
    {
		public Function_GAMMAINV(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "GammaInv";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(work, tempParameter);
            if (args3.IsError) return args3;

            var probability = args1.DoubleValue;
            var alpha = args2.DoubleValue;
            var beta = args3.DoubleValue;
            if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.GammaInv((double)probability, (double)alpha, (double)beta));
        }

    }

}
