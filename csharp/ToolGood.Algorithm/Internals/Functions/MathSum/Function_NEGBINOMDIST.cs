using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NEGBINOMDIST : Function_3
    {
		public Function_NEGBINOMDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "NegBinomDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;
            int k = args1.IntValue;
            var r = args2.DoubleValue;
            var p = args3.DoubleValue;

            if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, (double)r, (double)p));
        }

    }

}
