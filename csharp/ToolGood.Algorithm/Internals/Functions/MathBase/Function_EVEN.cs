using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_EVEN : Function_1
    {
        public Function_EVEN(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Even";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }
            var z = args1.NumberValue;
            if (z % 2 == 0) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }

    }

    

}
