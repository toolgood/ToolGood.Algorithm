using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_GAMMAINV : Function_3
    {
        public Function_GAMMAINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "GammaInv";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "GammaInv", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = ConvertToNumber(args2, "GammaInv", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = ConvertToNumber(args3, "GammaInv", 3);
            if (args3.IsError) return args3;

            var probability = args1.DoubleValue;
            var alpha = args2.DoubleValue;
            var beta = args3.DoubleValue;
            if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
                return Operand.Error("Function '{0}' parameter is error!", "GammaInv");
            }
            return Operand.Create(ExcelFunctions.GammaInv((double)probability, (double)alpha, (double)beta));
        }

    }

}
