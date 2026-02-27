using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ASINH : Function_1
    {
        public Function_ASINH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Asinh";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            return Operand.Create(Math.Asinh(args1.DoubleValue));
        }

    }

    

}
