using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_TINV : Function_2
    {
        public Function_TINV(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "TInv";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "TInv", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "TInv", 2);
            if (args2.IsError) return args2;
            var p = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0.0 || p < 0.0 || p > 1.0) {
                return Operand.Error("Function '{0}' parameter is error!", "TInv");
            }
            return Operand.Create(ExcelFunctions.TInv((double)p, degreesFreedom));
        }

    }

}
