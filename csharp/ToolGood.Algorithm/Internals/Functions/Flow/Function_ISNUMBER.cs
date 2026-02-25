using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISNUMBER : Function_1
    {
        public Function_ISNUMBER(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "IsNumber";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNumber) { return Operand.True; }
            return Operand.False;
        }

    }

}
