using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_COS : Function_1
    {
        public Function_COS(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Cos";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Cos(args1.DoubleValue));
        }

    }


}
