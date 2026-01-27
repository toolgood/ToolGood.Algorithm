using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	#region isXXXX

	internal class Function_ISLOGICAL : Function_1
    {
        public Function_ISLOGICAL(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsBoolean) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsLogical");
        }
    }

    #endregion
}
