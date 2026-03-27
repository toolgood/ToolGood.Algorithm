using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_TIME : Function_3
    {
		public Function_TIME(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Time";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }

			var hour = args1.IntValue;
			var minute = args2.IntValue;
			if (hour < 0 || hour > 23) {
				return ParameterError(1);
			}
			if (minute < 0 || minute > 59) {
				return ParameterError(2);
			}

			MyDate d;
			if (func3 != null) {
				var args3 = GetNumber_3(engine, tempParameter);
				if (args3.IsErrorOrNone) { return args3; }
				var second = args3.IntValue;
				if (second < 0 || second > 59) {
					return ParameterError(3);
				}
				d = new MyDate(0, 0, 0, hour, minute, second);
			} else {
				d = new MyDate(0, 0, 0, hour, minute, 0);
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
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
