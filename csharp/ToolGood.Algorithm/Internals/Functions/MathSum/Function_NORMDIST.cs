using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NORMDIST : Function_4
    {
		public Function_NORMDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "NormDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var args4 = GetBoolean_4(engine, tempParameter);
            if (args4.IsError) return args4;

            var num = args1.DoubleValue;
            var avg = args2.DoubleValue;
            var STDEV = args3.DoubleValue;
            var b = args4.BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist((double)num, (double)avg, (double)STDEV, b));
        }

    }

}
