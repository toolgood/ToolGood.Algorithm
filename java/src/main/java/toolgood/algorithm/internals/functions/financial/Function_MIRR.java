package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

/**
 * MIRR: 返回一组定期现金流的修正内部收益率
 * MIRR(values, finance_rate, reinvest_rate)
 */
public class Function_MIRR extends Function_3 {
    public Function_MIRR(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsError()) return valuesArg;

        List<Double> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            values.add(v.IsNumber() ? v.DoubleValue() : 0.0);
        }

        Operand financeRateArg = GetNumber_2(engine, tempParameter);
        if (financeRateArg.IsError()) return financeRateArg;
        double financeRate = financeRateArg.DoubleValue();

        Operand reinvestRateArg = GetNumber_3(engine, tempParameter);
        if (reinvestRateArg.IsError()) return reinvestRateArg;
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
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "MIRR");
    }
}
