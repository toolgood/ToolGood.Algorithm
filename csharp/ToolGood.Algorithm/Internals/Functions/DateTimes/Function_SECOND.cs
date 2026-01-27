using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_SECOND : Function_1
    {
        public Function_SECOND(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Second"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Second);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Second");
        }
    }

}
