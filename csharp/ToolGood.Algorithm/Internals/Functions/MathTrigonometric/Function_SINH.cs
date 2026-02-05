using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_SINH : Function_1
    {
        public Function_SINH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Sinh";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "Sinh", 1);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Sinh(args1.DoubleValue));
        }

    }

    

}
