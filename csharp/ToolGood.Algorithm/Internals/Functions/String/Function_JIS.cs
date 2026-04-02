using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_JIS : Function_1
	{
		public Function_JIS(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Jis";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			return Operand.Create(F_base_ToSBC(args1.TextValue));
		}

		private static string F_base_ToSBC(string input)
		{
			bool needModify = false;
			for (int i = 0; i < input.Length; i++) {
				var c = input[i];
				if (c == ' ' || c < 127) {
					needModify = true;
					break;
				}
			}
			if (!needModify) {
				return input;
			}
			char[] chars = input.ToCharArray();
			Span<char> span = chars;
			for (int i = 0; i < span.Length; i++) {
				var c = span[i];
				if (c == ' ') {
					span[i] = (char)12288;
				} else if (c < 127) {
					span[i] = (char)(c + 65248);
				}
			}
			return new string(chars);
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
