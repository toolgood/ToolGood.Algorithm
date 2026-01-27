using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_FISHERINV : Function_1
    {
        public Function_FISHERINV(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "FisherInv"); if (args1.IsError) { return args1; } }
            var x = args1.DoubleValue;
            var n = (Math.Exp((2 * x)) - 1) / (Math.Exp((2 * x)) + 1);
            return Operand.Create(n);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FisherInv");
        }
    }

}
