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
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToNumber(args1, "Ln", 1);
			if (args1.IsError) { return args1; }
            var z = args1.DoubleValue;
            if (z <= 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Ln");
            }
            return Operand.Create(Math.Log((double)z));
        }

    }

    

}
