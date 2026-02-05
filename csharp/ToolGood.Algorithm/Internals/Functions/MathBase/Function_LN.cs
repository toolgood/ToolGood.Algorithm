using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LN : Function_1
    {
        public Function_LN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Ln";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }
            var z = args1.DoubleValue;
            if (z <= 0) {
                return FunctionError();
            }
            return Operand.Create(Math.Log((double)z));
        }

    }

    

}
