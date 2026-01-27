using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_CEILING : Function_2
    {
        public Function_CEILING(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 1); if (args1.IsError) { return args1; } }

            if (func2 == null)
                return Operand.Create(Math.Ceiling(args1.NumberValue));

            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 2); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Create(0); }
            if (b < 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "Ceiling", 2); }

            var a = args1.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Ceiling");
        }
    }

    

}
