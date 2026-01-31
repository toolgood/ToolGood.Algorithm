using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_MONTH : Function_1
    {
        public Function_MONTH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Month"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Month == null) {
                return Operand.Error("Function '{0}' is error!", "Month");
            }
            return Operand.Create((int)args1.DateValue.Month.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Month");
        }
    }

}
