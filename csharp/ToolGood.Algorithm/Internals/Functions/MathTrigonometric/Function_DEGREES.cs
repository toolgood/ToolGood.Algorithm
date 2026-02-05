using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_DEGREES : Function_1
    {
        public Function_DEGREES(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Degrees";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToNumber(args1, "Degrees", 1);
            if (args1.IsError) { return args1; }
            var z = args1.DoubleValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }

    }

    

}
