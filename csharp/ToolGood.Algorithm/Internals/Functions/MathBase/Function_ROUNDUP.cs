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
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "RoundUp", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "RoundUp", 2);
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
