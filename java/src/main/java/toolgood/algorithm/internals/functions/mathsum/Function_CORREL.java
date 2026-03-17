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
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_CORREL extends Function_N {
    public Function_CORREL(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "CORREL";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        if (funcs.length < 2) return ParameterError(1);

        Operand array1Arg = GetArray(engine, tempParameter, 0);
        if (array1Arg.IsErrorOrNone()) return array1Arg;

        Operand array2Arg = GetArray(engine, tempParameter, 1);
        if (array2Arg.IsErrorOrNone()) return array2Arg;

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

        BigDecimal meanX = sumX.divide(new BigDecimal(n), java.math.MathContext.DECIMAL128);
        BigDecimal meanY = sumY.divide(new BigDecimal(n), java.math.MathContext.DECIMAL128);

        BigDecimal numerator = BigDecimal.ZERO, denomX = BigDecimal.ZERO, denomY = BigDecimal.ZERO;

        for (int i = 0; i < n; i++) {
            BigDecimal dx = xValues.get(i).subtract(meanX);
            BigDecimal dy = yValues.get(i).subtract(meanY);
            numerator = numerator.add(dx.multiply(dy));
            denomX = denomX.add(dx.multiply(dx));
            denomY = denomY.add(dy.multiply(dy));
        }

        if (denomX.compareTo(BigDecimal.ZERO) == 0 || denomY.compareTo(BigDecimal.ZERO) == 0) return Div0Error();

        return Operand.Create(numerator.divide(MathEx.Sqrt(denomX.multiply(denomY)), java.math.MathContext.DECIMAL128));
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
