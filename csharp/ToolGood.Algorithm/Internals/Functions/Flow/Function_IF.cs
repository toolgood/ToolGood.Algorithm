using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_IF : Function_3
    {
		public Function_IF(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "If";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetBoolean_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
            if (args1.BooleanValue) return func2.Evaluate(engine, tempParameter);
            if (func3 == null) { return Operand.False; }
            return func3.Evaluate(engine, tempParameter);
        }

    }
}
