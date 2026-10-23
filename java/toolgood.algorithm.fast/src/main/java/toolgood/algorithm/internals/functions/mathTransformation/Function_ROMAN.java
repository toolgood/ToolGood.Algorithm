package toolgood.algorithm.internals.functions.mathTransformation;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_ROMAN extends Function_2 {
	public Function_ROMAN(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "ROMAN"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand numArg = GetNumber_1(engine, tempParameter);
		if (numArg.IsErrorOrNone()) return numArg;
		int num = numArg.IntValue();
		// Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
		if (num < 0 || num > 3999) return ParameterError(1);
		int form = 0;
		if (func2 != null) {
			Operand formArg = GetNumber_2(engine, tempParameter);
			if (formArg.IsErrorOrNone()) return formArg;
			form = formArg.IntValue();
			// Excel: form 超出 0~4 范围返回 #VALUE!
			if (form < 0 || form > 4) return ParameterError(2);
		}
		return Operand.Create(ArabicToRoman(num, form));
	}

	// form 0: 经典形式(标准减法表示)
	// form 1: 增加 V-L=45, V-C=95, L-D=450, L-M=950
	// form 2: 再增加 I-L=49, I-C=99, X-D=490, X-M=990
	// form 3: 再增加 V-D=495, V-M=995
	// form 4: 再增加 I-D=499, I-M=999(最简化)
	private static final int[][] ROMAN_VALUES = {
		{1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1},
		{1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1},
		{1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
		{1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
		{1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1}
	};

	private static final String[][] ROMAN_SYMBOLS = {
		{"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"},
		{"M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV", "I"},
		{"M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"},
		{"M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"},
		{"M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"}
	};

	private String ArabicToRoman(int num, int form) {
		if (num == 0) { return ""; }
		StringBuilder sb = new StringBuilder(16);
		int[] values = ROMAN_VALUES[form];
		String[] symbols = ROMAN_SYMBOLS[form];
		for (int i = 0; i < values.length; i++) {
			while (num >= values[i]) {
				sb.append(symbols[i]);
				num -= values[i];
			}
		}
		return sb.toString();
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		if (func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
	}
}
