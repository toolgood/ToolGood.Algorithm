package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_IRR extends Function_2 {
    public Function_IRR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException(String.format("Function '%s' requires 1 to 2 parameters.", Name()));
        }
    }

    @Override
    public String Name() {
        return "IRR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        List<BigDecimal> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            if (v.IsNumber()) {
                values.add(v.NumberValue());
            } else {
                Operand v2 = v.ToNumber(String.format("Function '%s' parameter 1 is error!", Name()));
                if (v2.IsErrorOrNone()) return v2;
                values.add(v2.NumberValue());
            }
        }

        if (values.size() == 0) {
            return ParameterError(1);
        }

        boolean hasPositive = false;
        boolean hasNegative = false;
        for (BigDecimal v : values) {
            if (v.compareTo(BigDecimal.ZERO) > 0) hasPositive = true;
            if (v.compareTo(BigDecimal.ZERO) < 0) hasNegative = true;
        }
        if (!hasPositive || !hasNegative) {
            return ParameterError(1);
        }

        BigDecimal guess = new BigDecimal("0.1");
        if (func2 != null) {
            Operand guessArg = GetNumber_2(engine, tempParameter);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        try {
            BigDecimal irr = NewtonRaphsonIRR(values, guess);
            return Operand.Create(irr);
        } catch (Exception e) {
            return FunctionError();
        }
    }

    private BigDecimal NewtonRaphsonIRR(List<BigDecimal> values, BigDecimal guess) {
        BigDecimal rate = guess;
        for (int iter = 0; iter < 100; iter++) {
            BigDecimal npv = BigDecimal.ZERO;
            BigDecimal dnpv = BigDecimal.ZERO;

            for (int i = 0; i < values.size(); i++) {
                BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), i));
                npv = npv.add(values.get(i).divide(factor, MathContext.DECIMAL128));
                dnpv = dnpv.subtract(
                        BigDecimal.valueOf(i).multiply(values.get(i))
                                .divide(factor.multiply(BigDecimal.ONE.add(rate)), MathContext.DECIMAL128));
            }

            if (dnpv.abs().compareTo(new BigDecimal("1e-12")) < 0) break;
            BigDecimal newRate = rate.subtract(npv.divide(dnpv, MathContext.DECIMAL128));

            if (newRate.subtract(rate).abs().compareTo(new BigDecimal("1e-10")) < 0) {
                return newRate;
            }
            rate = newRate;
        }
        throw new RuntimeException("IRR calculation did not converge");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        if (func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
