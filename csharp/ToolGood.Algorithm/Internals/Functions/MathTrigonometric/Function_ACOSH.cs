using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ACOSH : Function_1
    {
        public Function_ACOSH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Acosh";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var z = args1.DoubleValue;
            if (z < 1) {
                return Operand.Error("Function '{0}' parameter is error!", "Acosh");
            }
            return Operand.Create(Math.Acosh((double)z));
        }

    }

    

}
