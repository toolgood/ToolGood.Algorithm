using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_DEC2OCT : Function_2
    {
		public Function_DEC2OCT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Dec2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 8);
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
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
