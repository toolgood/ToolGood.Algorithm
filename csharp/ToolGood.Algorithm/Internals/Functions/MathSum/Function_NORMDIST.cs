using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NORMDIST : Function_4
    {
        public Function_NORMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override string Name => "NormDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "NormDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "NormDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToNumber(args3, "NormDist", 3);
            if (args3.IsError) return args3;

            var args4 = func4.Evaluate(work, tempParameter);
            args4 = FunctionUtil.ConvertToBoolean(args4, "NormDist", 4);
            if (args4.IsError) return args4;

            var num = args1.DoubleValue;
            var avg = args2.DoubleValue;
            var STDEV = args3.DoubleValue;
            var b = args4.BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist((double)num, (double)avg, (double)STDEV, b));
        }

    }

}
