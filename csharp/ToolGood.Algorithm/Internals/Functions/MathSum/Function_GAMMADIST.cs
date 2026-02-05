using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_GAMMADIST : Function_4
    {
        public Function_GAMMADIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override string Name => "GammaDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "GammaDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "GammaDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToNumber(args3, "GammaDist", 3);
            if (args3.IsError) return args3;

            var args4 = func4.Evaluate(work, tempParameter);
            args4 = FunctionUtil.ConvertToBoolean(args4, "GammaDist", 4);
            if (args4.IsError) return args4;

            var x = args1.DoubleValue;
            var alpha = args2.DoubleValue;
            var beta = args3.DoubleValue;
            var cumulative = args4.BooleanValue;
            if (alpha < 0.0 || beta < 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "GammaDist");
            }
            return Operand.Create(ExcelFunctions.GammaDist((double)x, (double)alpha, (double)beta, cumulative));
        }

    }

}
