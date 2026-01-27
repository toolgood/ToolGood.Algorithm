using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DataTimes
{
	internal class Function_YEAR : Function_1
    {
        public Function_YEAR(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Year"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Year == null) {
                return Operand.Error("Function 'Year' is error!");
            }
            return Operand.Create(args1.DateValue.Year.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Year");
        }
    }

}
