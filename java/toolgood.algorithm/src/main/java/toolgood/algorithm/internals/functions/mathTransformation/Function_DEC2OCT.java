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

public final class Function_DEC2OCT extends Function_2 {
	public Function_DEC2OCT(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 1 || funcs.length > 2) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
		}
	}

	@Override
	public String Name() { return "Dec2Oct"; }

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand args1 = GetNumber_1(engine, tempParameter);
		if (args1.IsErrorOrNone()) { return args1; }
		// Excel 范围:-536870912 ~ 536870911
		BigDecimal numValue = args1.NumberValue();
		if (numValue.compareTo(BigDecimal.valueOf(-536870912)) < 0 || numValue.compareTo(BigDecimal.valueOf(536870911)) > 0) {
			return ParameterError(1);
		}
		int num = numValue.intValue();
		if (num < 0) {
			// 负数:返回 10 位八进制补码,按 Excel 语义忽略 places
			return Operand.Create(String.format("%10s", Integer.toOctalString(num & 0x3FFFFFFF)).replace(' ', '0'));
		}
		String oct = Integer.toString(num, 8);
		if (func2 != null) {
			Operand args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone()) { return args2; }
			if (args2.IntValue() < 0) {
				return ParameterError(2);
			}
			if (oct.length() > args2.IntValue()) {
				return ParameterError(2);
			}
			return Operand.Create(String.format("%" + args2.IntValue() + "s", oct).replace(' ', '0'));
		}
		return Operand.Create(oct);
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
