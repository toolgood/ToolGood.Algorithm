package toolgood.algorithm.internals.functions.financial;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_DB extends Function_5 {
    public Function_DB(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "DB";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand costArg = GetNumber_1(engine, tempParameter);
        if (costArg.IsErrorOrNone()) return costArg;
        double cost = costArg.DoubleValue();

        Operand salvageArg = GetNumber_2(engine, tempParameter);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        double salvage = salvageArg.DoubleValue();

        Operand lifeArg = GetNumber_3(engine, tempParameter);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        double life = lifeArg.DoubleValue();

        Operand periodArg = GetNumber_4(engine, tempParameter);
        if (periodArg.IsErrorOrNone()) return periodArg;
        double period = periodArg.DoubleValue();

        int month = 12;
        if (func5 != null) {
            Operand monthArg = GetNumber_5(engine, tempParameter);
            if (monthArg.IsErrorOrNone()) return monthArg;
            month = monthArg.IntValue();
            if (month < 1 || month > 12) {
                return ParameterError(5);
            }
        }

        if (life == 0 || cost == 0) return Div0Error();
        if (period < 1 || period > life + 1) return ParameterError(4);
        if (life < 1) return ParameterError(3);

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
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
