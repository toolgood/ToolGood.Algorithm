using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_FLOOR : Function_2
    {
		public Function_FLOOR(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Floor";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			if (func2 == null)
				return Operand.Create(Math.Floor(args1.NumberValue));

			var args2 = GetNumber_2(work, tempParameter);
			if (args2.IsError) { return args2; }
            var b = args2.NumberValue;
            if (b >= 1) { return Operand.Create(args1.IntValue); }
            if (b <= 0) { return ParameterError(2); }

            var a = args1.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }

    }

    

}
