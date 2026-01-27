using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DataTimes
{
	internal class Function_ADDMINUTES : Function_2
    {
        public Function_ADDMINUTES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddMinutes", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddMinutes", 2); if (args2.IsError) { return args2; } }
            var date = args1.DateValue.AddMinutes(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddMinutes");
        }
    }

}
