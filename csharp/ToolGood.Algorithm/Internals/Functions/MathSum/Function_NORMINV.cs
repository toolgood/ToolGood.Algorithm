using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NORMINV : Function_3
    {
        public Function_NORMINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "NormInv";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "NormInv", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "NormInv", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToNumber(args3, "NormInv", 3);
            if (args3.IsError) return args3;
            var p = args1.DoubleValue;
            var avg = args2.DoubleValue;
            var STDEV = args3.DoubleValue;
            return Operand.Create(ExcelFunctions.NormInv((double)p, (double)avg, (double)STDEV));
        }

    }

}
