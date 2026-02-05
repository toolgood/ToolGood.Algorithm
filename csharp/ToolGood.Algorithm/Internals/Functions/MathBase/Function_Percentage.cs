using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_Percentage : Function_1
    {
        public Function_Percentage(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Percentage";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Percentage", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create(args1.NumberValue / 100.0m);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            func1.ToString(stringBuilder, false);
            stringBuilder.Append('%');
        }
    }

    

}
