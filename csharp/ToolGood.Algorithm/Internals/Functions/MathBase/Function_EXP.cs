using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_EXP : Function_1
    {
        public Function_EXP(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Exp";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Exp", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create(Math.Exp(args1.DoubleValue));
        }

    }

    

}
