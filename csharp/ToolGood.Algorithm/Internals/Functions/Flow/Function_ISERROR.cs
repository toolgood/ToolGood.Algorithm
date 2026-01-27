using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
	#region isXXXX

	internal class Function_ISERROR : Function_2
    {
        public Function_ISERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (func2 != null) {
                if (args1.IsError) { return func2.Evaluate(work, tempParameter); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsError");
        }
    }

    #endregion
}
