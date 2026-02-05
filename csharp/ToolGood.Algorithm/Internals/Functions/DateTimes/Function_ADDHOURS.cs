using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
    internal class Function_ADDHOURS : Function_2
    {
		public Function_ADDHOURS(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "AddHours";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            var date = args1.DateValue.AddHours(args2.IntValue);
            return Operand.Create(date);
        }

    }

}
