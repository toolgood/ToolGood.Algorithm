using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DATE : Function_6
    {
        public Function_DATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Date";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }

			var args3 = GetNumber_3(engine, tempParameter);
			if (args3.IsErrorOrNone) { return args3; }

			var year = args1.IntValue;
			var month = args2.IntValue;
			var day = args3.IntValue;

			if (month < 1 || month > 12) {
				return ParameterError(2);
			}
			if (day < 1 || day > 31) {
				return ParameterError(3);
			}

            MyDate d;
            if (func4 == null) {
                d = new MyDate(year, month, day, 0, 0, 0);
            } else if (func5 == null) {
                var args4 = GetNumber_4(engine, tempParameter);
                if (args4.IsErrorOrNone) { return args4; }
				var hour = args4.IntValue;
				if (hour < 0 || hour > 23) {
					return ParameterError(4);
				}
                d = new MyDate(year, month, day, hour, 0, 0);
            } else if (func6 == null) {
                var args4 = GetNumber_4(engine, tempParameter);
                if (args4.IsErrorOrNone) { return args4; }

                var args5 = GetNumber_5(engine, tempParameter);
                if (args5.IsErrorOrNone) { return args5; }

				var hour = args4.IntValue;
				var minute = args5.IntValue;
				if (hour < 0 || hour > 23) {
					return ParameterError(4);
				}
				if (minute < 0 || minute > 59) {
					return ParameterError(5);
				}
                d = new MyDate(year, month, day, hour, minute, 0);
            } else {
                var args4 = GetNumber_4(engine, tempParameter);
                if (args4.IsErrorOrNone) { return args4; }

                var args5 = GetNumber_5(engine, tempParameter);
                if (args5.IsErrorOrNone) { return args5; }

                var args6 = GetNumber_6(engine, tempParameter);
                if (args6.IsErrorOrNone) { return args6; }

				var hour = args4.IntValue;
				var minute = args5.IntValue;
				var second = args6.IntValue;
				if (hour < 0 || hour > 23) {
					return ParameterError(4);
				}
				if (minute < 0 || minute > 59) {
					return ParameterError(5);
				}
				if (second < 0 || second > 59) {
					return ParameterError(6);
				}
                d = new MyDate(year, month, day, hour, minute, second);
            }
            return Operand.Create(d);
        }
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
