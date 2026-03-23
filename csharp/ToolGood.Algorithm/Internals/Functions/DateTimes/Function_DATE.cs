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

			DateTime baseDate;
			try {
				baseDate = new DateTime(year, 1, 1);
				baseDate = baseDate.AddMonths(month - 1);
				baseDate = baseDate.AddDays(day - 1);
			} catch {
				return ParameterError(1);
			}

			int hour = 0, minute = 0, second = 0;
            if (func4 != null) {
                var args4 = GetNumber_4(engine, tempParameter);
                if (args4.IsErrorOrNone) { return args4; }
				hour = args4.IntValue;
				if (hour < 0 || hour > 23) {
					return ParameterError(4);
				}
            }
			if (func5 != null) {
                var args5 = GetNumber_5(engine, tempParameter);
                if (args5.IsErrorOrNone) { return args5; }
				minute = args5.IntValue;
				if (minute < 0 || minute > 59) {
					return ParameterError(5);
				}
            }
			if (func6 != null) {
                var args6 = GetNumber_6(engine, tempParameter);
                if (args6.IsErrorOrNone) { return args6; }
				second = args6.IntValue;
				if (second < 0 || second > 59) {
					return ParameterError(6);
				}
            }

			MyDate d = new MyDate(baseDate.Year, baseDate.Month, baseDate.Day, hour, minute, second);
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
