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
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

public final class Function_MIRR extends Function_3 {
    public Function_MIRR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException(String.format("Function '%s' requires exactly 3 parameters.", Name()));
        }
    }

    @Override
    public String Name() {
        return "MIRR";
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

        Operand financeRateArg = GetNumber_2(engine, tempParameter);
        if (financeRateArg.IsErrorOrNone()) return financeRateArg;
        BigDecimal financeRate = financeRateArg.NumberValue();

        Operand reinvestRateArg = GetNumber_3(engine, tempParameter);
        if (reinvestRateArg.IsErrorOrNone()) return reinvestRateArg;
        BigDecimal reinvestRate = reinvestRateArg.NumberValue();

        BigDecimal npvNegative = BigDecimal.ZERO;
        BigDecimal npvPositive = BigDecimal.ZERO;
        int n = values.size();

        if (n == 0) {
            return ParameterError(1);
        }
        if (n == 1) {
            return Div0Error();
        }

        for (int i = 0; i < n; i++) {
            if (values.get(i).compareTo(BigDecimal.ZERO) < 0) {
                BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(financeRate).doubleValue(), i));
                npvNegative = npvNegative.add(values.get(i).divide(factor, MathContext.DECIMAL128));
            } else {
                BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(reinvestRate).doubleValue(), n - 1 - i));
                npvPositive = npvPositive.add(values.get(i).multiply(factor));
            }
        }

        if (npvNegative.compareTo(BigDecimal.ZERO) == 0) return Div0Error();

        BigDecimal base = npvPositive.negate().divide(npvNegative, MathContext.DECIMAL128);
        BigDecimal exponent = BigDecimal.ONE.divide(BigDecimal.valueOf(n - 1), MathContext.DECIMAL128);
        BigDecimal mirr = BigDecimal.valueOf(Math.pow(base.doubleValue(), exponent.doubleValue()))
                .subtract(BigDecimal.ONE);
        return Operand.Create(mirr);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
