using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_LOG : Function_2
    {
        public Function_LOG(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Log", 1);
			if (args1.IsError) { return args1; }

			if (func2 == null)
				return Operand.Create(Math.Log10(args1.DoubleValue));

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Log", 2);
			if (args2.IsError) { return args2; }
			return Operand.Create(Math.Log(args1.DoubleValue, args2.DoubleValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Log");
        }
    }

    

}
