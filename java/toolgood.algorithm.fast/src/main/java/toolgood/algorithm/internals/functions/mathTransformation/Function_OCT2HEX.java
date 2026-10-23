package toolgood.algorithm.internals.functions.mathTransformation;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.RegexHelper;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_OCT2HEX extends Function_2 {
	public Function_OCT2HEX(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Oct2Hex"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		if (!RegexHelper.IsOct(args1.TextValue())) { return ParameterError(1); }
		String text = args1.TextValue();
		if (text.length() > 10) { return ParameterError(1); }
		// 10 位八进制补码解析
		int num = Integer.parseInt(text, 8);
		if (num >= 536870912) { num -= 1073741824; }
		String hex;
		if (num < 0) {
			// 负数:10 位十六进制补码
			hex = String.format("%10s", Long.toHexString(num + 0x10000000000L).toUpperCase()).replace(' ', '0');
		} else {
			hex = Integer.toHexString(num).toUpperCase();
		}
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0 || args2.IntValue() > 10) {
				return ParameterError(2);
			}
			if (hex.length() > args2.IntValue()) {
				return ParameterError(2);
			}
			return Operand.Create(String.format("%" + args2.IntValue() + "s", hex).replace(' ', '0'));
		}
		return Operand.Create(hex);
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		if (func2 != null) {
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
