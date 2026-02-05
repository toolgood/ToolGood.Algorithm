using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_OCT2BIN : Function_2
    {
        public Function_OCT2BIN(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Oct2Bin";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            if (RegexHelper.OctRegex.IsMatch(args1.TextValue) == false)  { return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2BIN", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 2);
            if (func2 != null) {
                var args2 = GetNumber_2(work, tempParameter);
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "OCT2BIN", 2);
            }
            return Operand.Create(num);
        }

    }

    

}
