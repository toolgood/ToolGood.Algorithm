using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TIME : Function_3
    {
		public Function_TIME(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Time";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }

			MyDate d;
			if (func3 != null) {
				var args3 = GetNumber_3(engine, tempParameter);
				if (args3.IsError) { return args3; }
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
			} else {
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
			}
            return Operand.Create(d);
        }

    }

}
