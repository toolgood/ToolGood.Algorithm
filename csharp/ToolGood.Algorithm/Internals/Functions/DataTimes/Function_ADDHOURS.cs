using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DataTimes
{
    internal class Function_ADDHOURS : Function_2
    {
        public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddHours", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddHours", 2); if (args2.IsError) { return args2; } }
            var date = args1.DateValue.AddHours(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddHours");
        }
    }

}
