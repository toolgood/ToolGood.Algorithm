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
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_DEC2HEX extends Function_2 {
	public Function_DEC2HEX(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Dec2Hex"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetNumber_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		// Excel 范围:-549755813888 ~ 549755813887,超出 int 范围,用 BigDecimal 校验后转 long
		BigDecimal numValue = args1.NumberValue();
		if (numValue.compareTo(BigDecimal.valueOf(-549755813888L)) < 0 || numValue.compareTo(BigDecimal.valueOf(549755813887L)) > 0) {
			return ParameterError(1);
		}
		long num = numValue.longValue();
		if (num < 0) {
			// 负数:返回 10 位十六进制补码,按 Excel 语义忽略 places
			return Operand.Create(String.format("%10s", Long.toHexString(num & 0xFFFFFFFFFFL).toUpperCase()).replace(' ', '0'));
		}
		String hex = Long.toHexString(num).toUpperCase();
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0) {
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
		func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		if (func2 != null) {
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
