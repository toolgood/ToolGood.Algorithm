using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_FACTDOUBLE : Function_1
    {
        public Function_FACTDOUBLE(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "FactDouble";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }
            var z = args1.IntValue;
            if (z < 0) { return FunctionError(); }

            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }

    }

    

}
