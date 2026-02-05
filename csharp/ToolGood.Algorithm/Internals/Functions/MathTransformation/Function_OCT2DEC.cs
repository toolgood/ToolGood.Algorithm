using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_OCT2DEC : Function_1
    {
        public Function_OCT2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "Oct2Dec";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = ConvertToText(args1, "OCT2DEC", 1);
            if (args1.IsError) { return args1; }

            if (RegexHelper.OctRegex.IsMatch(args1.TextValue) == false) { return Operand.Error("Function '{0}' parameter is error!", "OCT2DEC"); }
            var num = Convert.ToInt32(args1.TextValue, 8);
            return Operand.Create(num);
        }
    }

    

}
