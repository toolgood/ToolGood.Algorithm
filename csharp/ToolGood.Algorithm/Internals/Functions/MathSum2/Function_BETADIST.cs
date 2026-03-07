using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BETADIST : Function_3
    {
		public Function_BETADIST(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "BetaDist";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsError) return args3;

            var x = args1.DoubleValue;
            var alpha = args2.DoubleValue;
            var beta = args3.DoubleValue;

            if (alpha < 0.0) {
                return ParameterError(2);
            }
            if (beta < 0.0) {
                return ParameterError(3);
            }
            return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
        }


    }

}
