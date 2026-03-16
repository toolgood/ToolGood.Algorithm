package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public class Function_CORREL extends Function_N {
    public Function_CORREL(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "CORREL";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2) return ParameterError(1);

        Operand array1Arg = GetArray(engine, tempParameter, 0);
        if (array1Arg.IsError()) return array1Arg;

        Operand array2Arg = GetArray(engine, tempParameter, 1);
        if (array2Arg.IsError()) return array2Arg;

        List<BigDecimal> xValues = new ArrayList<>();
        for (Operand item : array1Arg.ArrayValue()) {
            if (item.IsNumber()) xValues.add(item.NumberValue());
        }

        List<BigDecimal> yValues = new ArrayList<>();
        for (Operand item : array2Arg.ArrayValue()) {
            if (item.IsNumber()) yValues.add(item.NumberValue());
        }

        if (xValues.size() != yValues.size() || xValues.size() < 2) return FunctionError();

        int n = xValues.size();
        BigDecimal sumX = BigDecimal.ZERO, sumY = BigDecimal.ZERO;

        for (int i = 0; i < n; i++) {
            sumX = sumX.add(xValues.get(i));
            sumY = sumY.add(yValues.get(i));
        }

        BigDecimal meanX = sumX.divide(new BigDecimal(n), 20, java.math.RoundingMode.HALF_UP);
        BigDecimal meanY = sumY.divide(new BigDecimal(n), 20, java.math.RoundingMode.HALF_UP);

        BigDecimal numerator = BigDecimal.ZERO, denomX = BigDecimal.ZERO, denomY = BigDecimal.ZERO;

        for (int i = 0; i < n; i++) {
            BigDecimal dx = xValues.get(i).subtract(meanX);
            BigDecimal dy = yValues.get(i).subtract(meanY);
            numerator = numerator.add(dx.multiply(dy));
            denomX = denomX.add(dx.multiply(dx));
            denomY = denomY.add(dy.multiply(dy));
        }

        if (denomX.compareTo(BigDecimal.ZERO) == 0 || denomY.compareTo(BigDecimal.ZERO) == 0) return Div0Error();

        double denomD = Math.sqrt(denomX.multiply(denomY).doubleValue());
        BigDecimal result = numerator.divide(new BigDecimal(denomD), 20, java.math.RoundingMode.HALF_UP);
        return Operand.Create(result);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void getParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].getParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
        funcs[1].getParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
    }
}
