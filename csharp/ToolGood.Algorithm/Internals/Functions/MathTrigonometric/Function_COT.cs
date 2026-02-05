using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_COT : Function_1
    {
        public Function_COT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Cot";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var d = Math.Tan(args1.DoubleValue);
            if (d == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Cot");
            }
            return Operand.Create(1.0 / d);
        }

    }


}
