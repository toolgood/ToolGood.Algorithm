using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_WEIBULL : Function_4
    {
		public Function_WEIBULL(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Weibull";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(work, tempParameter);
            if (args3.IsError) return args3;

            var args4 = GetBoolean_4(work, tempParameter);
            if (args4.IsError) return args4;
            var x = args1.DoubleValue;
            var shape = args2.DoubleValue;
            var scale = args3.DoubleValue;
            var state = args4.BooleanValue;
            if (shape <= 0.0 || scale <= 0.0) {
                return FunctionError();
            }

            return Operand.Create(ExcelFunctions.Weibull((double)x, (double)shape, (double)scale, state));
        }

    }

}
