using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_ATAN2 : Function_2
    {
		public Function_ATAN2(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_ATAN2(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Atan2";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            return Operand.Create(Math.Atan2(args2.DoubleValue, args1.DoubleValue));
        }

    }

    

}
