using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_EDATE : Function_2
    {
		public Function_EDATE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "EDate";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }

    }

}
