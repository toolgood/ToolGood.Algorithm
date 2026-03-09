using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ARABIC : Function_1
	{
		public Function_ARABIC(FunctionBase func1) : base(func1) { }

		public override string Name => "ARABIC";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var arg = GetText_1(engine, tempParameter);
			if (arg.IsErrorOrNone) return arg;
			var text = arg.TextValue.ToUpperInvariant();
			return Operand.Create(RomanToArabic(text));
		}

		private int RomanToArabic(string roman)
		{
			int result = 0;
			int prevValue = 0;

			for (int i = roman.Length - 1; i >= 0; i--) {
				int value = GetRomanValue(roman[i]);
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
				default: return 0;
			}
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
