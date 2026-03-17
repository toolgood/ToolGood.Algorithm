package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_MIRR extends Function_3 {
    public Function_MIRR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "MIRR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;

        List<Double> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            values.add(v.DoubleValue());
        }

        Operand financeRateArg = GetNumber_2(engine, tempParameter);
        if (financeRateArg.IsErrorOrNone()) return financeRateArg;
        double financeRate = financeRateArg.DoubleValue();

        Operand reinvestRateArg = GetNumber_3(engine, tempParameter);
        if (reinvestRateArg.IsErrorOrNone()) return reinvestRateArg;
        double reinvestRate = reinvestRateArg.DoubleValue();

        int n = values.size();
        if (n == 0) return ParameterError(1);
        if (n == 1) return Div0Error();

        double npvNegative = 0;
        double npvPositive = 0;

        for (int i = 0; i < n; i++) {
            if (values.get(i) < 0) {
                npvNegative += values.get(i) / Math.pow(1 + financeRate, i);
            } else {
                npvPositive += values.get(i) * Math.pow(1 + reinvestRate, n - 1 - i);
            }
        }

        if (npvNegative == 0) return Div0Error();

        double mirr = Math.pow(-npvPositive / npvNegative, 1.0 / (n - 1)) - 1;
        return Operand.Create(mirr);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
