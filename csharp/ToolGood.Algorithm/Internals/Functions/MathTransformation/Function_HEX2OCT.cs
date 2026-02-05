using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_HEX2OCT : Function_2
    {
        public Function_HEX2OCT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Hex2Oct";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToText(args1, "HEX2OCT", 1);
            if (args1.IsError) { return args1; }
            if (RegexHelper.HexRegex.IsMatch(args1.TextValue) == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2OCT", 1); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 8);
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter);
                args2 = ConvertToNumber(args2, "HEX2OCT", 2);
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "HEX2OCT", 2);
            }
            return Operand.Create(num);
        }

    }

    

}
