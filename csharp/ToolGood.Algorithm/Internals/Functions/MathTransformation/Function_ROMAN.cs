using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_ROMAN : Function_2
	{
		public Function_ROMAN(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		public override string Name => "ROMAN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var numArg = GetNumber_1(engine, tempParameter);
			if (numArg.IsErrorOrNone) return numArg;
			var num = numArg.IntValue;

			// Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
			if (num < 0 || num > 3999) return ParameterError(1);

			int form = 0;
			if (func2 != null) {
				var formArg = GetNumber_2(engine, tempParameter);
				if (formArg.IsErrorOrNone) return formArg;
				form = formArg.IntValue;
				// Excel: form 超出 0~4 范围返回 #VALUE!
				if (form < 0 || form > 4) return ParameterError(2);
			}

			return Operand.Create(ArabicToRoman(num, form));
		}

		private static readonly int[][] RomanValues = {
			new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 },
			new[] { 1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1 },
			new[] { 1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1 },
			new[] { 1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1 },
			new[] { 1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1 }
		};

		private static readonly string[][] RomanSymbols = {
			new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" },
			new[] { "M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV", "I" },
			new[] { "M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I" },
			new[] { "M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I" },
			new[] { "M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I" }
		};

		// form 0: 经典形式(标准减法表示)
		// form 1: 增加 V-L=45, V-C=95, L-D=450, L-M=950
		// form 2: 再增加 I-L=49, I-C=99, X-D=490, X-M=990
		// form 3: 再增加 V-D=495, V-M=995
		// form 4: 再增加 I-D=499, I-M=999(最简化)
		private string ArabicToRoman(int num, int form)
		{
			if (num == 0) return string.Empty;

			var sb = new StringBuilder(16);
			var values = RomanValues[form];
			var symbols = RomanSymbols[form];
			for (int i = 0; i < values.Length; i++) {
				while (num >= values[i]) {
					sb.Append(symbols[i]);
					num -= values[i];
				}
			}

			return sb.ToString();
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
