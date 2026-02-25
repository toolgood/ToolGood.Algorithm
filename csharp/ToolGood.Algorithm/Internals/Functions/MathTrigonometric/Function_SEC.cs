using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_SEC : Function_1
    {
        public Function_SEC(FunctionBase[] func1) : base(func1)
        {
        }

        public override string Name => "Sec";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(work, tempParameter);
            if (args1.IsError) { return args1; }
            var d = Math.Cos(args1.DoubleValue);
            if (d == 0) {
                return Div0Error();
            }
            return Operand.Create(1.0 / d);
        }

    }


}
