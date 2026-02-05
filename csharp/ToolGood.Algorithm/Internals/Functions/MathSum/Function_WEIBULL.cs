using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_WEIBULL : Function_4
    {
        public Function_WEIBULL(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override string Name => "Weibull";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "Weibull", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = ConvertToNumber(args2, "Weibull", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = ConvertToNumber(args3, "Weibull", 3);
            if (args3.IsError) return args3;

            var args4 = func4.Evaluate(work, tempParameter);
            args4 = ConvertToBoolean(args4, "Weibull", 4);
            if (args4.IsError) return args4;
            var x = args1.DoubleValue;
            var shape = args2.DoubleValue;
            var scale = args3.DoubleValue;
            var state = args4.BooleanValue;
            if (shape <= 0.0 || scale <= 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "Weibull");
            }

            return Operand.Create(ExcelFunctions.Weibull((double)x, (double)shape, (double)scale, state));
        }

    }

}
