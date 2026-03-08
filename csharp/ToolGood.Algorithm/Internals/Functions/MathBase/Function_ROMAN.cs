using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_ROMAN : Function_N
	{
		public Function_ROMAN(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "ROMAN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 1) return ParameterError(1);

			var numArg = GetNumber(engine, tempParameter, 0);
			if (numArg.IsError) return numArg;
			var num = numArg.IntValue;

			if (num < 0 || num > 3999) return Operand.Create(string.Empty);

			int form = 0;
			if (funcs.Length > 1) {
				var formArg = GetNumber(engine, tempParameter, 1);
				if (formArg.IsError) return formArg;
				form = formArg.IntValue;
			}

			return Operand.Create(ArabicToRoman(num, form));
		}

		private string ArabicToRoman(int num, int form)
		{
			if (num == 0) return string.Empty;

			var sb = new StringBuilder();
			int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

			for (int i = 0; i < values.Length; i++) {
				while (num >= values[i]) {
					sb.Append(numerals[i]);
					num -= values[i];
				}
			}

			return sb.ToString();
		}
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}
}
