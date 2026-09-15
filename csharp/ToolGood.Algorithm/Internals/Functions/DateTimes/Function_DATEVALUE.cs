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
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		public override string Name => "DateValue";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			int type = 0;
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				type = args2.IntValue;
			}
			// 先校验第 2 参数，避免非法 type 被 args1 是日期的短路逻辑静默忽略
			if(type < 0 || type > 4) { return ParameterError(2); }
			if(args1.IsDate) { return args1; }

			if(type == 0) {
				if(args1.IsText) {
					if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
						return Operand.Create(time);
					}
				}
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				if(arg1.LongValue <= 2958465L) {
					// Excel/OLE 序列号：以 1899-12-30 为 0 的日数，2958465 对应 9999-12-31
					if(arg1.NumberValue < -657435M) { return ParameterError(1); }
					return Operand.Create(DateTime.FromOADate(arg1.DoubleValue));
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
				return ParameterError(1);
			} else if(type == 2) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				// 与 type==0 的数字分支一致：按 Excel/OLE 序列号解析
				if(arg1.NumberValue < -657435M || arg1.NumberValue > 2958465.99999999M) { return ParameterError(1); }
				return Operand.Create(DateTime.FromOADate(arg1.DoubleValue));
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
			return ParameterError(2);
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
