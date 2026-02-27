using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ROUND : Function_2
    {
		public Function_ROUND(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Round";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			if (func2 == null) {
				return Operand.Create(Math.Round((decimal)args1.NumberValue, 0, MidpointRounding.AwayFromZero));
			}
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
            return Operand.Create(Math.Round((decimal)args1.NumberValue, args2.IntValue, MidpointRounding.AwayFromZero));
        }

    }

    

}
