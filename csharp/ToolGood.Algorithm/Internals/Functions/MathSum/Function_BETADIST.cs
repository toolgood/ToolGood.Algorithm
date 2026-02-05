using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_BETADIST : Function_3
    {
        public Function_BETADIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "BetaDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) return args1;

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) return args2;

            var args3 = GetNumber_3(work, tempParameter);
            if (args3.IsError) return args3;

            var x = args1.DoubleValue;
            var alpha = args2.DoubleValue;
            var beta = args3.DoubleValue;

            if (alpha < 0.0 || beta < 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "BetaDist");
            }
            return Operand.Create(ExcelFunctions.BetaDist((double)x, (double)alpha, (double)beta));
        }


    }

}
