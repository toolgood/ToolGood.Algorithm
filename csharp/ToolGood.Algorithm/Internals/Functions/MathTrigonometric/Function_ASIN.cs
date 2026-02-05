using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ASIN : Function_1
    {
        public Function_ASIN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Asin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "Asin", 1);
            if (args1.IsError) { return args1; }
            var x = args1.DoubleValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function '{0}' parameter is error!", "Asin");
            }
            return Operand.Create(Math.Asin((double)x));
        }

    }

    

}
