using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_ROUNDUP : Function_2
    {
        public Function_ROUNDUP(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "RoundUp";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            if (args1.NumberValue == 0.0m) { return args1; }
            var a = Math.Pow(10, args2.IntValue);
            var b = args1.DoubleValue;

            var t = (Math.Ceiling(Math.Abs((double)b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }

    }

    

}
