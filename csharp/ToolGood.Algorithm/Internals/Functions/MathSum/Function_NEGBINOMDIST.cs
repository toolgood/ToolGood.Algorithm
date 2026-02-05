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
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "NegbinomDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = ConvertToNumber(args2, "NegbinomDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = ConvertToNumber(args3, "NegbinomDist", 3);
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
