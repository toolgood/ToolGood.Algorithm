using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISERROR : Function_2
    {
		public Function_ISERROR(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "IsError";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (func2 != null) {
                if (args1.IsError) { return func2.Evaluate(engine, tempParameter); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }

    }

}
