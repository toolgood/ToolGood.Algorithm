using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal sealed class Function_NOT : Function_1
    {
        public Function_NOT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Not";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetBoolean_1(engine, tempParameter);
            if (args1.IsError) { return args1; }
            return args1.BooleanValue ? Operand.False : Operand.True;
        }

    }
}
