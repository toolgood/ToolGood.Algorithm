using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_TRUNC : Function_1
    {
        public Function_TRUNC(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Trunc";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToNumber(args1, "Trunc", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create((int)(args1.IntValue));
        }

    }

    

}
