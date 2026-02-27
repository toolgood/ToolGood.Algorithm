using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_BINOMDIST : Function_4
    {
		public Function_BINOMDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "BinomDist";

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

            var n2 = args2.IntValue;
            var n3 = args3.DoubleValue;
            if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
        }

    }

}
