using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_COMBIN : Function_2
    {
		public Function_COMBIN(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Combin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }

            var total = args1.IntValue;
            var count = args2.IntValue;
            if (total < 0 || count < 0 || total < count) {
                return FunctionError();
            }
            decimal sum = 1;
            decimal sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return Operand.Create(sum / sum2);
        }

    }

    

}
