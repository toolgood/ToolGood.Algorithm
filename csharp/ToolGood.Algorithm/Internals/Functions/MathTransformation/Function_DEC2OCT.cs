using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal class Function_DEC2OCT : Function_2
    {
        public Function_DEC2OCT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Dec2Oct";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "DEC2OCT", 1);
            if (args1.IsError) { return args1; }
            var num = Convert.ToString(args1.IntValue, 8);
            if (func2 != null) {
                var args2 = func2.Evaluate(work, tempParameter);
                args2 = FunctionUtil.ConvertToNumber(args2, "DEC2OCT", 2);
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function '{0}' parameter {1} is error!", "DEC2OCT", 2);
            }
            return Operand.Create(num);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DEC2OCT");
        }
    }

    

}
