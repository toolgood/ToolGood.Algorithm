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

final class Function_RATE extends Function_6 {
    public Function_RATE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 3 || funcs.length > 6) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 3 to 6 parameters.");
        }
    }

    @Override
    public String Name() {
        return "RATE";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand nperArg = GetNumber_1(engine, tempParameter);
        if (nperArg.IsErrorOrNone()) return nperArg;
        BigDecimal nper = nperArg.NumberValue();

        Operand pmtArg = GetNumber_2(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        BigDecimal pmt = pmtArg.NumberValue();

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsErrorOrNone()) return pvArg;
        BigDecimal pv = pvArg.NumberValue();

        if (nper.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(1);
        }

        BigDecimal fv = BigDecimal.ZERO;
        if (func4 != null) {
            Operand fvArg = GetNumber_4(engine, tempParameter);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsErrorOrNone()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) {
                return ParameterError(5);
            }
        }

        BigDecimal guess = new BigDecimal("0.1");
        if (func6 != null) {
            Operand guessArg = GetNumber_6(engine, tempParameter);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        BigDecimal rate = NewtonRaphson(nper, pmt, pv, fv, type, guess);
        return Operand.Create(rate);
    }

    private BigDecimal NewtonRaphson(BigDecimal n, BigDecimal p, BigDecimal v, BigDecimal f, int type, BigDecimal rate) {
        for (int i = 0; i < 100; i++) {
            BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), n.doubleValue()));
            BigDecimal fn;

            if (type == 1) {
                fn = v.multiply(factor)
                        .add(p.multiply(BigDecimal.ONE.add(rate)).multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128))
                        .add(f);
            } else {
                fn = v.multiply(factor)
                        .add(p.multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128))
                        .add(f);
            }

            BigDecimal pow_n_minus_1 = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(),
                    n.subtract(BigDecimal.ONE).doubleValue()));

            BigDecimal dfn;
            // term1 = v * n * pow_n_minus_1
            BigDecimal term1 = v.multiply(n).multiply(pow_n_minus_1);
            // inner = n * rate * pow_n_minus_1 - (factor - 1)
            BigDecimal inner = n.multiply(rate).multiply(pow_n_minus_1).subtract(factor.subtract(BigDecimal.ONE));

            if (type == 1) {
                // term2 = p * (1 + rate) * inner / (rate * rate)
                BigDecimal term2 = p.multiply(BigDecimal.ONE.add(rate)).multiply(inner)
                        .divide(rate.multiply(rate), MathContext.DECIMAL128);
                // term3 = p * (factor - 1) / rate
                BigDecimal term3 = p.multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128);
                dfn = term1.add(term2).add(term3);
            } else {
                // term2 = p * inner / (rate * rate)
                BigDecimal term2 = p.multiply(inner).divide(rate.multiply(rate), MathContext.DECIMAL128);
                dfn = term1.add(term2);
            }

            if (dfn.abs().compareTo(new BigDecimal("1e-12")) < 0) break;
            BigDecimal newRate = rate.subtract(fn.divide(dfn, MathContext.DECIMAL128));

            if (newRate.subtract(rate).abs().compareTo(new BigDecimal("1e-10")) < 0) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
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
        if (func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
