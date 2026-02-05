using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	internal class Function_ISEVEN : Function_1
    {
        public Function_ISEVEN(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsEven";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNumber) {
                if (args1.IntValue % 2 == 0) { return Operand.True; }
            }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsEven");
        }
    }

}
