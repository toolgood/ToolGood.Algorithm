using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_PROPER : Function_1
	{
		public Function_PROPER(FunctionBase func1) : base(func1)
		{
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
				if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
					isFirst = true;
				} else if (isFirst) {
					if (char.IsLower(t)) {
						needModify = true;
						break;
					}
					isFirst = false;
				}
			}
			if (!needModify) {
				return args1; // no change
			}
			char[] chars = text.ToCharArray();
			Span<char> span = chars;
			isFirst = true;
			for (int i = 0; i < span.Length; i++) {
				var t = span[i];
				if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
					isFirst = true;
				} else if (isFirst) {
					span[i] = char.ToUpper(t);
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
