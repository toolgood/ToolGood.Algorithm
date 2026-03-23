using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	#region second minute hour month year day

	#endregion
	internal sealed class Function_WEEKDAY : Function_2
    {
		public Function_WEEKDAY(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Weekday";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var type = 1;
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if (args2.IsErrorOrNone) { return args2; }
				type = args2.IntValue;
				if (type != 1 && type != 2 && type != 3 && (type < 11 || type > 17)) {
					return ParameterError(2);
				}
			}

            var dayOfWeek = (int)args1.DateValue.ToDateTime().DayOfWeek;

            if (type == 1 || type == 17) {
                return Operand.Create(dayOfWeek + 1);
            } else if (type == 2 || type == 11) {
                if (dayOfWeek == 0) return Operand.Create(7);
                return Operand.Create(dayOfWeek);
            } else if (type == 3) {
                if (dayOfWeek == 0) return Operand.Create(6);
                return Operand.Create(dayOfWeek - 1);
            } else if (type == 12) {
                int[] mapping = { 6, 7, 1, 2, 3, 4, 5 };
                return Operand.Create(mapping[dayOfWeek]);
            } else if (type == 13) {
                int[] mapping = { 5, 6, 7, 1, 2, 3, 4 };
                return Operand.Create(mapping[dayOfWeek]);
            } else if (type == 14) {
                int[] mapping = { 4, 5, 6, 7, 1, 2, 3 };
                return Operand.Create(mapping[dayOfWeek]);
            } else if (type == 15) {
                int[] mapping = { 3, 4, 5, 6, 7, 1, 2 };
                return Operand.Create(mapping[dayOfWeek]);
            } else if (type == 16) {
                int[] mapping = { 2, 3, 4, 5, 6, 7, 1, 2 };
                return Operand.Create(mapping[dayOfWeek]);
            }

            return ParameterError(2);
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
