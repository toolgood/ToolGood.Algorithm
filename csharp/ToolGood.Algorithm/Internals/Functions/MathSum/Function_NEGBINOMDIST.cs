using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NEGBINOMDIST : Function_3
    {
        public Function_NEGBINOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "NegBinomDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(work, tempParameter);
            if (args3.IsError) return args3;
            int k = args1.IntValue;
            var r = args2.DoubleValue;
            var p = args3.DoubleValue;

            if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
                return Operand.Error("Function '{0}' parameter is error!", "NegbinomDist");
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, (double)r, (double)p));
        }

    }

}
