using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDSECONDS : Function_2
    {
		public Function_ADDSECONDS(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "AddSeconds";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            var date = args1.DateValue.AddSeconds(args2.IntValue);
            return Operand.Create(date);
        }

    }

}
