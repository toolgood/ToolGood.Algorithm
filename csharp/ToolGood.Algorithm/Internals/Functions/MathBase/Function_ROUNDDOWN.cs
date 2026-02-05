using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_ROUNDDOWN : Function_2
    {
		public Function_ROUNDDOWN(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "RoundDown";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            if (args1.NumberValue == 0.0m) {
                return args1;
            }
            var a = (decimal)Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            b = ((int)(b * a)) / a;
            return Operand.Create(b);
        }

    }

    

}
