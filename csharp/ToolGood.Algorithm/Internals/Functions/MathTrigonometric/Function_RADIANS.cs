using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_RADIANS : Function_1
    {
        public Function_RADIANS(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Radians";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var r = args1.DoubleValue / 180 * Math.PI;
            return Operand.Create(r);
        }

    }


}
