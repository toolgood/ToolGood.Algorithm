package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * DB: 使用固定余额递减法，计算一笔资产在给定期间内的折旧�?
 * DB(cost, salvage, life, period, [month])
 */
public class Function_DB extends Function_5 {
    public Function_DB(FunctionBase[] funcs) {
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

        int month = 12;
        if (func5 != null) {
            Operand monthArg = GetNumber_5(engine, tempParameter);
            if (monthArg.IsError()) return monthArg;
            month = monthArg.IntValue();
            if (month < 1 || month > 12) {
                return ParameterError(5);
            }
        }

        if (life == 0 || cost == 0) return Div0Error();
        if (period < 1 || period > life + 1) return ParameterError(4);
        if (life < 1) return ParameterError(3);

        // rate 保留3位小�?
        double rate = 1 - Math.pow(salvage / cost, 1.0 / life);
        rate = Math.round(rate * 1000.0) / 1000.0;

        double depreciation = 0;
        if (period == 1) {
            depreciation = cost * rate * month / 12.0;
        } else if ((int) period == (int) life) {
            double remainingCost = cost;
            for (int i = 1; i < (int) life; i++) {
                remainingCost -= depreciation;
                if (i == 1) {
                    depreciation = cost * rate * month / 12.0;
                } else if (i < (int) life) {
                    depreciation = remainingCost * rate;
                }
            }
            remainingCost -= depreciation;
            depreciation = remainingCost * rate * (12 - month) / 12.0;
        } else {
            double remainingCost = cost;
            for (int i = 1; i <= (int) period; i++) {
                if (i == 1) {
                    depreciation = cost * rate * month / 12.0;
                } else {
                    remainingCost -= depreciation;
                    depreciation = remainingCost * rate;
                }
            }
        }

        return Operand.Create(depreciation);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DB");
    }
}
