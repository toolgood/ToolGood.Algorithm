using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NORMSINV : Function_1
    {
        public Function_NORMSINV(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "NormSInv";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) return args1;
            var p = args1.DoubleValue;
            return Operand.Create(ExcelFunctions.NormSInv((double)p));
        }

    }

}
