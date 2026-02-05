using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LOG : Function_2
    {
		public Function_LOG(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Log";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			if (func2 == null)
				return Operand.Create(Math.Log10(args1.DoubleValue));

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
			return Operand.Create(Math.Log(args1.DoubleValue, args2.DoubleValue));
        }

    }

    

}
