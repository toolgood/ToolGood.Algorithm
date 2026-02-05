using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_ADDMONTHS : Function_2
    {
        public Function_ADDMONTHS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "AddMonths";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToDate(args1, "AddMonths", 1);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "AddMonths", 2);
            if (args2.IsError) { return args2; }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddMonths");
        }
    }

}
