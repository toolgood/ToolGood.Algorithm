using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_TIMESTAMP : Function_2
	{
		public Function_TIMESTAMP(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Timestamp";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			int type = 0; // 类型
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				type = args2.IntValue;
			}
			var args0 = GetDate_1(engine, tempParameter); if(args0.IsErrorOrNone) { return args0; }
			DateTime args1;
			if(engine.UseLocalTime) {
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
