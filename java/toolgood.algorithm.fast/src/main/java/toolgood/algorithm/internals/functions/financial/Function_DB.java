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

public final class Function_DB extends Function_5 {
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

		// Excel: month<12 时折旧跨越 life+1 个期间(第1年部分月 + life-1 个整年 + 最后部分月),
		// 最后一期乘 (12-month)/12 修正系数, 其余期间(含第 life 期)为完整年折旧
		int totalPeriods = (month == 12) ? life.intValue() : life.intValue() + 1;
		if (period.compareTo(BigDecimal.ONE) < 0 || period.compareTo(BigDecimal.valueOf(totalPeriods)) > 0) {
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

		BigDecimal remainingCost = cost;
		BigDecimal depreciation = BigDecimal.ZERO;
		for (int i = 1; i <= period.intValue(); i++) {
			if (i == 1) {
				depreciation = cost.multiply(rate).multiply(BigDecimal.valueOf(month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
			} else if (i == totalPeriods && month != 12) {
				depreciation = remainingCost.multiply(rate).multiply(BigDecimal.valueOf(12 - month)).divide(BigDecimal.valueOf(12), MathContext.DECIMAL128);
			} else {
				depreciation = remainingCost.multiply(rate);
			}
			remainingCost = remainingCost.subtract(depreciation);
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
