using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal class Function_SEC : Function_1
    {
        public Function_SEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "Sec", 1);
            if (args1.IsError) { return args1; }
            var d = Math.Cos(args1.DoubleValue);
            if (d == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "Sec");
            }
            return Operand.Create(1.0 / d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sec");
        }
    }


}
