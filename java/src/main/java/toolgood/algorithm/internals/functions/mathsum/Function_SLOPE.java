package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SLOPE extends Function_N {
    public Function_SLOPE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SLOPE";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2) return ParameterError(1);

        Operand yArrayArg = GetArray(engine, tempParameter, 0);
        if (yArrayArg.IsErrorOrNone()) return yArrayArg;

        Operand xArrayArg = GetArray(engine, tempParameter, 1);
        if (xArrayArg.IsErrorOrNone()) return xArrayArg;

        List<BigDecimal> yValues = new ArrayList<>();
        for (Operand item : yArrayArg.ArrayValue()) {
            if (item.IsNumber()) yValues.add(item.NumberValue());
        }

        List<BigDecimal> xValues = new ArrayList<>();
        for (Operand item : xArrayArg.ArrayValue()) {
            if (item.IsNumber()) xValues.add(item.NumberValue());
        }

        if (yValues.size() != xValues.size() || yValues.size() < 2) return FunctionError();

        BigDecimal sumX = BigDecimal.ZERO, sumY = BigDecimal.ZERO, sumXY = BigDecimal.ZERO, sumX2 = BigDecimal.ZERO;
        int n = yValues.size();

        for (int i = 0; i < n; i++) {
            sumX = sumX.add(xValues.get(i));
            sumY = sumY.add(yValues.get(i));
            sumXY = sumXY.add(xValues.get(i).multiply(yValues.get(i)));
            sumX2 = sumX2.add(xValues.get(i).multiply(xValues.get(i)));
        }

        BigDecimal denominator = new BigDecimal(n).multiply(sumX2).subtract(sumX.multiply(sumX));
        if (denominator.compareTo(BigDecimal.ZERO) == 0) {
            return Div0Error();
        }
        BigDecimal slope = new BigDecimal(n).multiply(sumXY).subtract(sumX.multiply(sumY)).divide(denominator, java.math.MathContext.DECIMAL128);
        return Operand.Create(slope);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
