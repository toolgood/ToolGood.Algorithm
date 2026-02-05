using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_MROUND : Function_2
    {
		public Function_MROUND(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Mround";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            var a = args2.NumberValue;
            if (a <= 0) { return ParameterError(2); }

            var b = args1.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }

    }

    

}
