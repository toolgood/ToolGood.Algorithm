using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_SQRT : Function_1
    {
        public Function_SQRT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sqrt"); if (args1.IsError) { return args1; } }
            if (args1.NumberValue < 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Sqrt");
            }
            return Operand.Create(Math.Sqrt(args1.DoubleValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sqrt");
        }
    }

    

}
