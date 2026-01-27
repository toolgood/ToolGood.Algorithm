using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_ROUNDDOWN : Function_2
    {
        public Function_ROUNDDOWN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 2); if (args2.IsError) { return args2; } }
            if (args1.NumberValue == 0.0m) {
                return args1;
            }
            var a = (decimal)Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            b = ((int)(b * a)) / a;
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "RoundDown");
        }
    }

    

}
