using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_IFERROR : Function_3
    {
		public Function_IFERROR(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "IfError";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (args1.IsError) { return func2.Evaluate(engine, tempParameter); }
            return func3.Evaluate(engine, tempParameter);
        }

    }
}
