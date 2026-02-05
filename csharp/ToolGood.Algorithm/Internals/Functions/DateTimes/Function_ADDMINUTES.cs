using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_ADDMINUTES : Function_2
    {
		public Function_ADDMINUTES(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "AddMinutes";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }
            var date = args1.DateValue.AddMinutes(args2.IntValue);
            return Operand.Create(date);
        }

    }

}
