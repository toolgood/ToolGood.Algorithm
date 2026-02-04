using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
    internal class Function_ADDHOURS : Function_2
    {
        public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToDate(args1, "AddHours", 1);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "AddHours", 2);
            if (args2.IsError) { return args2; }
            var date = args1.DateValue.AddHours(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddHours");
        }
    }

}
