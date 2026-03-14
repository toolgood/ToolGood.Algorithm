using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_WEEKNUM : Function_2
    {
		public Function_WEEKNUM(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Weeknum";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var startMyDate = args1.DateValue.ToDateTime();

			var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if (args2.IsErrorOrNone) { return args2; }
				if (args2.IntValue != 1 && args2.IntValue != 2) {
					return ParameterError(2);
				}
				if (args2.IntValue == 2) {
					days--;
				}
			}

            var week = Math.Ceiling(days / 7.0m);
            return Operand.Create(week);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
