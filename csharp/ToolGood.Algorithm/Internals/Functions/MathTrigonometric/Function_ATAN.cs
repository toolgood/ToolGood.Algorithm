using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ATAN : Function_1
    {
        public Function_ATAN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Atan";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "Atan", 1);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Atan(args1.DoubleValue));
        }

    }

    

}
