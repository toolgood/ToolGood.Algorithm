using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISTEXT : Function_1
    {
        public Function_ISTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsText";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsText) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsText");
        }
    }

}
