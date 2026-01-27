using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LN : Function_1
    {
        public Function_LN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Ln"); if (args1.IsError) { return args1; } }
            var z = args1.DoubleValue;
            if (z <= 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Ln");
            }
            return Operand.Create(Math.Log((double)z));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Ln");
        }
    }

    

}
