package toolgood.algorithm.internals.functions.mathSum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_FORECAST extends Function_N {
    public Function_FORECAST(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "FORECAST";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 3)
            return ParameterError(1);

        Operand xArg = GetNumber(engine, tempParameter, 0);
        if (xArg.IsErrorOrNone())
            return xArg;
        BigDecimal x = xArg.NumberValue();

        Operand yArrayArg = GetArray(engine, tempParameter, 1);
        if (yArrayArg.IsErrorOrNone())
            return yArrayArg;

        Operand xArrayArg = GetArray(engine, tempParameter, 2);
        if (xArrayArg.IsErrorOrNone())
            return xArrayArg;

        List<BigDecimal> yValues = new ArrayList<>();
        for (Operand item : yArrayArg.ArrayValue()) {
            if (item.IsNumber())
                yValues.add(item.NumberValue());
        }

        List<BigDecimal> xValues = new ArrayList<>();
        for (Operand item : xArrayArg.ArrayValue()) {
            if (item.IsNumber())
                xValues.add(item.NumberValue());
        }

        if (yValues.size() != xValues.size() || yValues.size() < 2)
            return FunctionError();

        double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
        int n = yValues.size();

        for (int i = 0; i < n; i++) {
            double xv = xValues.get(i).doubleValue();
            double yv = yValues.get(i).doubleValue();
            sumX += xv;
            sumY += yv;
            sumXY += xv * yv;
            sumX2 += xv * xv;
        }

        double meanX = sumX / n;
        double meanY = sumY / n;

        double denominator = n * sumX2 - sumX * sumX;
        if (denominator == 0) {
            return Div0Error();
        }
        double slope = (n * sumXY - sumX * sumY) / denominator;
        double intercept = meanY - slope * meanX;

        return Operand.Create(BigDecimal.valueOf(intercept + slope * x.doubleValue()));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        funcs[2].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
