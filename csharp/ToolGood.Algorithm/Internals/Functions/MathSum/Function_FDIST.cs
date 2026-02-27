using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_FDIST : Function_3
    {
		public Function_FDIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "FDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var x = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
                return FunctionError();
            }
            return Operand.Create(ExcelFunctions.FDist((double)x, degreesFreedom, degreesFreedom2));
        }

    }

}
