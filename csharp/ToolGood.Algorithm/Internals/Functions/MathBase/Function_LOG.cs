using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LOG : Function_2
    {
        public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Log", 1); if (args1.IsError) { return args1; } }
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Log", 2); if (args2.IsError) { return args2; } }
                return Operand.Create(Math.Log(args1.DoubleValue, args2.DoubleValue));
            }
            return Operand.Create(Math.Log(args1.DoubleValue, 10));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Log");
        }
    }

    

}
