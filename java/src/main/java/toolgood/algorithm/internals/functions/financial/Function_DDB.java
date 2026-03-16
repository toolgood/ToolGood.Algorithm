package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * DDB: 使用双倍余额递减法或其他指定方法，计算一笔资产在给定期间内的折旧值
 * DDB(cost, salvage, life, period, [factor])
 */
public class Function_DDB extends Function_5 {
    public Function_DDB(FunctionBase[] funcs) {
        super(funcs);
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

        double factor = 2;
        if (func5 != null) {
            Operand factorArg = GetNumber_5(engine, tempParameter);
            if (factorArg.IsError()) return factorArg;
            factor = factorArg.DoubleValue();
        }

        if (life == 0 || factor == 0) return Div0Error();
        if (period < 1 || period > life) return ParameterError(4);
        if (life < 1) return ParameterError(3);

        double depreciation = 0;
        double remainingCost = cost;

        for (int i = 1; i <= (int) period; i++) {
            double ddb = remainingCost * factor / life;
            double maxDepreciation = remainingCost - salvage;
            if (ddb > maxDepreciation) {
                ddb = maxDepreciation;
            }
            if (i == (int) period) {
                depreciation = ddb;
            }
            remainingCost -= ddb;
            if (remainingCost <= salvage) {
                if (i == (int) period) {
                    depreciation = remainingCost + ddb - salvage;
                }
                break;
            }
        }

        return Operand.Create(depreciation);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DDB");
    }
}
