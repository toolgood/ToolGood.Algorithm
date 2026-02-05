using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_POWER : Function_2
    {
        public Function_POWER(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Power";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            return Operand.Create(Math.Pow(args1.DoubleValue, args2.DoubleValue));
        }

    }

    

}
