using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_IF : Function_3
    {
        public Function_IF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "If";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToBoolean(args1, "If", 1);
			if (args1.IsError) { return args1; }
            if (args1.BooleanValue) return func2.Evaluate(work, tempParameter);
            if (func3 == null) { return Operand.False; }
            return func3.Evaluate(work, tempParameter);
        }

    }
}
