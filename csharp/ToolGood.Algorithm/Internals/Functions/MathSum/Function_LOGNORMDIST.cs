using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_LOGNORMDIST : Function_3
    {
        public Function_LOGNORMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "LogNormDist";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "LognormDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "LognormDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToNumber(args3, "LognormDist", 3);
            if (args3.IsError) return args3;

            var n3 = args3.DoubleValue;
            if (n3 < 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "LognormDist");
            }
            return Operand.Create(ExcelFunctions.LognormDist(args1.DoubleValue, args2.DoubleValue, n3));
        }

    }

}
