using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_ADDDAYS : Function_2
    {
        public Function_ADDDAYS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "AddDays";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddDays(args2.DoubleValue)));
        }

    } 

}
