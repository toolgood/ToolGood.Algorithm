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
				try {
					type = args2.IntValue;
				} catch (OverflowException) {
					return ParameterError(2);
				}
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
				// 用 NumberValue 判定分段：LongValue 会截断小数，导致临界值落入错误分支
				// Excel/OLE 序列号：以 1899-12-30 为 0 的日数，2958465.99999999 对应 9999-12-31 23:59:59
				if(arg1.NumberValue <= 2958465.99999999M) {
					if(arg1.NumberValue <= -657435M) { return ParameterError(1); }
					try {
						return Operand.Create(DateTime.FromOADate(arg1.DoubleValue));
					} catch (ArgumentException) {
						return ParameterError(1);
					}
				}
				// Unix 时间戳（秒）：LongValue 按截断取整，故上界取 253402300800（不含），
				// 使 253402300799.9 这类值截断后仍落到秒分支，不会掉入毫秒分支产生量级跳变
				if(arg1.NumberValue < 253402300800M) {
					var time = FunctionUtil.StartDateUtc.AddSeconds(arg1.LongValue);
					if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
					return Operand.Create(time);
				}
				// Unix 时间戳（毫秒）：上限 9999-12-31 23:59:59.999，同样按截断粒度对齐上界
				if(arg1.NumberValue >= 253402300800000M) { return ParameterError(1); }
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
				// 与 type==0 的数字分支一致：按 Excel/OLE 序列号解析（下界为开区间）
				if(arg1.NumberValue <= -657435M || arg1.NumberValue > 2958465.99999999M) { return ParameterError(1); }
				try {
					return Operand.Create(DateTime.FromOADate(arg1.DoubleValue));
				} catch (ArgumentException) {
					return ParameterError(1);
				}
			} else if(type == 3) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				try {
					var time = FunctionUtil.StartDateUtc.AddMilliseconds(arg1.LongValue);
					if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
					return Operand.Create(time);
				} catch (ArgumentOutOfRangeException) {
					return ParameterError(1);
				} catch (OverflowException) {
					// LongValue 为 decimal 到 long 的直接转换，超范围抛 OverflowException
					return ParameterError(1);
				}
			} else if(type == 4) {
				var arg1 = ConvertToNumber(args1, 1);
				if(arg1.IsErrorOrNone) { return arg1; }
				try {
					var time = FunctionUtil.StartDateUtc.AddSeconds(arg1.LongValue);
					if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
					return Operand.Create(time);
				} catch (ArgumentOutOfRangeException) {
					return ParameterError(1);
				} catch (OverflowException) {
					// LongValue 为 decimal 到 long 的直接转换，超范围抛 OverflowException
					return ParameterError(1);
				}
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
