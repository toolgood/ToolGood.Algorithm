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

public final class Function_CORREL extends Function_N {
    public Function_CORREL(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "CORREL";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2)
            return ParameterError(1);

        Operand array1Arg = GetArray(engine, tempParameter, 0);
        if (array1Arg.IsErrorOrNone())
            return array1Arg;

        Operand array2Arg = GetArray(engine, tempParameter, 1);
        if (array2Arg.IsErrorOrNone())
            return array2Arg;

        List<BigDecimal> xValues = new ArrayList<>();
        for (Operand item : array1Arg.ArrayValue()) {
            if (item.IsNumber()) {
                xValues.add(item.NumberValue());
            } else {
                return ParameterError(1);
            }
        }

        List<BigDecimal> yValues = new ArrayList<>();
        for (Operand item : array2Arg.ArrayValue()) {
            if (item.IsNumber()) {
                yValues.add(item.NumberValue());
            } else {
                return ParameterError(2);
            }
        }

        if (xValues.size() != yValues.size() || xValues.size() < 2)
            return FunctionError();

        int n = xValues.size();
        double sumX = 0, sumY = 0;

        for (int i = 0; i < n; i++) {
            sumX += xValues.get(i).doubleValue();
            sumY += yValues.get(i).doubleValue();
        }

        double meanX = sumX / n;
        double meanY = sumY / n;

        double numerator = 0, denomX = 0, denomY = 0;

        for (int i = 0; i < n; i++) {
            double dx = xValues.get(i).doubleValue() - meanX;
            double dy = yValues.get(i).doubleValue() - meanY;
            numerator += dx * dy;
            denomX += dx * dx;
            denomY += dy * dy;
        }

        if (denomX == 0 || denomY == 0)
            return Div0Error();

        return Operand.Create(BigDecimal.valueOf(numerator / Math.sqrt(denomX * denomY)));
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
