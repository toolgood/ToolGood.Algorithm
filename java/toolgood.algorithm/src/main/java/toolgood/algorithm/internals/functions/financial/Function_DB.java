package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

final class Function_DB extends Function_5 {
	public Function_DB(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 4 || funcs.length > 5) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 4 to 5 parameters.");
		}
	}

	@Override
	public String Name() {
		return "DB";
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

		int month = 12;
		if (func5 != null) {
			Operand monthArg = GetNumber_5(engine, tempParameter);
			if (monthArg.IsErrorOrNone()) return monthArg;
			month = monthArg.IntValue();
			if (month < 1 || month > 12) {
				return ParameterError(5);
			}
		}

		if (life.compareTo(BigDecimal.ZERO) == 0 || cost.compareTo(BigDecimal.ZERO) == 0) return Div0Error();
		if (period.compareTo(BigDecimal.ONE) < 0 || period.compareTo(life) > 0) {
			return ParameterError(4);
		}
		if (life.compareTo(BigDecimal.ONE) < 0) {
			return ParameterError(3);
		}

		BigDecimal rate = BigDecimal.ONE.subtract(
			BigDecimal.valueOf(Math.pow(
				salvage.divide(cost, MathContext.DECIMAL128).doubleValue(),
				BigDecimal.ONE.divide(life, MathContext.DECIMAL128).doubleValue()
			))
		);
		rate = rate.setScale(3, RoundingMode.HALF_UP);

		BigDecimal depreciation = BigDecimal.ZERO;

		if (period.compareTo(BigDecimal.ONE) == 0) {
			depreciation = cost.multiply(rate).multiply(BigDecimal.valueOf(month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
		} else if (period.intValue() == life.intValue()) {
			BigDecimal remainingCost = cost;
			for (int i = 1; i < life.intValue(); i++) {
				remainingCost = remainingCost.subtract(depreciation);
				if (i == 1) {
					depreciation = cost.multiply(rate).multiply(BigDecimal.valueOf(month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
				} else if (i < life.intValue()) {
					depreciation = remainingCost.multiply(rate);
				}
			}
			remainingCost = remainingCost.subtract(depreciation);
			depreciation = remainingCost.multiply(rate).multiply(BigDecimal.valueOf(12 - month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
		} else {
			BigDecimal remainingCost = cost;
			for (int i = 1; i <= period.intValue(); i++) {
				if (i == 1) {
					depreciation = cost.multiply(rate).multiply(BigDecimal.valueOf(month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
				} else {
					remainingCost = remainingCost.subtract(depreciation);
					depreciation = remainingCost.multiply(rate);
				}
			}
		}

		return Operand.Create(depreciation);
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
		if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
	}
}
