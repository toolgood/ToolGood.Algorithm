using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ATANH : Function_1
    {
        public Function_ATANH(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Atanh";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            var x = args1.DoubleValue;
            if (x >= 1 || x <= -1) {
                return FunctionError();
            }
            return Operand.Create(Math.Atanh((double)x));
        }

    }

    

}
