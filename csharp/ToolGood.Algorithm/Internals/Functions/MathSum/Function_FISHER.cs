using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_FISHER : Function_1
    {
        public Function_FISHER(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Fisher";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var x = args1.DoubleValue;
            if (x >= 1 || x <= -1) {
                return FunctionError();
            }
            var n = 0.5 * Math.Log((double)((1 + x) / (1 - x)));
            return Operand.Create(n);
        }

    }

}
