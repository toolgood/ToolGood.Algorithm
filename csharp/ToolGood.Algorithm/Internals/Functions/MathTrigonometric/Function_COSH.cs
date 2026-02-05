using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_COSH : Function_1
    {
        public Function_COSH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Cosh";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "Cosh", 1);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Cosh(args1.DoubleValue));
        }

    }


}
