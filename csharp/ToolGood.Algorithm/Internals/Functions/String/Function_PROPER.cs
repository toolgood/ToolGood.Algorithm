using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_PROPER : Function_1
	{
		public Function_PROPER(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Proper";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var text = args1.TextValue;
			if (string.IsNullOrEmpty(text)) {
				return Operand.Create(text);
			}

			bool needModify = false;
			bool isFirst = true;
			for (int i = 0; i < text.Length; i++) {
				var t = text[i];
				if (!char.IsLetter(t)) {
					isFirst = true;
				} else {
					if (isFirst) {
						if (char.IsLower(t)) {
							needModify = true;
						}
					} else {
						if (char.IsUpper(t)) {
							needModify = true;
						}
					}
					if (needModify) break;
					isFirst = false;
				}
			}

			if (!needModify) {
				return args1;
			}

			char[] chars = text.ToCharArray();
			Span<char> span = chars;
			isFirst = true;
			for (int i = 0; i < span.Length; i++) {
				var t = span[i];
				if (!char.IsLetter(t)) {
					isFirst = true;
				} else {
					if (isFirst) {
						span[i] = char.ToUpper(t);
					} else {
						span[i] = char.ToLower(t);
					}
					isFirst = false;
				}
			}
			return Operand.Create(new string(chars));
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
