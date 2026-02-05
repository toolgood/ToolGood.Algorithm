using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_COMBIN : Function_2
    {
        public Function_COMBIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Combin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Combin", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Combin", 2);
			if (args2.IsError) { return args2; }

            var total = args1.IntValue;
            var count = args2.IntValue;
            if (total < 0 || count < 0 || total < count) {
                return Operand.Error("Function '{0}' parameter is error!", "Combin");
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
