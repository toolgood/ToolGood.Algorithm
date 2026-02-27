using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_ISTEXT : Function_1
    {
        public Function_ISTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsText";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (args1.IsText) { return Operand.True; }
            return Operand.False;
        }

    }

}
