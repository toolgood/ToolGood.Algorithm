using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DATE : Function_N
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

			var year = args1.IntValue;
			var month = args2.IntValue;
			var day = args3.IntValue;

			if (month < 1 || month > 12 || day < 1 || day > 31) {
				return ParameterError(1);
			}

            MyDate d;
            if (funcs.Length == 3) {
                d = new MyDate(year, month, day, 0, 0, 0);
            } else if (funcs.Length == 4) {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }
				var hour = args4.IntValue;
				if (hour < 0 || hour > 23) {
					return ParameterError(4);
				}
                d = new MyDate(year, month, day, hour, 0, 0);
            } else if (funcs.Length == 5) {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }

                var args5 = GetNumber(engine, tempParameter, 4);
                if (args5.IsError) { return args5; }

				var hour = args4.IntValue;
				var minute = args5.IntValue;
				if (hour < 0 || hour > 23 || minute < 0 || minute > 59) {
					return ParameterError(4);
				}
                d = new MyDate(year, month, day, hour, minute, 0);
            } else {
                var args4 = GetNumber(engine, tempParameter, 3);
                if (args4.IsError) { return args4; }

                var args5 = GetNumber(engine, tempParameter, 4);
                if (args5.IsError) { return args5; }

                var args6 = GetNumber(engine, tempParameter, 5);
                if (args6.IsError) { return args6; }

				var hour = args4.IntValue;
				var minute = args5.IntValue;
				var second = args6.IntValue;
				if (hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59) {
					return ParameterError(4);
				}
                d = new MyDate(year, month, day, hour, minute, second);
            }
            return Operand.Create(d);
        }

    }

}