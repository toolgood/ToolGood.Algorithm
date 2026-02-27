using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISNULLORERROR : Function_2
    {
		public Function_ISNULLORERROR(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "IsNullOrError";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (func2 != null) {
                if (args1.IsNull || args1.IsError) { return func2.Evaluate(engine, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(engine, tempParameter); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            if (args1.IsText && args1.TextValue == null) { return Operand.True; }
            return Operand.False;
        }

    }

}
