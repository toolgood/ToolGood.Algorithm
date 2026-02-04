using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_FACT : Function_1
    {
        public Function_FACT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Fact", 1);
			if (args1.IsError) { return args1; }

            var z = args1.IntValue;
            if (z < 0) {
                return Operand.Error("Function '{0}' parameter is error!", "Fact");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Fact");
        }
    }

    

}
