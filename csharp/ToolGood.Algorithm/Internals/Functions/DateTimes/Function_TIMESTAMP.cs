using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TIMESTAMP : Function_2
	{
		public Function_TIMESTAMP(FunctionBase[] funcs) : base(funcs)
		{
		}



		public override string Name => "Timestamp";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			int type = 0; // 毫秒
			if(func2 != null) {
				var args2 = GetNumber_2(work, tempParameter);
				if(args2.IsError) { return args2; }
				type = args2.IntValue;
			}
			var args0 = GetDate_1(work, tempParameter); if(args0.IsError) { return args0; }
			DateTime args1;
			if(work.UseLocalTime) {
				args1 = args0.DateValue.ToDateTime(DateTimeKind.Local).ToUniversalTime();
			} else {
				args1 = args0.DateValue.ToDateTime(DateTimeKind.Utc);
			}
			if(type == 0) {
				var ms = (args1 - FunctionUtil.StartDateUtc).TotalMilliseconds;
				return Operand.Create(ms);
			} else if(type == 1) {
				var s = (args1 - FunctionUtil.StartDateUtc).TotalSeconds;
				return Operand.Create(s);
			}
			return FunctionError();
		}

	}

}
