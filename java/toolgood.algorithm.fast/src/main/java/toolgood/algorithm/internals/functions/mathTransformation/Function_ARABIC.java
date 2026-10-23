package toolgood.algorithm.internals.functions.mathTransformation;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public final class Function_ARABIC extends Function_1 {
	public Function_ARABIC(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 1) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
		}
	}

	@Override
	public String Name() { return "ARABIC"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand arg = GetText_1(engine, tempParameter);
		if (arg.IsErrorOrNone()) return arg;
		String text = arg.TextValue().toUpperCase();
		// Excel: 空文本或不符合罗马数字语法(非法字符/非法减法/非法重复)时返回 #VALUE!
		if (text.length() == 0 || !RomanRegex.matcher(text).matches()) return ParameterError(1);
		int result = RomanToArabic(text);
		if (result < 0) return ParameterError(1);
		return Operand.Create(BigDecimal.valueOf(result));
	}

	// 标准罗马数字(1~3999)语法:千位 M 最多 3 个,百/十/个位遵循减法规则且不可非法重复
	private static final java.util.regex.Pattern RomanRegex =
		java.util.regex.Pattern.compile("^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");

	private int RomanToArabic(String roman) {
		int result = 0;
		int prevValue = 0;
		for (int i = roman.length() - 1; i >= 0; i--) {
			int value = GetRomanValue(roman.charAt(i));
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

	private int GetRomanValue(char c) {
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

	@Override
	public OperandType GetResultType() {
		return OperandType.NUMBER;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
	}
}
