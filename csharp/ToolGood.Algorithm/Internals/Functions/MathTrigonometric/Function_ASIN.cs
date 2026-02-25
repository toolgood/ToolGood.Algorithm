using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ASIN : Function_1
    {
        public Function_ASIN(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Asin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var x = args1.DoubleValue;
            if (x < -1 || x > 1) {
                return FunctionError();
            }
            return Operand.Create(Math.Asin((double)x));
        }

    }

    

}
