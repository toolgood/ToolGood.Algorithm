package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;

final class Function_SYD extends Function_4 {
	public Function_SYD(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length != 4) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 4 parameters.");
		}
	}

	@Override
	public String Name() {
		return "SYD";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand costArg = GetNumber_1(engine, tempParameter);
		if (costArg.IsErrorOrNone()) return costArg;
		BigDecimal cost = costArg.NumberValue();

		Operand salvageArg = GetNumber_2(engine, tempParameter);
		if (salvageArg.IsErrorOrNone()) return salvageArg;
		BigDecimal salvage = salvageArg.NumberValue();

		Operand lifeArg = GetNumber_3(engine, tempParameter);
		if (lifeArg.IsErrorOrNone()) return lifeArg;
		BigDecimal life = lifeArg.NumberValue();

		Operand periodArg = GetNumber_4(engine, tempParameter);
		if (periodArg.IsErrorOrNone()) return periodArg;
		BigDecimal period = periodArg.NumberValue();

		if (life.compareTo(BigDecimal.ZERO) == 0) return Div0Error();
		if (period.compareTo(BigDecimal.ONE) < 0 || period.compareTo(life) > 0) {
			return ParameterError(4);
		}
		if (life.compareTo(BigDecimal.ONE) < 0) {
			return ParameterError(3);
		}

		BigDecimal numerator = cost.subtract(salvage).multiply(life.subtract(period).add(BigDecimal.ONE)).multiply(BigDecimal.valueOf(2));
		BigDecimal denominator = life.multiply(life.add(BigDecimal.ONE));
		BigDecimal syd = numerator.divide(denominator, MathContext.DECIMAL128);

		return Operand.Create(syd);
	}

	@Override
	public OperandType GetResultType() {
		return OperandType.NUMBER;
	}

	@Override
	public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
		func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
	}
}
