package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;

/**
 * SYD: 返回某项资产按年数总和折旧法计算的指定期间的折旧值
 * SYD(cost, salvage, life, period)
 */
public class Function_SYD extends Function_4 {
    public Function_SYD(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand costArg = GetNumber_1(engine, tempParameter);
        if (costArg.IsError()) return costArg;
        double cost = costArg.DoubleValue();

        Operand salvageArg = GetNumber_2(engine, tempParameter);
        if (salvageArg.IsError()) return salvageArg;
        double salvage = salvageArg.DoubleValue();

        Operand lifeArg = GetNumber_3(engine, tempParameter);
        if (lifeArg.IsError()) return lifeArg;
        double life = lifeArg.DoubleValue();

        Operand periodArg = GetNumber_4(engine, tempParameter);
        if (periodArg.IsError()) return periodArg;
        double period = periodArg.DoubleValue();

        if (life == 0) return Div0Error();
        if (period < 1 || period > life) return ParameterError(4);
        if (life < 1) return ParameterError(3);

        double syd = (cost - salvage) * (life - period + 1) * 2 / (life * (life + 1));
        return Operand.Create(syd);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "SYD");
    }
}
