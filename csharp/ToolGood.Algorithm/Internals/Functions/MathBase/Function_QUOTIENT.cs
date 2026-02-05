using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_QUOTIENT : Function_2
    {
		public Function_QUOTIENT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_QUOTIENT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Quotient";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }

            if (args2.NumberValue == 0) {
                return Div0Error();
            }
            return Operand.Create((int)(args1.NumberValue / args2.NumberValue));
        }

    }


}
