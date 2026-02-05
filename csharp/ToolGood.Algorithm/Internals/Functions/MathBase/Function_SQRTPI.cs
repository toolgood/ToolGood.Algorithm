using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_SQRTPI : Function_1
    {
        public Function_SQRTPI(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "SqrtPi";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "SqrtPI", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create(Math.Sqrt(args1.DoubleValue * Math.PI));
        }

    }

    

}
