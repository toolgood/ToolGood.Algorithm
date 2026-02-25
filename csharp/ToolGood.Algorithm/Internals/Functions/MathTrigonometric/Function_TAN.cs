using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_TAN : Function_1
    {
        public Function_TAN(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Tan";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Tan(args1.DoubleValue));
        }

    }

    

}
