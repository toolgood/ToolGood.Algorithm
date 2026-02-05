using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_HEX2DEC : Function_1
    {
        public Function_HEX2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Hex2Dec";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            if (RegexHelper.HexRegex.IsMatch(args1.TextValue) == false) { return FunctionError(); }
            var num = Convert.ToInt32(args1.TextValue, 16);
            return Operand.Create(num);
        }

    }

    

}
