using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ATAN2 : Function_2
    {
        public Function_ATAN2(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Atan2";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "Atan2", 1);
            if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "Atan2", 2);
            if (args2.IsError) { return args2; }
            return Operand.Create(Math.Atan2(args2.DoubleValue, args1.DoubleValue));
        }

    }

    

}
