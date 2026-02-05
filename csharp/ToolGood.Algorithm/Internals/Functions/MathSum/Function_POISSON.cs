using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_POISSON : Function_3
    {
        public Function_POISSON(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "Poisson";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "Poisson", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "Poisson", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToBoolean(args3, "Poisson", 3);
            if (args3.IsError) return args3;
            int k = args1.IntValue;
            var lambda = args2.DoubleValue;
            bool state = args3.BooleanValue;
            if (!(lambda > 0.0)) {
                return Operand.Error("Function '{0}' parameter is error!", "Poisson");
            }
            return Operand.Create(ExcelFunctions.Poisson(k, (double)lambda, state));
        }

    }

}
