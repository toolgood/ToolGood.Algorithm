using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISNULLOREMPTY : Function_1
    {
        public Function_ISNULLOREMPTY(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsNullOrEmpty";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNull) { return Operand.True; }
            args1 = FunctionUtil.ConvertToText(args1, "IsNullOrEmpty", 1);
			if (args1.IsError) { return args1; }
            return Operand.Create(string.IsNullOrEmpty(args1.TextValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNullOrEmpty");
        }
    }

}
