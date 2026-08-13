using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_ARABIC : Function_1
	{
		public Function_ARABIC(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "ARABIC";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var arg = GetText_1(engine, tempParameter);
			if (arg.IsErrorOrNone) return arg;
			var text = arg.TextValue.ToUpperInvariant();
			// Excel: 空文本或不符合罗马数字语法(非法字符/非法减法/非法重复)时返回 #VALUE!
			if (text.Length == 0 || !RomanRegex.IsMatch(text)) return ParameterError(1);
			var result = RomanToArabic(text);
			if (result < 0) return ParameterError(1);
			return Operand.Create(result);
		}

		// 标准罗马数字(1~3999)语法:千位 M 最多 3 个,百/十/个位遵循减法规则且不可非法重复
		private static readonly System.Text.RegularExpressions.Regex RomanRegex =
			new System.Text.RegularExpressions.Regex("^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");

		private int RomanToArabic(string roman)
		{
			int result = 0;
			int prevValue = 0;

			for (int i = roman.Length - 1; i >= 0; i--) {
				int value = GetRomanValue(roman[i]);
				if (value < 0) return -1;
				if (value < prevValue) {
					result -= value;
				} else {
					result += value;
				}
				prevValue = value;
			}

			return result;
		}

		private int GetRomanValue(char c)
		{
			switch (c) {
				case 'I': return 1;
				case 'V': return 5;
				case 'X': return 10;
				case 'L': return 50;
				case 'C': return 100;
				case 'D': return 500;
				case 'M': return 1000;
				default: return -1;
			}
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
