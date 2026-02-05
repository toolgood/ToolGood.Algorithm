using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_TANH : Function_1
    {
        public Function_TANH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Tanh";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "Tanh", 1);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Tanh(args1.DoubleValue));
        }

    }

    

}
