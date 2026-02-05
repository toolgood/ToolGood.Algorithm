using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_IFERROR : Function_3
    {
        public Function_IFERROR(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "IfError";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsError) { return func2.Evaluate(work, tempParameter); }
            return func3.Evaluate(work, tempParameter);
        }

    }
}
