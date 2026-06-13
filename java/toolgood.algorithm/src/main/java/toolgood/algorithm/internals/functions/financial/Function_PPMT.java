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
import toolgood.algorithm.internals.functions.Function_6;

final class Function_PPMT extends Function_6 {
	public Function_PPMT(FunctionBase[] funcs) {
		super(funcs);
		if (funcs.length < 4 || funcs.length > 6) {
			throw new IllegalArgumentException("Function '" + Name() + "' requires 4 to 6 parameters.");
		}
	}

	@Override
	public String Name() {
		return "PPMT";
	}

	@Override
	public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
		Operand rateArg = GetNumber_1(engine, tempParameter);
		if (rateArg.IsErrorOrNone()) return rateArg;
		BigDecimal rate = rateArg.NumberValue();

		Operand perArg = GetNumber_2(engine, tempParameter);
		if (perArg.IsErrorOrNone()) return perArg;
		BigDecimal per = perArg.NumberValue();

		Operand nperArg = GetNumber_3(engine, tempParameter);
		if (nperArg.IsErrorOrNone()) return nperArg;
		BigDecimal nper = nperArg.NumberValue();

		Operand pvArg = GetNumber_4(engine, tempParameter);
		if (pvArg.IsErrorOrNone()) return pvArg;
		BigDecimal pv = pvArg.NumberValue();

		BigDecimal fv = BigDecimal.ZERO;
		if (func5 != null) {
			Operand fvArg = GetNumber_5(engine, tempParameter);
			if (fvArg.IsErrorOrNone()) return fvArg;
			fv = fvArg.NumberValue();
		}

		int type = 0;
		if (func6 != null) {
			Operand typeArg = GetNumber_6(engine, tempParameter);
			if (typeArg.IsErrorOrNone()) return typeArg;
			type = typeArg.IntValue();
			if (type != 0 && type != 1) {
				return ParameterError(6);
			}
		}

		BigDecimal pmtResult = calculatePMT(rate, nper, pv, fv, type);
		BigDecimal ipmtResult = calculateIPMT(rate, per, nper, pv, fv, type);
		return Operand.Create(pmtResult.subtract(ipmtResult));
	}

	private BigDecimal calculatePMT(BigDecimal rate, BigDecimal nper, BigDecimal pv, BigDecimal fv, int type) {
		if (rate.compareTo(BigDecimal.ZERO) == 0) {
			return pv.add(fv).negate().divide(nper, MathContext.DECIMAL128);
		}
		BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), nper.doubleValue()));
		BigDecimal pmt = pv.multiply(factor).add(fv).negate().multiply(rate).divide(factor.subtract(BigDecimal.ONE), MathContext.DECIMAL128);
		if (type == 1) {
			pmt = pmt.divide(BigDecimal.ONE.add(rate), MathContext.DECIMAL128);
		}
		return pmt;
	}

	private BigDecimal calculateIPMT(BigDecimal rate, BigDecimal per, BigDecimal nper, BigDecimal pv, BigDecimal fv, int type) {
		if (rate.compareTo(BigDecimal.ZERO) == 0) {
			return BigDecimal.ZERO;
		}
		BigDecimal pmt = calculatePMT(rate, nper, pv, fv, type);
		BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), per.subtract(BigDecimal.ONE).doubleValue()));

		BigDecimal term1 = pv.multiply(factor);
		BigDecimal term2 = pmt.multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128);
		BigDecimal ipmt = term1.add(term2).negate().multiply(rate);

		if (type == 1 && per.compareTo(BigDecimal.ONE) == 0) {
			ipmt = BigDecimal.ZERO;
		}
		return ipmt;
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
		if (func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
	}
}
