using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_QUOTIENT : Function_2
    {
        public Function_QUOTIENT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Quotient";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Quotient", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Quotient", 2);
			if (args2.IsError) { return args2; }

            if (args2.NumberValue == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Quotient");
            }
            return Operand.Create((int)(args1.NumberValue / args2.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Quotient");
        }
    }


}
