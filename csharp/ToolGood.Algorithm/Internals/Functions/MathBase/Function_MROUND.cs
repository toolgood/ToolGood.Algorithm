using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_MROUND : Function_2
    {
        public Function_MROUND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "MRound", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "MRound", 2);
			if (args2.IsError) { return args2; }
            var a = args2.NumberValue;
            if (a <= 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "MRound", 2); }

            var b = args1.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "MRound");
        }
    }

    

}
