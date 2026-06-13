package toolgood.algorithm.internals.functions.mathSum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_INTERCEPT extends Function_N {
    public Function_INTERCEPT(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "INTERCEPT";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2)
            return ParameterError(1);

        Operand yArrayArg = GetArray(engine, tempParameter, 0);
        if (yArrayArg.IsErrorOrNone())
            return yArrayArg;

        Operand xArrayArg = GetArray(engine, tempParameter, 1);
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

        return Operand.Create(BigDecimal.valueOf(intercept));
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
