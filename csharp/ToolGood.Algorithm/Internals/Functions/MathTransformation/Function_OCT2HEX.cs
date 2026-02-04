using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_OCT2HEX : Function_2
    {
        public Function_OCT2HEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToText(args1, "OCT2HEX", 1);
            if (args1.IsError) { return args1; }

            if (RegexHelper.OctRegex.IsMatch(args1.TextValue) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2HEX", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 16).ToUpper();
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter);
                args2 = FunctionUtil.ConvertToNumber(args2, "OCT2HEX", 2);
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2HEX", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "OCT2HEX");
        }
    }

    

}
