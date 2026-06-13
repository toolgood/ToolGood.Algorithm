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

public final class Function_DEC2BIN extends Function_2 {
	public Function_DEC2BIN(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Dec2Bin"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetNumber_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		int num = args1.IntValue();
		if (num < -512 || num > 511) {
			return ParameterError(1);
		}
		String binaryStr = Integer.toString(num, 2);
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0) {
				return ParameterError(2);
			}
			if (binaryStr.length() > args2.IntValue()) {
				return ParameterError(2);
			}
			if (binaryStr.length() <= args2.IntValue()) {
				return Operand.Create(String.format("%" + args2.IntValue() + "s", binaryStr).replace(' ', '0'));
			}
			return ParameterError(2);
		}
		return Operand.Create(binaryStr);
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.TEXT;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		if (func2 != null) {
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
