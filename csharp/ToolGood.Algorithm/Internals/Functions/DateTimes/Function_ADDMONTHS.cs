using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_ADDMONTHS : Function_2
    {
		public Function_ADDMONTHS(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "AddMonths";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }

    }

}
