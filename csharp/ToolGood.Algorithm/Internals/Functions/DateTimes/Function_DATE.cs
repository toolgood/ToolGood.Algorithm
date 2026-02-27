using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_DATE : Function_N
    {
        public Function_DATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Date";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber(engine, tempParameter, 0);
			if (args1.IsError) { return args1; }

			var args2 = GetNumber(engine, tempParameter, 1);
			if (args2.IsError) { return args2; }

			var args3 = GetNumber(engine, tempParameter, 2);
			if (args3.IsError) { return args3; }

            MyDate d;
            if (funcs.Length == 3) {
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
            } else if (funcs.Length == 4) {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
            } else if (funcs.Length == 5) {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }

                var args5 = GetNumber(engine, tempParameter, 4);
                if (args5.IsError) { return args5; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
            } else {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }

                var args5 = GetNumber(engine, tempParameter, 4);
                if (args5.IsError) { return args5; }

                var args6 = GetNumber(engine, tempParameter, 5);
                if (args6.IsError) { return args6; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
            }
            return Operand.Create(d);
        }

    }

}