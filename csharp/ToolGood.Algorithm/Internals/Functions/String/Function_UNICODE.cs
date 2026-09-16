using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UNICODE : Function_1
	{
		public Function_UNICODE(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Unicode";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var text = args1.TextValue;
			if (string.IsNullOrEmpty(text)) {
				return ParameterError(1);
			}
			// 首字符为孤立代理项时 char.ConvertToUtf32 会抛出 ArgumentException,此处按参数错误处理
			var first = text[0];
			if (char.IsHighSurrogate(first)) {
				if (text.Length == 1 || char.IsLowSurrogate(text[1]) == false) {
					return ParameterError(1);
				}
				return Operand.Create(char.ConvertToUtf32(first, text[1]));
			}
			if (char.IsLowSurrogate(first)) {
				return ParameterError(1);
			}
			return Operand.Create((int)first);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}
}
