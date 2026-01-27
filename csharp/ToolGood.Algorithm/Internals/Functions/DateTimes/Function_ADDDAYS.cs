using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_ADDDAYS : Function_2
    {
        public Function_ADDDAYS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddDays", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddDays", 2); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddDays(args2.DoubleValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddDays");
        }
    } 

}
