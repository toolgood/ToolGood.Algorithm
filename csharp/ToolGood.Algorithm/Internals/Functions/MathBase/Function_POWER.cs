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
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToNumber(args1, "Power", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToNumber(args2, "Power", 2);
			if (args2.IsError) { return args2; }
            return Operand.Create(Math.Pow(args1.DoubleValue, args2.DoubleValue));
        }

    }

    

}
