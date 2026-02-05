using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_EXPONDIST : Function_3
    {
        public Function_EXPONDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "ExpOnDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "ExponDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = ConvertToNumber(args2, "ExponDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = ConvertToBoolean(args3, "ExponDist", 3);
            if (args3.IsError) return args3;

            var n1 = args1.DoubleValue;
            if (n1 < 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "ExponDist");
            }
            return Operand.Create(ExcelFunctions.ExponDist(n1, args2.DoubleValue, args3.BooleanValue));
        }

    }

}
