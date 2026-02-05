using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISNULLORERROR : Function_2
    {
		public Function_ISNULLORERROR(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_ISNULLORERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "IsNullOrError";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (func2 != null) {
                if (args1.IsNull || args1.IsError) { return func2.Evaluate(work, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(work, tempParameter); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            if (args1.IsText && args1.TextValue == null) { return Operand.True; }
            return Operand.False;
        }

    }

}
