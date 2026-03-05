using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_CEILING : Function_2
    {
		public Function_CEILING(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Ceiling";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			if (func2 == null)
				return Operand.Create(Math.Ceiling(args1.NumberValue));

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Zero; }
            if (b < 0) { return ParameterError(2); }

            var a = args1.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }

    }

    

}
