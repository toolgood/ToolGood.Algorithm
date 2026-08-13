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

public final class Function_HEX2BIN extends Function_2 {
	public Function_HEX2BIN(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Hex2Bin"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		if (!RegexHelper.IsHex(args1.TextValue())) { return ParameterError(1); }
		String text = args1.TextValue();
		if (text.length() > 10) { return ParameterError(1); }
		// 10 位十六进制补码解析
		long num = Long.parseLong(text, 16);
		if (num >= 0x8000000000L) { num -= 0x10000000000L; }
		// Excel HEX2BIN 结果范围为 -512~511
		if (num < -512 || num > 511) {
			return ParameterError(1);
		}
		String bin;
		if (num < 0) {
			// 负数:10 位二进制补码
			bin = String.format("%10s", Integer.toBinaryString((int) (num & 1023))).replace(' ', '0');
		} else {
			bin = Long.toBinaryString(num);
		}
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0 || args2.IntValue() > 10) {
				return ParameterError(2);
			}
			if (bin.length() > args2.IntValue()) {
				return ParameterError(2);
			}
			return Operand.Create(String.format("%" + args2.IntValue() + "s", bin).replace(' ', '0'));
		}
		return Operand.Create(bin);
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
