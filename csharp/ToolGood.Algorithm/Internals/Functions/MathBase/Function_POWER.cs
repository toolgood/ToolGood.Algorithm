using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_POWER : Function_2
    {
		public Function_POWER(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Power";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
            return Operand.Create(Math.Pow(args1.DoubleValue, args2.DoubleValue));
        }

    }

    

}
