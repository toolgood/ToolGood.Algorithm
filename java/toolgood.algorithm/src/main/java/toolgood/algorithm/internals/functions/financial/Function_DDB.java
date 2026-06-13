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
import toolgood.algorithm.internals.functions.Function_5;

public final class Function_DDB extends Function_5 {
	public Function_DDB(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 4 || funcs.length > 5) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 4 to 5 parameters.");
		}
	}

	@Override
	public String Name() {
		return "DDB";
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

		BigDecimal factor = BigDecimal.valueOf(2);
		if (func5 != null) {
			Operand factorArg = GetNumber_5(engine, tempParameter);
			if (factorArg.IsErrorOrNone()) return factorArg;
			factor = factorArg.NumberValue();
		}

		if (life.compareTo(BigDecimal.ZERO) == 0 || factor.compareTo(BigDecimal.ZERO) == 0) return Div0Error();
		if (period.compareTo(BigDecimal.ONE) < 0 || period.compareTo(life) > 0) {
			return ParameterError(4);
		}
		if (life.compareTo(BigDecimal.ONE) < 0) {
			return ParameterError(3);
		}

		BigDecimal depreciation = BigDecimal.ZERO;
		BigDecimal remainingCost = cost;

		for (int i = 1; i <= period.intValue(); i++) {
			BigDecimal ddb = remainingCost.multiply(factor).divide(life, MathContext.DECIMAL128);
			BigDecimal maxDepreciation = remainingCost.subtract(salvage);
			if (ddb.compareTo(maxDepreciation) > 0) {
				ddb = maxDepreciation;
			}
			if (i == period.intValue()) {
				depreciation = ddb;
			}
			remainingCost = remainingCost.subtract(ddb);
			if (remainingCost.compareTo(salvage) <= 0) {
				if (i == period.intValue()) {
					depreciation = remainingCost.add(ddb).subtract(salvage);
				}
				break;
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
