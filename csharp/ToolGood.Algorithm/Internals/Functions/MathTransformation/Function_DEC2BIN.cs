using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_DEC2BIN : Function_2
    {
		public Function_DEC2BIN(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_DEC2BIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Dec2Bin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 2);
            if (func2 != null) {
                var args2 = GetNumber_2(work, tempParameter);
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return ParameterError(2);
            }
            return Operand.Create(num);
        }

    }

    

}
