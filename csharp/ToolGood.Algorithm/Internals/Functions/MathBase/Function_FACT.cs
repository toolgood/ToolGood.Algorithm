using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_FACT : Function_1
    {
        public Function_FACT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Fact";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

            var z = args1.IntValue;
            if (z < 0) {
                return FunctionError();
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }

    }

    

}
