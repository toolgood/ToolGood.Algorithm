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
		if (num < 0 || num > 3999) return Operand.Create("");
		int form = 0;
		if (func2 != null) {
			Operand formArg = GetNumber_2(engine, tempParameter);
			if (formArg.IsErrorOrNone()) return formArg;
			form = formArg.IntValue();
		}
		return Operand.Create(ArabicToRoman(num));
	}

	private String ArabicToRoman(int num) {
		if (num == 0) { return ""; }
		StringBuilder sb = new StringBuilder(16);
		int[] values = {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};
		String[] numerals = {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};
		for (int i = 0; i < values.length; i++) {
			while (num >= values[i]) {
				sb.append(numerals[i]);
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
