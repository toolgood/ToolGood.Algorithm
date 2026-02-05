using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_PERMUT : Function_2
    {
		public Function_PERMUT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Permut";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }

            var total = args1.IntValue;
            var count = args2.IntValue;

            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return Operand.Create(sum);
        }

    }

    

}
