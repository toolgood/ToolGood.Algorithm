using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_BIN2DEC : Function_1
    {
        public Function_BIN2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Bin2Dec";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            if (RegexHelper.BinRegex.IsMatch(args1.TextValue) == false) { return FunctionError(); }
            var num = Convert.ToInt32(args1.TextValue, 2);
            return Operand.Create(num);
        }

    }

    

}
