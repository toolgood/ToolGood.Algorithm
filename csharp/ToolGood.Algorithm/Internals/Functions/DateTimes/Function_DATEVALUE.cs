using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DATEVALUE : Function_2
	{
		public Function_DATEVALUE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "DateValue";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			if(args1.IsDate) { return args1; }
			
			int type = 0;
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				type = args2.IntValue;
			}
			if(type == 0) {
				if(args1.IsText) {
					if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
						return Operand.Create(time);
					}
				}
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				if(arg1.LongValue <= 2958465L) {
					return arg1.ToMyDate();
				}
				if(arg1.LongValue <= 253402232399L) {
					var time = FunctionUtil.StartDateUtc.AddSeconds(arg1.LongValue);
					if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
					return Operand.Create(time);
				}
				var time2 = FunctionUtil.StartDateUtc.AddMilliseconds(arg1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time2.ToLocalTime()); }
				return Operand.Create(time2);
			} else if(type == 1) {
				var arg1 = ConvertToText(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				if(DateTime.TryParse(arg1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					return Operand.Create(dt);
				}
			} else if(type == 2) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				return arg1.ToMyDate();
			} else if(type == 3) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				var time = FunctionUtil.StartDateUtc.AddMilliseconds(arg1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
				return Operand.Create(time);
			} else if(type == 4) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				var time = FunctionUtil.StartDateUtc.AddSeconds(arg1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
				return Operand.Create(time);
			}
			return ParameterError(1);
		}
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
