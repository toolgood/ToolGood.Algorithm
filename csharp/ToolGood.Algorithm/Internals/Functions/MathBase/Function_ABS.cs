using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_ABS : Function_1
    {
        public Function_ABS(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Abs";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Abs", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create(Math.Abs(args1.NumberValue));
        }

    }

    

}
