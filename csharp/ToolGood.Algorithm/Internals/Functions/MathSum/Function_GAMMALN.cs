using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_GAMMALN : Function_1
    {
        public Function_GAMMALN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "GammaLn";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            return Operand.Create(ExcelFunctions.GAMMALN(args1.DoubleValue));
        }

    }

}
