using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_BIN2DEC : Function_1
    {
        public Function_BIN2DEC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "BIN2DEC"); if (args1.IsError) { return args1; } }

            if (RegexHelper.BinRegex.IsMatch(args1.TextValue) == false) { return Operand.Error("Function '{0}' parameter is error!", "BIN2DEC"); }
            var num = Convert.ToInt32(args1.TextValue, 2);
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BIN2DEC");
        }
    }

    

}
