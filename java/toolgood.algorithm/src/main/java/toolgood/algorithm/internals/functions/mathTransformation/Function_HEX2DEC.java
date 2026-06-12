package toolgood.algorithm.internals.functions.mathTransformation;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.RegexHelper;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_HEX2DEC extends Function_2 {
	public Function_HEX2DEC(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Hex2Dec"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetText_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		if (!RegexHelper.IsHex(args1.TextValue())) { return ParameterError(1); }
		int num = Integer.parseInt(args1.TextValue(), 16);
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0) {
				return ParameterError(2);
			}
			String n = Integer.toString(num);
			if (n.length() <= args2.IntValue()) {
				return Operand.Create(String.format("%" + args2.IntValue() + "s", n).replace(' ', '0'));
			}
			return ParameterError(2);
		}
		return Operand.Create(BigDecimal.valueOf(num));
	}

	@Override
	public OperandType GetResultType() {
		if (func2 != null) {
			return OperandType.TEXT;
		}
		return OperandType.NUMBER;
	}

	@Override
	void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		if (func2 != null) {
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
