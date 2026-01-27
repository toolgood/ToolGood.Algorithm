using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_EOMONTH : Function_2
    {
        public Function_EOMONTH(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotDate) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "EoMonth", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "EoMonth", 2); if (args2.IsError) { return args2; } }
            var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "EoMonth");
        }
    }

}
