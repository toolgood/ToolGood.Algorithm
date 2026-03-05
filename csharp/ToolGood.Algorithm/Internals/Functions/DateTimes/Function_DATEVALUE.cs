using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DATEVALUE : Function_N
	{
		public Function_DATEVALUE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "DateValue";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			foreach(var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if(aa.IsError) { return aa; } args.Add(aa); }
			if(args[0].IsDate) { return args[0]; }
			int type = 0;
			if(args.Count == 2) {
				var args2 = ConvertToNumber(args[1], 2);
				if(args2.IsError) { return args2; }
				type = args2.IntValue;
			}
			if(type == 0) {
				if(args[0].IsText) {
					if(DateTime.TryParse(args[0].TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
						return Operand.Create(time);
					}
				}
				var args1 = ConvertToNumber(args[0], 1);
				if(args1.IsError) { return args1; }
				if(args1.LongValue <= 2958465L) { // 9999-12-31 日时间在excel的数字为 2958465
					return args1.ToMyDate();
				}
				if(args1.LongValue <= 253402232399L) { // 9999-12-31 12:59:59 日时�?�?时间�?�?253402232399L�?
					var time = FunctionUtil.StartDateUtc.AddSeconds(args1.LongValue);
					if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
					return Operand.Create(time);
				}
				// 注：时间�?253402232399 ms 转时�?�?1978-01-12 05:30:32
				var time2 = FunctionUtil.StartDateUtc.AddMilliseconds(args1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time2.ToLocalTime()); }
				return Operand.Create(time2);
			} else if(type == 1) {
				var args1 = ConvertToText(args[0], 1);
				if(args1.IsError) { return args1; }
				if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					return Operand.Create(dt);
				}
			} else if(type == 2) {
				var args1 = ConvertToNumber(args[0], 1);
				if(args1.IsError) { return args1; }
				return args1.ToMyDate();
			} else if(type == 3) {
				var args1 = ConvertToNumber(args[0], 1);
				if(args1.IsError) { return args1; }
				var time = FunctionUtil.StartDateUtc.AddMilliseconds(args1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
				return Operand.Create(time);
			} else if(type == 4) {
				var args1 = ConvertToNumber(args[0], 1);
				if(args1.IsError) { return args1; }
				var time = FunctionUtil.StartDateUtc.AddSeconds(args1.LongValue);
				if(engine.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
				return Operand.Create(time);
			}
			return FunctionError();
		}

	}

}
